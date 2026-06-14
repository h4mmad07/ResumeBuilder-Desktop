using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Element;

namespace ResumeBuilder.Templates;

public static class ParagraphExtensions
{
    public static Paragraph SetBold(this Paragraph paragraph)
    {
        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        return paragraph.SetFont(boldFont);
    }
}
