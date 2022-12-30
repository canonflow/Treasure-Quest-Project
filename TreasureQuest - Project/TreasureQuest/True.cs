using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace TreasureQuest
{
    public partial class True : Form
    {
        #region PROPERTIES
        private SoundPlayer _music;
        const int INTERVAL_ANIMATION = 15;
        const int TYPE_EFFECT_INTERVAL = 12;

        // game
        int intervalAnimation = 15;
        string direction = "down";

        // Label
        string text = "HAHHAHA akhirnya aku menemukan harta karun yang aku cari - cari\nselama 15 tahun ini....";
        int cnt = 0;
        bool isFinished = false;  // Untuk teks
        bool typeEffect = false;
        int typeEffectInterval = TYPE_EFFECT_INTERVAL;

        
        #endregion
        public True()
        {
            InitializeComponent();
        }

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            if (intervalAnimation > 0)
            {
                if (direction == "down")
                {
                    ship.Top--;
                    sail.Top--;
                    player.Top--;
                    chest.Top--;
                }
                else
                {
                    ship.Top++;
                    sail.Top++;
                    player.Top++;
                    chest.Top++;
                }
                intervalAnimation--;
            }
            else
            {
                if (direction == "down")
                {
                    direction = "up";
                }
                else
                {
                    direction = "down";
                }
                intervalAnimation = INTERVAL_ANIMATION;
            }
        }
        #endregion

        #region LABEL TIMER
        private void LabelTimer(object sender, EventArgs e)
        {
            if (cnt == text.Length)
            {
                isFinished = true;
                typeEffect = true;
            }

            if (isFinished == false)
            {
                lblDisplay.Text = lblDisplay.Text.Substring(0, lblDisplay.Text.Length - 1) + text[cnt] + "|";
                cnt++;
            }

            if (typeEffect && typeEffectInterval > 0)
            {
                typeEffectInterval--;
            }

            if (typeEffectInterval == 0)
            {
                if (lblDisplay.Text.Contains("|"))
                {
                    lblDisplay.Text = lblDisplay.Text.Substring(0, lblDisplay.Text.Length - 1);
                }
                else
                {
                    lblDisplay.Text += "|";
                }
                typeEffectInterval = TYPE_EFFECT_INTERVAL;
            }
        }
        #endregion

        #region GAME KEY DOWN
        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            
            if (isFinished)
            {
                Player user = new Player();
                Home home = new Home();
                home.Init(user);
                this.Hide();
                _music.Stop();
                home.ShowDialog();
                this.Close();
            }
            
        }
        #endregion  

        #region TRUE LOAD
        private void True_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/True.wav");
            _music.PlayLooping();
        }
        #endregion
    }
}
