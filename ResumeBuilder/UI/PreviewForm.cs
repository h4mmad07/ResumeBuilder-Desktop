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
    public partial class PreviewForm : Form
    {
        private readonly string _tempPdfPath;
        private readonly Resume _resume;
        private readonly IResumeTemplate _template;
        private readonly PDFManager _pdfManager = new();

        public PreviewForm(string tempPdfPath, Resume resume, IResumeTemplate template)
        {
            _tempPdfPath = tempPdfPath;
            _resume = resume;
            _template = template;

            InitializeComponent();
            WireEvents();
            LoadPreview();
        }

        private void WireEvents()
        {
            btnSaveAs.Click += BtnSaveAs_Click;
            btnOpenExternal.Click += BtnOpenExternal_Click;
            btnClose.Click += (s, e) => this.Close();
        }

        private void LoadPreview()
        {
            try
            {
                if (File.Exists(_tempPdfPath))
                {
                    // Try to load in embedded browser; if it fails, offer to open externally
                    try
                    {
                        webBrowser.Navigate(_tempPdfPath);
                    }
                    catch
                    {
                        // If embedded preview fails, auto-open in external viewer
                        OpenExternal();
                    }
                }
                else
                {
                    MessageBox.Show("PDF file was not generated. Please try again.",
                        "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load preview:\n{ex.Message}",
                    "Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSaveAs_Click(object? sender, EventArgs e)
        {
            try
            {
                var savePath = FileHelper.GetSavePath("PDF Files|*.pdf", "pdf");
                if (savePath != null)
                {
                    // Copy the already-generated temp PDF to the target location
                    File.Copy(_tempPdfPath, savePath, true);
                    MessageBox.Show("PDF saved successfully!", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving PDF:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOpenExternal_Click(object? sender, EventArgs e)
        {
            OpenExternal();
        }

        private void OpenExternal()
        {
            try
            {
                if (File.Exists(_tempPdfPath))
                {
                    Process.Start(new ProcessStartInfo(_tempPdfPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open PDF:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up temp file - navigate away first so file is not locked
            try
            {
                webBrowser.Navigate("about:blank");
                // Small delay to let browser release the file
                System.Threading.Thread.Sleep(200);
                if (File.Exists(_tempPdfPath))
                    File.Delete(_tempPdfPath);
            }
            catch { /* ignore cleanup errors */ }

            base.OnFormClosing(e);
        }
    }
}
