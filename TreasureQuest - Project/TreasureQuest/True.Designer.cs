namespace TreasureQuest
{
    partial class True
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(True));
            this.water = new System.Windows.Forms.PictureBox();
            this.ship = new System.Windows.Forms.PictureBox();
            this.sail = new System.Windows.Forms.PictureBox();
            this.player = new System.Windows.Forms.PictureBox();
            this.chest = new System.Windows.Forms.PictureBox();
            this.Game = new System.Windows.Forms.Timer(this.components);
            this.lblDisplay = new System.Windows.Forms.Label();
            this.Label = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.water)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ship)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.player)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chest)).BeginInit();
            this.SuspendLayout();
            // 
            // water
            // 
            this.water.BackColor = System.Drawing.Color.Transparent;
            this.water.Image = global::TreasureQuest.Properties.Resources.water;
            this.water.Location = new System.Drawing.Point(0, 361);
            this.water.Name = "water";
            this.water.Size = new System.Drawing.Size(928, 89);
            this.water.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.water.TabIndex = 57;
            this.water.TabStop = false;
            this.water.Tag = "water";
            // 
            // ship
            // 
            this.ship.BackColor = System.Drawing.Color.Transparent;
            this.ship.Image = global::TreasureQuest.Properties.Resources.shipIdleBig;
            this.ship.Location = new System.Drawing.Point(333, 313);
            this.ship.Name = "ship";
            this.ship.Size = new System.Drawing.Size(234, 66);
            this.ship.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ship.TabIndex = 62;
            this.ship.TabStop = false;
            this.ship.Tag = "ship";
            // 
            // sail
            // 
            this.sail.BackColor = System.Drawing.Color.Transparent;
            this.sail.Image = ((System.Drawing.Image)(resources.GetObject("sail.Image")));
            this.sail.Location = new System.Drawing.Point(498, 128);
            this.sail.Name = "sail";
            this.sail.Size = new System.Drawing.Size(84, 150);
            this.sail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.sail.TabIndex = 63;
            this.sail.TabStop = false;
            // 
            // player
            // 
            this.player.BackColor = System.Drawing.Color.Transparent;
            this.player.Image = global::TreasureQuest.Properties.Resources.pirateHuntersIdleCrop;
            this.player.InitialImage = null;
            this.player.Location = new System.Drawing.Point(612, 279);
            this.player.Name = "player";
            this.player.Size = new System.Drawing.Size(24, 28);
            this.player.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.player.TabIndex = 64;
            this.player.TabStop = false;
            this.player.Tag = "player";
            // 
            // chest
            // 
            this.chest.BackColor = System.Drawing.Color.Transparent;
            this.chest.Image = global::TreasureQuest.Properties.Resources.treasure_true;
            this.chest.Location = new System.Drawing.Point(347, 272);
            this.chest.Name = "chest";
            this.chest.Size = new System.Drawing.Size(64, 35);
            this.chest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.chest.TabIndex = 65;
            this.chest.TabStop = false;
            // 
            // Game
            // 
            this.Game.Enabled = true;
            this.Game.Interval = 20;
            this.Game.Tick += new System.EventHandler(this.GameTimer);
            // 
            // lblDisplay
            // 
            this.lblDisplay.AutoSize = true;
            this.lblDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisplay.Location = new System.Drawing.Point(26, 33);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(22, 24);
            this.lblDisplay.TabIndex = 66;
            this.lblDisplay.Text = "|";
            // 
            // Label
            // 
            this.Label.Enabled = true;
            this.Label.Interval = 40;
            this.Label.Tick += new System.EventHandler(this.LabelTimer);
            // 
            // True
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::TreasureQuest.Properties.Resources.BG_Image;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(924, 450);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.water);
            this.Controls.Add(this.ship);
            this.Controls.Add(this.player);
            this.Controls.Add(this.sail);
            this.Controls.Add(this.chest);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "True";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Treasure Quest";
            this.Load += new System.EventHandler(this.True_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.water)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ship)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.player)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox water;
        private System.Windows.Forms.PictureBox ship;
        private System.Windows.Forms.PictureBox sail;
        private System.Windows.Forms.PictureBox player;
        private System.Windows.Forms.PictureBox chest;
        private System.Windows.Forms.Timer Game;
        private System.Windows.Forms.Label lblDisplay;
        private System.Windows.Forms.Timer Label;
    }
}