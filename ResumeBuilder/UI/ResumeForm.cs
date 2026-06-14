using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ResumeBuilder.BLL;
using ResumeBuilder.DAL;
using ResumeBuilder.Helpers;
using ResumeBuilder.Models;

namespace ResumeBuilder.UI
{
    public partial class ResumeForm : Form
    {
        private readonly ResumeManager _resumeManager = new();
        private string _profileImagePath = string.Empty;
        private Resume? _loadedResume;

        public ResumeForm()
        {
            InitializeComponent();
            WireEvents();
        }

        /// <summary>
        /// Constructor for loading an existing resume.
        /// </summary>
        public ResumeForm(Resume existingResume) : this()
        {
            _loadedResume = existingResume;
            PopulateFromResume(existingResume);
        }

        private void WireEvents()
        {
            btnBrowseImage.Click += BtnBrowseImage_Click;
            btnAddEducation.Click += (s, e) => dgvEducation.Rows.Add();
            btnRemoveEducation.Click += (s, e) => RemoveSelectedRow(dgvEducation);
            btnAddExperience.Click += (s, e) => dgvExperience.Rows.Add();
            btnRemoveExperience.Click += (s, e) => RemoveSelectedRow(dgvExperience);
            btnAddSkill.Click += (s, e) => dgvSkills.Rows.Add();
            btnRemoveSkill.Click += (s, e) => RemoveSelectedRow(dgvSkills);
            btnSaveJson.Click += BtnSaveJson_Click;
            btnNext.Click += BtnNext_Click;
            btnBack.Click += BtnBack_Click;
        }

        private void BtnBrowseImage_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Title = "Select Profile Photo"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (ImageHelper.IsValidImage(dialog.FileName))
                {
                    _profileImagePath = dialog.FileName;
                    lblImagePath.Text = System.IO.Path.GetFileName(dialog.FileName);
                    lblImagePath.ForeColor = System.Drawing.Color.FromArgb(82, 183, 136);
                }
                else
                {
                    MessageBox.Show("Please select a valid image file (JPG, PNG, BMP).",
                        "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private Resume BuildResumeFromForm()
        {
            var educations = new List<Education>();
            foreach (DataGridViewRow row in dgvEducation.Rows)
            {
                if (row.IsNewRow) continue;
                educations.Add(new Education
                {
                    Institution = row.Cells["Institution"].Value?.ToString() ?? "",
                    Degree = row.Cells["Degree"].Value?.ToString() ?? "",
                    FieldOfStudy = row.Cells["FieldOfStudy"].Value?.ToString() ?? "",
                    StartDate = row.Cells["StartDate"].Value?.ToString() ?? "",
                    EndDate = row.Cells["EndDate"].Value?.ToString() ?? "",
                    Grade = row.Cells["Grade"].Value?.ToString() ?? ""
                });
            }

            var experiences = new List<Experience>();
            foreach (DataGridViewRow row in dgvExperience.Rows)
            {
                if (row.IsNewRow) continue;
                experiences.Add(new Experience
                {
                    Company = row.Cells["Company"].Value?.ToString() ?? "",
                    JobTitle = row.Cells["JobTitle"].Value?.ToString() ?? "",
                    Description = row.Cells["Description"].Value?.ToString() ?? "",
                    StartDate = row.Cells["StartDate"].Value?.ToString() ?? "",
                    EndDate = row.Cells["EndDate"].Value?.ToString() ?? ""
                });
            }

            var skills = new List<Skill>();
            foreach (DataGridViewRow row in dgvSkills.Rows)
            {
                if (row.IsNewRow) continue;
                var name = row.Cells["SkillName"].Value?.ToString() ?? "";
                var profStr = row.Cells["Proficiency"].Value?.ToString() ?? "Beginner";
                Enum.TryParse<ProficiencyLevel>(profStr, out var prof);
                skills.Add(new Skill { Name = name, Proficiency = prof });
            }

            return _resumeManager.CreateResume(
                txtFullName.Text, txtJobTitle.Text, txtEmail.Text, txtPhone.Text,
                txtAddress.Text, txtSummary.Text, txtLanguages.Text, _profileImagePath,
                educations, experiences, skills);
        }

        private void PopulateFromResume(Resume resume)
        {
            txtFullName.Text = resume.FullName;
            txtJobTitle.Text = resume.JobTitle;
            txtEmail.Text = resume.Email;
            txtPhone.Text = resume.Phone;
            txtAddress.Text = resume.Address;
            txtSummary.Text = resume.Summary;
            txtLanguages.Text = resume.Languages;
            _profileImagePath = resume.ProfileImagePath;
            if (!string.IsNullOrEmpty(_profileImagePath))
                lblImagePath.Text = System.IO.Path.GetFileName(_profileImagePath);

            foreach (var edu in resume.EducationList)
                dgvEducation.Rows.Add(edu.Institution, edu.Degree, edu.FieldOfStudy, edu.StartDate, edu.EndDate, edu.Grade);

            foreach (var exp in resume.ExperienceList)
                dgvExperience.Rows.Add(exp.Company, exp.JobTitle, exp.Description, exp.StartDate, exp.EndDate);

            foreach (var skill in resume.SkillList)
                dgvSkills.Rows.Add(skill.Name, skill.Proficiency.ToString());
        }

        private void BtnSaveJson_Click(object? sender, EventArgs e)
        {
            var resume = BuildResumeFromForm();
            var path = FileHelper.GetSavePath("Resume Files|*.json", "json");
            if (path != null)
            {
                JsonStorageHelper.SaveResume(resume, path);
                MessageBox.Show("Resume saved successfully!", "Saved",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnNext_Click(object? sender, EventArgs e)
        {
            var resume = BuildResumeFromForm();
            var errors = _resumeManager.ValidateResume(resume);

            if (errors.Count > 0)
            {
                MessageBox.Show("Please fix the following:\n\n- " + string.Join("\n- ", errors),
                    "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var templateForm = new TemplateSelectionForm(resume);
            templateForm.Show();
            this.Hide();
            templateForm.FormClosed += (s, args) => this.Show();
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void RemoveSelectedRow(DataGridView dgv)
        {
            if (dgv.SelectedRows.Count > 0 && !dgv.SelectedRows[0].IsNewRow)
                dgv.Rows.Remove(dgv.SelectedRows[0]);
            else if (dgv.CurrentRow != null && !dgv.CurrentRow.IsNewRow)
                dgv.Rows.Remove(dgv.CurrentRow);
        }
    }
}
