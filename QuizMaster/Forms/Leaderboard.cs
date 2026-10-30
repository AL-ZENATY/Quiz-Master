
using System;
using System.Windows.Forms;

namespace QuizMaster.Forms
{
    public partial class Leaderboard : UserControl
    {
        // ==========================================
        // STATE
        // ==========================================

        private bool isHallOfFame = true;
        private string selectedCategory = "General";

        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public Leaderboard()
        {
            InitializeComponent();

            SetupButtons();
            ShowHall();
            LoadLeaderboard();
        }

        // ==========================================
        // BUTTON SETUP
        // ==========================================

        private void SetupButtons()
        {
            picHallSwitch.Cursor = Cursors.Hand;
            picExit.Cursor = Cursors.Hand;

            picGeneral.Cursor = Cursors.Hand;
            picMixed.Cursor = Cursors.Hand;
            picFlags.Cursor = Cursors.Hand;
            picCars.Cursor = Cursors.Hand;
            picRapidFire.Cursor = Cursors.Hand;

            // Category artwork uses BackgroundImage.
            picGeneral.Image = null;
            picMixed.Image = null;
            picFlags.Image = null;
            picCars.Image = null;
            picRapidFire.Image = null;

            // Category click events.
            picGeneral.Click += Category_Click;
            picMixed.Click += Category_Click;
            picFlags.Click += Category_Click;
            picCars.Click += Category_Click;
            picRapidFire.Click += Category_Click;

            // Category hover events.
            picGeneral.MouseEnter += Category_MouseEnter;
            picMixed.MouseEnter += Category_MouseEnter;
            picFlags.MouseEnter += Category_MouseEnter;
            picCars.MouseEnter += Category_MouseEnter;
            picRapidFire.MouseEnter += Category_MouseEnter;

            picGeneral.MouseLeave += Category_MouseLeave;
            picMixed.MouseLeave += Category_MouseLeave;
            picFlags.MouseLeave += Category_MouseLeave;
            picCars.MouseLeave += Category_MouseLeave;
            picRapidFire.MouseLeave += Category_MouseLeave;

            // Hall switch artwork.
            picHallSwitch.BackgroundImage = null;
            picHallSwitch.SizeMode = PictureBoxSizeMode.Zoom;

            // Exit artwork uses BackgroundImage.
            picExit.Image = null;

            // Hall switch events.
            picHallSwitch.Click -= PicHallSwitch_Click;
            picHallSwitch.Click += PicHallSwitch_Click;

            picHallSwitch.MouseEnter -= PicHallSwitch_MouseEnter;
            picHallSwitch.MouseEnter += PicHallSwitch_MouseEnter;

            picHallSwitch.MouseLeave -= PicHallSwitch_MouseLeave;
            picHallSwitch.MouseLeave += PicHallSwitch_MouseLeave;

            // Exit events.
            picExit.Click -= PicExit_Click;
            picExit.Click += PicExit_Click;

            picExit.MouseEnter -= PicExit_MouseEnter;
            picExit.MouseEnter += PicExit_MouseEnter;

            picExit.MouseLeave -= PicExit_MouseLeave;
            picExit.MouseLeave += PicExit_MouseLeave;
        }

        // ==========================================
        // HALL SWITCH CLICK
        // ==========================================

        private void PicHallSwitch_Click(
            object sender, EventArgs e)
        {
            isHallOfFame = !isHallOfFame;

            ShowHall();
            LoadLeaderboard();
        }

        // ==========================================
        // HALL SWITCH HOVER
        // ==========================================

        private void PicHallSwitch_MouseEnter(
            object sender, EventArgs e)
        {
            picHallSwitch.Image = isHallOfFame
                ? Properties.Resources.button_hall_of_shame_hover
                : Properties.Resources.button_hall_of_fame_hover;
        }

        private void PicHallSwitch_MouseLeave(
            object sender, EventArgs e)
        {
            UpdateHallSwitchImage();
        }

        // ==========================================
        // EXIT BUTTON HOVER
        // ==========================================

        private void PicExit_MouseEnter(
            object sender, EventArgs e)
        {
            picExit.BackgroundImage =
                Properties.Resources.button_back_hover;
        }

        private void PicExit_MouseLeave(
            object sender, EventArgs e)
        {
            picExit.BackgroundImage =
                Properties.Resources.button_back;
        }

        // ==========================================
        // LOAD LEADERBOARD
        // ==========================================

        private void LoadLeaderboard()
        {
            Label[] usernameLabels =
            {
        lblUsername01, lblUsername02,
        lblUsername03, lblUsername04,
        lblUsername05, lblUsername06,
        lblUsername07, lblUsername08,
        lblUsername09, lblUsername10
    };

            Label[] scoreLabels =
            {
        lblScore01, lblScore02,
        lblScore03, lblScore04,
        lblScore05, lblScore06,
        lblScore07, lblScore08,
        lblScore09, lblScore10
    };

            bool isRapidFire = selectedCategory == "Rapid Fire";

            if (isRapidFire)
            {
                var rankings =
                    QuizDatabase.GetRapidFireLeaderboard(isHallOfFame);

                for (int i = 0; i < 10; i++)
                {
                    if (i < rankings.Count)
                    {
                        usernameLabels[i].Text = rankings[i].Username;

                        TimeSpan time =
                            TimeSpan.FromSeconds(rankings[i].Time);

                        scoreLabels[i].Text =
                            $"{(int)time.TotalMinutes:00}:" +
                            $"{time.Seconds:00}:" +
                            $"{time.Milliseconds / 10:00}";
                    }
                    else
                    {
                        usernameLabels[i].Text = "";
                        scoreLabels[i].Text = "";
                    }
                }
            }
            else
            {
                var scores = QuizDatabase.GetLeaderboardScores(
                    selectedCategory,
                    isHallOfFame);

                for (int i = 0; i < 10; i++)
                {
                    if (i < scores.Count)
                    {
                        usernameLabels[i].Text = scores[i].Username;
                        scoreLabels[i].Text =
                            scores[i].Score.ToString("0.##");
                    }
                    else
                    {
                        usernameLabels[i].Text = "";
                        scoreLabels[i].Text = "";
                    }
                }
            }
        }

        // ==========================================
        // CATEGORY BUTTONS
        // ==========================================

        private void Category_Click(object sender, EventArgs e)
        {
            if (sender == picGeneral)
                selectedCategory = "General";
            else if (sender == picMixed)
                selectedCategory = "Mixed";
            else if (sender == picFlags)
                selectedCategory = "Flags";
            else if (sender == picCars)
                selectedCategory = "Cars";
            else if (sender == picRapidFire)
                selectedCategory = "Rapid Fire";
            else
                return;

            ShowHall();
            LoadLeaderboard();
        }

        private void Category_MouseEnter(object sender, EventArgs e)
        {
            if (sender == picGeneral)
                picGeneral.BackgroundImage =
                    Properties.Resources.button_general_hover;

            else if (sender == picMixed)
                picMixed.BackgroundImage =
                    Properties.Resources.button_mixed_hover;

            else if (sender == picFlags)
                picFlags.BackgroundImage =
                    Properties.Resources.button_flags_hover;

            else if (sender == picCars)
                picCars.BackgroundImage =
                    Properties.Resources.button_cars_hover;

            else if (sender == picRapidFire)
                picRapidFire.BackgroundImage =
                    Properties.Resources.button_rapidfire_hover;
        }

        private void Category_MouseLeave(object sender, EventArgs e)
        {
            UpdateCategoryImages();
        }

        // ==========================================
        // CATEGORY BUTTON COLORS
        // ==========================================

        private void UpdateCategoryImages()
        {
            picGeneral.BackgroundImage = selectedCategory == "General"
                ? Properties.Resources.button_general_hover
                : isHallOfFame
                    ? Properties.Resources.button_general_fame
                    : Properties.Resources.button_general_shame;

            picMixed.BackgroundImage = selectedCategory == "Mixed"
                ? Properties.Resources.button_mixed_hover
                : isHallOfFame
                    ? Properties.Resources.button_mixed_fame
                    : Properties.Resources.button_mixed_shame;

            picFlags.BackgroundImage = selectedCategory == "Flags"
                ? Properties.Resources.button_flags_hover
                : isHallOfFame
                    ? Properties.Resources.button_flags_fame
                    : Properties.Resources.button_flags_shame;

            picCars.BackgroundImage = selectedCategory == "Cars"
                ? Properties.Resources.button_cars_hover
                : isHallOfFame
                    ? Properties.Resources.button_cars_fame
                    : Properties.Resources.button_cars_shame;

            picRapidFire.BackgroundImage = selectedCategory == "Rapid Fire"
            ? Properties.Resources.button_rapidfire_hover
            : isHallOfFame
                ? Properties.Resources.button_rapidfire_fame
                : Properties.Resources.button_rapidfire_shame;
        }

        // ==========================================
        // HALL DISPLAY
        // ==========================================

        private void ShowHall()
        {
            BackgroundImage = isHallOfFame
                ? Properties.Resources.background_fame
                : Properties.Resources.background_shame;

            BackgroundImageLayout = ImageLayout.Stretch;

            pnlLeaderboardTable.BackgroundImage =
                selectedCategory == "Rapid Fire"
                    ? isHallOfFame
                        ? Properties.Resources.leaderboard_fame_rapidfire
                        : Properties.Resources.leaderboard_shame_rapidfire
                    : isHallOfFame
                        ? Properties.Resources.leaderboard_fame
                        : Properties.Resources.leaderboard_shame;

            pnlLeaderboardTable.BackgroundImageLayout =
                ImageLayout.Stretch;

            UpdateHallSwitchImage();
            UpdateCategoryImages();
        }

        // ==========================================
        // HALL SWITCH IMAGE
        // ==========================================

        private void UpdateHallSwitchImage()
        {
            picHallSwitch.Image = isHallOfFame
                ? Properties.Resources.button_hall_of_shame
                : Properties.Resources.button_hall_of_fame;

            picHallSwitch.SizeMode = PictureBoxSizeMode.Zoom;
        }

        // ==========================================
        // BACK TO CATEGORY SELECTION
        // ==========================================

        private void PicExit_Click(
            object sender, EventArgs e)
        {
            Control? container = Parent;

            if (container == null)
                return;

            container.Controls.Remove(this);
            Dispose();
        }
    }
}
