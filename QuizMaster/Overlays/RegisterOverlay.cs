
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class RegisterOverlay : UserControl
    {
        public RegisterOverlay()
        {
            InitializeComponent();

            // Live registration validation.
            txtUsername.TextChanged += UpdateRequirements;
            txtEmail.TextChanged += UpdateRequirements;
            txtPassword.TextChanged += UpdateRequirements;
            txtConfirmPassword.TextChanged += UpdateRequirements;

            // Make requirement labels transparent over the artwork.
            Label[] requirementLabels =
            {
                lblEmailValid,
                lblUsernameLength,
                lblPasswordLength,
                lblSpecialCharacter,
                lblNumber,
                lblUppercase,
                lblLowercase,
                lblPasswordsMatch,
                lblRegistrationStatus
            };

            foreach (Label label in requirementLabels)
            {
                Point position = picRequirementsPanel.PointToClient(
                    label.PointToScreen(Point.Empty));

                label.Parent = picRequirementsPanel;
                label.Location = position;
                label.BackColor = Color.Transparent;
                label.BringToFront();
            }

            // Keep existing buttons positioned over the register artwork.
            MoveToPanel(picRegisterButton, picRegisterPanel);
            MoveToPanel(picLoginButton, picRegisterPanel);
            MoveToPanel(picPasswordEye, picRegisterPanel);
            MoveToPanel(pnlClose, picRegisterPanel);

            UpdateRequirements(null, EventArgs.Empty);
        }

        private void MoveToPanel(Control control, Control panel)
        {
            Point position = panel.PointToClient(
                control.PointToScreen(Point.Empty));

            control.Parent = panel;
            control.Location = position;
            control.BackColor = Color.Transparent;
            control.BringToFront();
        }

        private static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(
                email,
                @"^[^@\s.]+(\.[^@\s.]+)*@[^@\s.]+(\.[^@\s.]+)*\.[A-Za-z]{2,}$");
        }

        private void UpdateRequirements(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            SetRequirement(lblUsernameLength,
                username.Length >= 2 && username.Length <= 15);

            SetRequirement(lblEmailValid, IsValidEmail(email));

            SetRequirement(lblPasswordLength,
                password.Length >= 8 && password.Length <= 20);

            SetRequirement(lblSpecialCharacter,
                password.Any(c => "!#?@".Contains(c)));

            SetRequirement(lblNumber,
                password.Any(char.IsDigit));

            SetRequirement(lblUppercase,
                password.Any(char.IsUpper));

            SetRequirement(lblLowercase,
                password.Any(char.IsLower));

            SetRequirement(lblPasswordsMatch,
                confirmPassword.Length > 0 &&
                password == confirmPassword);
        }

        private void SetRequirement(Label label, bool valid)
        {
            label.ForeColor = valid ? Color.LimeGreen : Color.Red;
        }

        private void picRegisterButton_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            bool validPassword =
                password.Length >= 8 &&
                password.Length <= 20 &&
                password.Any(c => "!#?@".Contains(c)) &&
                password.Any(char.IsDigit) &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower);

            string message;
            bool success = false;

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                message = "Please fill in all fields!";
            }
            else if (username.Length < 2 || username.Length > 15)
            {
                message = "Invalid username length!";
            }
            else if (!IsValidEmail(email))
            {
                message = "Invalid email address!";
            }
            else if (!validPassword)
            {
                message = "Password requirements not met!";
            }
            else if (password != confirmPassword)
            {
                message = "Passwords do not match!";
            }
            else
            {
                message = QuizDatabase.RegisterUser(
                    username,
                    email,
                    password);

                success = message == "Account created successfully!";
            }

            lblRegistrationStatus.Text = message;
            lblRegistrationStatus.ForeColor =
                success ? Color.LimeGreen : Color.Red;
        }

        private void pnlClose_Click(object sender, EventArgs e)
        {
            Parent?.Controls.Remove(this);
            Dispose();
        }

        private void picPasswordEye_Click(object sender, EventArgs e)
        {
            bool hide = !txtPassword.UseSystemPasswordChar;

            txtPassword.UseSystemPasswordChar = hide;
            txtConfirmPassword.UseSystemPasswordChar = hide;

            UpdatePasswordEye();
        }

        private void picPasswordEye_MouseEnter(object sender, EventArgs e)
        {
            UpdatePasswordEye();
        }

        private void picPasswordEye_MouseLeave(object sender, EventArgs e)
        {
            UpdatePasswordEye();
        }

        private void UpdatePasswordEye()
        {
            bool hidden = txtPassword.UseSystemPasswordChar;

            bool hovered = picPasswordEye.ClientRectangle.Contains(
                picPasswordEye.PointToClient(Cursor.Position));

            if (hidden)
            {
                picPasswordEye.BackgroundImage = hovered
                    ? Properties.Resources.button_show_password_hover
                    : Properties.Resources.button_show_password;
            }
            else
            {
                picPasswordEye.BackgroundImage = hovered
                    ? Properties.Resources.button_hide_password_hover
                    : Properties.Resources.button_hide_password;
            }
        }

        private void picRegisterButton_MouseEnter(object sender, EventArgs e)
        {
            picRegisterButton.BackgroundImage =
                Properties.Resources.button_register_overlay_hover;
        }

        private void picRegisterButton_MouseLeave(object sender, EventArgs e)
        {
            picRegisterButton.BackgroundImage =
                Properties.Resources.button_register_overlay;
        }

        private void picLoginButton_MouseEnter(object sender, EventArgs e)
        {
            picLoginButton.BackgroundImage =
                Properties.Resources.button_login_overlay_hover;
        }

        private void picLoginButton_MouseLeave(object sender, EventArgs e)
        {
            picLoginButton.BackgroundImage =
                Properties.Resources.button_login_overlay;
        }

        private void picLoginButton_Click(object sender, EventArgs e)
        {
            LoginOverlay login = new LoginOverlay();

            login.BackgroundImage = BackgroundImage;
            login.BackgroundImageLayout = ImageLayout.Stretch;
            login.Dock = DockStyle.Fill;

            Control parent = Parent!;

            parent.Controls.Add(login);
            login.BringToFront();

            parent.Controls.Remove(this);
            BackgroundImage = null;
            Dispose();
        }
    }
}
