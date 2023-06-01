using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;

namespace elixir_searcher.Tests {
    [TestFixture]
    public class ElixirTests {

        [Test]
        public void SearchElixirsTest() {
            var i1 = new IngredientDto() {
                Name = "ingredient 1"
            };

            var i2 = new IngredientDto() {
                Name = "ingredient 2"
            };

            var i3 = new IngredientDto() {
                Name = "ingredient 3"
            };

            var e1 = new ElixirDto() {
                Name = "elixir 1",
                Ingredients = new List<IngredientDto>() { i1 }
            };

            var e2 = new ElixirDto() {
                Name = "elixir 2",
                Ingredients = new List<IngredientDto>() { i1, i2, i3 }
            };

            var e3 = new ElixirDto() {
                Name = "elixir 3",
                Ingredients = new List<IngredientDto>() { i2, i3 }
            };

            var e4 = new ElixirDto() {
                Name = "elixir 4",
                Ingredients = new List<IngredientDto>()
            };
            
            var e5 = new ElixirDto() {
                Name = "elixir 5",
                Ingredients = new List<IngredientDto>() { i1, i3 }
            };
            var x = new List<ElixirDto> {
                e1, e2, e3, e4, e5
            };

            var client = Substitute.For<IWizardClient>();
            client.GetAllElixirsAsync().Returns(x);

            var ds = new Datastore(client);

            var res = ds.SearchElixirs(new List<string>() { "ingredient 1" , "ingredient 3"}).Result;
            Assert.AreEqual(2, res.Count());
            Assert.Contains(e1, res.ToList());
            Assert.Contains(e5, res.ToList());

            
            res = ds.SearchElixirs(new List<string>() { "ingredient 1" , "ingredient 3"}, true).Result;
            Assert.AreEqual(3, res.Count());
            Assert.Contains(e1, res.ToList());
            Assert.Contains(e5, res.ToList());
            Assert.Contains(e4, res.ToList());

            res = ds.SearchElixirs(new List<string>() { "ingredient 7"}).Result;
            Assert.IsEmpty(res.ToList());
        }
    }
}
