using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elixir_searcher {
    internal interface IDatastore {

        Task Initialize();
        Task<IEnumerable<ElixirDto>> SearchElixirs(IEnumerable<string> ingredientsToSearch, bool includeNoIngredients = false);
    }
}
