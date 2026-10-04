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
    public partial class InformationOverlay : UserControl
    {
        public InformationOverlay()
        {
            InitializeComponent();
        }

        private void pnlClose_Click(object sender, EventArgs e)
        {
            if (BackgroundImage != null)
            {
                Image oldImage = BackgroundImage;
                BackgroundImage = null;
                oldImage.Dispose();
            }

            Parent.Controls.Remove(this);
            Dispose();
        }

        private void picCloseButton_MouseEnter(object sender, EventArgs e)
        {
            picCloseButton.BackgroundImage = Properties.Resources.button_back_hover;
        }

        private void picCloseButton_MouseLeave(object sender, EventArgs e)
        {
            picCloseButton.BackgroundImage = Properties.Resources.button_back;
        }
    }
}
