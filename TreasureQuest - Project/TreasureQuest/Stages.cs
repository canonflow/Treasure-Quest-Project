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
using System.Collections;

namespace TreasureQuest
{
    public partial class Stages : Form
    {
        #region PROPERTIES
        private SoundPlayer _music;
        Player user;
        #endregion

        public Stages()
        {
            InitializeComponent();
        }

        #region STAGES LOAD
        private void Stages_Load(object sender, EventArgs e)
        {
            _music = new SoundPlayer(@"Music/wav/Stages.wav");
            _music.PlayLooping();
            // Kalo user punya key
            if (user.HasKey)
            {
                cmbItems.Items.Add("Key");
                cmbStages.Items.Add("Treasure Quest");

                cmbStages.SelectedIndex = cmbStages.Items.Count - 1;
                cmbItems.SelectedIndex = 0;
            }
            else
            {
                // Items Init
                foreach (KeyValuePair<string, bool> pairs in user.Items)
                {
                    if (pairs.Value)
                    {
                        cmbItems.Items.Add(pairs.Key);
                    }
                    else
                    {
                        cmbItems.Items.Add("Missing: " + pairs.Key);
                    }
                }

                // Stages Init
                foreach (int stage in user.Stages)
                {
                    if (stage == 5)
                    {
                        cmbStages.Items.Add("Treasure Quest");
                    }
                    else
                    {
                        cmbStages.Items.Add(stage);
                    }
                }

                // !Di cmbStages paling sedikit PASTI ada 1 item, yaitu 1 (INT)
                cmbStages.SelectedIndex = cmbStages.Items.Count - 1;
                cmbItems.SelectedIndex = 0;
            }

        }
        #endregion

        #region INITIALIZE PLAYER
        public void Init(Player p)
        {
            user = p;
        }
        #endregion

        #region METHOD
        private void btnHome_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Init(user);
            this.Hide();
            _music.Stop();
            home.ShowDialog();
            this.Close();
        }

        private void PlayHover(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = Properties.Resources.PLAYplayRed;
        }

        private void PlayLeave(object sender, EventArgs e)
        {
            btnPlay.BackgroundImage = Properties.Resources.PLAYplay;
        }

        private void HomeHover(object sender, EventArgs e)
        {
            btnHome.BackgroundImage = Properties.Resources.HOMEhomeRed;
        }

        private void HomeLeave(object sender, EventArgs e)
        {
            btnHome.BackgroundImage = Properties.Resources.HOMEhome;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            int selectedStage = cmbStages.SelectedIndex;

            if (user.HasKey)
            {
                TreasureQuest tq = new TreasureQuest();
                tq.Init(user);
                this.Hide();
                _music.Stop();
                tq.ShowDialog();
                this.Close();
            }
            else
            {
                if (selectedStage == 0)
                {
                    Stage1 stage1 = new Stage1();
                    stage1.Init(user);
                    this.Hide();
                    _music.Stop();
                    stage1.ShowDialog();
                    this.Close();
                }
                else if (selectedStage == 1)
                {
                    Stage2 stage2 = new Stage2();
                    stage2.Init(user);
                    this.Hide();
                    _music.Stop();
                    stage2.ShowDialog();
                    this.Close();
                }
                else if (selectedStage == 2)
                {
                    Stage3 stage3 = new Stage3();
                    stage3.Init(user);
                    this.Hide();
                    _music.Stop();
                    stage3.ShowDialog();
                    this.Close();
                }
                else if (selectedStage == 3)
                {
                    Stage4 stage4 = new Stage4();
                    stage4.Init(user);
                    this.Hide();
                    _music.Stop();
                    stage4.ShowDialog();
                    this.Close();
                }
                else if (selectedStage == 4)
                {
                    TreasureQuest tq = new TreasureQuest();
                    tq.Init(user);
                    this.Hide();
                    _music.Stop();
                    tq.ShowDialog();
                    this.Close();
                }

            } 
        }
        #endregion
    }
}
