
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void picExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void picExit_MouseEnter(object sender, EventArgs e)
        {
            picExit.Image = Properties.Resources.button_exit_hover;
        }

        private void picExit_MouseLeave(object sender, EventArgs e)
        {
            picExit.Image = Properties.Resources.button_exit;
        }

        private void picLogin_Click(object sender, EventArgs e)
        {
            // Capture the StartPage
            Bitmap background = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(background,
                new Rectangle(0, 0, this.Width, this.Height));

            // Blur the background
            int smallWidth = Math.Max(1, background.Width / 12);
            int smallHeight = Math.Max(1, background.Height / 12);

            using (Bitmap small = new Bitmap(
                background, new Size(smallWidth, smallHeight)))
            using (Bitmap blurred = new Bitmap(
                background.Width, background.Height))
            {
                using (Graphics g = Graphics.FromImage(blurred))
                {
                    g.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                    g.DrawImage(small, 0, 0,
                        blurred.Width, blurred.Height);
                }

                background.Dispose();
                background = new Bitmap(blurred);
            }

            // Darken the screenshot
            using (Graphics g = Graphics.FromImage(background))
            using (SolidBrush brush = new SolidBrush(
                Color.FromArgb(170, 0, 0, 0)))
            {
                g.FillRectangle(brush, 0, 0,
                    background.Width, background.Height);
            }

            // Open the login overlay
            LoginOverlay login = new LoginOverlay();

            login.BackgroundImage = background;
            login.BackgroundImageLayout = ImageLayout.Stretch;
            login.Dock = DockStyle.Fill;

            this.Controls.Add(login);
            login.BringToFront();
        }

        private void picLogin_MouseEnter(object sender, EventArgs e)
        {
            picLogin.Image = Properties.Resources.button_login_hover;
        }

        private void picLogin_MouseLeave(object sender, EventArgs e)
        {
            picLogin.Image = Properties.Resources.button_login;
        }


        private void picRegister_Click(object sender, EventArgs e)
        {
            // Capture the StartPage
            Bitmap background = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(background,
                new Rectangle(0, 0, this.Width, this.Height));

            // Blur the background
            int smallWidth = Math.Max(1, background.Width / 12);
            int smallHeight = Math.Max(1, background.Height / 12);

            using (Bitmap small = new Bitmap(
                background, new Size(smallWidth, smallHeight)))
            using (Bitmap blurred = new Bitmap(
                background.Width, background.Height))
            {
                using (Graphics g = Graphics.FromImage(blurred))
                {
                    g.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                    g.DrawImage(small, 0, 0,
                        blurred.Width, blurred.Height);
                }

                background.Dispose();
                background = new Bitmap(blurred);
            }

            // Darken the screenshot
            using (Graphics g = Graphics.FromImage(background))
            using (SolidBrush brush = new SolidBrush(
                Color.FromArgb(170, 0, 0, 0)))
            {
                g.FillRectangle(brush, 0, 0,
                    background.Width, background.Height);
            }

            // Open the register overlay
            RegisterOverlay register = new RegisterOverlay();

            register.BackgroundImage = background;
            register.BackgroundImageLayout = ImageLayout.Stretch;
            register.Dock = DockStyle.Fill;

            this.Controls.Add(register);
            register.BringToFront();
        }


        private void picRegister_MouseEnter(object sender, EventArgs e)
        {
            picRegister.Image = Properties.Resources.button_register_hover;
        }

        private void picRegister_MouseLeave(object sender, EventArgs e)
        {
            picRegister.Image = Properties.Resources.button_register;
        }


        private void picGuest_Click(object sender, EventArgs e)
        {
            // Capture the StartPage
            Bitmap background = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(background,
                new Rectangle(0, 0, this.Width, this.Height));

            // Blur the background
            int smallWidth = Math.Max(1, background.Width / 12);
            int smallHeight = Math.Max(1, background.Height / 12);

            using (Bitmap small = new Bitmap(
                background, new Size(smallWidth, smallHeight)))
            using (Bitmap blurred = new Bitmap(
                background.Width, background.Height))
            {
                using (Graphics g = Graphics.FromImage(blurred))
                {
                    g.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                    g.DrawImage(small, 0, 0,
                        blurred.Width, blurred.Height);
                }

                background.Dispose();
                background = new Bitmap(blurred);
            }

            // Darken the screenshot
            using (Graphics g = Graphics.FromImage(background))
            using (SolidBrush brush = new SolidBrush(
                Color.FromArgb(170, 0, 0, 0)))
            {
                g.FillRectangle(brush, 0, 0,
                    background.Width, background.Height);
            }

            // Open the Guest overlay
            GuestOverlay guest = new GuestOverlay();

            guest.BackgroundImage = background;
            guest.BackgroundImageLayout = ImageLayout.Stretch;
            guest.Dock = DockStyle.Fill;

            this.Controls.Add(guest);
            guest.BringToFront();
        }

        private void picGuest_MouseEnter(object sender, EventArgs e)
        {
            picGuest.Image = Properties.Resources.button_guest_hover;
        }

        private void picGuest_MouseLeave(object sender, EventArgs e)
        {
            picGuest.Image = Properties.Resources.button_guest;
        }
    }
}
