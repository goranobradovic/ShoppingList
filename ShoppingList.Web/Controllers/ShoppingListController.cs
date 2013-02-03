using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.WebApi;
using Newtonsoft.Json.Linq;
using ShoppingList.Web.Models;

namespace ShoppingList.Web.Controllers
{
    [JsonFormatter, ODataActionFilter]
    public class ShoppingListController : ApiController
    {
        readonly EFContextProvider<Models.ŞhoppingListDbContext> _contextProvider =
            new EFContextProvider<Models.ŞhoppingListDbContext>();


        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        [HttpGet]
        public IQueryable<Models.ShoppingList> ShoppingLists()
        {
            return _contextProvider.Context.ShoppingList;
        }

        public void SeedData()
        {

            var shoppingList = new Models.ShoppingList()
            {
                Name = "Sample list",
                SecretUrl = "SampleList",
                Active = true,
                Items = new List<Item>()
                                {
                                    new Item()
                                        {
                                            Name = "Eggs",
                                            Amount = 18,
                                            Unit = "piece"
                                        },
                                    new Item()
                                        {
                                            Name = "Milk",
                                            Amount = 8,
                                            Unit = "liter"
                                        },
                                    new Item()
                                        {
                                            Name = "Cheese",
                                            Amount = 0.3M,
                                            Unit = "kg"
                                        }
                                }
            };

            _contextProvider.Context.Set<Models.ShoppingList>().Add(shoppingList);
            _contextProvider.Context.SaveChanges();
        }

    }
}
