namespace QuizMaster
{
    partial class LoginOverlay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            picLoginPanel = new PictureBox();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            picPasswordEye = new PictureBox();
            picLoginButton = new PictureBox();
            picRegisterButton = new PictureBox();
            picForgotPassword = new PictureBox();
            pnlClose = new Panel();
            picLoginErrorPanel = new PictureBox();
            lblLoginError = new OutlinedLabel();
            ((System.ComponentModel.ISupportInitialize)picLoginPanel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPasswordEye).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoginButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRegisterButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picForgotPassword).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoginErrorPanel).BeginInit();
            SuspendLayout();
            // 
            // picLoginPanel
            // 
            picLoginPanel.BackColor = Color.Transparent;
            picLoginPanel.BackgroundImage = Properties.Resources.overlay_login;
            picLoginPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picLoginPanel.Location = new Point(695, 97);
            picLoginPanel.Name = "picLoginPanel";
            picLoginPanel.Size = new Size(597, 843);
            picLoginPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picLoginPanel.TabIndex = 0;
            picLoginPanel.TabStop = false;
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtUsername.Location = new Point(840, 371);
            txtUsername.MaxLength = 15;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(348, 25);
            txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtPassword.Location = new Point(840, 465);
            txtPassword.MaxLength = 20;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(276, 25);
            txtPassword.TabIndex = 2;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // picPasswordEye
            // 
            picPasswordEye.BackColor = Color.Transparent;
            picPasswordEye.BackgroundImage = Properties.Resources.button_show_password;
            picPasswordEye.BackgroundImageLayout = ImageLayout.Stretch;
            picPasswordEye.Cursor = Cursors.Hand;
            picPasswordEye.Location = new Point(1122, 444);
            picPasswordEye.Name = "picPasswordEye";
            picPasswordEye.Size = new Size(74, 54);
            picPasswordEye.TabIndex = 3;
            picPasswordEye.TabStop = false;
            picPasswordEye.Click += picPasswordEye_Click;
            picPasswordEye.MouseEnter += picPasswordEye_MouseEnter;
            picPasswordEye.MouseLeave += picPasswordEye_MouseLeave;
            // 
            // picLoginButton
            // 
            picLoginButton.BackColor = Color.Transparent;
            picLoginButton.BackgroundImage = Properties.Resources.button_login_overlay;
            picLoginButton.BackgroundImageLayout = ImageLayout.Stretch;
            picLoginButton.Cursor = Cursors.Hand;
            picLoginButton.Location = new Point(817, 579);
            picLoginButton.Name = "picLoginButton";
            picLoginButton.Size = new Size(356, 115);
            picLoginButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picLoginButton.TabIndex = 4;
            picLoginButton.TabStop = false;
            picLoginButton.Click += picLoginButton_Click;
            picLoginButton.MouseEnter += picLoginButton_MouseEnter;
            picLoginButton.MouseLeave += picLoginButton_MouseLeave;
            // 
            // picRegisterButton
            // 
            picRegisterButton.BackColor = Color.Transparent;
            picRegisterButton.BackgroundImage = Properties.Resources.button_register_overlay;
            picRegisterButton.BackgroundImageLayout = ImageLayout.Stretch;
            picRegisterButton.Cursor = Cursors.Hand;
            picRegisterButton.Location = new Point(839, 742);
            picRegisterButton.Name = "picRegisterButton";
            picRegisterButton.Size = new Size(311, 105);
            picRegisterButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picRegisterButton.TabIndex = 5;
            picRegisterButton.TabStop = false;
            picRegisterButton.Click += picRegisterButton_Click;
            picRegisterButton.MouseEnter += picRegisterButton_MouseEnter;
            picRegisterButton.MouseLeave += picRegisterButton_MouseLeave;
            // 
            // picForgotPassword
            // 
            picForgotPassword.BackColor = Color.Transparent;
            picForgotPassword.BackgroundImage = Properties.Resources.button_forgot_password;
            picForgotPassword.BackgroundImageLayout = ImageLayout.Stretch;
            picForgotPassword.Cursor = Cursors.Hand;
            picForgotPassword.Location = new Point(770, 508);
            picForgotPassword.Name = "picForgotPassword";
            picForgotPassword.Size = new Size(195, 67);
            picForgotPassword.SizeMode = PictureBoxSizeMode.StretchImage;
            picForgotPassword.TabIndex = 6;
            picForgotPassword.TabStop = false;
            picForgotPassword.Click += picForgotPassword_Click;
            picForgotPassword.MouseEnter += picForgotPassword_MouseEnter;
            picForgotPassword.MouseLeave += picForgotPassword_MouseLeave;
            // 
            // pnlClose
            // 
            pnlClose.BackColor = Color.Transparent;
            pnlClose.Cursor = Cursors.Hand;
            pnlClose.Location = new Point(1178, 157);
            pnlClose.Name = "pnlClose";
            pnlClose.Size = new Size(84, 87);
            pnlClose.TabIndex = 7;
            pnlClose.Click += pnlClose_Click;
            // 
            // picLoginErrorPanel
            // 
            picLoginErrorPanel.BackColor = Color.Transparent;
            picLoginErrorPanel.BackgroundImage = Properties.Resources.overlay_login_failed;
            picLoginErrorPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picLoginErrorPanel.Location = new Point(172, 189);
            picLoginErrorPanel.Name = "picLoginErrorPanel";
            picLoginErrorPanel.Size = new Size(547, 359);
            picLoginErrorPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picLoginErrorPanel.TabIndex = 8;
            picLoginErrorPanel.TabStop = false;
            // 
            // lblLoginError
            // 
            lblLoginError.BackColor = Color.Transparent;
            lblLoginError.Font = new Font("Cooper Black", 18F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblLoginError.ForeColor = Color.Red;
            lblLoginError.Location = new Point(263, 395);
            lblLoginError.Name = "lblLoginError";
            lblLoginError.OutlineColor = Color.Black;
            lblLoginError.OutlineWidth = 2F;
            lblLoginError.Size = new Size(400, 69);
            lblLoginError.TabIndex = 19;
            lblLoginError.Text = "Incorrect username / email or password!";
            lblLoginError.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // LoginOverlay
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(lblLoginError);
            Controls.Add(picLoginErrorPanel);
            Controls.Add(pnlClose);
            Controls.Add(picForgotPassword);
            Controls.Add(picRegisterButton);
            Controls.Add(picLoginButton);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(picPasswordEye);
            Controls.Add(picLoginPanel);
            Name = "LoginOverlay";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picLoginPanel).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPasswordEye).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoginButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRegisterButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picForgotPassword).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoginErrorPanel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picLoginPanel;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private PictureBox picPasswordEye;
        private PictureBox picLoginButton;
        private PictureBox picRegisterButton;
        private PictureBox picForgotPassword;
        private Panel pnlClose;
        private PictureBox picLoginErrorPanel;
        private OutlinedLabel lblLoginError;
    }
}
