using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;
using System.Timers;

namespace Marice_NFO_Viewer
{
    public partial class Form1 : Form
    {
        List<string> lijst = new List<string>();
        String imdbId;
        int lineCounter = 38;
        int LineTel = 1;
        Boolean loaded = false;
        string ansi;
        Bitmap bmp;
        
        




        public Form1()
        {
            InitializeComponent();
             pictureBox1.Hide();
           
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            
        }


        public static string parse(string strSource)
        {
            int Start, End;
            if (strSource.Contains("imdb.com") && strSource.Contains("\n"))
            {
                Start = strSource.IndexOf("imdb.com", 0) + "imdb.com".Length + 7;
                End = strSource.IndexOf("\n", Start + 3);
                return strSource.Substring(Start, 9);
            }
            else
            {
                return "";
            }
        }


        // TODO Fix dit, want sommige nfos renderen tab achtig 
        public void setTxt(String nfo)
        {
            string t1 = nfo.Replace("\n", Environment.NewLine);
            string t2 = t1.Replace("\t", "");
            nfobox.Text = t2;
            ansi = nfobox.Text;
            
        }



     
       
      

  


        private void nfobox_TextChanged(object sender, EventArgs e)
        {

        }


        private void IMDBbutton_Click(object sender, EventArgs e)
        {

        }

     

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (loaded == true)
            {
               
                nfobox.HideSelection = true;
                nfobox.SelectionStart = nfobox.GetFirstCharIndexFromLine(lineCounter - 1);
                nfobox.SelectionLength = nfobox.Lines[lineCounter - 1].Length;
                nfobox.ScrollToCaret();
               
                lineCounter++;
                //   nfobox.Update();
                //   nfobox.Refresh();
                if (lineCounter > LineTel)
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                    loaded = false;
                  
                    lineCounter = 1;
                    return;
                }
            }
        }

        private void scrollButton_Click(object sender, EventArgs e)
        {
    
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }






        private Bitmap plaatje(string sImageText)
        {
            Bitmap objBmpImage = new Bitmap(1, 1);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            Font objFont = new Font("ASCII", 9, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;
            int hoog = nfobox.Lines.Length + 1;
            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.White);
            objGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
            objGraphics.Flush();

            return (objBmpImage);
        }



        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        m.Result = (IntPtr)HTCAPTION;
                    }

                    return;
            }

            base.WndProc(ref m);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void closeLabel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "iNFO Files (.nfo)|*.nfo";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = true;
            String nfo;
            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Hide();
                nfo = File.ReadAllText(openFileDialog1.FileName, Encoding.GetEncoding(437));
                imdbId = parse(nfo);
                if (imdbId.Length > 2)
                {
                    pictureBox1.Show();
                }
                lineCounter = 38;
                setTxt(nfo);
                LineTel = nfo.Count(f => f == '\n') + 1;
               
            }
        }

        private void optionsMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toggleAutoscrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nfobox.Lines.Length < 39)
            {
                return;

            }

            if (loaded == false)
            {

                timer1.Enabled = true;
                timer1.Start();
                loaded = true;
            }
            else
            {
                timer1.Stop();
                timer1.Enabled = false;
                loaded = false;
            }
        }

        private void saveNFOAsJpgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ansi == null)
            {
                MessageBox.Show("No nfo loaded");
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Jpeg Image|*.jpg";
            saveFileDialog1.Title = "Save Jpeg File";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName == null)
            {
                return;
            }
            plaatje(ansi).Save(saveFileDialog1.FileName, ImageFormat.Jpeg); 
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var im = new imdb(imdbId);
            im.Show();
        }
        

   
      


    }
}
