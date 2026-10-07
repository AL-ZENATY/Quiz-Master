namespace QuizMaster
{
    partial class RegisterOverlay
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
            picRegisterPanel = new PictureBox();
            pnlClose = new Panel();
            picRegisterButton = new PictureBox();
            picLoginButton = new PictureBox();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            picPasswordEye = new PictureBox();
            txtEmail = new TextBox();
            txtConfirmPassword = new TextBox();
            picRequirementsPanel = new PictureBox();
            lblUsernameLength = new OutlinedLabel();
            lblEmailValid = new OutlinedLabel();
            lblPasswordLength = new OutlinedLabel();
            lblUppercase = new OutlinedLabel();
            lblNumber = new OutlinedLabel();
            lblSpecialCharacter = new OutlinedLabel();
            lblPasswordsMatch = new OutlinedLabel();
            lblLowercase = new OutlinedLabel();
            lblRegistrationStatus = new OutlinedLabel();
            ((System.ComponentModel.ISupportInitialize)picRegisterPanel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRegisterButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoginButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picPasswordEye).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picRequirementsPanel).BeginInit();
            SuspendLayout();
            // 
            // picRegisterPanel
            // 
            picRegisterPanel.BackColor = Color.Transparent;
            picRegisterPanel.BackgroundImage = Properties.Resources.overlay_register;
            picRegisterPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picRegisterPanel.Location = new Point(618, 95);
            picRegisterPanel.Name = "picRegisterPanel";
            picRegisterPanel.Size = new Size(597, 843);
            picRegisterPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picRegisterPanel.TabIndex = 0;
            picRegisterPanel.TabStop = false;
            // 
            // pnlClose
            // 
            pnlClose.BackColor = Color.Transparent;
            pnlClose.Cursor = Cursors.Hand;
            pnlClose.Location = new Point(1120, 138);
            pnlClose.Name = "pnlClose";
            pnlClose.Size = new Size(84, 89);
            pnlClose.TabIndex = 13;
            pnlClose.Click += pnlClose_Click;
            // 
            // picRegisterButton
            // 
            picRegisterButton.BackColor = Color.Transparent;
            picRegisterButton.BackgroundImage = Properties.Resources.button_register_overlay;
            picRegisterButton.BackgroundImageLayout = ImageLayout.Stretch;
            picRegisterButton.Cursor = Cursors.Hand;
            picRegisterButton.Location = new Point(727, 631);
            picRegisterButton.Name = "picRegisterButton";
            picRegisterButton.Size = new Size(386, 122);
            picRegisterButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picRegisterButton.TabIndex = 12;
            picRegisterButton.TabStop = false;
            picRegisterButton.Click += picRegisterButton_Click;
            picRegisterButton.MouseEnter += picRegisterButton_MouseEnter;
            picRegisterButton.MouseLeave += picRegisterButton_MouseLeave;
            // 
            // picLoginButton
            // 
            picLoginButton.BackColor = Color.Transparent;
            picLoginButton.BackgroundImage = Properties.Resources.button_login_overlay;
            picLoginButton.BackgroundImageLayout = ImageLayout.Stretch;
            picLoginButton.Cursor = Cursors.Hand;
            picLoginButton.Location = new Point(759, 797);
            picLoginButton.Name = "picLoginButton";
            picLoginButton.Size = new Size(317, 107);
            picLoginButton.SizeMode = PictureBoxSizeMode.StretchImage;
            picLoginButton.TabIndex = 11;
            picLoginButton.TabStop = false;
            picLoginButton.Click += picLoginButton_Click;
            picLoginButton.MouseEnter += picLoginButton_MouseEnter;
            picLoginButton.MouseLeave += picLoginButton_MouseLeave;
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtPassword.Location = new Point(762, 508);
            txtPassword.MaxLength = 20;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Password";
            txtPassword.Size = new Size(289, 25);
            txtPassword.TabIndex = 9;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtUsername.Location = new Point(762, 425);
            txtUsername.MaxLength = 15;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Username";
            txtUsername.Size = new Size(341, 25);
            txtUsername.TabIndex = 8;
            // 
            // picPasswordEye
            // 
            picPasswordEye.BackColor = Color.Transparent;
            picPasswordEye.BackgroundImage = Properties.Resources.button_show_password;
            picPasswordEye.BackgroundImageLayout = ImageLayout.Stretch;
            picPasswordEye.Cursor = Cursors.Hand;
            picPasswordEye.Location = new Point(1057, 489);
            picPasswordEye.Name = "picPasswordEye";
            picPasswordEye.Size = new Size(74, 54);
            picPasswordEye.TabIndex = 10;
            picPasswordEye.TabStop = false;
            picPasswordEye.Click += picPasswordEye_Click;
            picPasswordEye.MouseEnter += picPasswordEye_MouseEnter;
            picPasswordEye.MouseLeave += picPasswordEye_MouseLeave;
            // 
            // txtEmail
            // 
            txtEmail.BorderStyle = BorderStyle.None;
            txtEmail.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtEmail.Location = new Point(771, 338);
            txtEmail.MaxLength = 254;
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.Size = new Size(341, 25);
            txtEmail.TabIndex = 14;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderStyle = BorderStyle.None;
            txtConfirmPassword.Font = new Font("Cooper Black", 15.75F, FontStyle.Italic | FontStyle.Underline);
            txtConfirmPassword.Location = new Point(762, 597);
            txtConfirmPassword.MaxLength = 20;
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Confirm Password";
            txtConfirmPassword.Size = new Size(341, 25);
            txtConfirmPassword.TabIndex = 15;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // picRequirementsPanel
            // 
            picRequirementsPanel.BackColor = Color.Transparent;
            picRequirementsPanel.BackgroundImage = Properties.Resources.overlay_requirements;
            picRequirementsPanel.BackgroundImageLayout = ImageLayout.Stretch;
            picRequirementsPanel.Location = new Point(1197, 301);
            picRequirementsPanel.Name = "picRequirementsPanel";
            picRequirementsPanel.Size = new Size(482, 620);
            picRequirementsPanel.SizeMode = PictureBoxSizeMode.Zoom;
            picRequirementsPanel.TabIndex = 16;
            picRequirementsPanel.TabStop = false;
            // 
            // lblUsernameLength
            // 
            lblUsernameLength.BackColor = Color.Transparent;
            lblUsernameLength.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblUsernameLength.ForeColor = Color.Red;
            lblUsernameLength.Location = new Point(1267, 519);
            lblUsernameLength.Name = "lblUsernameLength";
            lblUsernameLength.OutlineColor = Color.Black;
            lblUsernameLength.OutlineWidth = 2F;
            lblUsernameLength.Size = new Size(369, 23);
            lblUsernameLength.TabIndex = 17;
            lblUsernameLength.Text = "Username: 2–15 characters";
            lblUsernameLength.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblEmailValid
            // 
            lblEmailValid.BackColor = Color.Transparent;
            lblEmailValid.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblEmailValid.ForeColor = Color.Red;
            lblEmailValid.Location = new Point(1267, 489);
            lblEmailValid.Name = "lblEmailValid";
            lblEmailValid.OutlineColor = Color.Black;
            lblEmailValid.OutlineWidth = 2F;
            lblEmailValid.Size = new Size(369, 23);
            lblEmailValid.TabIndex = 18;
            lblEmailValid.Text = "Valid email address";
            lblEmailValid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPasswordLength
            // 
            lblPasswordLength.BackColor = Color.Transparent;
            lblPasswordLength.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblPasswordLength.ForeColor = Color.Red;
            lblPasswordLength.Location = new Point(1267, 566);
            lblPasswordLength.Name = "lblPasswordLength";
            lblPasswordLength.OutlineColor = Color.Black;
            lblPasswordLength.OutlineWidth = 2F;
            lblPasswordLength.Size = new Size(369, 23);
            lblPasswordLength.TabIndex = 19;
            lblPasswordLength.Text = "Password: 8–20 characters";
            lblPasswordLength.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUppercase
            // 
            lblUppercase.BackColor = Color.Transparent;
            lblUppercase.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblUppercase.ForeColor = Color.Red;
            lblUppercase.Location = new Point(1266, 664);
            lblUppercase.Name = "lblUppercase";
            lblUppercase.OutlineColor = Color.Black;
            lblUppercase.OutlineWidth = 2F;
            lblUppercase.Size = new Size(366, 23);
            lblUppercase.TabIndex = 22;
            lblUppercase.Text = "Contains at least 1 uppercase letter";
            lblUppercase.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblNumber
            // 
            lblNumber.BackColor = Color.Transparent;
            lblNumber.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblNumber.ForeColor = Color.Red;
            lblNumber.Location = new Point(1267, 631);
            lblNumber.Name = "lblNumber";
            lblNumber.OutlineColor = Color.Black;
            lblNumber.OutlineWidth = 2F;
            lblNumber.Size = new Size(366, 23);
            lblNumber.TabIndex = 21;
            lblNumber.Text = "Contains at least 1 number";
            lblNumber.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSpecialCharacter
            // 
            lblSpecialCharacter.BackColor = Color.Transparent;
            lblSpecialCharacter.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblSpecialCharacter.ForeColor = Color.Red;
            lblSpecialCharacter.Location = new Point(1267, 597);
            lblSpecialCharacter.Name = "lblSpecialCharacter";
            lblSpecialCharacter.OutlineColor = Color.Black;
            lblSpecialCharacter.OutlineWidth = 2F;
            lblSpecialCharacter.Size = new Size(369, 23);
            lblSpecialCharacter.TabIndex = 20;
            lblSpecialCharacter.Text = "Contains !, #, ? or @";
            lblSpecialCharacter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPasswordsMatch
            // 
            lblPasswordsMatch.BackColor = Color.Transparent;
            lblPasswordsMatch.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblPasswordsMatch.ForeColor = Color.Red;
            lblPasswordsMatch.Location = new Point(1266, 730);
            lblPasswordsMatch.Name = "lblPasswordsMatch";
            lblPasswordsMatch.OutlineColor = Color.Black;
            lblPasswordsMatch.OutlineWidth = 2F;
            lblPasswordsMatch.Size = new Size(365, 23);
            lblPasswordsMatch.TabIndex = 24;
            lblPasswordsMatch.Text = "Passwords match";
            lblPasswordsMatch.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblLowercase
            // 
            lblLowercase.BackColor = Color.Transparent;
            lblLowercase.Font = new Font("Cooper Black", 14.25F, FontStyle.Italic);
            lblLowercase.ForeColor = Color.Red;
            lblLowercase.Location = new Point(1266, 697);
            lblLowercase.Name = "lblLowercase";
            lblLowercase.OutlineColor = Color.Black;
            lblLowercase.OutlineWidth = 2F;
            lblLowercase.Size = new Size(365, 23);
            lblLowercase.TabIndex = 23;
            lblLowercase.Text = "Contains at least 1 lowercase letter";
            lblLowercase.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblRegistrationStatus
            // 
            lblRegistrationStatus.BackColor = Color.Transparent;
            lblRegistrationStatus.Font = new Font("Cooper Black", 12.75F, FontStyle.Italic);
            lblRegistrationStatus.ForeColor = Color.Red;
            lblRegistrationStatus.Location = new Point(1266, 787);
            lblRegistrationStatus.Name = "lblRegistrationStatus";
            lblRegistrationStatus.OutlineColor = Color.Black;
            lblRegistrationStatus.OutlineWidth = 2F;
            lblRegistrationStatus.Size = new Size(370, 80);
            lblRegistrationStatus.TabIndex = 25;
            lblRegistrationStatus.Text = "Please make sure all the fields filled";
            lblRegistrationStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // RegisterOverlay
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Black;
            Controls.Add(lblRegistrationStatus);
            Controls.Add(lblPasswordsMatch);
            Controls.Add(lblLowercase);
            Controls.Add(lblUppercase);
            Controls.Add(lblNumber);
            Controls.Add(lblSpecialCharacter);
            Controls.Add(lblPasswordLength);
            Controls.Add(lblEmailValid);
            Controls.Add(lblUsernameLength);
            Controls.Add(picRequirementsPanel);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtEmail);
            Controls.Add(pnlClose);
            Controls.Add(picRegisterButton);
            Controls.Add(picLoginButton);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(picPasswordEye);
            Controls.Add(picRegisterPanel);
            Name = "RegisterOverlay";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picRegisterPanel).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRegisterButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoginButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)picPasswordEye).EndInit();
            ((System.ComponentModel.ISupportInitialize)picRequirementsPanel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picRegisterPanel;
        private Panel pnlClose;
        private PictureBox picRegisterButton;
        private PictureBox picLoginButton;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private PictureBox picPasswordEye;
        private TextBox txtEmail;
        private TextBox txtConfirmPassword;
        private PictureBox picRequirementsPanel;
        private OutlinedLabel lblUsernameLength;
        private OutlinedLabel lblEmailValid;
        private OutlinedLabel lblPasswordLength;
        private OutlinedLabel lblUppercase;
        private OutlinedLabel lblNumber;
        private OutlinedLabel lblSpecialCharacter;
        private OutlinedLabel lblPasswordsMatch;
        private OutlinedLabel lblLowercase;
        private OutlinedLabel lblRegistrationStatus;
    }
}
