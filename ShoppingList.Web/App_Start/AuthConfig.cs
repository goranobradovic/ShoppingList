using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using ShoppingList.Web.Models;
using WebMatrix.WebData;

namespace ShoppingList.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "193276167373459",
                appSecret: "d0ab63af612cc52e9dcd5c774deb2b16");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
