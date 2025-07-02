namespace MsalExample
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.Button ExitButton;
            System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Button SignInButton;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.AccessTokenSourceLabel = new System.Windows.Forms.Label();
            this.SignOutButton = new System.Windows.Forms.Button();
            this.SignInCallToActionLabel = new System.Windows.Forms.Label();
            this.GraphResultsPanel = new System.Windows.Forms.Panel();
            this.GraphResultsTextBox = new System.Windows.Forms.TextBox();
            ExitButton = new System.Windows.Forms.Button();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label2 = new System.Windows.Forms.Label();
            SignInButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            tableLayoutPanel1.SuspendLayout();
            this.GraphResultsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            ExitButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            ExitButton.Location = new System.Drawing.Point(432, 598);
            ExitButton.Name = "ExitButton";
            ExitButton.Size = new System.Drawing.Size(112, 34);
            ExitButton.TabIndex = 3;
            ExitButton.Text = "E&xit";
            ExitButton.UseVisualStyleBackColor = true;
            ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel1.Controls.Add(this.AccessTokenSourceLabel, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(this.SignOutButton, 2, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 486);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new System.Drawing.Size(954, 43);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // AccessTokenSourceLabel
            // 
            this.AccessTokenSourceLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.AccessTokenSourceLabel.AutoSize = true;
            this.AccessTokenSourceLabel.Location = new System.Drawing.Point(123, 9);
            this.AccessTokenSourceLabel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.AccessTokenSourceLabel.Name = "AccessTokenSourceLabel";
            this.AccessTokenSourceLabel.Size = new System.Drawing.Size(218, 25);
            this.AccessTokenSourceLabel.TabIndex = 1;
            this.AccessTokenSourceLabel.Text = "[Cached | Newly Acquired]";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 9);
            label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(120, 25);
            label2.TabIndex = 0;
            label2.Text = "Access Token:";
            // 
            // SignOutButton
            // 
            this.SignOutButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SignOutButton.Location = new System.Drawing.Point(839, 4);
            this.SignOutButton.Name = "SignOutButton";
            this.SignOutButton.Size = new System.Drawing.Size(112, 34);
            this.SignOutButton.TabIndex = 2;
            this.SignOutButton.Text = "Sign &Out";
            this.SignOutButton.UseVisualStyleBackColor = true;
            this.SignOutButton.Click += new System.EventHandler(this.SignOutButton_Click);
            // 
            // SignInButton
            // 
            SignInButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            SignInButton.Location = new System.Drawing.Point(329, 17);
            SignInButton.Name = "SignInButton";
            SignInButton.Size = new System.Drawing.Size(315, 34);
            SignInButton.TabIndex = 0;
            SignInButton.Text = "&Sign In (if needed) && Call Graph";
            SignInButton.UseVisualStyleBackColor = true;
            SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(226, 25);
            label1.TabIndex = 0;
            label1.Text = "Microsoft Graph Response:";
            // 
            // SignInCallToActionLabel
            // 
            this.SignInCallToActionLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.SignInCallToActionLabel.AutoSize = true;
            this.SignInCallToActionLabel.Location = new System.Drawing.Point(189, 211);
            this.SignInCallToActionLabel.Name = "SignInCallToActionLabel";
            this.SignInCallToActionLabel.Size = new System.Drawing.Size(578, 75);
            this.SignInCallToActionLabel.TabIndex = 2;
            this.SignInCallToActionLabel.Text = "This application will access Microsoft Graph, if you authorize it to do so.\r\n\r\nCl" +
    "ick the Sign In button above to get started.";
            this.SignInCallToActionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.SignInCallToActionLabel.UseMnemonic = false;
            // 
            // GraphResultsPanel
            // 
            this.GraphResultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphResultsPanel.Controls.Add(label1);
            this.GraphResultsPanel.Controls.Add(this.GraphResultsTextBox);
            this.GraphResultsPanel.Controls.Add(tableLayoutPanel1);
            this.GraphResultsPanel.Location = new System.Drawing.Point(12, 63);
            this.GraphResultsPanel.Name = "GraphResultsPanel";
            this.GraphResultsPanel.Size = new System.Drawing.Size(954, 529);
            this.GraphResultsPanel.TabIndex = 1;
            this.GraphResultsPanel.Visible = false;
            // 
            // GraphResultsTextBox
            // 
            this.GraphResultsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphResultsTextBox.Location = new System.Drawing.Point(8, 28);
            this.GraphResultsTextBox.Multiline = true;
            this.GraphResultsTextBox.Name = "GraphResultsTextBox";
            this.GraphResultsTextBox.ReadOnly = true;
            this.GraphResultsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.GraphResultsTextBox.Size = new System.Drawing.Size(940, 452);
            this.GraphResultsTextBox.TabIndex = 1;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = ExitButton;
            this.ClientSize = new System.Drawing.Size(978, 644);
            this.Controls.Add(this.GraphResultsPanel);
            this.Controls.Add(SignInButton);
            this.Controls.Add(ExitButton);
            this.Controls.Add(this.SignInCallToActionLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainWindow";
            this.Text = "MSAL Windows Forms Sample";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            this.GraphResultsPanel.ResumeLayout(false);
            this.GraphResultsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label SignInCallToActionLabel;
        private Panel GraphResultsPanel;
        private Label AccessTokenSourceLabel;
        private Button SignOutButton;
        private TextBox GraphResultsTextBox;
    }
}