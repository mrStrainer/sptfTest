using System;
using System.Net.Security;
using System.Threading;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;


namespace sptfApiLib
{
    public class ApiLib
    {
        static AutorizationCodeAuth auth;
        public static void Authorize()
        {
            //Create the auth object
            auth = new AutorizationCodeAuth()
            {
                //Your client Id
                ClientId = "45a81fcff87243e596ed0ad3916e9080",
                //Set this to localhost if you want to use the built-in HTTP Server
                RedirectUri = "http://localhost",
                //How many permissions we need?
                Scope = Scope.UserReadPrivate,
            };
            //This will be called, if the user cancled/accept the auth-request
            auth.OnResponseReceivedEvent += auth_OnResponseReceivedEvent;
            //a local HTTP Server will be started (Needed for the response)
            auth.StartHttpServer();
            //This will open the spotify auth-page. The user can decline/accept the request
            auth.DoAuth();

            Thread.Sleep(60000);
            auth.StopHttpServer();
            Console.WriteLine("Too long, didnt respond, exiting now...");
        }

        private static void auth_OnResponseReceivedEvent(AutorizationCodeAuthResponse response)
        {

            //ClientSecret.
            
            Token token = auth.ExchangeAuthCode(response.Code, "fbb43c1b8ff842698382999b13f4a0e2");

            var spotify = new SpotifyWebAPI()
            {
                TokenType = token.TokenType,
                AccessToken = token.AccessToken
            };

            //With the token object, you can now make API calls
            Console.WriteLine("Authorize complete, on response event going...");
            //Stop the HTTP Server, done.
            auth.StopHttpServer();
        }
    }
}