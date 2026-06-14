namespace ResumeBuilder.UI
{
    partial class ResumeForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ─── Form Settings ───
            this.Text = "Resume Builder - Enter Your Details";
            this.Size = new System.Drawing.Size(950, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(24, 28, 36);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);

            // ─── Tab Control ───
            tabControl = new TabControl();
            tabControl.Location = new System.Drawing.Point(20, 20);
            tabControl.Size = new System.Drawing.Size(895, 610);
            tabControl.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);

            // ════════ TAB 1: Personal Info ════════
            tabPersonal = new TabPage("Personal Info");
            tabPersonal.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);
            tabPersonal.Padding = new Padding(20);
            tabPersonal.AutoScroll = true;

            int y = 15;
            int labelWidth = 130;
            int inputWidth = 700;
            int rowHeight = 38;

            var lblRequiredNote = new Label();
            lblRequiredNote.Text = "* Indicates mandatory fields";
            lblRequiredNote.ForeColor = System.Drawing.Color.FromArgb(230, 92, 92);
            lblRequiredNote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            lblRequiredNote.Location = new System.Drawing.Point(20, y);
            lblRequiredNote.AutoSize = true;
            tabPersonal.Controls.Add(lblRequiredNote);
            y += 30;

            txtFullName = CreateTextBox("Full Name", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: true);
            txtJobTitle = CreateTextBox("Job Title", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: false);
            txtEmail = CreateTextBox("Email", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: true);
            txtPhone = CreateTextBox("Phone", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: true);
            txtAddress = CreateTextBox("Address", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: false);
            txtLanguages = CreateTextBox("Languages", ref y, labelWidth, inputWidth, rowHeight, tabPersonal, isRequired: false);

            // Languages hint
            var lblLangHint = new Label();
            lblLangHint.Text = "(comma-separated, e.g. Urdu, English)";
            lblLangHint.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            lblLangHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            lblLangHint.Location = new System.Drawing.Point(160, y - 15);
            lblLangHint.AutoSize = true;
            tabPersonal.Controls.Add(lblLangHint);
            y += 5;

            // Summary (multi-line)
            var lblSummary = new Label();
            lblSummary.Text = "Summary *:";
            lblSummary.ForeColor = System.Drawing.Color.FromArgb(180, 190, 205);
            lblSummary.Location = new System.Drawing.Point(20, y + 5);
            lblSummary.Size = new System.Drawing.Size(labelWidth, 25);
            tabPersonal.Controls.Add(lblSummary);

            txtSummary = new TextBox();
            txtSummary.Location = new System.Drawing.Point(160, y);
            txtSummary.Size = new System.Drawing.Size(inputWidth, 100);
            txtSummary.Multiline = true;
            txtSummary.ScrollBars = ScrollBars.Vertical;
            StyleTextBox(txtSummary);
            tabPersonal.Controls.Add(txtSummary);
            y += 110;

            // Profile Image
            var lblImage = new Label();
            lblImage.Text = "Profile Photo:";
            lblImage.ForeColor = System.Drawing.Color.FromArgb(180, 190, 205);
            lblImage.Location = new System.Drawing.Point(20, y + 5);
            lblImage.Size = new System.Drawing.Size(labelWidth, 25);
            tabPersonal.Controls.Add(lblImage);

            btnBrowseImage = new Button();
            btnBrowseImage.Text = "Browse...";
            btnBrowseImage.Location = new System.Drawing.Point(160, y);
            btnBrowseImage.Size = new System.Drawing.Size(150, 35);
            btnBrowseImage.FlatStyle = FlatStyle.Flat;
            btnBrowseImage.BackColor = System.Drawing.Color.FromArgb(50, 58, 72);
            btnBrowseImage.ForeColor = System.Drawing.Color.White;
            btnBrowseImage.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(82, 183, 136);
            btnBrowseImage.Cursor = Cursors.Hand;
            tabPersonal.Controls.Add(btnBrowseImage);

            lblImagePath = new Label();
            lblImagePath.Text = "No image selected";
            lblImagePath.ForeColor = System.Drawing.Color.FromArgb(120, 130, 145);
            lblImagePath.Location = new System.Drawing.Point(320, y + 8);
            lblImagePath.Size = new System.Drawing.Size(500, 25);
            tabPersonal.Controls.Add(lblImagePath);

            tabControl.TabPages.Add(tabPersonal);

            // ════════ TAB 2: Education ════════
            tabEducation = new TabPage("Education *");
            tabEducation.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);

            dgvEducation = new DataGridView();
            dgvEducation.Location = new System.Drawing.Point(20, 20);
            dgvEducation.Size = new System.Drawing.Size(840, 430);
            StyleDataGridView(dgvEducation);
            dgvEducation.Columns.Add("Institution", "Institution");
            dgvEducation.Columns.Add("Degree", "Degree");
            dgvEducation.Columns.Add("FieldOfStudy", "Field of Study");
            dgvEducation.Columns.Add("StartDate", "Start Date");
            dgvEducation.Columns.Add("EndDate", "End Date");
            dgvEducation.Columns.Add("Grade", "Grade");
            foreach (DataGridViewColumn col in dgvEducation.Columns)
                col.Width = 135;
            tabEducation.Controls.Add(dgvEducation);

            btnAddEducation = CreateActionButton("+ Add", 20, 465);
            btnRemoveEducation = CreateActionButton("- Remove", 160, 465);
            tabEducation.Controls.Add(btnAddEducation);
            tabEducation.Controls.Add(btnRemoveEducation);

            tabControl.TabPages.Add(tabEducation);

            // ════════ TAB 3: Experience ════════
            tabExperience = new TabPage("Experience");
            tabExperience.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);

            dgvExperience = new DataGridView();
            dgvExperience.Location = new System.Drawing.Point(20, 20);
            dgvExperience.Size = new System.Drawing.Size(840, 430);
            StyleDataGridView(dgvExperience);
            dgvExperience.Columns.Add("Company", "Company");
            dgvExperience.Columns.Add("JobTitle", "Job Title");
            dgvExperience.Columns.Add("Description", "Description");
            dgvExperience.Columns.Add("StartDate", "Start Date");
            dgvExperience.Columns.Add("EndDate", "End Date");
            dgvExperience.Columns["Company"]!.Width = 160;
            dgvExperience.Columns["JobTitle"]!.Width = 150;
            dgvExperience.Columns["Description"]!.Width = 280;
            dgvExperience.Columns["StartDate"]!.Width = 110;
            dgvExperience.Columns["EndDate"]!.Width = 110;
            tabExperience.Controls.Add(dgvExperience);

            btnAddExperience = CreateActionButton("+ Add", 20, 465);
            btnRemoveExperience = CreateActionButton("- Remove", 160, 465);
            tabExperience.Controls.Add(btnAddExperience);
            tabExperience.Controls.Add(btnRemoveExperience);

            tabControl.TabPages.Add(tabExperience);

            // ════════ TAB 4: Skills ════════
            tabSkills = new TabPage("Skills *");
            tabSkills.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);

            dgvSkills = new DataGridView();
            dgvSkills.Location = new System.Drawing.Point(20, 20);
            dgvSkills.Size = new System.Drawing.Size(840, 430);
            StyleDataGridView(dgvSkills);
            dgvSkills.Columns.Add("SkillName", "Skill Name");
            dgvSkills.Columns["SkillName"]!.Width = 400;

            var proficiencyCol = new DataGridViewComboBoxColumn();
            proficiencyCol.Name = "Proficiency";
            proficiencyCol.HeaderText = "Proficiency Level";
            proficiencyCol.Items.AddRange("Beginner", "Intermediate", "Advanced", "Expert");
            proficiencyCol.Width = 400;
            dgvSkills.Columns.Add(proficiencyCol);
            tabSkills.Controls.Add(dgvSkills);

            btnAddSkill = CreateActionButton("+ Add", 20, 465);
            btnRemoveSkill = CreateActionButton("- Remove", 160, 465);
            tabSkills.Controls.Add(btnAddSkill);
            tabSkills.Controls.Add(btnRemoveSkill);

            tabControl.TabPages.Add(tabSkills);

            // ─── Bottom Buttons ───
            pnlBottom = new Panel();
            pnlBottom.Location = new System.Drawing.Point(20, 640);
            pnlBottom.Size = new System.Drawing.Size(895, 60);
            pnlBottom.BackColor = System.Drawing.Color.Transparent;

            btnSaveJson = new Button();
            btnSaveJson.Text = "Save Resume";
            btnSaveJson.Size = new System.Drawing.Size(180, 45);
            btnSaveJson.Location = new System.Drawing.Point(0, 5);
            StyleBottomButton(btnSaveJson, System.Drawing.Color.FromArgb(86, 156, 214));

            btnNext = new Button();
            btnNext.Text = "Select Template  >>";
            btnNext.Size = new System.Drawing.Size(220, 45);
            btnNext.Location = new System.Drawing.Point(660, 5);
            StyleBottomButton(btnNext, System.Drawing.Color.FromArgb(82, 183, 136));

            btnBack = new Button();
            btnBack.Text = "<< Back";
            btnBack.Size = new System.Drawing.Size(130, 45);
            btnBack.Location = new System.Drawing.Point(510, 5);
            StyleBottomButton(btnBack, System.Drawing.Color.FromArgb(120, 130, 145));

            pnlBottom.Controls.Add(btnSaveJson);
            pnlBottom.Controls.Add(btnBack);
            pnlBottom.Controls.Add(btnNext);

            // ─── Add to Form ───
            this.Controls.Add(tabControl);
            this.Controls.Add(pnlBottom);

            this.ResumeLayout(false);
        }

        private TextBox CreateTextBox(string label, ref int y, int labelWidth, int inputWidth, int rowHeight, TabPage parent, bool isRequired = false)
        {
            var lbl = new Label();
            lbl.Text = label + (isRequired ? " *" : "") + ":";
            lbl.ForeColor = System.Drawing.Color.FromArgb(180, 190, 205);
            lbl.Location = new System.Drawing.Point(20, y + 5);
            lbl.Size = new System.Drawing.Size(labelWidth, 25);
            parent.Controls.Add(lbl);

            var txt = new TextBox();
            txt.Location = new System.Drawing.Point(160, y);
            txt.Size = new System.Drawing.Size(inputWidth, 30);
            StyleTextBox(txt);
            parent.Controls.Add(txt);

            y += rowHeight;
            return txt;
        }

        private void StyleTextBox(TextBox txt)
        {
            txt.BackColor = System.Drawing.Color.FromArgb(40, 46, 58);
            txt.ForeColor = System.Drawing.Color.White;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new System.Drawing.Font("Segoe UI", 10F);
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = System.Drawing.Color.FromArgb(36, 42, 54);
            dgv.GridColor = System.Drawing.Color.FromArgb(60, 68, 82);
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(40, 46, 58);
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(82, 183, 136);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(82, 183, 136);
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        private Button CreateActionButton(string text, int x, int y)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(120, 38);
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = System.Drawing.Color.FromArgb(50, 58, 72);
            btn.ForeColor = System.Drawing.Color.White;
            btn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(82, 183, 136);
            btn.Cursor = Cursors.Hand;
            return btn;
        }

        private void StyleBottomButton(Button btn, System.Drawing.Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = color;
            btn.BackColor = System.Drawing.Color.FromArgb(36, 42, 54);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        // Controls
        private TabControl tabControl;
        private TabPage tabPersonal;
        private TabPage tabEducation;
        private TabPage tabExperience;
        private TabPage tabSkills;
        private Panel pnlBottom;

        // Personal Info
        private TextBox txtFullName;
        private TextBox txtJobTitle;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private TextBox txtAddress;
        private TextBox txtLanguages;
        private TextBox txtSummary;
        private Button btnBrowseImage;
        private Label lblImagePath;

        // Education
        private DataGridView dgvEducation;
        private Button btnAddEducation;
        private Button btnRemoveEducation;

        // Experience
        private DataGridView dgvExperience;
        private Button btnAddExperience;
        private Button btnRemoveExperience;

        // Skills
        private DataGridView dgvSkills;
        private Button btnAddSkill;
        private Button btnRemoveSkill;

        // Bottom
        private Button btnSaveJson;
        private Button btnNext;
        private Button btnBack;
    }
}
