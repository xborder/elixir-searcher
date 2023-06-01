# elixir-searcher

CLI Tool that uses The Wizard World [API](https://wizard-world-api.herokuapp.com/swagger/index.html) to fetch all elixirs and search which elixirs can be conjured by providing a list of ingredients.

To build:
```
dotnet build
```
To run tests:
```
dotnet test
```
Usage:
```
elixir-searcher.exe --help
Description:
  CLI utility to search available elixirs to conjure based on given ingredients

Usage:
  elixir-searcher [options]

Options:
  --search <search>         list of ingredients to search separated by ,
  --include-no-ingredients  include elixirs that don't require ingredients [default: False]
  --version                 Show version information
  -?, -h, --help            Show help and usage information
```

Example:
```
elixir-searcher.exe --search jewelweed, bicorn horn, mandrake root, cheese

```

# Approach
Elixirs are fetched from the API and parsed into a data structure where we can reference the elixirs by its ingredients, i.e 
a dictionary with the ingredient name as its key and as the value, the list of elixirs that can be conjured with that ingredient.

Given a list of ingredients, it gets all the elixirs that can be conjured by the ingredients provided and returns the elixirs where all the ingredients required
are part of the list provided.

`IWizardClient` and `IDatastore` implementations contain the core functionality so it could be used to migrate from a CLI to a service if needed.

Uses [Microsoft.dotnet-openapi](https://learn.microsoft.com/en-us/aspnet/core/web-api/microsoft.dotnet-openapi?view=aspnetcore-6.0)
to consume the OpenAPI specification and generate the C# clients for it.

Uses [System.CommandLine](https://learn.microsoft.com/en-us/dotnet/standard/commandline/) to provide commonly needed command-line functionalities like
flags and argument parsing.

# Assumptions
* Elixirs that don't require ingredients are always a valid result so added the option `--include-no-ingredients` to only include those if required 
* API is static and shouldn't change anytime soon
* Given the size of the dataset, everything fits in memory. Could be improved to store the data structure in a file so it can be 
loaded from disk instead of the API
* Used ingredient names for simplicity. Ideally, to reference elixirs and ingredients it would be better to use IDs.
* Elixirs [API](https://wizard-world-api.herokuapp.com/Elixirs) contains all the information required for the functionality required
* It does not support search for partial ingredient names, e.g search for `Moth` instead of `Flitterby Moth` <!-- case in point Stewed Mandrake is not the same as Mandrake Root -->
* Not needed to use service container