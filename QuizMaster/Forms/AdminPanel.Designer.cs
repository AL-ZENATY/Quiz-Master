namespace QuizMaster.Forms
{
    partial class AdminPanel
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
            picButtonDelete = new PictureBox();
            picButtonEdit = new PictureBox();
            picButtonSave = new PictureBox();
            picButtonAdd = new PictureBox();
            picButtonQuestions = new PictureBox();
            picButtonAccounts = new PictureBox();
            picButtonAdminBack = new PictureBox();
            lblCatagory = new Label();
            ((System.ComponentModel.ISupportInitialize)picButtonDelete).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonEdit).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonSave).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAdd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonQuestions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAccounts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAdminBack).BeginInit();
            SuspendLayout();
            // 
            // picButtonDelete
            // 
            picButtonDelete.BackColor = Color.Transparent;
            picButtonDelete.BackgroundImage = Properties.Resources.button_delete;
            picButtonDelete.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonDelete.Cursor = Cursors.Hand;
            picButtonDelete.Location = new Point(1632, 278);
            picButtonDelete.Name = "picButtonDelete";
            picButtonDelete.Size = new Size(239, 124);
            picButtonDelete.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonDelete.TabIndex = 5;
            picButtonDelete.TabStop = false;
            // 
            // picButtonEdit
            // 
            picButtonEdit.BackColor = Color.Transparent;
            picButtonEdit.BackgroundImage = Properties.Resources.button_edit;
            picButtonEdit.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonEdit.Cursor = Cursors.Hand;
            picButtonEdit.Location = new Point(1387, 278);
            picButtonEdit.Name = "picButtonEdit";
            picButtonEdit.Size = new Size(239, 124);
            picButtonEdit.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonEdit.TabIndex = 6;
            picButtonEdit.TabStop = false;
            // 
            // picButtonSave
            // 
            picButtonSave.BackColor = Color.Transparent;
            picButtonSave.BackgroundImage = Properties.Resources.button_save;
            picButtonSave.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonSave.Cursor = Cursors.Hand;
            picButtonSave.Location = new Point(897, 278);
            picButtonSave.Name = "picButtonSave";
            picButtonSave.Size = new Size(239, 124);
            picButtonSave.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonSave.TabIndex = 7;
            picButtonSave.TabStop = false;
            // 
            // picButtonAdd
            // 
            picButtonAdd.BackColor = Color.Transparent;
            picButtonAdd.BackgroundImage = Properties.Resources.button_add;
            picButtonAdd.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonAdd.Cursor = Cursors.Hand;
            picButtonAdd.Location = new Point(1142, 278);
            picButtonAdd.Name = "picButtonAdd";
            picButtonAdd.Size = new Size(239, 124);
            picButtonAdd.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonAdd.TabIndex = 7;
            picButtonAdd.TabStop = false;
            // 
            // picButtonQuestions
            // 
            picButtonQuestions.BackColor = Color.Transparent;
            picButtonQuestions.BackgroundImage = Properties.Resources.button_question;
            picButtonQuestions.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonQuestions.Cursor = Cursors.Hand;
            picButtonQuestions.Location = new Point(658, 181);
            picButtonQuestions.Name = "picButtonQuestions";
            picButtonQuestions.Size = new Size(368, 82);
            picButtonQuestions.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonQuestions.TabIndex = 8;
            picButtonQuestions.TabStop = false;
            // 
            // picButtonAccounts
            // 
            picButtonAccounts.BackColor = Color.Transparent;
            picButtonAccounts.BackgroundImage = Properties.Resources.button_accounts;
            picButtonAccounts.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonAccounts.Cursor = Cursors.Hand;
            picButtonAccounts.Location = new Point(1105, 181);
            picButtonAccounts.Name = "picButtonAccounts";
            picButtonAccounts.Size = new Size(368, 82);
            picButtonAccounts.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonAccounts.TabIndex = 9;
            picButtonAccounts.TabStop = false;
            // 
            // picButtonAdminBack
            // 
            picButtonAdminBack.BackColor = Color.Transparent;
            picButtonAdminBack.BackgroundImage = Properties.Resources.button_admin_back;
            picButtonAdminBack.BackgroundImageLayout = ImageLayout.Stretch;
            picButtonAdminBack.Cursor = Cursors.Hand;
            picButtonAdminBack.Location = new Point(1623, 61);
            picButtonAdminBack.Name = "picButtonAdminBack";
            picButtonAdminBack.Size = new Size(257, 94);
            picButtonAdminBack.SizeMode = PictureBoxSizeMode.StretchImage;
            picButtonAdminBack.TabIndex = 10;
            picButtonAdminBack.TabStop = false;
            // 
            // lblCatagory
            // 
            lblCatagory.AutoSize = true;
            lblCatagory.BackColor = Color.Transparent;
            lblCatagory.Font = new Font("Verdana", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCatagory.ForeColor = Color.White;
            lblCatagory.Location = new Point(461, 314);
            lblCatagory.Name = "lblCatagory";
            lblCatagory.Size = new Size(196, 40);
            lblCatagory.TabIndex = 11;
            lblCatagory.Text = "Category:";
            // 
            // AdminPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background_admin;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(lblCatagory);
            Controls.Add(picButtonAdminBack);
            Controls.Add(picButtonAccounts);
            Controls.Add(picButtonQuestions);
            Controls.Add(picButtonAdd);
            Controls.Add(picButtonSave);
            Controls.Add(picButtonEdit);
            Controls.Add(picButtonDelete);
            Name = "AdminPanel";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)picButtonDelete).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonEdit).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonSave).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAdd).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonQuestions).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAccounts).EndInit();
            ((System.ComponentModel.ISupportInitialize)picButtonAdminBack).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picButtonDelete;
        private PictureBox picButtonEdit;
        private PictureBox picButtonSave;
        private PictureBox picButtonAdd;
        private PictureBox picButtonQuestions;
        private PictureBox picButtonAccounts;
        private PictureBox picButtonAdminBack;
        private Label lblCatagory;
    }
}
