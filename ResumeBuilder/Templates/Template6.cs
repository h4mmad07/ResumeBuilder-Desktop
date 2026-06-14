using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using ResumeBuilder.Models;

namespace ResumeBuilder.Templates;

public class Template6 : IScalableTemplate
{
    public string TemplateName => "Elegant";
    public string Description => "Elegant single-column layout with navy and steel-blue accents.";
    public float Scale { get; set; } = 1.0f;

    private static readonly DeviceRgb NavyBlue    = new(23,  37,  84);
    private static readonly DeviceRgb SteelBlue   = new(70, 130, 180);
    private static readonly DeviceRgb LightBlue   = new(214, 234, 248);
    private static readonly DeviceRgb DarkText    = new(30,  30,  30);
    private static readonly DeviceRgb MutedText   = new(108, 117, 125);
    private static readonly DeviceRgb White       = new(255,255,255);

    public void ApplyTemplate(Document doc, Resume resume)
    {
        float s = Scale;
        doc.SetMargins(36 * s, 40 * s, 36 * s, 40 * s);

        // ── HEADER ───────────────────────────────────────────────────────────
        var headerTable = new Table(new float[] { 75f, 25f })
            .SetWidth(UnitValue.CreatePercentValue(100))
            .SetBorder(Border.NO_BORDER)
            .SetMarginBottom(6 * s);

        var nameCell = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(NavyBlue).SetPadding(16 * s);
        nameCell.Add(new Paragraph(resume.FullName)
            .SetFontColor(White).SetFontSize(22 * s).SetBold().SetMarginBottom(2 * s));
        nameCell.Add(new Paragraph(resume.JobTitle)
            .SetFontColor(new DeviceRgb(173, 216, 230)).SetFontSize(11 * s));

        var contactCell = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(SteelBlue).SetPadding(16 * s)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE);
        if (!string.IsNullOrEmpty(resume.Email))
            contactCell.Add(new Paragraph(resume.Email)
                .SetFontColor(White).SetFontSize(8 * s).SetMarginBottom(3 * s));
        if (!string.IsNullOrEmpty(resume.Phone))
            contactCell.Add(new Paragraph(resume.Phone)
                .SetFontColor(White).SetFontSize(8 * s).SetMarginBottom(3 * s));
        if (!string.IsNullOrEmpty(resume.Address))
            contactCell.Add(new Paragraph(resume.Address)
                .SetFontColor(White).SetFontSize(8 * s));

        if (!string.IsNullOrEmpty(resume.ProfileImagePath) && File.Exists(resume.ProfileImagePath))
        {
            try
            {
                var imgData = ImageDataFactory.Create(resume.ProfileImagePath);
                var img = new iText.Layout.Element.Image(imgData)
                    .SetWidth(60 * s).SetHeight(60 * s)
                    .SetBorderRadius(new BorderRadius(30 * s))
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.RIGHT);
                contactCell.Add(img);
            }
            catch { }
        }

        headerTable.AddCell(nameCell);
        headerTable.AddCell(contactCell);
        doc.Add(headerTable);

        // ── SUMMARY ──────────────────────────────────────────────────────────
        if (!string.IsNullOrEmpty(resume.Summary))
        {
            AddSection(doc, "PROFESSIONAL SUMMARY", s);
            doc.Add(new Paragraph(resume.Summary)
                .SetFontSize(9.5f * s).SetFontColor(DarkText)
                .SetMarginBottom(10 * s).SetMarginLeft(4 * s));
        }

        // ── EDUCATION ────────────────────────────────────────────────────────
        if (resume.EducationList.Count > 0)
        {
            AddSection(doc, "EDUCATION", s);
            foreach (var edu in resume.EducationList)
            {
                var row = new Table(new float[] { 70f, 30f })
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetBorder(Border.NO_BORDER).SetMarginBottom(6 * s);

                var left = new Cell().SetBorder(Border.NO_BORDER).SetPaddingLeft(4 * s);
                left.Add(new Paragraph(edu.Degree + (string.IsNullOrEmpty(edu.FieldOfStudy) ? "" : $" in {edu.FieldOfStudy}"))
                    .SetFontSize(10 * s).SetBold().SetFontColor(NavyBlue).SetMarginBottom(1 * s));
                left.Add(new Paragraph(edu.Institution)
                    .SetFontSize(9 * s).SetFontColor(DarkText));

                var right = new Cell().SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetVerticalAlignment(VerticalAlignment.TOP);
                // Badge style date
                right.Add(new Paragraph($"{edu.StartDate} – {edu.EndDate}")
                    .SetFontSize(8 * s).SetFontColor(White)
                    .SetBackgroundColor(SteelBlue)
                    .SetPaddingLeft(6).SetPaddingRight(6)
                    .SetPaddingTop(2).SetPaddingBottom(2)
                    .SetBorderRadius(new BorderRadius(3)));
                if (!string.IsNullOrEmpty(edu.Grade))
                    right.Add(new Paragraph($"Grade: {edu.Grade}")
                        .SetFontSize(8 * s).SetFontColor(MutedText).SetTextAlignment(TextAlignment.RIGHT));

                row.AddCell(left);
                row.AddCell(right);
                doc.Add(row);
            }
        }

        // ── EXPERIENCE ───────────────────────────────────────────────────────
        if (resume.ExperienceList.Count > 0)
        {
            AddSection(doc, "WORK EXPERIENCE", s);
            foreach (var exp in resume.ExperienceList)
            {
                var row = new Table(new float[] { 70f, 30f })
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetBorder(Border.NO_BORDER).SetMarginBottom(2 * s);

                var left = new Cell().SetBorder(Border.NO_BORDER).SetPaddingLeft(4 * s);
                left.Add(new Paragraph(exp.JobTitle)
                    .SetFontSize(10 * s).SetBold().SetFontColor(NavyBlue).SetMarginBottom(1 * s));
                left.Add(new Paragraph(exp.Company)
                    .SetFontSize(9 * s).SetFontColor(SteelBlue));

                var right = new Cell().SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetVerticalAlignment(VerticalAlignment.TOP);
                right.Add(new Paragraph($"{exp.StartDate} – {exp.EndDate}")
                    .SetFontSize(8 * s).SetFontColor(White)
                    .SetBackgroundColor(SteelBlue)
                    .SetPaddingLeft(6).SetPaddingRight(6)
                    .SetPaddingTop(2).SetPaddingBottom(2)
                    .SetBorderRadius(new BorderRadius(3)));

                row.AddCell(left);
                row.AddCell(right);
                doc.Add(row);

                if (!string.IsNullOrEmpty(exp.Description))
                    doc.Add(new Paragraph(exp.Description)
                        .SetFontSize(9 * s).SetFontColor(DarkText)
                        .SetMarginLeft(4 * s).SetMarginBottom(8 * s));
            }
        }

        // ── SKILLS & LANGUAGES ───────────────────────────────────────────────
        if (resume.SkillList.Count > 0 || !string.IsNullOrEmpty(resume.Languages))
        {
            var bottomTable = new Table(new float[] { resume.SkillList.Count > 0 ? 65f : 0f, 35f })
                .SetWidth(UnitValue.CreatePercentValue(100)).SetBorder(Border.NO_BORDER);

            if (resume.SkillList.Count > 0)
            {
                var skillCell = new Cell().SetBorder(Border.NO_BORDER);
                skillCell.Add(new Paragraph("SKILLS").SetFontSize(10 * s).SetBold()
                    .SetFontColor(NavyBlue).SetMarginBottom(6 * s));
                skillCell.Add(new LineSeparator(new SolidLine(1.5f))
                    .SetStrokeColor(SteelBlue).SetMarginBottom(8 * s));
                var skillWrap = new Paragraph().SetMarginBottom(4 * s);
                foreach (var skill in resume.SkillList)
                {
                    skillWrap.Add(new Text($" {skill.Name} [{skill.Proficiency}] ")
                        .SetFontSize(8 * s).SetFontColor(NavyBlue)
                        .SetBackgroundColor(LightBlue));
                    skillWrap.Add(new Text("  "));
                }
                skillCell.Add(skillWrap);
                bottomTable.AddCell(skillCell);
            }

            if (!string.IsNullOrEmpty(resume.Languages))
            {
                var langCell = new Cell().SetBorder(Border.NO_BORDER).SetPaddingLeft(10 * s);
                langCell.Add(new Paragraph("LANGUAGES").SetFontSize(10 * s).SetBold()
                    .SetFontColor(NavyBlue).SetMarginBottom(6 * s));
                langCell.Add(new LineSeparator(new SolidLine(1.5f))
                    .SetStrokeColor(SteelBlue).SetMarginBottom(8 * s));
                foreach (var lang in resume.Languages.Split(',', StringSplitOptions.RemoveEmptyEntries))
                    langCell.Add(new Paragraph("• " + lang.Trim())
                        .SetFontSize(9 * s).SetFontColor(DarkText).SetMarginBottom(2 * s));
                bottomTable.AddCell(langCell);
            }
            doc.Add(bottomTable);
        }
    }

    private static void AddSection(Document doc, string title, float s)
    {
        doc.Add(new Paragraph(title)
            .SetFontSize(11 * s).SetBold().SetFontColor(NavyBlue).SetMarginBottom(4 * s));
        doc.Add(new LineSeparator(new SolidLine(1.5f))
            .SetStrokeColor(SteelBlue).SetMarginBottom(8 * s));
    }
}
