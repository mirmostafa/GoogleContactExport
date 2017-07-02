namespace GoogleContactExport
{
    partial class GCE_MainForm
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
            this.btnAuth = new System.Windows.Forms.Button();
            this.btnGetContacts = new System.Windows.Forms.Button();
            this.pbrContacts = new System.Windows.Forms.ProgressBar();
            this.btnRemoveAuth = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAuth
            // 
            this.btnAuth.Location = new System.Drawing.Point(12, 12);
            this.btnAuth.Name = "btnAuth";
            this.btnAuth.Size = new System.Drawing.Size(83, 23);
            this.btnAuth.TabIndex = 0;
            this.btnAuth.Text = "Start Auth";
            this.btnAuth.UseVisualStyleBackColor = true;
            this.btnAuth.Click += new System.EventHandler(this.btnAuth_Click);
            // 
            // btnGetContacts
            // 
            this.btnGetContacts.Enabled = false;
            this.btnGetContacts.Location = new System.Drawing.Point(192, 12);
            this.btnGetContacts.Name = "btnGetContacts";
            this.btnGetContacts.Size = new System.Drawing.Size(83, 23);
            this.btnGetContacts.TabIndex = 1;
            this.btnGetContacts.Text = "Import";
            this.btnGetContacts.UseVisualStyleBackColor = true;
            this.btnGetContacts.Click += new System.EventHandler(this.btnGetContacts_Click);
            // 
            // pbrContacts
            // 
            this.pbrContacts.Location = new System.Drawing.Point(12, 41);
            this.pbrContacts.Name = "pbrContacts";
            this.pbrContacts.Size = new System.Drawing.Size(263, 23);
            this.pbrContacts.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbrContacts.TabIndex = 3;
            // 
            // btnRemoveAuth
            // 
            this.btnRemoveAuth.Enabled = false;
            this.btnRemoveAuth.Location = new System.Drawing.Point(102, 12);
            this.btnRemoveAuth.Name = "btnRemoveAuth";
            this.btnRemoveAuth.Size = new System.Drawing.Size(83, 23);
            this.btnRemoveAuth.TabIndex = 4;
            this.btnRemoveAuth.Text = "Remove Auth";
            this.btnRemoveAuth.UseVisualStyleBackColor = true;
            this.btnRemoveAuth.Click += new System.EventHandler(this.btnRemoveAuth_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 71);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(196, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "You should not be able to see this text...";
            // 
            // GCE_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 95);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnRemoveAuth);
            this.Controls.Add(this.pbrContacts);
            this.Controls.Add(this.btnGetContacts);
            this.Controls.Add(this.btnAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GCE_MainForm";
            this.Text = "GCE_MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GCE_MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.GCE_MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAuth;
        private System.Windows.Forms.Button btnGetContacts;
        private System.Windows.Forms.ProgressBar pbrContacts;
        private System.Windows.Forms.Button btnRemoveAuth;
        private System.Windows.Forms.Label lblStatus;
    }
}