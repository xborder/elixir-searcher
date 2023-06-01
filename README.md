# elixir-searcher

CLI Tool that uses The Wizard World (API)[https://wizard-world-api.herokuapp.com/swagger/index.html] to fetch all elixirs and search which elixirs can be conjured by providing a list of ingredients.

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
elixir-searcher.exe --search jewelweed, bicorn horn, mandrake root

```