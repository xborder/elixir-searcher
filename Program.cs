// See https://aka.ms/new-console-template for more information
using System.CommandLine;
using elixir_searcher;

var options = new Option<IEnumerable<string>>(name: "--ingredients", description: "") {
    AllowMultipleArgumentsPerToken = true
};
options.AddAlias("-i");

var rootCommand = new RootCommand("Sample app for System.CommandLine");
rootCommand.AddOption(options);

rootCommand.SetHandler((args) => {
    var ingredients = string.Join(" ", args).Split(',').Select(s => s.TrimStart().ToLower());
    SearchElixirs(ingredients);
}, options);

return await rootCommand.InvokeAsync(args);

const String NO_INGREDIENTS = "NO_INGREDIENTS";

static void SearchElixirs(IEnumerable<string> ingredients) {

    var client = new WizardWorldClient();
    // ingredient id to dictionary<elixir id, elixir details>
    var ingredientsList = new Dictionary<Guid, HashSet<ElixirDto>>();
    var ingredientsName = new Dictionary<string, Guid>();
    var results = client.GetAllElixirsAsync().Result;
    
    
    foreach (var elixir in results) {
        foreach (var ingredient in elixir.Ingredients) {

            if (!ingredientsList.ContainsKey(ingredient.Id)) {
                ingredientsList.Add(ingredient.Id, new HashSet<ElixirDto>());
                ingredientsName.Add(ingredient.Name.ToLower(), ingredient.Id);
            }

            var elixirDict = ingredientsList[ingredient.Id];

            if (!elixirDict.Contains(elixir)) {
                elixirDict.Add(elixir);
            }
        }
    }
    
    Console.WriteLine("Ingredients: " + string.Join(",", ingredientsName.OrderBy(i => i.Key).Select(kvp => kvp.Key)));
    Console.WriteLine("Elixirs: " + string.Join(",", results.OrderBy(i => i.Name).Select(kvp => kvp.Name)));

    var result = new HashSet<ElixirDto>();

    foreach (var ingredient in ingredients) {
        if (ingredientsName.ContainsKey(ingredient)) {
            var guid = ingredientsName[ingredient];
            var hs = ingredientsList[guid];

            result = new HashSet<ElixirDto>(result.Union(hs));
        }
    }


    Console.WriteLine(string.Join(", ",result.Select(e => e.Name)));
}