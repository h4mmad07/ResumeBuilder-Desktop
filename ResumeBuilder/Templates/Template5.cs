using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using ResumeBuilder.Models;

namespace ResumeBuilder.Templates;

public class Template5 : IScalableTemplate
{
    public string TemplateName => "Professional";
    public string Description => "Professional business two-column layout with slate-blue sidebar and teal accents.";
    public float Scale { get; set; } = 1.0f;

    private static readonly DeviceRgb SidebarBg   = new(44,  62,  80);
    private static readonly DeviceRgb TealAccent  = new(26, 188, 156);
    private static readonly DeviceRgb White       = new(255,255,255);
    private static readonly DeviceRgb LightGray   = new(236,240,241);
    private static readonly DeviceRgb DarkText    = new(44, 62, 80);
    private static readonly DeviceRgb MutedText   = new(127,140,141);

    public void ApplyTemplate(Document doc, Resume resume)
    {
        float s = Scale;
        doc.SetMargins(0, 0, 0, 0);

        // Outer table: sidebar (35%) + main (65%)
        var outer = new Table(new float[] { 35f, 65f })
            .SetWidth(UnitValue.CreatePercentValue(100))
            .SetBorder(Border.NO_BORDER);

        // ── SIDEBAR ──────────────────────────────────────────────────────────
        var sidebar = new Cell().SetBackgroundColor(SidebarBg)
            .SetBorder(Border.NO_BORDER)
            .SetPadding(16 * s);

        // Profile image
        if (!string.IsNullOrEmpty(resume.ProfileImagePath) && File.Exists(resume.ProfileImagePath))
        {
            try
            {
                var imgData = ImageDataFactory.Create(resume.ProfileImagePath);
                var img = new iText.Layout.Element.Image(imgData)
                    .SetWidth(90 * s).SetHeight(90 * s)
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .SetBorderRadius(new BorderRadius(45 * s))
                    .SetMarginBottom(10 * s);
                sidebar.Add(img);
            }
            catch { }
        }

        // Name
        sidebar.Add(new Paragraph(resume.FullName)
            .SetFontColor(White).SetFontSize(15 * s).SetBold()
            .SetTextAlignment(TextAlignment.CENTER).SetMarginBottom(2 * s));
        sidebar.Add(new Paragraph(resume.JobTitle)
            .SetFontColor(TealAccent).SetFontSize(10 * s)
            .SetTextAlignment(TextAlignment.CENTER).SetMarginBottom(12 * s));

        AddSidebarSection(sidebar, "CONTACT", s);
        if (!string.IsNullOrEmpty(resume.Phone))
            AddSidebarItem(sidebar, "📞 " + resume.Phone, s);
        if (!string.IsNullOrEmpty(resume.Email))
            AddSidebarItem(sidebar, "✉ " + resume.Email, s);
        if (!string.IsNullOrEmpty(resume.Address))
            AddSidebarItem(sidebar, "📍 " + resume.Address, s);

        if (!string.IsNullOrEmpty(resume.Languages))
        {
            AddSidebarSection(sidebar, "LANGUAGES", s);
            foreach (var lang in resume.Languages.Split(',', StringSplitOptions.RemoveEmptyEntries))
                AddSidebarItem(sidebar, lang.Trim(), s);
        }

        if (resume.SkillList.Count > 0)
        {
            AddSidebarSection(sidebar, "SKILLS", s);
            foreach (var skill in resume.SkillList)
            {
                sidebar.Add(new Paragraph(skill.Name)
                    .SetFontColor(White).SetFontSize(9 * s).SetMarginBottom(1 * s));
                // Progress bar
                int filled = skill.Proficiency switch
                {
                    ProficiencyLevel.Beginner => 1,
                    ProficiencyLevel.Intermediate => 2,
                    ProficiencyLevel.Advanced => 3,
                    _ => 4
                };
                var barTable = new Table(4).SetWidth(UnitValue.CreatePercentValue(100))
                    .SetBorder(Border.NO_BORDER).SetMarginBottom(6 * s);
                for (int i = 0; i < 4; i++)
                {
                    var seg = new Cell().SetHeight(5 * s)
                        .SetBackgroundColor(i < filled ? TealAccent : new DeviceRgb(74, 95, 110))
                        .SetBorder(Border.NO_BORDER).SetPaddingRight(2);
                    barTable.AddCell(seg);
                }
                sidebar.Add(barTable);
            }
        }
        outer.AddCell(sidebar);

        // ── MAIN CONTENT ─────────────────────────────────────────────────────
        var main = new Cell().SetBorder(Border.NO_BORDER).SetPadding(20 * s)
            .SetBackgroundColor(new DeviceRgb(250, 250, 250));

        if (!string.IsNullOrEmpty(resume.Summary))
        {
            AddMainSection(main, "PROFESSIONAL SUMMARY", TealAccent, s);
            main.Add(new Paragraph(resume.Summary)
                .SetFontSize(9.5f * s).SetFontColor(DarkText).SetMarginBottom(10 * s));
        }

        if (resume.EducationList.Count > 0)
        {
            AddMainSection(main, "EDUCATION", TealAccent, s);
            foreach (var edu in resume.EducationList)
            {
                main.Add(new Paragraph($"{edu.Degree}" + (string.IsNullOrEmpty(edu.FieldOfStudy) ? "" : $" – {edu.FieldOfStudy}"))
                    .SetFontSize(10 * s).SetBold().SetFontColor(DarkText).SetMarginBottom(1 * s));
                main.Add(new Paragraph(edu.Institution)
                    .SetFontSize(9 * s).SetFontColor(TealAccent).SetMarginBottom(1 * s));
                main.Add(new Paragraph($"{edu.StartDate} – {edu.EndDate}" + (string.IsNullOrEmpty(edu.Grade) ? "" : $"  |  Grade: {edu.Grade}"))
                    .SetFontSize(8.5f * s).SetFontColor(MutedText).SetMarginBottom(7 * s));
            }
        }

        if (resume.ExperienceList.Count > 0)
        {
            AddMainSection(main, "EXPERIENCE", TealAccent, s);
            foreach (var exp in resume.ExperienceList)
            {
                main.Add(new Paragraph(exp.JobTitle)
                    .SetFontSize(10 * s).SetBold().SetFontColor(DarkText).SetMarginBottom(1 * s));
                main.Add(new Paragraph(exp.Company)
                    .SetFontSize(9 * s).SetFontColor(TealAccent).SetMarginBottom(1 * s));
                main.Add(new Paragraph($"{exp.StartDate} – {exp.EndDate}")
                    .SetFontSize(8.5f * s).SetFontColor(MutedText).SetMarginBottom(2 * s));
                if (!string.IsNullOrEmpty(exp.Description))
                    main.Add(new Paragraph(exp.Description)
                        .SetFontSize(9 * s).SetFontColor(DarkText).SetMarginBottom(7 * s));
            }
        }

        outer.AddCell(main);
        doc.Add(outer);
    }

    private static void AddSidebarSection(Cell cell, string title, float s)
    {
        cell.Add(new Paragraph(title)
            .SetFontColor(new DeviceRgb(26, 188, 156))
            .SetFontSize(8.5f * s).SetBold().SetMarginTop(10 * s).SetMarginBottom(4 * s));
        cell.Add(new LineSeparator(new SolidLine(0.5f))
            .SetStrokeColor(new DeviceRgb(74, 95, 110)).SetMarginBottom(4 * s));
    }

    private static void AddSidebarItem(Cell cell, string text, float s)
    {
        cell.Add(new Paragraph(text)
            .SetFontColor(new DeviceRgb(189, 195, 199))
            .SetFontSize(8.5f * s).SetMarginBottom(3 * s));
    }

    private static void AddMainSection(Cell cell, string title, DeviceRgb color, float s)
    {
        cell.Add(new Paragraph(title)
            .SetFontSize(11 * s).SetBold().SetFontColor(color).SetMarginBottom(4 * s));
        cell.Add(new LineSeparator(new SolidLine(1f))
            .SetStrokeColor(color).SetMarginBottom(6 * s));
    }
}
