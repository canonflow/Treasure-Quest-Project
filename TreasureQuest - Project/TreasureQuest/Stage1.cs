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
using System.Runtime.InteropServices;

namespace TreasureQuest
{
    public partial class Stage1 : Form
    {
        #region PROPERTIES
        Player user;
        private SoundPlayer _music;
        List<string> lifesImg = new List<string> { "", "💜", "💜💜", "💜💜💜" };

        // Variabel konstanta
        const int FORCE = 6;
        const int GRAVITY = 10;
        const int PLAYERSPEED = 5;
        const int INTERVALSHIP = 15;
        const int POTIONINTERVAL = 15;

        bool left, right, pause, jump, hasMap;
        int gravity = GRAVITY;
        int force = FORCE;
        int playerSpeed = PLAYERSPEED;
        int score = 0;
        int lifes = 3;
        string direction = "right";

        // !Ship
        int intervalShip = INTERVALSHIP;
        string shipDirection = "down";
        bool mapEffect = false;
        int mapEffectInterval = 10;

        // !Red Potion
        bool potionEffect = false;
        int potionEffectInterval = POTIONINTERVAL;
        
        #endregion

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            player.Top += gravity;
            lblForce.Text = "Force: " + force;

            #region OBJECTS MOVEMENT
            if (cloud1.Right > 0)
            {
                cloud1.Left -= 1;
            }
            else
            {
                cloud1.Left = this.ClientSize.Width;
            }

            if (cloud2.Right > 0)
            {
                cloud2.Left -= 3;
            }
            else
            {
                cloud2.Left = this.ClientSize.Width;
            }

            if (cloud3.Right > 0)
            {
                cloud3.Left -= 5;
            }
            else
            {
                cloud3.Left = this.ClientSize.Width;
            }

            if (intervalShip > 0)
            {
                if (shipDirection == "down")
                {
                    ship.Top++;
                    sail.Top++;
                }
                else
                {
                    ship.Top--;
                    sail.Top--;
                }
                intervalShip--;
            }
            else
            {
                if (shipDirection == "down")
                {
                    shipDirection = "up";
                }
                else
                {
                    shipDirection = "down";
                }
                intervalShip = INTERVALSHIP;
            }
            #endregion

            /*----------------------- CONTROLS ---------------------------*/
            #region PLAYER CONTROLS

            if (left && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }

            if (right && player.Right < this.ClientSize.Width)
            {
                player.Left += playerSpeed;
            }

            
            if (jump && force < 0)
            {
                jump = false;
            }

            if (jump)
            {
                gravity = -1 * GRAVITY;
                force--;
            }
            else
            {
                gravity = GRAVITY;
            }
            #endregion

            /*------------------- Collision Objects ---------------------*/
            #region DETECTING COLLISION
            // TODO: Detecting
            foreach (Control x in this.Controls)
            {
                // !Platform
                if (x is PictureBox && (string)x.Tag == "platform")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }
                }  // !Platform

                // !Ceil
                if (x is PictureBox && (string)x.Tag == "ceil")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Top = x.Height;
                    }
                }  // !Ceil

                // !Flying
                if (x is PictureBox && (string)x.Tag == "flying")
                {
                    // atas <diam || lompat>
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false && player.Top < x.Top)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }

                    // samping
                    else if (player.Bounds.IntersectsWith(x.Bounds) && (player.Left < x.Left || player.Right > x.Right) && player.Top > x.Top)
                    {
                        // kiri
                        if (player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }

                        // kanan
                        else if (player.Left > x.Left)
                        {
                            player.Left = x.Left + x.Width;
                        }
                    }

                    // bawah <lompat>
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top)
                    {
                        if (player.Left + player.Width > x.Left && player.Right < x.Right)
                        {
                            player.Top = x.Top + x.Height;
                            force = -1;
                        }
                    }
                    /* !(BUG) <menembus dari kanan>
                    // samping kanan kiri
                    else if (player.Bounds.IntersectsWith(x.Bounds) && jump == false)
                    {
                        // dari kiri mau ke kenan
                        if (player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }
                        // dari kanan mau ke kiri
                        else if (player.Left > x.Right)
                        {
                            player.Left = x.Left + x.Width;
                        }
                    }
                    */

                    
                }  // !Flying

                // !Wall
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    // samping
                    /*
                       ! player.Top + (player.Height/2) agar saat player loncat tidak menembus wall
                       */
                    if (player.Bounds.IntersectsWith(x.Bounds) && player.Top + (player.Height / 2) > x.Top)
                    {
                        if (right && player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;

                        }

                        if (left && player.Left > x.Left)
                        {
                            player.Left = x.Left + x.Width;
                        }
                    }

                    // atas
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false && player.Top < x.Top)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }
                }  // !Wall

                // !Coin
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {
                        x.Visible = false;
                        score++;
                    }
                }  // !Coin

                // !Map
                if (x is PictureBox && (string)x.Tag == "map")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible && mapEffect == false)
                    {
                        hasMap = true;
                        mapEffect = true;
                        // x.Visible = false;
                        map.Image = Properties.Resources.mapEffect;
                    }
                }  // !Map

                // !Spike
                if (x is PictureBox && (string)x.Tag == "spike")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (lifes > 1)
                        {
                            if (direction == "right")
                            {
                                player.Left -= 35;
                            }
                            else
                            {
                                player.Left += 35;
                            }
                        }
                        lifes--;
                    }
                }

                // !Water
                if (x is PictureBox && (string)x.Tag == "water")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        lifes = 0;
                        player.Image = Properties.Resources.pirateHunterDead;
                    }
                }

                // !Ship
                if (x is PictureBox && (string)x.Tag == "ship")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false && player.Top < x.Top)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }
                }  // !Ship

            }  // !Detecting
            #endregion

            // ------------------- ADDITIONAL --------------------
            #region ITEMS APPEAR AND DISAPPEAR
            /*
             !Jika user belum memiliki item "Map" dan score sudah 21 (user telah mengumpulkan 21 coin)
             maka item "Map" akan muncul.
             */
            // !Map
            if (user.Items["Map"] == false && score == 21 && hasMap == false)
            {
                map.Visible = true;
            }

            // !Red Potion
            if (player.Bounds.IntersectsWith(redPotion.Bounds) && redPotion.Visible && potionEffect == false)
            {
                if (lifes < 3)
                {
                    lifes++;
                }
                redPotion.Top -= 40;
                redPotion.Image = Properties.Resources.potionEffect;
                potionEffect = true;
                // redPotion.Visible = false;
            }
            #endregion

            #region LABEL
            /*-------------- LABEL ------------------*/
            lblLifes.Text = "Lifes: " + lifesImg[lifes];
            lblScore.Text = "Score: " + score;
            #endregion

            #region ANIMATION
            // ANIMATION
            // !Map
            if (mapEffectInterval > 0 && mapEffect)
            {
                mapEffectInterval--;
            }

            if (mapEffectInterval == 0)
            {
                mapEffect = false;
                map.Visible = false;
            }

            // !Red Potion
            // animasi
            if (potionEffect && potionEffectInterval > 0)
            {
                potionEffectInterval--;
            }
            // jika animasi selesai
            if (potionEffectInterval == 0)
            {
                potionEffect = false;
                redPotion.Visible = false;
            }
            #endregion

            #region GAME END
            // ------------------- Gk tau mau dibilang apa -----------------
            // TODO: Ketika nyawa sudah habis.
            if (lifes == 0)
            {
                player.Image = Properties.Resources.pirateHunterDead;
                Game.Stop();
                MessageBox.Show("Kamu mati");
                Restart();
            }
            
            // TODO: Ketika player telah mencapai flag
            if (player.Bounds.IntersectsWith(flag.Bounds))
            {
                if (user.Stages.Count == 1)
                {
                    user.Stages.Add(user.Stages.Count + 1);
                }
                Game.Stop();
                Stage2 stage2 = new Stage2();
                if (hasMap)
                {
                    user.Items["Map"] = true;
                }
                stage2.Init(user);
                MessageBox.Show("Selamat anda telah melewati stage 1");
                this.Hide();
                stage2.ShowDialog();
                this.Close();
            }
            #endregion
        }
        #endregion

        #region GAME KEY DOWN

        // !Ketika menekan tombol
        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            // Menghilangkan teks petunjuk saat user menekan tombol apapun
            if (lblPetunjuk.Visible)
            {
                lblPetunjuk.Visible = false;
            }

            if (e.KeyCode == Keys.Left)
            {
                // mengubah direction
                if (direction != "left")
                {
                    direction = "left";
                }
                // mengubah player image menajadi "run" sesuai direction
                player.Image = Properties.Resources.pirateHuntersRunMirror;
                left = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                // mengubah direction
                if (direction != "right")
                {
                    direction = "right";
                }
                // mengubah player image menajadi "run" sesuai direction
                player.Image = Properties.Resources.pirateHuntersRun;
                right = true;
            }

            if ( (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && jump == false)
            {
                jump = true;
            }

            // Pause
            // TODO: Ketika user menekan tombol "Esc" maka game akan otomatis ter-pause.
            if (e.KeyCode == Keys.Escape && pause == false)
            {
                pause = true;
                string message = "Mau lanjut apa nggak nichhh";
                string title = "Paused";
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                DialogResult res = MessageBox.Show(message, title, button, MessageBoxIcon.Question);

                // !Jika user tidak ingin melanjutkan permainan, maka akan kembali ke halaman "Stages".
                if (res == DialogResult.No)
                {
                    Stages st = new Stages();
                    st.Init(user);
                    this.Hide();
                    _music.Stop();
                    st.ShowDialog();
                    this.Close();
                }
                else
                {
                    pause = false;
                }
            }
        }
        #endregion

        #region GAME KEY UP
        // !Ketika melepas tombol
        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                left = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                right = false;
            }

            // mengubah player image menjadi "Idle" sesuai direction
            if (direction == "right")
            {
                player.Image = Properties.Resources.pirateHuntersIdleCrop;
            }
            else
            {
                player.Image = Properties.Resources.pirateHuntersIdleMirrorFix;
            }
        }
        #endregion

        #region STAGE1 LOAD
        private void Stage1_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/Stage1.wav");
            _music.PlayLooping();
        }
        #endregion

        public Stage1()
        {
            InitializeComponent();
        }

        #region INITIALIZE PLAYER
        public void Init(Player p)
        {
            user = p;
        }
        #endregion

        #region RESTART GAME
        private void Restart()
        {
            Stage1 newGame = new Stage1();
            newGame.Init(user);
            this.Hide();
            newGame.ShowDialog();
            this.Close();
        }
        #endregion
    }
    
}
