using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using QuizMaster.Controls;

namespace QuizMaster.Forms
{
    public partial class AdminPanel : UserControl
    {
        // ==========================================
        // STATE AND PENDING CHANGES
        // ==========================================

        private string activeTab = "Questions";
        private GoldDataGrid? questionTable;
        private GoldDataGrid? accountsTable;
        private GoldDropdown? categoryDropdown;

        private readonly List<QuizQuestion> pendingQuestions = new();
        private readonly Dictionary<int, QuizQuestion> pendingEditedQuestions = new();
        private readonly HashSet<int> pendingDeletedQuestionIds = new();

        private readonly List<(string Username, string Email, string Password)> pendingAccounts = new();
        private readonly Dictionary<int, (string Username, string Email, bool IsAdmin)> pendingEditedAccounts = new();
        private readonly HashSet<int> pendingDeletedAccountIds = new();

        // Only newly copied images belong to this panel and may be cleaned up.
        private readonly HashSet<string> temporaryImageFiles = new(StringComparer.OrdinalIgnoreCase);

        private bool HasUnsavedChanges => pendingQuestions.Count > 0 ||
            pendingEditedQuestions.Count > 0 || pendingDeletedQuestionIds.Count > 0 ||
            pendingAccounts.Count > 0 || pendingEditedAccounts.Count > 0 ||
            pendingDeletedAccountIds.Count > 0;

        private string CurrentCategory => categoryDropdown?.SelectedCategory ?? "Flags";

        public AdminPanel()
        {
            InitializeComponent();
            SetupButtonHoverEffects();
            SetupCategoryDropdown();
            SetupGoldDataGrid();
            SetupAccountsTable();
            SetupButtonEvents();
            LoadAdminQuestions(CurrentCategory);
            LoadAdminAccounts();
            SwitchTab("Questions");
        }

        // ==========================================
        // BUTTONS AND NAVIGATION
        // ==========================================

        private bool IsActionEnabled(PictureBox button)
        {
            if (button == picButtonSave)
                return HasUnsavedChanges;

            if (button == picButtonEdit || button == picButtonDelete)
                return activeTab == "Questions"
                    ? questionTable?.SelectedRow != null
                    : accountsTable?.SelectedRow != null;

            return true;
        }

        private void UpdateActionButtons()
        {
            foreach (PictureBox button in new[] { picButtonEdit, picButtonDelete, picButtonSave })
            {
                bool enabled = IsActionEnabled(button);
                button.Cursor = enabled ? Cursors.Hand : Cursors.No;
                if (!enabled)
                    button.BackgroundImage = button == picButtonEdit
                        ? Properties.Resources.button_edit
                        : button == picButtonDelete
                            ? Properties.Resources.button_delete
                            : Properties.Resources.button_save;
            }
        }

        private void SetupButtonHoverEffects()
        {
            SetupButtonHover(picButtonAdd, Properties.Resources.button_add, Properties.Resources.button_add_hover);
            SetupButtonHover(picButtonEdit, Properties.Resources.button_edit, Properties.Resources.button_edit_hover);
            SetupButtonHover(picButtonDelete, Properties.Resources.button_delete, Properties.Resources.button_delete_hover);
            SetupButtonHover(picButtonSave, Properties.Resources.button_save, Properties.Resources.button_save_hover);
            SetupButtonHover(picButtonQuestions, Properties.Resources.button_question, Properties.Resources.button_questions_hover);
            SetupButtonHover(picButtonAccounts, Properties.Resources.button_accounts, Properties.Resources.button_accounts_hover);
            SetupButtonHover(picButtonAdminBack, Properties.Resources.button_admin_back, Properties.Resources.button_admin_back_hover);
        }

        private void SetupButtonHover(PictureBox button, Image normalImage, Image hoverImage)
        {
            button.BackgroundImage = normalImage;
            button.MouseEnter += (sender, e) =>
            {
                if (IsActionEnabled(button)) button.BackgroundImage = hoverImage;
            };
            button.MouseLeave += (sender, e) =>
            {
                bool selected = (button == picButtonQuestions && activeTab == "Questions") ||
                                (button == picButtonAccounts && activeTab == "Accounts");
                button.BackgroundImage = selected ? hoverImage : normalImage;
            };
        }

        private void SetupButtonEvents()
        {
            picButtonAdd.Click += (sender, e) =>
            {
                if (activeTab == "Questions") ShowQuestionDialog();
                else AddAccountFromDialog();
            };
            picButtonEdit.Click += (sender, e) =>
            {
                if (!IsActionEnabled(picButtonEdit)) return;
                if (activeTab == "Questions") EditSelectedQuestion();
                else EditSelectedAccount();
            };
            picButtonDelete.Click += (sender, e) =>
            {
                if (!IsActionEnabled(picButtonDelete)) return;
                if (activeTab == "Questions") DeleteSelectedQuestion();
                else DeleteSelectedAccount();
            };
            picButtonSave.Click += (sender, e) =>
            {
                if (IsActionEnabled(picButtonSave)) SavePendingChanges();
            };
            picButtonQuestions.Click += (sender, e) => SwitchTab("Questions");
            picButtonAccounts.Click += (sender, e) => SwitchTab("Accounts");
            picButtonAdminBack.Click += PicButtonAdminBack_Click;
        }

        private void SwitchTab(string tab)
        {
            if (activeTab != tab)
            {
                DiscardPendingChanges();
                if (tab == "Questions") LoadAdminQuestions(CurrentCategory);
                else LoadAdminAccounts();
            }

            activeTab = tab;
            bool questions = tab == "Questions";
            picButtonQuestions.BackgroundImage = questions
                ? Properties.Resources.button_questions_hover : Properties.Resources.button_question;
            picButtonAccounts.BackgroundImage = questions
                ? Properties.Resources.button_accounts : Properties.Resources.button_accounts_hover;
            if (questionTable != null) questionTable.Visible = questions;
            if (accountsTable != null) accountsTable.Visible = !questions;
            if (categoryDropdown != null) categoryDropdown.Visible = questions;
            lblCatagory.Visible = questions;
            UpdateActionButtons();
        }

        private void PicButtonAdminBack_Click(object? sender, EventArgs e)
        {
            DiscardPendingChanges();
            Control? container = Parent;
            if (container == null) return;
            container.Controls.Remove(this);
            Dispose();
        }

        // ==========================================
        // TABLES AND CATEGORY FILTER
        // ==========================================

        private void SetupGoldDataGrid()
        {
            questionTable = new GoldDataGrid
            {
                Name = "questionTable",
                Location = new Point(470, 410),
                Size = new Size(1380, 480)
            };
            questionTable.SelectionChanged += (sender, e) => UpdateActionButtons();
            Controls.Add(questionTable);
            questionTable.BringToFront();
        }

        private void SetupAccountsTable()
        {
            accountsTable = new GoldDataGrid
            {
                Name = "accountsTable",
                Location = new Point(470, 410),
                Size = new Size(1380, 480),
                Visible = false
            };
            accountsTable.SetColumns(
                new[] { "ID", "USERNAME", "EMAIL", "ADMIN", "CREATED AT" },
                new float[] { 60, 220, 350, 120, 220 });
            accountsTable.SelectionChanged += (sender, e) => UpdateActionButtons();
            Controls.Add(accountsTable);
            accountsTable.BringToFront();
        }

        private void SetupCategoryDropdown()
        {
            categoryDropdown = new GoldDropdown
            {
                Name = "categoryDropdown",
                Location = new Point(650, 315),
                Size = new Size(240, 48)
            };
            categoryDropdown.SelectedCategoryChanged += (sender, e) =>
            {
                DiscardPendingChanges();
                LoadAdminQuestions(categoryDropdown.SelectedCategory);
            };
            Controls.Add(categoryDropdown);
            categoryDropdown.BringToFront();
        }

        private void LoadAdminQuestions(string category)
        {
            if (questionTable == null) return;
            questionTable.ClearRows();
            foreach (QuizQuestion saved in QuizDatabase.GetAdminQuestions(category))
            {
                if (pendingDeletedQuestionIds.Contains(saved.Id)) continue;
                QuizQuestion question = pendingEditedQuestions.TryGetValue(saved.Id, out QuizQuestion? edited)
                    ? edited : saved;
                AddQuestionRow(saved.Id.ToString(), question);
            }
            for (int i = 0; i < pendingQuestions.Count; i++)
                AddQuestionRow("NEW " + (i + 1), pendingQuestions[i]);
            UpdateActionButtons();
        }

        private void AddQuestionRow(string id, QuizQuestion question)
        {
            questionTable?.AddRow(id, question.Category, question.QuestionText,
                question.ImagePath ?? "", question.CorrectAnswer,
                question.WrongAnswer1, question.WrongAnswer2, question.WrongAnswer3);
        }

        private void LoadAdminAccounts()
        {
            if (accountsTable == null) return;
            accountsTable.ClearRows();
            foreach (var saved in QuizDatabase.GetAdminAccounts())
            {
                if (pendingDeletedAccountIds.Contains(saved.Id)) continue;
                var account = pendingEditedAccounts.TryGetValue(saved.Id, out var edited)
                    ? edited : (saved.Username, saved.Email, saved.IsAdmin);
                accountsTable.AddRow(saved.Id.ToString(), account.Item1, account.Item2,
                    account.Item3 ? "Yes" : "No", saved.CreatedAt);
            }
            for (int i = 0; i < pendingAccounts.Count; i++)
            {
                var account = pendingAccounts[i];
                accountsTable.AddRow("NEW " + (i + 1), account.Username, account.Email,
                    "No", "Pending save");
            }
            UpdateActionButtons();
        }

        // ==========================================
        // QUESTION ADD / EDIT (ONE SHARED DIALOG)
        // ==========================================

        private void EditSelectedQuestion()
        {
            string[]? row = questionTable?.SelectedRow;
            if (row == null) return;
            if (row[0].StartsWith("NEW ", StringComparison.Ordinal))
            {
                if (int.TryParse(row[0].Substring(4), out int number) &&
                    number >= 1 && number <= pendingQuestions.Count)
                    ShowQuestionDialog(pendingQuestions[number - 1], number - 1);
                return;
            }
            if (!int.TryParse(row[0], out int id)) return;
            QuizQuestion? question = pendingEditedQuestions.TryGetValue(id, out QuizQuestion? edited)
                ? edited : QuizDatabase.GetAdminQuestions(CurrentCategory).FirstOrDefault(q => q.Id == id);
            if (question != null) ShowQuestionDialog(question);
        }

        private void ShowQuestionDialog(QuizQuestion? existingQuestion = null, int pendingIndex = -1)
        {
            string category = CurrentCategory;
            bool editing = existingQuestion != null;
            using var dialog = new Form
            {
                Text = (editing ? "Edit " : "Add ") + category + " Question",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(650, 690),
                BackColor = Color.FromArgb(24, 24, 24),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };

            TextBox AddField(string caption, int y, string value = "")
            {
                dialog.Controls.Add(new Label
                {
                    Text = caption,
                    Location = new Point(20, y),
                    Size = new Size(600, 24),
                    ForeColor = Color.Gold
                });
                var field = new TextBox
                {
                    Location = new Point(20, y + 25),
                    Size = new Size(605, 27),
                    Text = value
                };
                dialog.Controls.Add(field);
                return field;
            }

            TextBox questionText = AddField("Question text", 15, existingQuestion?.QuestionText ?? "");
            dialog.Controls.Add(new Label
            {
                Text = "Question image (required)",
                Location = new Point(20, 85),
                Size = new Size(600, 24),
                ForeColor = Color.Gold
            });
            var preview = new PictureBox
            {
                Location = new Point(20, 115),
                Size = new Size(230, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(35, 35, 35)
            };
            var imageName = new Label
            {
                Text = "No image selected",
                Location = new Point(270, 165),
                Size = new Size(350, 60),
                ForeColor = Color.White,
                AutoEllipsis = true
            };
            var browse = new Button
            {
                Text = "Browse Images...",
                Location = new Point(270, 115),
                Size = new Size(180, 36)
            };
            dialog.Controls.Add(preview);
            dialog.Controls.Add(browse);
            dialog.Controls.Add(imageName);

            string? selectedImage = null;
            string existingImage = existingQuestion?.ImagePath ?? "";
            if (editing && !string.IsNullOrWhiteSpace(existingImage))
            {
                string resolved = ResolveImagePath(existingImage);
                imageName.Text = resolved;
                TryLoadImagePreview(preview, resolved, existingImage);
            }
            browse.Click += (sender, e) =>
            {
                using var picker = new OpenFileDialog
                {
                    Title = "Select Question Image",
                    Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.webp",
                    CheckFileExists = true,
                    Multiselect = false
                };
                if (picker.ShowDialog(dialog) != DialogResult.OK) return;
                try
                {
                    using var loaded = Image.FromFile(picker.FileName);
                    Image? old = preview.Image;
                    preview.Image = new Bitmap(loaded);
                    old?.Dispose();
                    selectedImage = picker.FileName;
                    imageName.Text = selectedImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not open image: " + ex.Message,
                        "Image error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            TextBox correct = AddField("Correct answer", 285, existingQuestion?.CorrectAnswer ?? "");
            TextBox wrong1 = AddField("Wrong answer 1", 355, existingQuestion?.WrongAnswer1 ?? "");
            TextBox wrong2 = AddField("Wrong answer 2", 425, existingQuestion?.WrongAnswer2 ?? "");
            TextBox wrong3 = AddField("Wrong answer 3", 495, existingQuestion?.WrongAnswer3 ?? "");
            var apply = new Button
            {
                Text = editing ? "Change" : "Add",
                Location = new Point(435, 625),
                Size = new Size(90, 36)
            };
            var cancel = new Button
            {
                Text = "Cancel",
                Location = new Point(535, 625),
                Size = new Size(90, 36),
                DialogResult = DialogResult.Cancel
            };
            dialog.Controls.Add(apply);
            dialog.Controls.Add(cancel);
            dialog.AcceptButton = apply;
            dialog.CancelButton = cancel;

            apply.Click += (sender, e) =>
            {
                if (new[] { questionText, correct, wrong1, wrong2, wrong3 }
                    .Any(field => string.IsNullOrWhiteSpace(field.Text)) ||
                    (string.IsNullOrWhiteSpace(selectedImage) && string.IsNullOrWhiteSpace(existingImage)))
                {
                    MessageBox.Show("Fill in every field and select an image.",
                        "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string? copiedImage = null;
                try
                {
                    string imagePath = existingImage;
                    if (!string.IsNullOrWhiteSpace(selectedImage))
                    {
                        Directory.CreateDirectory(ImageFolder);
                        copiedImage = Path.Combine(ImageFolder,
                            Guid.NewGuid().ToString("N") + Path.GetExtension(selectedImage));
                        File.Copy(selectedImage, copiedImage);
                        imagePath = Path.GetFileName(copiedImage);
                    }
                    var updated = new QuizQuestion
                    {
                        Id = existingQuestion?.Id ?? 0,
                        Category = category,
                        QuestionText = questionText.Text.Trim(),
                        ImagePath = imagePath,
                        CorrectAnswer = correct.Text.Trim(),
                        WrongAnswer1 = wrong1.Text.Trim(),
                        WrongAnswer2 = wrong2.Text.Trim(),
                        WrongAnswer3 = wrong3.Text.Trim()
                    };
                    if (pendingIndex >= 0)
                    {
                        if (pendingQuestions[pendingIndex].ImagePath != updated.ImagePath)
                            ReleaseUnusedTemporaryImage(pendingQuestions[pendingIndex].ImagePath);
                        pendingQuestions[pendingIndex] = updated;
                    }
                    else if (editing)
                    {
                        if (pendingEditedQuestions.TryGetValue(updated.Id, out QuizQuestion? previous))
                            if (previous.ImagePath != updated.ImagePath)
                                ReleaseUnusedTemporaryImage(previous.ImagePath);
                        pendingEditedQuestions[updated.Id] = updated;
                    }
                    else pendingQuestions.Add(updated);

                    if (copiedImage != null) temporaryImageFiles.Add(copiedImage);
                    dialog.DialogResult = DialogResult.OK;
                    dialog.Close();
                }
                catch (Exception ex)
                {
                    if (copiedImage != null) TryDeleteFile(copiedImage);
                    MessageBox.Show("Could not apply question: " + ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            DialogResult result = dialog.ShowDialog(this);
            preview.Image?.Dispose();
            preview.Image = null;
            if (result == DialogResult.OK) LoadAdminQuestions(category);
        }

        private static string ImageFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "QuizMaster", "QuestionImages");

        private static string ResolveImagePath(string path)
        {
            if (File.Exists(path)) return path;
            return Path.Combine(ImageFolder, Path.GetFileName(path));
        }

        private static void TryLoadImagePreview(PictureBox preview, string resolvedPath, string originalPath)
        {
            try
            {
                if (File.Exists(resolvedPath))
                {
                    using var loaded = Image.FromFile(resolvedPath);
                    preview.Image = new Bitmap(loaded);
                }
                else
                {
                    string resourceName = Path.GetFileNameWithoutExtension(originalPath);
                    if (Properties.Resources.ResourceManager.GetObject(resourceName) is Image resource)
                        preview.Image = new Bitmap(resource);
                }
            }
            catch (Exception) { /* Keep the filename visible if preview cannot load. */ }
        }

        private void DeleteSelectedQuestion()
        {
            string[]? row = questionTable?.SelectedRow;
            if (row == null) return;
            if (row[0].StartsWith("NEW ", StringComparison.Ordinal))
            {
                if (int.TryParse(row[0].Substring(4), out int number) &&
                    number >= 1 && number <= pendingQuestions.Count)
                {
                    ReleaseUnusedTemporaryImage(pendingQuestions[number - 1].ImagePath);
                    pendingQuestions.RemoveAt(number - 1);
                }
            }
            else if (int.TryParse(row[0], out int id))
            {
                if (pendingEditedQuestions.TryGetValue(id, out QuizQuestion? edited))
                    ReleaseUnusedTemporaryImage(edited.ImagePath);
                pendingEditedQuestions.Remove(id);
                pendingDeletedQuestionIds.Add(id);
            }
            LoadAdminQuestions(CurrentCategory);
        }

        // ==========================================
        // ACCOUNT ADD / EDIT / DELETE
        // ==========================================

        private void AddAccountFromDialog()
        {
            if (!TryShowEntryDialog("Add Account",
                new[] { "Username", "Email", "Password", "Confirm password" },
                out string[] values, passwordIndices: new[] { 2, 3 })) return;
            if (!ValidateAccount(values[0], values[1], values[2], values[3])) return;
            pendingAccounts.Add((values[0], values[1], values[2]));
            LoadAdminAccounts();
        }

        private void EditSelectedAccount()
        {
            string[]? row = accountsTable?.SelectedRow;
            if (row == null) return;
            bool isNew = row[0].StartsWith("NEW ", StringComparison.Ordinal);
            int pendingIndex = -1;
            int id = 0;
            if (isNew)
            {
                if (!int.TryParse(row[0].Substring(4), out int number)) return;
                pendingIndex = number - 1;
                if (pendingIndex < 0 || pendingIndex >= pendingAccounts.Count) return;
            }
            else if (!int.TryParse(row[0], out id)) return;

            if (isNew)
            {
                var account = pendingAccounts[pendingIndex];
                if (!TryShowEntryDialog("Edit Account",
                    new[] { "Username", "Email", "Password", "Confirm password" },
                    out string[] values, new[] { 2, 3 },
                    new[] { account.Username, account.Email, account.Password, account.Password },
                    "Change")) return;
                if (!ValidateAccount(values[0], values[1], values[2], values[3], pendingIndex: pendingIndex)) return;
                pendingAccounts[pendingIndex] = (values[0], values[1], values[2]);
            }
            else
            {
                bool isAdmin = row[3].Equals("Yes", StringComparison.OrdinalIgnoreCase);
                if (!TryShowEntryDialog("Edit Account",
                    new[] { "Username", "Email", "Admin (Yes/No)" },
                    out string[] values, initialValues: new[]
                    { row[1], row[2], isAdmin ? "Yes" : "No" }, actionText: "Change")) return;
                if (!values[2].Equals("Yes", StringComparison.OrdinalIgnoreCase) &&
                    !values[2].Equals("No", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Admin must be Yes or No.", "Invalid value");
                    return;
                }
                if (!ValidateAccountIdentity(values[0], values[1], id)) return;
                pendingEditedAccounts[id] = (values[0], values[1],
                    values[2].Equals("Yes", StringComparison.OrdinalIgnoreCase));
            }
            LoadAdminAccounts();
        }

        private void DeleteSelectedAccount()
        {
            string[]? row = accountsTable?.SelectedRow;
            if (row == null) return;
            if (MessageBox.Show("Delete this account when Save is clicked?",
                "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes) return;
            if (row[0].StartsWith("NEW ", StringComparison.Ordinal))
            {
                if (int.TryParse(row[0].Substring(4), out int number) &&
                    number >= 1 && number <= pendingAccounts.Count)
                    pendingAccounts.RemoveAt(number - 1);
            }
            else if (int.TryParse(row[0], out int id))
            {
                pendingEditedAccounts.Remove(id);
                pendingDeletedAccountIds.Add(id);
            }
            LoadAdminAccounts();
        }

        private bool ValidateAccount(string username, string email, string password,
            string confirmation, int pendingIndex = -1)
        {
            if (!ValidateAccountIdentity(username, email, pendingIndex: pendingIndex)) return false;
            if (password.Length < 8 || password.Length > 20 ||
                !password.Any(char.IsUpper) || !password.Any(char.IsLower) ||
                !password.Any(char.IsDigit) || !password.Any(c => !char.IsLetterOrDigit(c)))
            {
                MessageBox.Show("Password must contain 8–20 characters, uppercase and lowercase letters, a number, and at least one special character.",
                    "Invalid password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (password != confirmation)
            {
                MessageBox.Show("Passwords do not match.", "Invalid password",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool ValidateAccountIdentity(string username, string email,
            int existingId = 0, int pendingIndex = -1)
        {
            if (username.Length < 2 || username.Length > 15)
            {
                MessageBox.Show("Username must contain 2–15 characters.", "Invalid username");
                return false;
            }
            try
            {
                var address = new MailAddress(email);
                if (!string.Equals(address.Address, email, StringComparison.OrdinalIgnoreCase) ||
                    !email.Contains('.')) throw new FormatException();
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter a valid email address.", "Invalid email");
                return false;
            }

            for (int i = 0; i < pendingAccounts.Count; i++)
            {
                if (i == pendingIndex) continue;
                if (string.Equals(pendingAccounts[i].Username, username, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(pendingAccounts[i].Email, email, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("That username or email is already staged.", "Duplicate account");
                    return false;
                }
            }
            foreach (var saved in QuizDatabase.GetAdminAccounts())
            {
                if (saved.Id == existingId || pendingDeletedAccountIds.Contains(saved.Id)) continue;
                var account = pendingEditedAccounts.TryGetValue(saved.Id, out var edited)
                    ? edited : (saved.Username, saved.Email, saved.IsAdmin);
                if (string.Equals(account.Item1, username, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(account.Item2, email, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("That username or email already exists.", "Duplicate account");
                    return false;
                }
            }
            return true;
        }


        private bool TryShowEntryDialog(string title, string[] labels,
            out string[] values, int[]? passwordIndices = null,
            string[]? initialValues = null, string actionText = "Add")
        {
            values = Array.Empty<string>();

            using var dialog = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(540, labels.Length * 66 + 85),
                BackColor = Color.FromArgb(24, 24, 24),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10)
            };

            var fields = new TextBox[labels.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                dialog.Controls.Add(new Label
                {
                    Text = labels[i],
                    Location = new Point(20, 14 + i * 66),
                    Size = new Size(490, 23),
                    ForeColor = Color.Gold
                });

                fields[i] = new TextBox
                {
                    Location = new Point(20, 37 + i * 66),
                    Size = new Size(495, 27),
                    UseSystemPasswordChar = passwordIndices?.Contains(i) == true,
                    Text = initialValues != null && i < initialValues.Length
                        ? initialValues[i] : ""
                };

                dialog.Controls.Add(fields[i]);
            }

            var confirm = new Button
            {
                Text = actionText,
                Location = new Point(325, labels.Length * 66 + 25),
                Size = new Size(90, 34)
            };

            var cancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(425, labels.Length * 66 + 25),
                Size = new Size(90, 34)
            };

            confirm.Click += (sender, e) =>
            {
                string[] entered = fields
                    .Select(field => field.Text.Trim())
                    .ToArray();

                bool valid;

                if (entered.Length == 4)
                {
                    int pendingIndex = -1;

                    if (title == "Edit Account" &&
                        accountsTable?.SelectedRow is string[] row &&
                        row[0].StartsWith("NEW ") &&
                        int.TryParse(row[0].Substring(4), out int number))
                    {
                        pendingIndex = number - 1;
                    }

                    valid = ValidateAccount(
                        entered[0],
                        entered[1],
                        entered[2],
                        entered[3],
                        pendingIndex);
                }
                else if (entered.Length == 3)
                {
                    if (!entered[2].Equals("Yes", StringComparison.OrdinalIgnoreCase) &&
                        !entered[2].Equals("No", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "Admin must be Yes or No.",
                            "Invalid value",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }

                    int accountId = 0;

                    if (accountsTable?.SelectedRow is string[] row)
                        int.TryParse(row[0], out accountId);

                    valid = ValidateAccountIdentity(
                        entered[0],
                        entered[1],
                        accountId);
                }
                else
                {
                    valid = entered.All(value => !string.IsNullOrWhiteSpace(value));
                }

                if (!valid)
                    return;

                // Only valid input is allowed to close the dialog.
                dialog.Tag = entered;
                dialog.DialogResult = DialogResult.OK;
                dialog.Close();
            };

            dialog.Controls.Add(confirm);
            dialog.Controls.Add(cancel);

            dialog.AcceptButton = confirm;
            dialog.CancelButton = cancel;

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return false;

            values = (string[])dialog.Tag!;
            return true;
        }


        // ==========================================
        // SAVE / DISCARD
        // ==========================================

        private void SavePendingChanges()
        {
            if (!HasUnsavedChanges) return;
            try
            {
                // Preserve the original Normal/Tricky difficulty for edited questions.
                foreach (var pair in pendingEditedQuestions)
                    UpdateQuestionWithoutChangingDifficulty(pair.Value);
                foreach (QuizQuestion question in pendingQuestions)
                    QuizDatabase.AddQuestion(question, "Normal");
                foreach (int id in pendingDeletedQuestionIds)
                    QuizDatabase.DeleteQuestion(id);

                foreach (var pair in pendingEditedAccounts)
                {
                    var account = pair.Value;
                    QuizDatabase.UpdateAccount(pair.Key, account.Username,
                        account.Email, account.IsAdmin);
                }
                foreach (var account in pendingAccounts)
                {
                    string result = QuizDatabase.RegisterUser(account.Username,
                        account.Email, account.Password);
                    if (result != "Account created successfully!")
                        throw new InvalidOperationException(result);
                }
                foreach (int id in pendingDeletedAccountIds)
                    QuizDatabase.DeleteAccount(id);

                // Images copied for saved questions now belong to the database.
                temporaryImageFiles.Clear();
                ClearPendingCollections();
                if (activeTab == "Questions") LoadAdminQuestions(CurrentCategory);
                else LoadAdminAccounts();
                MessageBox.Show("Changes saved successfully!", "Save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save changes: " + ex.Message +
                    "\nSome earlier operations may already have been saved.",
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateActionButtons();
        }

        private static void UpdateQuestionWithoutChangingDifficulty(QuizQuestion question)
        {
            // QuizDatabase.UpdateQuestion requires a difficulty argument and overwrites it.
            // This targeted update deliberately leaves Difficulty untouched.
            string databasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "QuizMaster", "quizmaster.db");
            using var connection = new SqliteConnection("Data Source=" + databasePath);
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Questions SET
                    Category = $category,
                    QuestionText = $question,
                    ImagePath = $image,
                    CorrectAnswer = $correct,
                    WrongAnswer1 = $wrong1,
                    WrongAnswer2 = $wrong2,
                    WrongAnswer3 = $wrong3
                WHERE Id = $id";
            command.Parameters.AddWithValue("$id", question.Id);
            command.Parameters.AddWithValue("$category", question.Category);
            command.Parameters.AddWithValue("$question", question.QuestionText);
            command.Parameters.AddWithValue("$image", (object?)question.ImagePath ?? DBNull.Value);
            command.Parameters.AddWithValue("$correct", question.CorrectAnswer);
            command.Parameters.AddWithValue("$wrong1", question.WrongAnswer1);
            command.Parameters.AddWithValue("$wrong2", question.WrongAnswer2);
            command.Parameters.AddWithValue("$wrong3", question.WrongAnswer3);
            command.ExecuteNonQuery();
        }

        private void DiscardPendingChanges()
        {
            foreach (string path in temporaryImageFiles.ToArray()) TryDeleteFile(path);
            temporaryImageFiles.Clear();
            ClearPendingCollections();
            UpdateActionButtons();
        }

        private void ClearPendingCollections()
        {
            pendingQuestions.Clear();
            pendingEditedQuestions.Clear();
            pendingDeletedQuestionIds.Clear();
            pendingAccounts.Clear();
            pendingEditedAccounts.Clear();
            pendingDeletedAccountIds.Clear();
        }

        private void ReleaseUnusedTemporaryImage(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath)) return;
            string path = ResolveImagePath(imagePath);
            if (temporaryImageFiles.Remove(path)) TryDeleteFile(path);
        }

        private static void TryDeleteFile(string path)
        {
            try { if (File.Exists(path)) File.Delete(path); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
