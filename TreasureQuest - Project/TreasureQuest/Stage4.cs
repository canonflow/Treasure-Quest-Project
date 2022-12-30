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
using System.Runtime.Remoting.Lifetime;

namespace TreasureQuest
{
    public partial class Stage4 : Form
    {

        #region PROPERTIES
        Player user;
        private SoundPlayer _music;
        List<string> lifesImg = new List<string> { "", "💜", "💜💜", "💜💜💜" };
        const int PLAYER_SPEED = 5;
        const int GRAVITY = 10;
        const int FORCE = 6;
        const int INTERVALSHIP = 15;
        const int FIERCE_TOOTH_INTERVAL = 75;
        const int FIERCE_TOOTH_ATTACK_INTERVAL = 15;
        const int INTERVAL_SHIP_HELM_EFFECT = 20;
        const int POTION_INTERVAL = 15;


        // Player
        bool left, right, jump, pause, hasGreenPotion;
        int playerSpeed = PLAYER_SPEED;
        int gravity = GRAVITY;
        int force = FORCE; 
        int score = 0;
        int lifes = 3;
        string direction = "right";

        // Ship
        int intervalShip = INTERVALSHIP;
        string shipDirection = "down";

        // Fierce Tooth
        int fierceToothInterval = FIERCE_TOOTH_INTERVAL;
        string fierceToothDirection = "left";
        bool fierceToothAttack = false;
        int fierceToothAttackInterval = FIERCE_TOOTH_ATTACK_INTERVAL;

        // Ship Helm 1
        int intervalShipHelm1Effect = INTERVAL_SHIP_HELM_EFFECT;
        bool shipHelm1Effect = false;
        bool openGate1 = false;

        // Ship Helm 2
        int intervalShipHelm2Effect = INTERVAL_SHIP_HELM_EFFECT;
        bool shipHelm2Effect = false;
        bool openGate2 = false;

        // Red Potion
        int potion1EffectInterval = POTION_INTERVAL;
        bool potion1Effect = false;
        int potion2EffectInterval = POTION_INTERVAL;
        bool potion2Effect = false;

        // Green Potion
        bool greenPotionEffect = false;
        int greenPotionEffectInterval = POTION_INTERVAL;
        #endregion


        public Stage4()
        {
            InitializeComponent();
        }

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            lblForce.Text = "Force: " + force;
            #region OBJECTS MOVEMENT
            // Cloud1
            if (cloud1.Right > 0)
            {
                cloud1.Left--;
            }
            else
            {
                cloud1.Left = this.ClientSize.Width;
            }

            // Cloud2
            if (cloud2.Right > 0)
            {
                cloud2.Left -= 3;
            }
            else
            {
                cloud2.Left = this.ClientSize.Width;
            }

            // Cloud 3
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

            // !Fierce Tooth
            if (fierceToothAttack == false)
            {
                if (fierceToothInterval > 0)
                {
                    if (fierceToothDirection == "left")
                    {
                        fierceTooth.Left--;
                    }
                    else
                    {
                        fierceTooth.Left++;
                    }

                    fierceToothInterval--;
                }
                else
                {
                    // GANTI ARAH dan GAMBAR setelah selese intervalnya
                    if (fierceToothDirection == "left")
                    {
                        fierceToothDirection = "right";
                        fierceTooth.Image = Properties.Resources.fierceToothMirror;
                    }
                    else
                    {
                        fierceToothDirection = "left";
                        fierceTooth.Image = Properties.Resources.fierceTooth;
                    }
                    // Kembalikan intertvalnya
                    fierceToothInterval = FIERCE_TOOTH_INTERVAL;
                }
            }
            #endregion

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

            #region DETECTING COLLISION
            // TODO: Detecting
            foreach(Control x in this.Controls)
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

                // !Water
                if (x is PictureBox && (string)x.Tag == "water")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        lifes = 0;
                    }
                }  // !Water

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

                // !Ceil
                if (x is PictureBox && (string)x.Tag == "ceil")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Top = x.Top + x.Height;
                    }
                }  // !Ceil

                // !Fierce Tooth
                if (x is PictureBox && (string)x.Tag == "fierceTooth")
                {
                    /*!
                        P = Player ; T = Fierce Tooth
                        
                        P-> | <-T (KENA) <1>
                        T-> | <-P (KENA) <2>
                        T->P-> (KENA) <3>
                        <-P<-T (KENA) <4>
                        P -> | T-> (GK KENA)
                        <-T  | <-P (GK KENA)
                       */
                    if (player.Bounds.IntersectsWith(x.Bounds) && fierceToothAttack == false)
                    {
                        // !<1>
                        if (player.Left < x.Left && direction == "right" && fierceToothDirection == "left")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttack;
                            player.Left -= 35;
                            fierceToothAttack = true;
                            lifes--;
                        }
                        // !<2>
                        else if (player.Right > x.Right && direction == "left" && fierceToothDirection == "right")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttackMirror;
                            player.Left += 35;
                            fierceToothAttack = true;
                            lifes--;
                        }
                        // !<3>
                        else if (player.Right > x.Right && direction == "right" && fierceToothDirection == "right")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttackMirror;
                            player.Left += 35;
                            fierceToothAttack = true;
                            lifes--;
                        }
                        // !<4>
                        if (player.Left < x.Left && direction == "left" && fierceToothDirection == "left")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttack;
                            player.Left -= 35;
                            fierceToothAttack = true;
                            lifes--;
                        }
                    }
                }  // !Fierce Tooth

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

                    // bawah
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top && (player.Left + player.Width / 2 >= x.Left) && (player.Left + player.Width / 2 <= x.Right) && jump)
                    {
                        /*
                        if (player.Left >= x.Left && player.Right <= x.Right)
                        {
                            player.Top = x.Top + x.Height;
                            force = -1;
                        }
                        */
                        player.Top = x.Top + x.Height;
                        force = -1;
                    }

                    // samping
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top && (player.Left > x.Left || player.Left < x.Left))
                        {
                        // kiri
                        if (left && player.Left > x.Left)
                        {
                            player.Left = x.Left + x.Width;
                        }
                        // kanan
                        if (right && player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }
                    }
                }  // !Wall

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

                // !Label Ship Helm1
                if (x is PictureBox && (string)x.Tag == "shipHelm1")
                {
                    if (player.Bounds.IntersectsWith(shipHelm1.Bounds) && shipHelm1Effect == false && openGate1 == false)
                    {
                        lblShipHelm1.Visible = true;
                    }
                    else
                    {
                        lblShipHelm1.Visible = false;
                    }
                }  // !Label Ship Helm1

                // !Label Ship Helm2
                if (x is PictureBox && (string)x.Tag == "shipHelm2")
                {
                    if (player.Bounds.IntersectsWith(shipHelm2.Bounds) && shipHelm2Effect == false && openGate2 == false)
                    {
                        lblShipHelm2.Visible = true;
                    }
                    else
                    {
                        lblShipHelm2.Visible = false;
                    }
                }

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
                    }
                }  // !Red Potion

                // !Coin
                if (x is PictureBox && (string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {
                        score++;
                        x.Visible = false;
                    }
                }  // !Coin

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

                // !Green Potion
                if (x is PictureBox && (string)x.Tag == "greenPotion")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && greenPotion.Visible && greenPotionEffect == false)
                    {
                        greenPotionEffect = true;
                        hasGreenPotion = true;
                        greenPotion.Top -= 40;
                        greenPotion.Image = Properties.Resources.potionEffect;
                    }
                }  // !Green Potion
                
            }
            #endregion

            #region ITEM APPEAR AND DISAPPEAR
            if (score == 48 && user.Items["Green Potion"] == false && hasGreenPotion == false)
            {
                greenPotion.Visible = true;
            }
            #endregion

            #region ANIMATION
            // !Fierce Tooth
            if (fierceToothAttack && fierceToothAttackInterval > 0)
            {
                fierceToothAttackInterval--;
            }

            if (fierceToothAttackInterval == 0)
            {
                fierceToothAttack = false;
                fierceToothAttackInterval = FIERCE_TOOTH_ATTACK_INTERVAL;

                // Kemabalikan gambar seperti sebelum meneyerang
                if (fierceToothDirection == "left")
                {
                    fierceTooth.Image = Properties.Resources.fierceTooth;
                }
                else
                {
                    fierceTooth.Image = Properties.Resources.fierceToothMirror;
                }
            }

            // !Ship Helm 1
            if (shipHelm1Effect && openGate1 && intervalShipHelm1Effect > 0)
            {
                intervalShipHelm1Effect--;
                flyingGate1.Top += 35;
            }

            if (intervalShipHelm1Effect == 0)
            {
                shipHelm1Effect = false;
                shipHelm1.Image = Properties.Resources.shipHelmIdle;
            }

            // !Ship Helm 2
            if (shipHelm2Effect && openGate2 && intervalShipHelm2Effect > 0)
            {
                intervalShipHelm2Effect--;
                flyingGate2.Top += 35;
            }

            if (intervalShipHelm2Effect == 0)
            {
                shipHelm2Effect = false;
                shipHelm2.Image = Properties.Resources.shipHelmIdle;
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
            if (potion2Effect && potion2EffectInterval > 0)
            {
                potion2EffectInterval--;
            }

            if (potion2EffectInterval == 0)
            {
                potion2Effect = false;
                redPotion2.Visible = false;
            }

            // !Green Potion
            if (greenPotionEffect && greenPotionEffectInterval > 0)
            {
                greenPotionEffectInterval--;
            }

            if (greenPotionEffectInterval == 0)
            {
                greenPotionEffect = false;
                greenPotion.Visible = false;
            }
            #endregion

            #region LABEL
            // ------------------- LABEL ---------------------
            lblLifes.Text = "Lifes: " + lifesImg[lifes];
            lblScore.Text = "Score: " + score;
            #endregion

            #region GAME END
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
                if (user.Stages.Count == 4)
                {
                    user.Stages.Add(user.Items.Count + 1);
                }
                Game.Stop();
                TreasureQuest tq = new TreasureQuest();
                if (hasGreenPotion)
                {
                    user.Items["Green Potion"] = true;
                }
                tq.Init(user);
                MessageBox.Show("Selamat anda telah melewati stage 4");
                this.Hide();
                _music.Stop();
                tq.ShowDialog();
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

            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up) && jump == false)
            {
                jump = true;
            }

            // !Pause
            if (e.KeyCode == Keys.Escape && pause == false)
            {
                pause = true;
                string message = "May lanjut apa nggak nichhh";
                string title = "Paused";
                MessageBoxButtons button = MessageBoxButtons.YesNo;
                DialogResult res = MessageBox.Show(message, title, button, MessageBoxIcon.Question);
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

            // !Ship Helm 1
            if (e.KeyCode == Keys.Enter && openGate1 == false && shipHelm1Effect == false && player.Bounds.IntersectsWith(shipHelm1.Bounds))
            {
                shipHelm1.Image = Properties.Resources.shipHelmTurn;
                openGate1 = true;
                shipHelm1Effect = true;
            }

            // !Ship Helm 2
            if (e.KeyCode == Keys.Enter && openGate2 == false && shipHelm2Effect == false && player.Bounds.IntersectsWith(shipHelm2.Bounds))
            {
                shipHelm2.Image = Properties.Resources.shipHelmTurn;
                openGate2 = true;
                shipHelm2Effect = true;
            }
        }
        #endregion

        #region STAGE4 LOAD
        private void Stage4_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/Stage4.wav");
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
            Stage4 stage4 = new Stage4();
            stage4.Init(user);
            this.Hide();
            _music.Stop();
            stage4.ShowDialog();
            this.Close();
        }
        #endregion
    }
}
