using System;
using System.Linq;
using System.Net.Mime;
using System.Web.Mvc;
using Microsoft.Web.WebPages.OAuth;
using Newtonsoft.Json;
using ShoppingList.Web.Helpers;
using ShoppingList.Web.Models;

namespace ShoppingList.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(string request_ids)
        {
            return View();
        }

        [Authorize]
        public ActionResult GetFbData(string id = "me")
        {
            var fb = Oauth.GetUserFacebookClient();

            var test = fb.Get(id);
            return Content(JsonConvert.SerializeObject(test), "application/json");
            
            return Json(test, JsonRequestBehavior.AllowGet);

        }
    }
}