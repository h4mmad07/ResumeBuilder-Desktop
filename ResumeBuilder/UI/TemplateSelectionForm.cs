using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ResumeBuilder.BLL;
using ResumeBuilder.Helpers;
using ResumeBuilder.Models;
using ResumeBuilder.Templates;

namespace ResumeBuilder.UI
{
    public partial class TemplateSelectionForm : Form
    {
        private readonly Resume _resume;
        private readonly TemplateManager _templateManager = new();
        private readonly PDFManager _pdfManager = new();
        private int _selectedTemplateIndex = 0; // 0-based index for selected template

        public TemplateSelectionForm(Resume resume)
        {
            _resume = resume;
            InitializeComponent();
            WireEvents();
            HighlightCards();
        }

        private void WireEvents()
        {
            btnPreview.Click += BtnPreview_Click;
            btnGenerate.Click += BtnGenerate_Click;

            AttachCardClick(pnlTemplate1, 0);
            AttachCardClick(pnlTemplate2, 1);
            AttachCardClick(pnlTemplate3, 2);
            AttachCardClick(pnlTemplate4, 3);
            AttachCardClick(pnlTemplate5, 4);
            AttachCardClick(pnlTemplate6, 5);

            // Attach click event to panel and all its children
            void AttachCardClick(Panel panel, int index)
            {
                panel.Click += (s, e) => { _selectedTemplateIndex = index; HighlightCards(); };
                foreach (Control c in panel.Controls)
                    c.Click += (s, e) => { _selectedTemplateIndex = index; HighlightCards(); };
            }
        }

        private void HighlightCards()
        {
            var selected = System.Drawing.Color.FromArgb(44, 52, 68);
            var normal = System.Drawing.Color.FromArgb(36, 42, 54);

            // Border color for selection
            var highlightBorder = System.Drawing.Color.FromArgb(82, 183, 136); // Green highlight
            var normalBorder = System.Drawing.Color.FromArgb(36, 42, 54);

            // Helper to set both background and border color
            void SetPanelStyle(Panel panel, bool isSelected)
            {
                panel.BackColor = isSelected ? selected : normal;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Padding = new System.Windows.Forms.Padding(2);
                panel.ForeColor = System.Drawing.Color.White;
                // Use WinForms native border color hack
                if (isSelected)
                {
                    panel.BackColor = selected;
                    panel.CreateGraphics().DrawRectangle(new System.Drawing.Pen(highlightBorder, 4), 0, 0, panel.Width - 1, panel.Height - 1);
                }
                else
                {
                    panel.BackColor = normal;
                    panel.CreateGraphics().DrawRectangle(new System.Drawing.Pen(normalBorder, 2), 0, 0, panel.Width - 1, panel.Height - 1);
                }
            }

            SetPanelStyle(pnlTemplate1, _selectedTemplateIndex == 0);
            SetPanelStyle(pnlTemplate2, _selectedTemplateIndex == 1);
            SetPanelStyle(pnlTemplate3, _selectedTemplateIndex == 2);
            SetPanelStyle(pnlTemplate4, _selectedTemplateIndex == 3);
            SetPanelStyle(pnlTemplate5, _selectedTemplateIndex == 4);
            SetPanelStyle(pnlTemplate6, _selectedTemplateIndex == 5);
        }

        private IResumeTemplate GetSelectedTemplate()
        {
            // Map to original template class names
            string[] names = { "Modern", "Classic", "Elegant", "Minimal", "Professional", "Creative" };
            return _templateManager.GetTemplate(names[_selectedTemplateIndex])!;
        }

        private void BtnPreview_Click(object? sender, EventArgs e)
        {
            try
            {
                var template = GetSelectedTemplate();
                string tempPath = Path.Combine(Path.GetTempPath(), $"ResumePreview_{Guid.NewGuid():N}.pdf");
                _pdfManager.GeneratePDF(_resume, template, tempPath);

                var previewForm = new PreviewForm(tempPath, _resume, template);
                previewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating preview:\n{ex.Message}\n\n{ex.InnerException?.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerate_Click(object? sender, EventArgs e)
        {
            try
            {
                var template = GetSelectedTemplate();
                var savePath = FileHelper.GetSavePath("PDF Files|*.pdf", "pdf");

                if (savePath != null)
                {
                    _pdfManager.GeneratePDF(_resume, template, savePath);
                    var result = MessageBox.Show(
                        "PDF generated successfully!\n\nWould you like to open it?",
                        "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(savePath) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating PDF:\n{ex.Message}\n\n{ex.InnerException?.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TemplateSelectionForm_Load(object sender, EventArgs e)
        {

        }
    }
}
