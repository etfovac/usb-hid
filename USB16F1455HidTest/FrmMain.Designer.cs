namespace USB16F1455HidTest
{
    partial class FrmMain
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
            this.PnlTop = new System.Windows.Forms.Panel();
            this.BtnGetstatus = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PbOn = new System.Windows.Forms.PictureBox();
            this.PbOff = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PbDn = new System.Windows.Forms.PictureBox();
            this.PbUp = new System.Windows.Forms.PictureBox();
            this.BtnToggleLed = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.TbxReseived = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TbxSent = new System.Windows.Forms.TextBox();
            this.BtnClear = new System.Windows.Forms.Button();
            this.PnlTop.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbOff)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbDn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbUp)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlTop
            // 
            this.PnlTop.Controls.Add(this.BtnClear);
            this.PnlTop.Controls.Add(this.BtnGetstatus);
            this.PnlTop.Controls.Add(this.groupBox2);
            this.PnlTop.Controls.Add(this.groupBox1);
            this.PnlTop.Controls.Add(this.BtnToggleLed);
            this.PnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlTop.Location = new System.Drawing.Point(0, 0);
            this.PnlTop.Name = "PnlTop";
            this.PnlTop.Size = new System.Drawing.Size(474, 48);
            this.PnlTop.TabIndex = 0;
            // 
            // BtnGetstatus
            // 
            this.BtnGetstatus.Location = new System.Drawing.Point(134, 18);
            this.BtnGetstatus.Name = "BtnGetstatus";
            this.BtnGetstatus.Size = new System.Drawing.Size(75, 23);
            this.BtnGetstatus.TabIndex = 7;
            this.BtnGetstatus.Text = "GetStatus";
            this.BtnGetstatus.UseVisualStyleBackColor = true;
            this.BtnGetstatus.Click += new System.EventHandler(this.BtnGetstatusClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.PbOn);
            this.groupBox2.Controls.Add(this.PbOff);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(64, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(64, 48);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Led";
            // 
            // PbOn
            // 
            this.PbOn.Image = global::USB16F1455HidTest.Properties.Resources.LedOn;
            this.PbOn.InitialImage = global::USB16F1455HidTest.Properties.Resources.LedOn;
            this.PbOn.Location = new System.Drawing.Point(6, 19);
            this.PbOn.Name = "PbOn";
            this.PbOn.Size = new System.Drawing.Size(50, 22);
            this.PbOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbOn.TabIndex = 5;
            this.PbOn.TabStop = false;
            this.PbOn.Visible = false;
            // 
            // PbOff
            // 
            this.PbOff.Image = global::USB16F1455HidTest.Properties.Resources.LedOff;
            this.PbOff.InitialImage = global::USB16F1455HidTest.Properties.Resources.LedOff;
            this.PbOff.Location = new System.Drawing.Point(6, 19);
            this.PbOff.Name = "PbOff";
            this.PbOff.Size = new System.Drawing.Size(50, 22);
            this.PbOff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbOff.TabIndex = 4;
            this.PbOff.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PbDn);
            this.groupBox1.Controls.Add(this.PbUp);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(64, 48);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Button";
            // 
            // PbDn
            // 
            this.PbDn.Image = global::USB16F1455HidTest.Properties.Resources.ButtonDn;
            this.PbDn.InitialImage = global::USB16F1455HidTest.Properties.Resources.ButtonDn;
            this.PbDn.Location = new System.Drawing.Point(6, 19);
            this.PbDn.Name = "PbDn";
            this.PbDn.Size = new System.Drawing.Size(50, 22);
            this.PbDn.TabIndex = 3;
            this.PbDn.TabStop = false;
            this.PbDn.Visible = false;
            // 
            // PbUp
            // 
            this.PbUp.Image = global::USB16F1455HidTest.Properties.Resources.ButtonUp;
            this.PbUp.InitialImage = global::USB16F1455HidTest.Properties.Resources.ButtonUp;
            this.PbUp.Location = new System.Drawing.Point(6, 19);
            this.PbUp.Name = "PbUp";
            this.PbUp.Size = new System.Drawing.Size(50, 22);
            this.PbUp.TabIndex = 2;
            this.PbUp.TabStop = false;
            // 
            // BtnToggleLed
            // 
            this.BtnToggleLed.Location = new System.Drawing.Point(215, 18);
            this.BtnToggleLed.Name = "BtnToggleLed";
            this.BtnToggleLed.Size = new System.Drawing.Size(75, 23);
            this.BtnToggleLed.TabIndex = 1;
            this.BtnToggleLed.Text = "Toggle Led";
            this.BtnToggleLed.UseVisualStyleBackColor = true;
            this.BtnToggleLed.Click += new System.EventHandler(this.BtnToggleLedClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.splitter1);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 48);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(474, 342);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "RawData";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.TbxReseived);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 155);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(468, 184);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Received";
            // 
            // TbxReseived
            // 
            this.TbxReseived.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbxReseived.Location = new System.Drawing.Point(3, 16);
            this.TbxReseived.Multiline = true;
            this.TbxReseived.Name = "TbxReseived";
            this.TbxReseived.Size = new System.Drawing.Size(462, 165);
            this.TbxReseived.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(3, 152);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(468, 3);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.TbxSent);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(3, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(468, 136);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Sent";
            // 
            // TbxSent
            // 
            this.TbxSent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbxSent.Location = new System.Drawing.Point(3, 16);
            this.TbxSent.Multiline = true;
            this.TbxSent.Name = "TbxSent";
            this.TbxSent.Size = new System.Drawing.Size(462, 117);
            this.TbxSent.TabIndex = 0;
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(296, 18);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 8;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClearClick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 390);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.PnlTop);
            this.Name = "FrmMain";
            this.Text = "Hid Test";
            this.Load += new System.EventHandler(this.FrmMainLoad);
            this.PnlTop.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbOff)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PbDn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PbUp)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlTop;
        private System.Windows.Forms.Button BtnToggleLed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox PbDn;
        private System.Windows.Forms.PictureBox PbUp;
        private System.Windows.Forms.PictureBox PbOff;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox PbOn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox TbxReseived;
        private System.Windows.Forms.TextBox TbxSent;
        private System.Windows.Forms.Button BtnGetstatus;
        private System.Windows.Forms.Button BtnClear;
    }
}

