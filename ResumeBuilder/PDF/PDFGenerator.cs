using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using ResumeBuilder.Models;
using ResumeBuilder.Templates;

namespace ResumeBuilder.PDF
{
    /// <summary>
    /// Generates PDF documents from Resume data using a selected template.
    /// </summary>
    public static class PDFGenerator
    {
        /// <summary>
        /// Generate a PDF file from a Resume using the specified template.
        /// </summary>
        public static void Generate(Resume resume, IResumeTemplate template, string outputPath)
        {
            if (template is IScalableTemplate scalableTemplate)
            {
                float optimalScale = FindOptimalScale(resume, scalableTemplate);
                scalableTemplate.Scale = optimalScale;
            }

            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);

            try
            {
                template.ApplyTemplate(document, resume);
            }
            finally
            {
                document.Close();
            }
        }

        /// <summary>
        /// Generate a PDF to a MemoryStream for previewing.
        /// </summary>
        public static byte[] GenerateToBytes(Resume resume, IResumeTemplate template)
        {
            if (template is IScalableTemplate scalableTemplate)
            {
                float optimalScale = FindOptimalScale(resume, scalableTemplate);
                scalableTemplate.Scale = optimalScale;
            }

            var memoryStream = new MemoryStream();
            var writer = new PdfWriter(memoryStream);
            writer.SetCloseStream(false);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);

            try
            {
                template.ApplyTemplate(document, resume);
            }
            finally
            {
                document.Close();
            }

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Find the optimal scale factor (between 0.5 and 1.0) so that the generated PDF fits on exactly 1 page.
        /// </summary>
        private static float FindOptimalScale(Resume resume, IScalableTemplate scalableTemplate)
        {
            float low = 0.5f;
            float high = 1.0f;
            float optimal = 1.0f;

            // Perform binary search with 5 iterations (giving precision of ~0.02)
            for (int i = 0; i < 5; i++)
            {
                float mid = (low + high) / 2.0f;
                scalableTemplate.Scale = mid;

                using (var ms = new MemoryStream())
                {
                    var writer = new PdfWriter(ms);
                    writer.SetCloseStream(false);
                    var pdf = new PdfDocument(writer);
                    var doc = new Document(pdf, iText.Kernel.Geom.PageSize.A4);

                    try
                    {
                        scalableTemplate.ApplyTemplate(doc, resume);
                    }
                    finally
                    {
                        doc.Close();
                    }

                    ms.Position = 0;
                    using (var reader = new PdfReader(ms))
                    {
                        using (var pdfDoc = new PdfDocument(reader))
                        {
                            int pages = pdfDoc.GetNumberOfPages();
                            if (pages <= 1)
                            {
                                // Fits! Try a larger scale
                                optimal = mid;
                                low = mid + 0.02f;
                            }
                            else
                            {
                                // Too big! Scale down
                                high = mid - 0.02f;
                            }
                        }
                    }
                }
            }

            return optimal;
        }
    }
}
