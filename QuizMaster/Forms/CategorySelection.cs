
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizMaster.Forms;

namespace QuizMaster
{
    public partial class CategorySelection : UserControl
    {
        // ==========================================
        // CATEGORY CAROUSEL
        // ==========================================

        private bool showRapidFire = false;

        private int firstVisibleCategory = 0;

        private void UpdateCategoryNavigation()
        {
            // Flags and Cars always stay in place.
            picFlagsButton.Tag = "Flags";
            picCarsButton.Tag = "Cars";

            // Only the third category changes.
            string category = showRapidFire
                ? "Rapid Fire"
                : "Mixed";

            picMixedButton.Tag = category;

            picMixedButton.BackgroundImage =
                GetCategoryImage(category, false);
        }

        private Image GetCategoryImage(
            string category,
            bool hovered)
        {
            return category switch
            {
                "Flags" => hovered
                    ? Properties.Resources.flags_category_hover
                    : Properties.Resources.flags_category,

                "Cars" => hovered
                    ? Properties.Resources.cars_category_hover
                    : Properties.Resources.cars_category,

                "Mixed" => hovered
                    ? Properties.Resources.mixed_category_hover
                    : Properties.Resources.mixed_category,

                "Rapid Fire" => hovered
                    ? Properties.Resources.rapidfire_category_hover
                    : Properties.Resources.rapidfire_category,

                _ => Properties.Resources.flags_category
            };
        }

        // ==========================================
        // CATEGORY HOVER EFFECTS
        // ==========================================

        private void CategoryBox_MouseEnter(
            object sender,
            EventArgs e)
        {
            if (sender is PictureBox box &&
                box.Tag is string category)
            {
                box.BackgroundImage =
                    GetCategoryImage(category, true);
            }
        }

        private void CategoryBox_MouseLeave(
            object sender,
            EventArgs e)
        {
            if (sender is PictureBox box &&
                box.Tag is string category)
            {
                box.BackgroundImage =
                    GetCategoryImage(category, false);
            }
        }

        // ==========================================
        // CATEGORY CLICK
        // ==========================================

        private void CategoryBox_Click(
            object sender,
            EventArgs e)
        {
            if (sender is not PictureBox box)
                return;

            UserControl? quiz = box.Tag?.ToString() switch
            {
                "Flags" => new FlagsQuiz(),
                "Cars" => new CarsQuiz(),
                "Mixed" => new MixedQuiz(),
                "Rapid Fire" => new RapidFire(),
                _ => null
            };

            if (quiz == null)
                return;

            quiz.Dock = DockStyle.Fill;

            Controls.Add(quiz);
            quiz.BringToFront();
        }

        // ==========================================
        // SWITCH CATEGORY BUTTON
        // ==========================================

        private void picSwitchCategory_Click(
            object sender,
            EventArgs e)
        {
            showRapidFire = !showRapidFire;

            UpdateCategoryNavigation();
        }

        private void picSwitchCategory_MouseEnter(
            object sender,
            EventArgs e)
        {
            picSwitchCategory.BackgroundImage =
                Properties.Resources.button_rotating_arrows_hover;
        }

        private void picSwitchCategory_MouseLeave(
            object sender,
            EventArgs e)
        {
            picSwitchCategory.BackgroundImage =
                Properties.Resources.button_rotating_arrows;
        }

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public CategorySelection()
        {
            InitializeComponent();

            SetUsername(UserSession.Username ?? "GUEST");

            // Remove original fixed-category handlers.
            picFlagsButton.Click -= picFlagsButton_Click;
            picFlagsButton.MouseEnter -= picFlagsButton_MouseEnter;
            picFlagsButton.MouseLeave -= picFlagsButton_MouseLeave;

            picCarsButton.Click -= picCarsButton_Click;
            picCarsButton.MouseEnter -= picCarsButton_MouseEnter;
            picCarsButton.MouseLeave -= picCarsButton_MouseLeave;

            picMixedButton.Click -= picMixedButton_Click;
            picMixedButton.MouseEnter -= picMixedButton_MouseEnter;
            picMixedButton.MouseLeave -= picMixedButton_MouseLeave;

            // Connect the three category slots.
            foreach (PictureBox box in new[]
            {
                picFlagsButton,
                picCarsButton,
                picMixedButton
            })
            {
                box.Click += CategoryBox_Click;
                box.MouseEnter += CategoryBox_MouseEnter;
                box.MouseLeave += CategoryBox_MouseLeave;
            }


            // Set up the category switch button.
            picSwitchCategory.BackgroundImage =
                Properties.Resources.button_rotating_arrows;

            picSwitchCategory.BackgroundImageLayout =
                ImageLayout.Stretch;

            UpdateCategoryNavigation();
        }

        // ==========================================
        // INFORMATION OVERLAY
        // ==========================================

        private void picInfoButton_Click(object sender, EventArgs e)
        {
            using (Bitmap screenshot = new Bitmap(Width, Height))
            {
                DrawToBitmap(
                    screenshot,
                    new Rectangle(0, 0, Width, Height));

                int smallWidth = Math.Max(1, Width / 12);
                int smallHeight = Math.Max(1, Height / 12);

                using (Bitmap small = new Bitmap(
                    screenshot,
                    smallWidth,
                    smallHeight))
                using (Bitmap blurred = new Bitmap(Width, Height))
                using (Graphics g = Graphics.FromImage(blurred))
                {
                    g.InterpolationMode =
                        System.Drawing.Drawing2D.InterpolationMode
                            .HighQualityBilinear;

                    g.DrawImage(small, 0, 0, Width, Height);

                    using (SolidBrush dark = new SolidBrush(
                        Color.FromArgb(170, 0, 0, 0)))
                    {
                        g.FillRectangle(
                            dark,
                            0,
                            0,
                            Width,
                            Height);
                    }

                    InformationOverlay overlay =
                        new InformationOverlay();

                    overlay.BackgroundImage = new Bitmap(blurred);
                    overlay.BackgroundImageLayout =
                        ImageLayout.Stretch;

                    overlay.Dock = DockStyle.Fill;

                    Controls.Add(overlay);
                    overlay.BringToFront();
                }
            }
        }

        // ==========================================
        // ORIGINAL CATEGORY HOVER HANDLERS
        // ==========================================

        // CARS hover
        private void picCarsButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picCarsButton.BackgroundImage =
                Properties.Resources.cars_category_hover;
        }

        private void picCarsButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picCarsButton.BackgroundImage =
                Properties.Resources.cars_category;
        }

        // FLAGS hover
        private void picFlagsButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picFlagsButton.BackgroundImage =
                Properties.Resources.flags_category_hover;
        }

        private void picFlagsButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picFlagsButton.BackgroundImage =
                Properties.Resources.flags_category;
        }

        // MIXED hover
        private void picMixedButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picMixedButton.BackgroundImage =
                Properties.Resources.mixed_category_hover;
        }

        private void picMixedButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picMixedButton.BackgroundImage =
                Properties.Resources.mixed_category;
        }

        // ==========================================
        // LOGOUT HOVER
        // ==========================================

        private void picLogoutButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picLogoutButton.BackgroundImage =
                Properties.Resources.button_logout_hover;
        }

        private void picLogoutButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picLogoutButton.BackgroundImage =
                Properties.Resources.button_logout;
        }

        // ==========================================
        // USERNAME
        // ==========================================

        public void SetUsername(string username)
        {
            lblUsername.Text = username;
        }

        // ==========================================
        // LOGOUT CLICK
        // ==========================================

        private void picLogoutButton_Click(
            object sender,
            EventArgs e)
        {
            UserSession.Logout();

            Control? container = Parent;

            if (container == null)
                return;

            StartPage startPage = new StartPage();
            startPage.Dock = DockStyle.Fill;

            container.Controls.Add(startPage);
            startPage.BringToFront();

            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // TROPHY BUTTON
        // ==========================================

        private void picTrophyButton_Click(
            object? sender,
            EventArgs e)
        {
            OpenLeaderboard();
        }

        private void OpenLeaderboard()
        {
            Leaderboard leaderboard = new Leaderboard();
            leaderboard.Dock = DockStyle.Fill;

            Controls.Add(leaderboard);
            leaderboard.BringToFront();
        }

        private void picTrophyButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picTrophyButton.Image =
                Properties.Resources.button_leaderboard_hover;
        }

        private void picTrophyButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picTrophyButton.Image =
                Properties.Resources.button_leaderboard;
        }

        // ==========================================
        // SETTINGS BUTTON
        // ==========================================

        private void picSettingsButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picSettingsButton.Image =
                Properties.Resources.button_sittings_hover;
        }

        private void picSettingsButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picSettingsButton.Image =
                Properties.Resources.button_sittings;
        }

        // ==========================================
        // INFO BUTTON HOVER
        // ==========================================

        private void picInfoButton_MouseEnter(
            object sender,
            EventArgs e)
        {
            picInfoButton.Image =
                Properties.Resources.button_info_hover;
        }

        private void picInfoButton_MouseLeave(
            object sender,
            EventArgs e)
        {
            picInfoButton.Image =
                Properties.Resources.button_info;
        }

        // ==========================================
        // ORIGINAL CATEGORY CLICK HANDLERS
        // ==========================================

        private void picCarsButton_Click(
            object sender,
            EventArgs e)
        {
            CarsQuiz carsQuiz = new CarsQuiz();

            carsQuiz.Dock = DockStyle.Fill;

            Controls.Add(carsQuiz);
            carsQuiz.BringToFront();
        }

        private void picMixedButton_Click(
            object sender,
            EventArgs e)
        {
            MixedQuiz mixedQuiz = new MixedQuiz();

            mixedQuiz.Dock = DockStyle.Fill;

            Controls.Add(mixedQuiz);
            mixedQuiz.BringToFront();
        }

        private void picFlagsButton_Click(
            object sender,
            EventArgs e)
        {
            FlagsQuiz flagsQuiz = new FlagsQuiz();

            flagsQuiz.Dock = DockStyle.Fill;

            Controls.Add(flagsQuiz);
            flagsQuiz.BringToFront();
        }
    }
}
