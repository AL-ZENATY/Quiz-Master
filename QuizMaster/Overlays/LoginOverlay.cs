
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class LoginOverlay : UserControl
    {
        // Automatically hide login errors after 5 seconds.
        private readonly System.Windows.Forms.Timer loginErrorTimer = new()
        {
            Interval = 5000
        };

        public LoginOverlay()
        {
            InitializeComponent();

            loginErrorTimer.Tick += LoginErrorTimer_Tick;

            // Position the error label over its artwork.
            Point errorPosition = picLoginErrorPanel.PointToClient(
                lblLoginError.PointToScreen(Point.Empty));

            lblLoginError.Parent = picLoginErrorPanel;
            lblLoginError.Location = errorPosition;
            lblLoginError.BackColor = Color.Transparent;
            lblLoginError.Visible = true;

            picLoginErrorPanel.Visible = false;

            // Position the controls over the login artwork.
            MoveToPanel(picPasswordEye, picLoginPanel);
            MoveToPanel(picLoginButton, picLoginPanel);
            MoveToPanel(picRegisterButton, picLoginPanel);
            MoveToPanel(picForgotPassword, picLoginPanel);
            MoveToPanel(pnlClose, picLoginPanel);
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

        private void LoginErrorTimer_Tick(object? sender, EventArgs e)
        {
            loginErrorTimer.Stop();
            picLoginErrorPanel.Visible = false;
        }

        private void ShowLoginError(string message)
        {
            // Restart the countdown for every new error.
            loginErrorTimer.Stop();

            lblLoginError.Text = message;
            lblLoginError.ForeColor = Color.Red;

            picLoginErrorPanel.Visible = true;
            picLoginErrorPanel.BringToFront();
            lblLoginError.BringToFront();

            loginErrorTimer.Start();
        }

        private void picLoginButton_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(usernameOrEmail) ||
                string.IsNullOrWhiteSpace(password))
            {
                ShowLoginError("Please fill in all fields!");
                return;
            }

            if (!QuizDatabase.ValidateLogin(usernameOrEmail, password))
            {
                ShowLoginError("Incorrect username/email or password!");
                return;
            }

            // Only authenticated users can reach this point.
            Control? startPage = Parent;

            if (startPage?.Parent == null)
                return;

            Control container = startPage.Parent;

            loginErrorTimer.Stop();
            picLoginErrorPanel.Visible = false;

            // Set the session BEFORE creating CategorySelection.
            string actualUsername = QuizDatabase.GetUsername(usernameOrEmail)!;
            int userId = QuizDatabase.GetUserId(usernameOrEmail)!.Value;

            UserSession.Login(userId, actualUsername);

            CategorySelection categories = new CategorySelection();
            categories.Dock = DockStyle.Fill;

            container.Controls.Add(categories);
            categories.BringToFront();

            container.Controls.Remove(startPage);
            startPage.Dispose();
        }

        private void picPasswordEye_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !txtPassword.UseSystemPasswordChar;

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

        private void picLoginButton_MouseEnter(object sender, EventArgs e)
        {
            picLoginButton.Image =
                Properties.Resources.button_login_overlay_hover;
        }

        private void picLoginButton_MouseLeave(object sender, EventArgs e)
        {
            picLoginButton.Image =
                Properties.Resources.button_login_overlay;
        }

        private void picRegisterButton_MouseEnter(object sender, EventArgs e)
        {
            picRegisterButton.Image =
                Properties.Resources.button_register_overlay_hover;
        }

        private void picRegisterButton_MouseLeave(object sender, EventArgs e)
        {
            picRegisterButton.Image =
                Properties.Resources.button_register_overlay;
        }

        private void picForgotPassword_MouseEnter(object sender, EventArgs e)
        {
            picForgotPassword.Image =
                Properties.Resources.button_forgot_password_hover;
        }

        private void picForgotPassword_MouseLeave(object sender, EventArgs e)
        {
            picForgotPassword.Image =
                Properties.Resources.button_forgot_password;
        }

        private void pnlClose_Click(object sender, EventArgs e)
        {
            loginErrorTimer.Stop();

            Parent?.Controls.Remove(this);
            Dispose();
        }

        private void picForgotPassword_Click(object sender, EventArgs e)
        {
            picForgotPanel forgot = new picForgotPanel();

            forgot.Dock = DockStyle.None;

            forgot.Location = new Point(
                (Width - forgot.Width) / 2,
                (Height - forgot.Height) / 2
            );

            Controls.Add(forgot);
            forgot.BringToFront();
        }

        private void picRegisterButton_Click(object sender, EventArgs e)
        {
            loginErrorTimer.Stop();

            RegisterOverlay register = new RegisterOverlay();

            register.BackgroundImage = BackgroundImage;
            register.BackgroundImageLayout = ImageLayout.Stretch;
            register.Dock = DockStyle.Fill;

            Control parent = Parent!;

            parent.Controls.Add(register);
            register.BringToFront();

            parent.Controls.Remove(this);
            BackgroundImage = null;
            Dispose();
        }
    }
}
