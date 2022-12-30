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
using System.Windows.Forms.Design;

namespace TreasureQuest
{
    public partial class TreasureQuest : Form
    {
        #region PROPERTIES
        Player user;
        private SoundPlayer _music;
        List<string> lifesImg = new List<string> { "", "💜", "💜💜", "💜💜💜" };

        // Locs
        List<List<int>> keyLocs = new List<List<int>>
        {
            new List<int> { 404, 46 },
            new List<int> { 1099, 58 }
        };
        List<List<int>> keyAppearLocs = new List<List<int>>
        {
            new List<int> { 407, 10 },
            new List<int> { 1101, 22 }
        };

        List<int> probs = new List<int> { 1, 0, 0, 1, 0 };  // 2/5 atau 40%

        const int PLAYER_SPEED = 5;
        const int GRAVITY = 10;
        const int FORCE = 6;
        const int INTERVAL_SHIP = 15;
        const int FIERCE_TOOTH_INTERVAL = 75;
        const int FIERCE_TOOTH_ATTACK_INTERVAL = 15;
        const int PINK_STAR_ATTACK_INTERVAL = 20;
        const int INTERVAL_SHIP_HELM_EFFECT = 20;
        const int CRABBY_INTERVAL = 125;
        const int CRABBY_ATTACK_INTERVAL = 15;
        const int TOTEM_PROCESS_INTERVAL = 100;
        const int KEY_EFFECT = 15;

        // Game 
        bool isEnd= false;

        // Chest
        bool chestOpened = false;
        int chestInterval = 20;

        // Player
        bool left, right, jump, pause;
        int playerSpeed = PLAYER_SPEED;
        int gravity = GRAVITY;
        int force = FORCE;
        int lifes = 3;
        string direction = "right";

        // Totem
        int totemProcessInterval = TOTEM_PROCESS_INTERVAL;
        bool totemProcess = false;
        bool isFinished = false;

        // Key 
        int keyAppearEffectInterval = KEY_EFFECT;
        bool keyAppearEffect = false;
        int loc;

        // Fierce Tooth
        int fierceToothInterval = FIERCE_TOOTH_INTERVAL;
        int fierceToothAttackInterval = FIERCE_TOOTH_ATTACK_INTERVAL;
        string fierceToothDirection = "left";
        bool fierceToothAttack = false;

        // Pink Star
        int pinkStarAttackInterval = PINK_STAR_ATTACK_INTERVAL;
        bool pinkStarAttack = false;

        // Crabby
        int crabbyInterval = CRABBY_INTERVAL;
        int crabbyAttackInterval = CRABBY_ATTACK_INTERVAL;
        bool crabbyAttack = false;
        string crabbyDirection = "left";

        // Ship Helm 1
        int intervalShipHelm1Effect = INTERVAL_SHIP_HELM_EFFECT;
        bool shipHelm1Effect = false;

        // Ship
        int intervalShip = INTERVAL_SHIP;
        string shipDirection = "down";
        #endregion

        public TreasureQuest()
        {
            InitializeComponent();
        }

        #region GAME TIMER
        private void GameTimer(object sender, EventArgs e)
        {
            lblForce.Text = "Force: " + force;

            #region OBJECTS MOVEMENT
            // Ship
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
                intervalShip = INTERVAL_SHIP;
            }

            // Fierce Tooth
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
                    fierceToothInterval = FIERCE_TOOTH_INTERVAL;
                }
            }

            // Crabby
            if (crabbyAttack == false)
            {
                if (crabbyInterval > 0)
                {
                    if (crabbyDirection == "left")
                    {
                        crabby.Left--;
                    }
                    else
                    {
                        crabby.Left++;
                    }
                    crabbyInterval--;
                }
                else
                {
                    if (crabbyDirection == "left")
                    {
                        crabbyDirection = "right";
                        crabby.Image = Properties.Resources.crabbyMirror;
                    }
                    else
                    {
                        crabbyDirection = "left";
                        crabby.Image = Properties.Resources.crabby;
                    }
                    crabbyInterval = CRABBY_INTERVAL;
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
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false && player.Top < x.Top)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }

                    // bawah
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top && (player.Left + player.Width / 2 >= x.Left) && (player.Left + player.Width / 2 <= x.Right) && jump)
                    {
                        player.Top = x.Bottom;
                        force = -1;
                    }

                    // samping
                    else if (player.Bounds.IntersectsWith(x.Bounds) && player.Top > x.Top && (player.Left > x.Left | player.Left < x.Left))
                    {
                        // kiri
                        if (left && player.Left > x.Left)
                        {
                            player.Left = x.Left + x.Width;
                        }
                        // kanan
                        else if (right && player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }
                    }  // !Wall
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

                // !Fierce Tooth
                if (x is PictureBox && (string)x.Tag == "fierceTooth")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && fierceToothAttack == false)
                    {
                        // P  <-T
                        if (player.Left < x.Left && fierceToothDirection == "left")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttack;
                            player.Left -= 35;
                            fierceToothAttack = true;
                            lifes--;
                        }
                        // T-> P
                        else if (player.Right > x.Right && fierceToothDirection == "right")
                        {
                            fierceTooth.Image = Properties.Resources.fierceToothAttackMirror;
                            fierceToothAttack = true;
                            player.Left += 35;
                            lifes--;
                        }
                    }
                }  // !FIerce Tooth

                // !Crabby
                if (x is PictureBox && (string)x.Tag == "crabby")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && crabbyAttack == false)
                    {
                        // P <- C
                        if (player.Left < x.Left && crabbyDirection == "left")
                        {
                            crabby.Image = Properties.Resources.crabbyAttack;
                            player.Left -= 35;
                            crabbyAttack = true;
                            lifes--;
                        }
                        // C -> P
                        else if (player.Right > x.Right && crabbyDirection == "right")
                        {
                            crabby.Image = Properties.Resources.crabbyAttackMirror;
                            player.Left += 35;
                            crabbyAttack = true;
                            lifes--;
                        }

                    }
                }  // !Crabby

                // !Water
                if (x is PictureBox && (string)x.Tag == "water")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        lifes = 0;
                        lblLifes.Text = "Lifes: " + lifesImg[lifes];
                    }
                }  // !Water

                // !Pink Star
                if (x is PictureBox && (string)x.Tag == "pinkStar")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && pinkStarAttack == false)
                    {
                        pinkStar.Image = Properties.Resources.pinkStarAttack;
                        pinkStarAttack = true;
                        if (player.Left < x.Left)
                        {
                            player.Left -= 35;
                        }
                        else if (player.Right > x.Right)
                        {
                            player.Left += 35;
                        }
                        lifes--;
                    }
                }  // !Pink Star

                // !Spike
                if (x is PictureBox && (string)x.Tag == "spike")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible)
                    {
                        if (lifes > 1)
                        {
                            if (player.Left < x.Left)
                            {
                                player.Left -= 35;
                            }
                            else if (player.Right > x.Right)
                            {
                                player.Left += 35;
                            }
                        }
                        lifes--;
                        x.BringToFront();
                    }
                }  // !Spike

                // !Front Palm Tree
                if (x is PictureBox && (string)x.Tag == "frontPalmTree")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && jump == false && player.Top < x.Top)
                    {
                        player.Top = x.Top - player.Height;
                        gravity = 0;
                        force = FORCE;
                    }
                }  // !Front Palm Tree
            }
            #endregion

            #region ITEM APPEAR AND DISAPPEAR
            if (player.Bounds.IntersectsWith(key.Bounds) && key.Visible)
            {
                key.Visible = false;
                user.HasKey = true;
            }
            #endregion

            #region LABEL
            // ------------------- LABEL ---------------------
            lblLifes.Text = "Lifes: " + lifesImg[lifes];
            #endregion

            #region ANIMATION
            // !Fierce tooth
            if (fierceToothAttack && fierceToothAttackInterval > 0)
            {
                fierceToothAttackInterval--;
            }

            if (fierceToothAttackInterval == 0)
            {
                fierceToothAttack = false;
                fierceToothAttackInterval = FIERCE_TOOTH_ATTACK_INTERVAL;

                // Kemabalikan gambar seperti sebelum meneyerang sesuai dengan arah awalnya
                if (fierceToothDirection == "left")
                {
                    fierceTooth.Image = Properties.Resources.fierceTooth;
                }
                else
                {
                    fierceTooth.Image = Properties.Resources.fierceToothMirror;
                }
            }

            // !Crabby
            if (crabbyAttack && crabbyAttackInterval > 0)
            {
                crabbyAttackInterval--;
            }

            if (crabbyAttackInterval == 0)
            {
                crabbyAttack = false;
                crabbyAttackInterval = CRABBY_ATTACK_INTERVAL;

                // Kemabalikan gambar seperti sebelum meneyerang sesuai dengan arah awalnya
                if (crabbyDirection == "left")
                {
                    crabby.Image = Properties.Resources.crabby;
                }
                else
                {
                    crabby.Image = Properties.Resources.crabbyMirror;
                }
            }

            // !Pink Star
            if (pinkStarAttack && pinkStarAttackInterval > 0)
            {
                pinkStarAttackInterval--;
            }

            if (pinkStarAttackInterval == 0)
            {
                pinkStar.Image = Properties.Resources.pinkStar;
                pinkStarAttack = false;
                pinkStarAttackInterval = PINK_STAR_ATTACK_INTERVAL;
            }

            // !Ship Helm 1
            if (shipHelm1Effect && intervalShipHelm1Effect > 0)
            {
                intervalShipHelm1Effect--;
            }

            if (intervalShipHelm1Effect == 0)
            {
                shipHelm1.Image = Properties.Resources.shipHelmIdle;
                shipHelm1Effect = false;
            }

            // !Totem and Key
            if (totemProcess && totemProcessInterval > 0)
            {
                totemProcessInterval--;
            }

            if (totemProcessInterval == 0 && isFinished == false)
            {
                Random random = new Random();
                totemProcess = false;
                totemProcessEffect.Visible = false;
                keyAppearEffect = true;
                totem.Image = Properties.Resources.TotemIdle;
                isFinished = true;

                loc = random.Next(keyLocs.Count);
                keyAppear.Location = new Point(keyAppearLocs[loc][0], keyAppearLocs[loc][1]);
                key.Location = new Point(keyLocs[loc][0], keyLocs[loc][1]);
                keyAppear.Visible = true;
                key.Visible = true;
            }

            if (keyAppearEffect && keyAppearEffectInterval > 0)
            {
                keyAppearEffectInterval--;
            }

            if (keyAppearEffectInterval == 0)
            {
                keyAppearEffect = false;
                keyAppear.Visible = false;
            }

            // !Chest
            if (chestOpened && chestInterval > 0)
            {
                chestInterval--;
            }

            if (chestInterval == 0 && isEnd == false)
            {
                isEnd = true; 
                chestOpened = false;
                chest.Image = Properties.Resources.chestUnlockedIdle;
            }
            #endregion

            #region GAME END
            if (lifes == 0)
            {
                player.Image = Properties.Resources.pirateHunterDead;
                player.SendToBack();
                Game.Stop();
                MessageBox.Show("Kamu mati");
                Restart();
            }

            if (isEnd)
            {
                Random random = new Random();
                int get = random.Next(probs.Count);
                Player restartUser = new Player();
                Game.Stop();
                if (get == 1)
                {
                    MessageBox.Show("Dapet apanichhhhh");
                    True true_ = new True();
                    this.Hide();
                    _music.Stop();
                    true_.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Dapet apanichhhhh");
                    Fake fake_ = new Fake();
                    this.Hide();
                    _music.Stop();
                    fake_.ShowDialog();
                    this.Close();
                }
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

            // !Spikes
            if (e.KeyCode == Keys.Enter && shipHelm1Effect == false && player.Bounds.IntersectsWith(shipHelm1.Bounds) && spike1.Visible && spike2.Visible && spike3.Visible && spike4.Visible)
            {
                shipHelm1.Image = Properties.Resources.shipHelmTurn;
                shipHelm1Effect = true;
                spike1.Visible = false;
                spike2.Visible = false;
                spike3.Visible = false;
                spike4.Visible = false;
            }

            // !Totem
            if (e.KeyCode == Keys.Enter && user.IsComplete() && player.Bounds.IntersectsWith(totem.Bounds) && totemProcess == false && totemProcessEffect.Visible == false && user.HasKey == false)
            {
                totem.Image = Properties.Resources.totemProcess;
                totemProcess = true;
                totemProcessEffect.Visible = true;

                List<string> keys = new List<string>();
                foreach(KeyValuePair<string, bool> pairs in user.Items)
                {
                    keys.Add(pairs.Key);
                }

                foreach(string key in keys)
                {
                    user.Items[key] = false;
                }
            }

            // !Chest
            if (e.KeyCode == Keys.Enter && user.HasKey && player.Bounds.IntersectsWith(chest.Bounds) && chestOpened == false)
            {
                chestOpened = true;
                chest.Image = Properties.Resources.chestUnlocked;
                user.HasKey = false;
            }
        }
        #endregion

        #region TREASURE QUEST LOAD
        private void TreasureQuest_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/TreasureQuest.wav");
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

        #region RESTART
        private void Restart()
        {
            TreasureQuest tq = new TreasureQuest();
            tq.Init(user);
            this.Hide();
            _music.Stop();
            tq.ShowDialog();
            this.Close();
        }
        #endregion

        #region INITIALIZE PLAYER
        public void Init(Player p)
        {
            user = p;
        }
        #endregion

    }
}
