using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreasureQuest
{
    public partial class Stage2 : Form
    {
        #region PROPERTIES
        Player user;
        private SoundPlayer _music;
        List<string> lifesImg = new List<string> { "", "💜", "💜💜", "💜💜💜" };

        // Variabel konstanta
        const int FORCE = 6;
        const int GRAVITY = 9;
        const int PLAYERSPEED = 5;
        const int INTERVALSHIP = 15;
        const int INTERVALBOMB = 70;
        const int INTERVALEXPLODE = 20;
        const int DIAMONDEFFECTINTERVAL = 15;
        const int POTIONINTERVAL = 15;

        bool left, right, pause, jump, hasDiamond;
        int gravity = GRAVITY;
        int force = FORCE;
        int playerSpeed = PLAYERSPEED;
        int score = 0;
        int lifes = 3;
        string direction = "right";

        // !Ship
        int intervalShip = INTERVALSHIP;
        string shipDirection = "down";

        // ! Bomb
        int intervalBomb = INTERVALBOMB;
        string bombDirection = "left";
        int intervalExplode = INTERVALEXPLODE;
        bool explode = false;

        // !Diamond
        bool diamondEffect = false;
        int diamontEffectInterval = DIAMONDEFFECTINTERVAL;

        // !Red Potion
        bool potionEffect = false;
        int potionEffectInterval = POTIONINTERVAL;
        #endregion

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            player.Top += gravity;
            lblForce.Text = "Force: " + force;

            // ----------------- MOVEMENT ------------------

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

            // !Ship
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
                    shipDirection= "up";
                }
                else
                {
                    shipDirection = "down";
                }
                intervalShip = INTERVALSHIP;
            }

            // !Bomb 
            /*
                Kalo nggam meledak, BOMN-nya bakalan gerak
              */
            if (explode == false)
            {
                if (intervalBomb > 0)
                {
                    if (bombDirection == "left")
                    {
                        bomb.Left--;
                    }
                    else
                    {
                        bomb.Left++;
                    }
                    intervalBomb--;
                }
                else
                {
                    if (bombDirection == "left")
                    {
                        bombDirection = "right";
                    }
                    else
                    {
                        bombDirection = "left";
                    }
                    intervalBomb = INTERVALBOMB;
                }
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
                    // atas
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

                    // bawah
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top)
                    {
                        if (player.Left >= x.Left && player.Right <= x.Right)
                        { 
                            player.Top = x.Top + x.Height;
                            force = -1;
                        }
                    }  
                }  // !Flying

                // !Wall
                if (x is PictureBox && (string)x.Tag == "wall")
                {
                    // atas
                    if (player.Bounds.IntersectsWith(x.Bounds) && player.Top < x.Top && jump == false)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }

                    // samping
                    else if (player.Bounds.IntersectsWith(x.Bounds) && (player.Top) > x.Top)
                    {
                        // dari kiri
                        if (left && player.Left > x.Left)
                        {
                            player.Left = x.Left + x.Width;
                        }

                        // dari kanan
                        else if (right && player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }
                    }
                }  // !Wall

                // !Coim
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {
                        score++;
                        x.Visible = false;
                    }
                }

                // !Water
                if (x is PictureBox && (string)x.Tag == "water")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        lifes = 0;
                        lblLifes.Text = "Lifes: " + lifesImg[lifes];
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

                // !Blue Diamond
                if (x is PictureBox && (string)x.Tag == "blueDiamond")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && blueDiamond.Visible && diamondEffect == false)
                    {
                        hasDiamond = true;
                        diamondEffect = true;
                        blueDiamond.Image = Properties.Resources.diamondEffect;
                    }
                }  // !Blue Diamond

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

                // !Bomb
                if (x is PictureBox && (string)x.Tag == "bomb")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible && explode == false)
                    {
                        explode = true;
                        bomb.Top -= 40;
                        bomb.Left -= 30;
                        bomb.Image = Properties.Resources.bombExplotion; 
                    }
                }  //  !Bomb
            }  // !Detecting
            #endregion

            #region LABEL
            /*-------------- LABEL ------------------*/
            lblLifes.Text = "Lifes: " + lifesImg[lifes];
            lblScore.Text = "Score: " + score;
            #endregion

            // -------------------- ANIMATION ------------------

            #region ANIMATION
            // !Bomb
            if (explode && intervalExplode > 0)
            {
                intervalExplode--;
            }

            // Setelah jangka waktu yg ditentukan, bomb yg meledak akan hilang dan player akan langsung mati
            if (intervalExplode == 0)
            {
                bomb.Visible = false;
                lifes = 0;
                lblLifes.Text = "Lifes: " + lifesImg[lifes];
            }

            // !Blue Diamond
            if (diamondEffect && diamontEffectInterval > 0)
            {
                diamontEffectInterval--;
            }

            if (diamontEffectInterval == 0)
            {
                diamondEffect = false;
                blueDiamond.Visible = false;
            }

            // !Red Potion
            // animasi
            if (potionEffect && potionEffectInterval > 0)
            {
                potionEffectInterval--;
            }
            // kalo animasi sudah selesai
            if (potionEffectInterval == 0)
            {
                potionEffect = false;
                redPotion.Visible = false;
            }
            #endregion

            // ------------------- Gk tau mau dibilang apa -----------------
            #region GAME END
            // TODO: Ketika nyawa sudah habis.
            if (lifes == 0)
            {
                player.Image = Properties.Resources.pirateHunterDead;
                Game.Stop();
                MessageBox.Show("Kamu mati");
                Restart();
            }

            // Flag
            if (player.Bounds.IntersectsWith(flag.Bounds))
            {
                if (user.Stages.Count == 2)
                {
                    user.Stages.Add(user.Stages.Count + 1);
                }
                Game.Stop();
                Stage3 stage3 = new Stage3();
                if (hasDiamond)
                {
                    user.Items["Blue Diamond"] = true;
                }
                stage3.Init(user);
                MessageBox.Show("Selamat anda telah melewati stage 2");
                this.Hide();
                _music.Stop();
                stage3.ShowDialog();
                this.Close();
            }
            #endregion

            #region ITEMS APPEAR AND DISAPPEAR
            // !Munculing Blue Diamond
            if (score == 26 && user.Items["Blue Diamond"] == false && hasDiamond == false)
            {
                blueDiamond.Visible = true;
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
        }
        #endregion

        #region GAME KEY DOWN
        private void GameKeyDown(object sender, KeyEventArgs e)
        {
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

            // !Pause
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
        private void GameKeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
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

        #region STAGE2 LOAD
        private void Stage2_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/Stage2.wav");
            _music.PlayLooping();
        }
        #endregion

        public Stage2()
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
        public void Restart()
        {
            Stage2 stage2 = new Stage2();
            stage2.Init(user);
            this.Hide();
            _music.Stop();
            stage2.ShowDialog();
            this.Close();
        }
        #endregion
    }
}
