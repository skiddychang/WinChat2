namespace WinChat2
{
    partial class Settings
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
            this.lblNick = new System.Windows.Forms.Label();
            this.tbNick = new System.Windows.Forms.TextBox();
            this.lblDefaultAddress = new System.Windows.Forms.Label();
            this.tbDefaultAddress = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.lblFont = new System.Windows.Forms.Label();
            this.btnChooseFont = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblNick
            // 
            this.lblNick.AutoSize = true;
            this.lblNick.Location = new System.Drawing.Point(12, 9);
            this.lblNick.Name = "lblNick";
            this.lblNick.Size = new System.Drawing.Size(35, 17);
            this.lblNick.TabIndex = 0;
            this.lblNick.Text = "Nick";
            // 
            // tbNick
            // 
            this.tbNick.Location = new System.Drawing.Point(53, 6);
            this.tbNick.Name = "tbNick";
            this.tbNick.Size = new System.Drawing.Size(139, 22);
            this.tbNick.TabIndex = 1;
            // 
            // lblDefaultAddress
            // 
            this.lblDefaultAddress.AutoSize = true;
            this.lblDefaultAddress.Location = new System.Drawing.Point(12, 41);
            this.lblDefaultAddress.Name = "lblDefaultAddress";
            this.lblDefaultAddress.Size = new System.Drawing.Size(112, 17);
            this.lblDefaultAddress.TabIndex = 2;
            this.lblDefaultAddress.Text = "Default address:";
            // 
            // tbDefaultAddress
            // 
            this.tbDefaultAddress.Location = new System.Drawing.Point(130, 38);
            this.tbDefaultAddress.Name = "tbDefaultAddress";
            this.tbDefaultAddress.Size = new System.Drawing.Size(100, 22);
            this.tbDefaultAddress.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(183, 265);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(264, 265);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fontDialog1
            // 
            this.fontDialog1.ShowEffects = false;
            this.fontDialog1.Apply += new System.EventHandler(this.fontDialog1_Apply);
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(12, 69);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(40, 17);
            this.lblFont.TabIndex = 6;
            this.lblFont.Text = "Font:";
            // 
            // btnChooseFont
            // 
            this.btnChooseFont.Location = new System.Drawing.Point(68, 66);
            this.btnChooseFont.Name = "btnChooseFont";
            this.btnChooseFont.Size = new System.Drawing.Size(75, 23);
            this.btnChooseFont.TabIndex = 7;
            this.btnChooseFont.Text = "Choose";
            this.btnChooseFont.UseVisualStyleBackColor = true;
            this.btnChooseFont.Click += new System.EventHandler(this.btnChooseFont_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 300);
            this.Controls.Add(this.btnChooseFont);
            this.Controls.Add(this.lblFont);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbDefaultAddress);
            this.Controls.Add(this.lblDefaultAddress);
            this.Controls.Add(this.tbNick);
            this.Controls.Add(this.lblNick);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.TextBox tbNick;
        private System.Windows.Forms.Label lblDefaultAddress;
        private System.Windows.Forms.TextBox tbDefaultAddress;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.Button btnChooseFont;
    }
}