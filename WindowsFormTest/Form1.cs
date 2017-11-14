using SpotifyAPI.Local;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using sptfApiLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormTest
{
    public partial class Form1 : Form
    {
        List<string> URIs;
        private SpotifyWebAPI _spotify;
        private readonly SpotifyLocalAPI _spotifyLocal;
        public Form1()
        {
            Task.Run(() => RunAuthentication());
            _spotifyLocal = new SpotifyLocalAPI();
            Connect();
            InitializeComponent();
            
        }

        private async void InitialSetup()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(InitialSetup));
                return;
            }

            //button1.Enabled = false;
            
        }

        public void Connect()
        {
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                MessageBox.Show(@"Spotify isn't running!");
                return;
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                MessageBox.Show(@"SpotifyWebHelper isn't running!");
                return;
            }

            bool successful = _spotifyLocal.Connect();
            if (successful)
            {
                //connectBtn.Text = @"Connection to Spotify successful";
                //connectBtn.Enabled = false;
                //UpdateInfos();
                _spotifyLocal.ListenForEvents = true;
               // playURI_Song("spotify:track:0Wnzdtso3gHPcSFZwT2Pi9");
            }
            else
            {
                DialogResult res = MessageBox.Show(@"Couldn't connect to the spotify client. Retry?", @"Spotify", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                    Connect();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            URIs = new List<string>();
            string myArtists = null;
            SearchItem searchItem = _spotify.SearchItems(textBox1.Text, SearchType.Track);
            listBox1.Items.Clear();
            List<FullTrack> sList = searchItem.Tracks.Items.ToList();
            foreach (FullTrack ft in sList) {
                List<SimpleArtist> list = ft.Artists;
                myArtists = null;
                foreach (SimpleArtist sa in list)
                {
                    if(myArtists == null) myArtists = sa.Name;
                    else myArtists += ", " + sa.Name;
                }
                System.Diagnostics.Debug.WriteLine(ft.Uri);
                URIs.Add(ft.Uri);
                listBox1.Items.Add(ft.Name + ": " + myArtists);
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           // Task.Run(() => RunAuthentication());
        }

        private async void RunAuthentication()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory(
                 "http://localhost",
                8000,
                "26d287105e31491889f3cd293d85bfea",
                Scope.UserReadPrivate | Scope.UserReadEmail | Scope.PlaylistReadPrivate | Scope.UserLibraryRead |
                Scope.UserReadPrivate | Scope.UserFollowRead | Scope.UserReadBirthdate | Scope.UserTopRead | Scope.PlaylistReadCollaborative |
                Scope.UserReadRecentlyPlayed | Scope.UserReadPlaybackState | Scope.UserModifyPlaybackState | Scope.PlaylistModifyPublic);

            try
            {
                _spotify = await webApiFactory.GetWebApi();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (_spotify == null)
                return;

            InitialSetup();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void playURI_Song(String uri)
        {
            await _spotifyLocal.PlayURL(uri);
        }
        private void ListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string myURI = URIs[listBox1.SelectedIndex];
                System.Diagnostics.Debug.WriteLine(myURI);
                playURI_Song(myURI);
            }
        }
        private void ListBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string myURI = URIs[listBox1.SelectedIndex];
                System.Diagnostics.Debug.WriteLine(myURI);
                playURI_Song(myURI);
            }
        }
    }
}
