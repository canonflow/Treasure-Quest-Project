using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Diagnostics;
using System.Security.Cryptography;

namespace TreasureQuest
{
    public partial class Stage3 : Form
    {
        #region PROPERTIES
        Player user;
        private SoundPlayer _music;
        List<string> lifesImg = new List<string> { "", "💜", "💜💜", "💜💜💜" };

        const int FORCE = 6;
        const int GRAVITY = 10;
        const int PLAYERSPEED = 5;
        const int INTERVALSHIP = 15;
        const int PINKINTERVAL = 45;
        const int ATTACKINTERVAL = 20;
        const int INTERVALSHIPHELMEFFECT = 20;
        const int GOLDENSKULLEFFECTINTERVAL = 15;
        const int POTIONINTERVAL = 15;

        // !player
        bool left, right, jump, pause;
        int force = FORCE;
        int gravity = GRAVITY;
        int playerSpeed = PLAYERSPEED;
        int score = 0;
        int lifes = 3;
        string direction = "right";

        // !ship
        int intervalShip = INTERVALSHIP;
        string shipDirection = "down";

        // !Pink Star
        bool pinkAttack = false;
        string pinkDirection = "left";
        int pinkInterval = PINKINTERVAL;
        int attackInterval = ATTACKINTERVAL;

        // !Ship Helm
        int intervaShipHelmEffect = INTERVALSHIPHELMEFFECT;
        bool shipHelmEffect = false;
        bool openGate = false;

        // !Golden Skull
        bool hasGoldenSkull = false;
        bool goldenSkullEffect = false;
        int goldenSkullEffectInterval = GOLDENSKULLEFFECTINTERVAL;

        // !Potion
        bool potion1Effect = false;
        int potion1EffectInterval = POTIONINTERVAL;
        bool potion2Effect = false;
        int potion2EffectInterval = POTIONINTERVAL;
        #endregion

        public Stage3()
        {
            InitializeComponent();
        }

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            lblForce.Text = "Force: " + force;

            // ------------------- Movement -----------------------
            #region OBJECTS MOVEMENT
            // cloud 1
            if (cloud1.Right > 0)
            {
                cloud1.Left--;
            }
            else
            {
                cloud1.Left = this.ClientSize.Width;
            }

            // cloud 2
            if (cloud2.Right > 0)
            {
                cloud2.Left -= 3;
            }
            else
            {
                cloud2.Left = this.ClientSize.Width;
            }

            // cloud 3
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
                    shipDirection = "up";
                }
                else
                {
                    shipDirection = "down";
                }
                intervalShip = INTERVALSHIP;
            }

            // !Pink Star
            if (pinkAttack == false)
            {
                if (pinkInterval > 0)
                {
                    if (pinkDirection == "left")
                    {
                        pinkStar.Left--;
                    }
                    else
                    {
                        pinkStar.Left++;
                    }
                    pinkInterval--;
                }
                else
                {
                    if (pinkDirection == "left")
                    {
                        pinkDirection = "right";
                    }
                    else
                    {
                        pinkDirection = "left";
                    }
                    pinkInterval = PINKINTERVAL;
                }
            }
            #endregion

            // ------------------- Player Controls -------------------
            #region PLAYER CONTROLS
            player.Top += gravity;
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

            // ---------------- Detecting Collision --------------------
            #region DETECTING COLLISION
            // TODO: Detecting

            foreach (Control x in this.Controls)
            {
                // !PLatform
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
                        else if (player.Right > x.Right)
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
                    if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top)
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

                // !Coin
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {
                        score++;
                        x.Visible = false;
                    }
                }  // !Coin

                // !Pink Star
                if (x is PictureBox && (string)x.Tag == "pinkStar")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && pinkAttack == false)
                    {
                        pinkStar.Image = Properties.Resources.pinkStarAttack;
                        pinkAttack = true;
                        if (player.Left < x.Left)
                        {
                            player.Left -= 35;
                        }
                        else if (player.Left > x.Left)
                        {
                            player.Left += 35;
                        }
                        lifes--;
                    }
                }  // !Pink Star

                // !Water
                if (x is PictureBox && (string)x.Tag == "water")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        lifes = 0;
                        lblLifes.Text = "Lifes: " + lifesImg[lifes];
                    }
                }  // !Water
                
                // !Spike
                if (x is PictureBox && (string)x.Tag == "spike")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (lifes > 1)
                        {
                            if (player.Left < x.Left)
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
                }  // !Spike

                // !Label Ship Helm
                if (x is Label && (string)x.Tag == "lblShipHelm")
                {
                    if (shipHelmEffect == false && openGate == false && player.Bounds.IntersectsWith(shipHelm.Bounds))
                    {
                        lblShipHelm.Visible = true;
                    }
                    else
                    {
                        lblShipHelm.Visible = false;
                    }
                }  // !Label Ship Helm

                // !Red Potion
                if (x is PictureBox && (string)x.Tag == "redPotion")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {   
                        if (x.Name == "redPotion1" && potion1Effect == false)
                        {
                            redPotion1.Top -= 40;
                            redPotion1.Image = Properties.Resources.potionEffect;
                            potion1Effect = true;
                            if (lifes < 3)
                            {
                                lifes++;
                            }
                        }
                        else if (x.Name == "redPotion2" && potion2Effect == false)
                        {
                            redPotion2.Top -= 40;
                            redPotion2.Image = Properties.Resources.potionEffect;
                            potion2Effect = true;
                            if (lifes < 3)
                            {
                                lifes++;
                            }
                        }
                        // x.Visible = false;
                    }
                }  // !Red Potion

                // !Golden Skull
                if (x is PictureBox && (string)x.Tag == "goldenSkull")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && goldenSkull.Visible && goldenSkullEffect == false)
                    {
                        hasGoldenSkull = true;
                        goldenSkullEffect = true;
                        goldenSkull.Top -= 38;
                        goldenSkull.Image = Properties.Resources.goldenSkullEffect;
                    }
                }  // !Golden Skull
                
                // !Front Palm Tree
                if (x is PictureBox && (string)x.Tag == "frontPalmTree")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && player.Top < x.Top && jump == false)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }
                }  // !Front Palm Tree
            }
            #endregion

            #region LABEL
            // ------------------- LABEL ---------------------
            lblLifes.Text = "Lifes: " + lifesImg[lifes];
            lblScore.Text = "Score: " + score;
            #endregion

            #region ANIMATION
            // animasi
            if (pinkAttack && attackInterval > 0)
            {
                attackInterval--;
            }
            // Jika interval animasi sudah selesai, maka kembalikan normal
            if (attackInterval == 0)
            {
                pinkAttack = false;
                pinkStar.Image = Properties.Resources.pinkStar;
                attackInterval = ATTACKINTERVAL;
            }

            // !Ship helm
            if (shipHelmEffect && openGate && intervaShipHelmEffect > 0)
            {
                intervaShipHelmEffect--;
                flyingGate.Top += 20;
            }

            if (intervaShipHelmEffect == 0)
            {
                shipHelmEffect = false;
                shipHelm.Image = Properties.Resources.shipHelmIdle;
            }

            // !Golden Skull
            if (goldenSkullEffect && goldenSkullEffectInterval > 0)
            {
                goldenSkullEffectInterval--;
            }

            if (goldenSkullEffectInterval == 0)
            {
                goldenSkullEffect = false;
                goldenSkull.Visible = false;
            }

            // !Red Potion 1
            if (potion1Effect && potion1EffectInterval > 0)
            {
                potion1EffectInterval--;
            }

            if (potion1EffectInterval == 0)
            {
                potion1Effect = false;
                redPotion1.Visible = false;
            }

            // !Red Potion 2
            if (potion2Effect & potion2EffectInterval > 0)
            {
                potion2EffectInterval--;
            }

            if (potion2EffectInterval == 0)
            {
                potion2Effect = false;
                redPotion2.Visible = false;
            }
            #endregion

            #region ITEM APPEAR AND DISAPPEAR
            if (score == 20 && user.Items["Golden Skull"] == false && hasGoldenSkull == false)
            {
                goldenSkull.Visible = true;
            }
            #endregion

            #region GAME END
            if (lifes == 0)
            {
                player.Image = Properties.Resources.pirateHunterDead;
                Game.Stop();
                MessageBox.Show("Kamu mati");
                Restart();
            }

            // !Flag
            if (player.Bounds.IntersectsWith(flag.Bounds))
            {
                if (user.Stages.Count == 3)
                {
                    user.Stages.Add(user.Stages.Count + 1);
                }
                Game.Stop();
                Stage4 stage4 = new Stage4();
                if (hasGoldenSkull)
                {
                    user.Items["Golden Skull"] = true;
                }
                stage4.Init(user);
                MessageBox.Show("Selamat anda telah melewati stage 3");
                this.Hide();
                _music.Stop();
                stage4.ShowDialog();
                this.Close();
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
                if (direction != "left")
                {
                    direction = "left";
                }
                player.Image = Properties.Resources.pirateHuntersRunMirror;
                left = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                if (direction != "right")
                {
                    direction = "right";
                }
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
                    st.ShowDialog();
                    this.Close();
                }
                else
                {
                    pause = false;
                }
            }  // !Pause

            // !Ship Helm 
            if (e.KeyCode == Keys.Enter && shipHelmEffect == false && openGate == false && player.Bounds.IntersectsWith(shipHelm.Bounds))
            {
                shipHelm.Image = Properties.Resources.shipHelmTurn;
                shipHelmEffect = true;
                openGate = true;
            }
        }
        #endregion

        #region STAGE3 LOAD
        private void Stage3_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/Stage3.wav");
            _music.PlayLooping();
        }
        #endregion

        #region GAME KEY UP
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

            if (direction == "left")
            {
                player.Image = Properties.Resources.pirateHuntersIdleMirrorFix;
            }
            else
            {
                player.Image = Properties.Resources.pirateHuntersIdleCrop;
            }

        }
        #endregion

        #region INITIALIZE PLAYER
        public void Init(Player p)
        {
            user = p;
        }
        #endregion

        #region RESTART GAME
        private void Restart()
        {
            Stage3 stage3 = new Stage3();
            stage3.Init(user);
            this.Hide();
            _music.Stop();
            stage3.ShowDialog();
            this.Close();
        }
        #endregion
    }
}
