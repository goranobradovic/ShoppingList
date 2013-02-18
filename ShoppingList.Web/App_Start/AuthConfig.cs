using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using ShoppingList.Web.Helpers;
using ShoppingList.Web.Models;
using WebMatrix.WebData;

namespace ShoppingList.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            var db = new ShoppingListDbContext();
            db.Database.Initialize(false);

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", typeof(UserProfile).Name, "UserId", "UserName", autoCreateTables: true);
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: Oauth.FbAppId,
                appSecret: Oauth.FbAppSecret);

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
