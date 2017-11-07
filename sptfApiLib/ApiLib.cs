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
using SpotifyAPI.Local.Models;


namespace sptfApiLib
{
    public class ApiLib
    {
        static AutorizationCodeAuth auth;
        public static SpotifyWebAPI _spotify;

        private static PrivateProfile _profile;
        private static List<SimplePlaylist> _playlists;
        public static Paging<PlaylistTrack> _aPlaylistsTracks;
        private static FullPlaylist newPlaylist;
        public static async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                "http://localhost",
                8000,
                "26d287105e31491889f3cd293d85bfea",
                Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState| Scope.PlaylistModifyPublic);

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
            //getting the user profile
            _profile = await _spotify.GetPrivateProfileAsync();
            Console.WriteLine("Profile:");
            Console.WriteLine($"Name: { _profile.Id }" );
            Console.WriteLine($"Email: { _profile.Email } ");
            Console.WriteLine($"Birth-date: { _profile.Birthdate }");
            Console.WriteLine("--------------------------------");
            //getting the user playlists
            _playlists = GetPlaylists();
            Console.WriteLine($"Number of Playlists: { _playlists.Count } \n");
            var k = 1;
            _playlists.ForEach(playlist =>
            {
                var trackList = GetTracks(playlist.Id);
                Console.WriteLine($"{ k }: {playlist.Name} - {trackList.Count}");
                //getting the tracks in each playlist
                trackList.ForEach(track =>
                {
                    Console.Write("\t");
                    track.Track.Artists.ForEach(artist => Console.Write(artist.Name+" "));
                    Console.Write($"- {track.Track.Name} \n");
                });
                k++;
            });
            //search track -> show result list
            //TODO
            //other search results show
            //select result
            //add result to new playlist or continue search
            List<FullTrack> searchResulTracks = searchTrack("King kunta");
            searchResulTracks.ForEach(track =>
            {
                Console.Write("\t");
                
                track.Artists.ForEach(artist => Console.Write(artist.Name+" "));
                Console.Write($"- {track.Name} \n");
            });
            //to create a new playlist
            //newPlaylist = CreatePlaylist("third playlist");
            //newPlaylist = await _spotify.CreatePlaylistAsync(_profile.Id, "Console playlist", true);
            //Console.WriteLine(newPlaylist.Name);
            //_playlists = GetPlaylists();

        }
        // get all the users playlists and put it in a list
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
        // get all the track of a playlist, and put it in a list
        private static List<PlaylistTrack> GetTracks(string Id)
        {
            Paging<PlaylistTrack> tracks = _spotify.GetPlaylistTracks(_profile.Id, Id);
            List<PlaylistTrack> list = tracks.Items.ToList();
            return list;

        }
        // create a playlist with a specified name
        private static FullPlaylist CreatePlaylist(string name)
        {
            return _spotify.CreatePlaylist(_profile.Id, name, true);
        }

        private static void addTrack(FullPlaylist playlist, FullTrack track)
        {
            _spotify.AddPlaylistTrack(_profile.Id, playlist.Id, track.Uri);
        }
        
        // search for a track 
        private static List<FullTrack> searchTrack(string q)
        {//artist - 1,  album - 2,  track - 4, playlist - 8, all - 16
            SearchItem searchItem = _spotify.SearchItems(q, SearchType.Track);
            List<FullTrack> sList = searchItem.Tracks.Items.ToList();
            return sList;
        }
        // search for artist
        private static List<FullArtist> searchArtist(string q)
        {//artist - 1,  album - 2,  track - 4, playlist - 8, all - 16
            SearchItem searchItem = _spotify.SearchItems(q, SearchType.Artist);
            List<FullArtist> aList = searchItem.Artists.Items.ToList();
            return aList;
        }
        // search for album
        private static List<SimpleAlbum> searchAlbum(string q)
        {//artist - 1,  album - 2,  track - 4, playlist - 8, all - 16
            SearchItem searchItem = _spotify.SearchItems(q, SearchType.Album);
            List<SimpleAlbum> aList = searchItem.Albums.Items.ToList();
            return aList;
        }
        // search for playlist
        private static List<SimplePlaylist> searchPlaylist(string q)
        {//artist - 1,  album - 2,  track - 4, playlist - 8, all - 16
            SearchItem searchItem = _spotify.SearchItems(q, SearchType.Playlist);
            List<SimplePlaylist> pList = searchItem.Playlists.Items.ToList();
            return pList;
        }
    }
}