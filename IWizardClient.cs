using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elixir_searcher {
    public interface IWizardClient {
        Task<ICollection<ElixirDto>> GetAllElixirsAsync();
    }
}
