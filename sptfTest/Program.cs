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

        private static async void GetProfile()
        {
            _profile = await ApiLib._spotify.GetPrivateProfileAsync();
        }

        
        private static List<SimplePlaylist> GetPlaylists()
        {
            Paging<SimplePlaylist> playlists = ApiLib._spotify.GetUserPlaylists(_profile.Id);
            List<SimplePlaylist> list = playlists.Items.ToList();

            while (playlists.Next != null)
            {
                playlists = ApiLib._spotify.GetUserPlaylists(_profile.Id, 20, playlists.Offset + playlists.Limit);
                list.AddRange(playlists.Items);
            }

            return list;
        }
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Sup");
            ApiLib.Authorize();
            if (ApiLib._spotify.AccessToken != null)
            {
                Console.WriteLine("Access token: " + ApiLib._spotify.AccessToken); 
                GetProfile();
                var _playlists = GetPlaylists();
                Console.WriteLine(_playlists.Count);
                
            }
            Console.WriteLine("after");
            Console.ReadLine();
        }
    }
}