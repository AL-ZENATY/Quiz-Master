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
    public partial class GuestOverlay : UserControl
    {
        public GuestOverlay()
        {
            InitializeComponent();


            Point continuePosition = picGuestPanel.PointToClient(
                picContinueButton.PointToScreen(Point.Empty));

            picContinueButton.Parent = picGuestPanel;
            picContinueButton.Location = continuePosition;
            picContinueButton.BackColor = Color.Transparent;
            picContinueButton.BringToFront();


            Point cancelPosition = picGuestPanel.PointToClient(
                picCancelButton.PointToScreen(Point.Empty));

            picCancelButton.Parent = picGuestPanel;
            picCancelButton.Location = cancelPosition;
            picCancelButton.BackColor = Color.Transparent;
            picCancelButton.BringToFront();


        }

        private void picContinueButton_Click(object sender, EventArgs e)
        {
            CategorySelection categories = new CategorySelection();

            categories.SetUsername("Guest");
            categories.Dock = DockStyle.Fill;

            Control parent = this.Parent;

            parent.Controls.Add(categories);
            categories.BringToFront();

            parent.Controls.Remove(this);
            this.Dispose();
        }

        private void picContinueButton_MouseEnter(object sender, EventArgs e)
        {
            picContinueButton.BackgroundImage =
                Properties.Resources.button_continue_hover;
        }

        private void picContinueButton_MouseLeave(object sender, EventArgs e)
        {
            picContinueButton.BackgroundImage =
                Properties.Resources.button_continue;
        }

        private void picCancelButton_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        private void picCancelButton_MouseEnter(object sender, EventArgs e)
        {
            picCancelButton.BackgroundImage =
                Properties.Resources.button_cancel_hover;
        }

        private void picCancelButton_MouseLeave(object sender, EventArgs e)
        {
            picCancelButton.BackgroundImage =
                Properties.Resources.button_cancel;
        }

    }
}
