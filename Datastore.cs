using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;

namespace elixir_searcher {
    internal class Datastore : IDatastore {
        const string NO_INGREDIENTS = "NO_INGREDIENTS";

        private IWizardClient _client;
        private readonly Dictionary<string, HashSet<ElixirDto>> _ingredientsList;
        private bool _initialized;
        public Datastore(IWizardClient client) {
            _client = client;
            _initialized = false;
            _ingredientsList = new Dictionary<string, HashSet<ElixirDto>>();
        }

        // Initializes datastore fetching all elixirs from the API and storing them in memory.
        // Elixirs are referenced by their ingredients.
        public async Task Initialize() {
            _ingredientsList.Add(NO_INGREDIENTS, new());

            var allElixirs = await _client.GetAllElixirsAsync();

            foreach (var elixir in allElixirs) {
                if (elixir.Ingredients.Count == 0) {
                    _ingredientsList[NO_INGREDIENTS].Add(elixir);
                }

                foreach (var ingredient in elixir.Ingredients) {
                    var name = ingredient.Name.ToLower();
                    if (!_ingredientsList.ContainsKey(name)) {
                        _ingredientsList.Add(name, new());
                    }

                    var elixirDict = _ingredientsList[name];

                    if (!elixirDict.Contains(elixir)) {
                        elixirDict.Add(elixir);
                    }
                }
            }
            _initialized = true;
        }

        // Searchs elixirs using a list of ingredients. Optionally, the final results can
        // include elixirs that don't require ingredients.
        public async Task<IEnumerable<ElixirDto>> SearchElixirs(IEnumerable<string> ingredientsToSearch, bool includeNoIngredients = false) {
            if (!_initialized) {
                await Initialize();
            }
            // Get all possible elixirs that contain a subset of the ingredients
            IEnumerable<ElixirDto> possibleElixirs = new HashSet<ElixirDto>();

            foreach (var ingredient in ingredientsToSearch) {
                if (_ingredientsList.ContainsKey(ingredient)) {
                    var hs = _ingredientsList[ingredient];

                    possibleElixirs = possibleElixirs.Union(hs);
                }
            }

            // Filter out all elixirs that can't be conjured because they only have a subset of the ingredients
            var resultElixirs = new HashSet<ElixirDto>();

            foreach (var elixir in possibleElixirs) {
                var intersection = elixir.Ingredients
                                    .Select(i => i.Name.ToLower())
                                    .Intersect(ingredientsToSearch);

                if (intersection.Count() == elixir.Ingredients.Count()
                    && !resultElixirs.Contains(elixir)) {
                    resultElixirs.Add(elixir);
                }
            }

            if (includeNoIngredients) {
                return resultElixirs.Union(_ingredientsList[NO_INGREDIENTS]);
            } else {
                return resultElixirs;
            }
        }

    }
}
