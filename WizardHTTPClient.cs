using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elixir_searcher {
    internal class WizardHTTPClient : IWizardClient {
        
        const string DEFAULT_URL = "https://wizard-world-api.herokuapp.com/";
        private readonly string baseUrl;
        private swaggerClient client;

        public WizardHTTPClient() { 
            baseUrl = Environment.GetEnvironmentVariable("ELIXIR_SEARCHER_URL") ?? DEFAULT_URL;
            client = new swaggerClient(baseUrl, new HttpClient());
        }

        public Task<ICollection<ElixirDto>> GetAllElixirsAsync() {
            return client.ElixirsAllAsync(string.Empty, null, string.Empty, string.Empty, string.Empty);
        }
    }
}
