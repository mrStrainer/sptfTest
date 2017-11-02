using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using sptfApiLib;
using SpotifyAPI.Web.Models;

namespace sptfTest
{
    internal class Program
    {
        private static PrivateProfile _profile;

       
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Sup");
            ApiLib.RunAuthentication();
            
            if (ApiLib._spotify != null)
            {
                Console.WriteLine("Access token: " + ApiLib._spotify.AccessToken); 
                 
            }
            Console.WriteLine("after");
            Console.ReadLine();
        }
    }
}