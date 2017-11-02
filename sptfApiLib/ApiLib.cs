using System;
using System.Threading;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Diagnostics;


namespace sptfApiLib
{
    public class ApiLib
    {
        static AutorizationCodeAuth auth;
        public static SpotifyWebAPI _spotify;

        public static PrivateProfile _profile;
        public static List<SimplePlaylist> _playlists;
        public static Paging<PlaylistTrack> _aPlaylistsTracks;
        public static async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost",
                8000,
                "26d287105e31491889f3cd293d85bfea",
                Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState);

            try
            {
                _spotify = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
            }

            if (_spotify == null)
                return;

            InitialSetup();
        }

        private static async void InitialSetup()
        {
            _profile = await _spotify.GetPrivateProfileAsync();
            Console.WriteLine($"Name: { _profile.Id }" );
            Console.WriteLine($"Email: { _profile.Email } ");
            Console.WriteLine($"Birth-date: { _profile.Birthdate }");
            _playlists = GetPlaylists();
            Console.WriteLine($"Number of Playlists: { _playlists.Count }");
            _playlists.ForEach(playlist =>
            {
                Console.WriteLine($"{playlist.Id}: {playlist.Name}");
                var trackList = GetTracks(playlist.Id);
                trackList.ForEach(track =>
                {
                    track.Track.Artists.ForEach(artist => Console.Write(artist.Name));
                    Console.Write($" - {track.Track.Name}");
                });
            });
            
            
        }
        
        private static List<SimplePlaylist> GetPlaylists()
        {
            Paging<SimplePlaylist> playlists = _spotify.GetUserPlaylists(_profile.Id);
            List<SimplePlaylist> list = playlists.Items.ToList();

            while (playlists.Next != null)
            {
                playlists = _spotify.GetUserPlaylists(_profile.Id, 20, playlists.Offset + playlists.Limit);
                list.AddRange(playlists.Items);
            }

            return list;
        }

        private static List<PlaylistTrack> GetTracks(string Id)
        {
            Paging<PlaylistTrack> tracks = _spotify.GetPlaylistTracks(_profile.Id, Id);
            List<PlaylistTrack> list = tracks.Items.ToList();
            return list;

        }
        
    }
}