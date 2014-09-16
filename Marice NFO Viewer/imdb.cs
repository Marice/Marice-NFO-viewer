using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using System.Runtime.InteropServices;

namespace Marice_NFO_Viewer
{
    public partial class imdb : Form
    {
        movie m = new movie();
        trailer t = new trailer();
        String urltje;
        String urlimdb;
        String trailerurl;
        public imdb(string imdmId)
        {
          
            urltje = "http://www.omdbapi.com/?i=" + imdmId;
            urlimdb = "http://www.imdb.com/title/" + imdmId;
            trailerurl = "http://api.traileraddict.com/?imdb=" + imdmId.Substring(2, 7) + "&count=1&width=640";
            var json = new WebClient().DownloadString(urltje);
            var xml = new WebClient().DownloadString(trailerurl);
            InitializeComponent();
            pictureBox2.Hide();
            if (xml.Length > 160)
            {
                pictureBox2.Show();
                trailerurl = getTrailer(xml);
            }
            getData(json);
        }


        //get mouse drag shizzle
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        private void imdb_Load(object sender, System.EventArgs e)
        {
            this.MouseDown += new MouseEventHandler(imdb_MouseDown);
        }
        void imdb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }



        public void getData(String json)
        { 
            m = JsonConvert.DeserializeObject<movie>(json);
            this.Text = "IMDB: " + m.Title;
            string Poster = m.Poster;
            if (Poster.Length > 10)
              {
                pictureBox1.Load(Poster);
              }
            lgenre.Text = m.Genre;
            lreleased.Text = m.Released;
            lrated.Text = m.Rated;
            lruntime.Text = m.Runtime;
            ldirector.Text = m.Director;
            lvotes.Text = m.imdbVotes;
            llanguage.Text = m.Language;
            lawards.Text = m.Awards;
            plotBox.Text = SpliceText(m.Plot, 64);
            lmetascore.Text = m.Metascore;
            limdbrating.Text = m.imdbRating;
            actorBox.Text = m.Actors;
          }



        public string getTrailer(string xml)
        {
            StringBuilder output = new StringBuilder();
            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                reader.ReadToFollowing("trailer");
                reader.MoveToFirstAttribute();
                string genre = reader.Value;
                reader.ReadToFollowing("link");
                output.AppendLine(reader.ReadElementContentAsString());
            }
            return output.ToString();
        }

        public static string SpliceText(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
        }
        


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(urlimdb);
        }

        private void label10_Click(object sender, System.EventArgs e)
        {

        }

        private void actorBox_TextChanged(object sender, System.EventArgs e)
        {

        }

      

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(urlimdb);
        }

        


        private void closeLabel_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void pictureBox3_Click(object sender, System.EventArgs e)
        {

        }

        private void trailerButton_Click(object sender, System.EventArgs e)
        {
     
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(trailerurl);
        }

    }
}
