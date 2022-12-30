using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace TreasureQuest
{
    public partial class Home : Form
    {
        private SoundPlayer _music;
        Player user;
        public Home()
        {
            InitializeComponent();

            // !Default
            user = new Player();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // !Menaampilkan form baru dan mematikan music
            Stages stages = new Stages();
            stages.Init(user);
            this.Hide();
            _music.Stop();
            stages.ShowDialog();
            this.Close();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/PiratesDominions.wav");
            _music.PlayLooping();
        }

        public void Init(Player p)
        {
            user = p;
        }
        private void btnAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            string message = "Apa iyahhhh???";
            string title = "Yang beneerrrrr";
            MessageBoxButtons button = MessageBoxButtons.YesNo;  // !Generate YesNo Button for MessageBox

            // Menampilkan MessageBox dan menampung pilihan user ke variabel "res"
            DialogResult res = MessageBox.Show(message, title, button, MessageBoxIcon.Question);

            /*
             MessageBox.Show(pesannya, judul, tombol, icon)
             */
            if (res == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // !Ketika tombol "Start" dihover maka teks akan berubah menjadi merah, sebaliknya putih
        private void StartHover(object sender, EventArgs e)
        {
            btnStart.BackgroundImage = Properties.Resources.STARTstartFixRed;
        }

        private void StartLeave(object sender, EventArgs e)
        {
            btnStart.BackgroundImage = Properties.Resources.STARTstartFix;
        }

        // !Ketika tombol "About" dihover maka teks akan berubah menjadi merah, sebaliknya putih
        private void AboutHover(object sender, EventArgs e)
        {
            btnAbout.BackgroundImage = Properties.Resources.ABOUTaboutRed;
        }

        private void AboutLeave(object sender, EventArgs e)
        {
            btnAbout.BackgroundImage = Properties.Resources.ABOUTabout;
        }

        // !Ketika tombol "Exit" dihover maka teks akan berubah menjadi merah, sebaliknya putih
        private void ExitHover(object sender, EventArgs e)
        {
            btnExit.BackgroundImage = Properties.Resources.EXITexitRed;
        }

        private void ExitLeave(object sender, EventArgs e)
        {
            btnExit.BackgroundImage = Properties.Resources.EXITexit;
        }

    }
}
