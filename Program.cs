// See https://aka.ms/new-console-template for more information
using elixir_searcher;

var elixirSearch = new ElixirSearch(new Datastore(new WizardHTTPClient()));

return await elixirSearch.InvokeAsync(args);