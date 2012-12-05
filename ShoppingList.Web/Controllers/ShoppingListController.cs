using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Breeze.WebApi;
using Newtonsoft.Json.Linq;

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

    }
}
