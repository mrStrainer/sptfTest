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
        public static void Main(string[] args)
        {
            ApiLib.RunAuthentication();
            
            /*if (ApiLib._spotify != null)
            {
                Console.WriteLine("Auth success."); 
            }*/
            Console.ReadLine();
        }
    }
}