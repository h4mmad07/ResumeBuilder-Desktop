using ResumeBuilder.Models;
using ResumeBuilder.PDF;
using ResumeBuilder.Templates;

namespace ResumeBuilder.BLL
{
    /// <summary>
    /// Orchestrates PDF generation using resume data and a selected template.
    /// </summary>
    public class PDFManager
    {
        /// <summary>
        /// Generate a PDF file from resume data with a specified template.
        /// </summary>
        public void GeneratePDF(Resume resume, IResumeTemplate template, string outputPath)
        {
            PDFGenerator.Generate(resume, template, outputPath);
        }

        /// <summary>
        /// Generate PDF bytes for previewing.
        /// </summary>
        public byte[] GeneratePDFBytes(Resume resume, IResumeTemplate template)
        {
            return PDFGenerator.GenerateToBytes(resume, template);
        }
    }
}
