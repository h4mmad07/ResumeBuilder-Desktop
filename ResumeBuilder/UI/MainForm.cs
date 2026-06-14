using System;
using System.Windows.Forms;
using ResumeBuilder.DAL;
using ResumeBuilder.Models;

namespace ResumeBuilder.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            WireEvents();
        }

        private void WireEvents()
        {
            btnCreateNew.Click += BtnCreateNew_Click;
            btnLoadExisting.Click += BtnLoadExisting_Click;
            btnExit.Click += BtnExit_Click;
        }

        private void BtnCreateNew_Click(object? sender, EventArgs e)
        {
            var resumeForm = new ResumeForm();
            resumeForm.Show();
            this.Hide();
            resumeForm.FormClosed += (s, args) => this.Show();
        }

        private void BtnLoadExisting_Click(object? sender, EventArgs e)
        {
            var path = Helpers.FileHelper.GetOpenPath("Resume Files|*.json", "json");
            if (path != null)
            {
                var resume = JsonStorageHelper.LoadResume(path);
                if (resume != null)
                {
                    var resumeForm = new ResumeForm(resume);
                    resumeForm.Show();
                    this.Hide();
                    resumeForm.FormClosed += (s, args) => this.Show();
                }
                else
                {
                    MessageBox.Show("Failed to load the resume file.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
