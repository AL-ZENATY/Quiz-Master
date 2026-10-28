using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuizMaster
{
    public partial class StopConfirmationOverlay : UserControl
    {
        // Let FlagsQuiz handle the timer and navigation.
        public event EventHandler? CancelRequested;
        public event EventHandler? ExitRequested;

        public StopConfirmationOverlay()
        {
            InitializeComponent();
            SetupOverlayButtons();
        }

        private void picCancelButton_MouseEnter(object sender, EventArgs e)
        {
            picCancelButton.Image =
                Properties.Resources.button_cancel_exit_quiz_hover;
        }

        private void picCancelButton_MouseLeave(object sender, EventArgs e)
        {
            picCancelButton.Image =
                Properties.Resources.button_cancel_exit_quiz;
        }

        private void picExitAnywayButton_MouseEnter(object sender, EventArgs e)
        {
            picExitAnywayButton.Image =
                Properties.Resources.button_exit_anyway_exit_quiz_hover;
        }

        private void picExitAnywayButton_MouseLeave(object sender, EventArgs e)
        {
            picExitAnywayButton.Image =
                Properties.Resources.button_exit_anyway_exit_quiz;
        }

        private void picCancelButton_Click(object sender, EventArgs e)
        {
            // Ask FlagsQuiz to close the overlay and resume the timers.
            if (CancelRequested != null)
            {
                CancelRequested(this, EventArgs.Empty);
            }
        }

        private void picExitAnywayButton_Click(object sender, EventArgs e)
        {
            // Ask FlagsQuiz to return to Category Selection.
            if (ExitRequested != null)
            {
                ExitRequested(this, EventArgs.Empty);
            }
        }

        private void SetupOverlayButtons()
        {
            AttachButton(picCancelButton, picStopPanel);
            AttachButton(picExitAnywayButton, picStopPanel);
        }

        private void AttachButton(PictureBox button, PictureBox panel)
        {
            Point position = panel.PointToClient(
                button.PointToScreen(Point.Empty));

            button.Parent = panel;
            button.Location = position;
            button.BackColor = Color.Transparent;
            button.BringToFront();
        }
    }
}
