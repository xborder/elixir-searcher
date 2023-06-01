using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elixir_searcher {

    internal class ElixirSearch {
        private readonly RootCommand _rootCommand;
        private readonly Datastore _datastore;
        public ElixirSearch(Datastore datastore) {

            var searchOption = new Option<IEnumerable<string>>(
                name: "--search",
                description: "list of ingredients to search separated by ,"
            ) {
                AllowMultipleArgumentsPerToken = true
            };
            var noIngredientsOption = new Option<bool>(
                name: "--include-no-ingredients",
                description: "include elixirs that don't require ingredients",
                getDefaultValue: () => false);

            _datastore = datastore;
            _rootCommand = new RootCommand("CLI utility to search available elixirs to conjure based on given ingredients");
            _rootCommand.AddOption(searchOption);
            _rootCommand.AddOption(noIngredientsOption);

            _rootCommand.SetHandler(
                (ingredientList, includeNoIngredients) => SearchElixirs(ingredientList, includeNoIngredients)
                ,searchOption, noIngredientsOption);
        }

        public Task<int> InvokeAsync(string[] args) {
            return _rootCommand.InvokeAsync(args);
        }

        // Returns ordered list of ingredients passed as argument split by ',' instead of space
        // to support passing ingredients with multiple words passed on to the CLI
        // Outputs: 
        //      [ "Cheese", "Scarab beetles", "Onion juice" ]
        //      instead of 
        //      [ "Cheese", ",Scarab ", "beetles", ",Onion ", "juice" ]
        private IEnumerable<string> ParseIngredients(IEnumerable<string> ingredientsList) {
            return string.Join(" ", ingredientsList)        // join all arguments that were splitted by space
                      .Split(',')                           // split the list of ingredients by ','
                      .Select(s => s.TrimStart()
                                    .TrimEnd()
                                    .ToLower())             // remove leading and ending spaces and change to lowercase
                      .Order();                             // order the list of ingredients
        }

        private async Task SearchElixirs(IEnumerable<string> ingredientsToSearch, bool includeNoIngredients) {
            var elixirs = await _datastore.SearchElixirs(ParseIngredients(ingredientsToSearch), includeNoIngredients);

            Console.WriteLine(string.Join(", ", elixirs.Select(e => e.Name)));
        }

    }
}
