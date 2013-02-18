using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DotNetOpenAuth.AspNet;
using Facebook;
using ShoppingList.Web.Models;
using log4net;

namespace ShoppingList.Web.Helpers
{

    public static class Oauth
    {
        public static ILog Logger = LogManager.GetLogger(typeof(Oauth));
        public struct FieldNames
        {
            public static string AccessToken = "accesstoken";
            public static string Name = "name";
            public static string Gender = "gender";
            public static string Picture = "picture";
        }

        internal static string FbAppId = "193276167373459";
        internal static string FbAppSecret = "d0ab63af612cc52e9dcd5c774deb2b16";

        internal static FacebookClient GetUserFacebookClient()
        {
            var fb = new FacebookClient
                {
                    AppId = FbAppId,
                    AppSecret = FbAppSecret,
                    AccessToken = GetAccessTokenForCurrentUser()
                };

            //fb.SetJsonSerializers(JsonConvert.SerializeObject,
            //                      JsonConvert.DeserializeObject);

            return fb;
        }

        internal static FacebookClient GetAppFacebookClient()
        {
            var fb = new FacebookClient
            {
                AppId = FbAppId,
                AppSecret = FbAppSecret
            };

            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = fb.AppId,
                client_secret = fb.AppSecret,
                grant_type = "client_credentials"
            });

            fb.AccessToken = result.access_token;

            //fb.SetJsonSerializers(JsonConvert.SerializeObject,
            //                      JsonConvert.DeserializeObject);

            return fb;
        }
        public static string ReadField(this IDictionary<string, string> extraData, string key)
        {
            return extraData.ContainsKey(key) ? extraData[key] : null;
        }

        public static string GetAccessTokenForCurrentUser()
        {
            var session = HttpContext.Current.Session;
            return (session != null ? session[FieldNames.AccessToken] as string : null) ?? ReadAccessTokenFromDb();
        }

        private static string ReadAccessTokenFromDb()
        {
            var db = new ShoppingListDbContext();
            var user = db.UserProfiles.FirstOrDefault(userProfile => userProfile.UserName.ToLower() == Thread.CurrentPrincipal.Identity.Name.ToLower());
            return user != null ? user.AccessToken : null;
        }

        public static Task UpdateUserAsync(AuthenticationResult result)
        {

            var fb = GetUserFacebookClient();
            return Task.Run(() =>
            {
                Logger.InfoFormat("Performing update of user {0}", result.UserName);
                try
                {
                    using (var db = new ShoppingListDbContext())
                    {
                        UserProfile user =
                            db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == result.UserName.ToLower());
                        if (user != null)
                        {
                            dynamic picture = fb.Get("me", new { fields = "picture", type = "square" });
                            user.AccessToken = result.ExtraData.ReadField(Oauth.FieldNames.AccessToken);
                            user.Name = result.ExtraData.ReadField(Oauth.FieldNames.Name);
                            user.Gender = result.ExtraData.ReadField(Oauth.FieldNames.Gender);
                            user.PictureUrl = picture.picture.data.url;
                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(String.Format("Update of user {0} failed.", result.UserName), ex);
                }
            });
        }
    }
}