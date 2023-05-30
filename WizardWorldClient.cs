using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elixir_searcher {
    internal class WizardWorldClient {
        private readonly string baseUrl;
        private swaggerClient client;
        public WizardWorldClient() {
            client = new swaggerClient("https://wizard-world-api.herokuapp.com/", new HttpClient());
        }

        public Task<ICollection<ElixirDto>> GetAllElixirsAsync() { 
            return client.ElixirsAllAsync(string.Empty, null, string.Empty, string.Empty, string.Empty);
        }
    }
}
