using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using ResumeBuilder.Models;

namespace ResumeBuilder.Templates;

public class Template4 : IScalableTemplate
{
    public string TemplateName => "Minimal";
    public string Description => "Sleek and clean two-column layout with a peach header banner and teal sidebar.";
    public float Scale { get; set; } = 1.0f;

    private static readonly DeviceRgb PeachHeader   = new(255, 111,  97);
    private static readonly DeviceRgb TealSidebar   = new( 32, 178, 170);
    private static readonly DeviceRgb TealLight     = new(224, 247, 245);
    private static readonly DeviceRgb PeachLight    = new(255, 235, 230);
    private static readonly DeviceRgb DarkText      = new( 40,  40,  40);
    private static readonly DeviceRgb White         = new(255,255,255);
    private static readonly DeviceRgb LightGray     = new(248,248,248);

    public void ApplyTemplate(Document doc, Resume resume)
    {
        float s = Scale;
        doc.SetMargins(0, 0, 0, 0);

        // ── HEADER BANNER ────────────────────────────────────────────────────
        var header = new Table(new float[] { 65f, 35f })
            .SetWidth(UnitValue.CreatePercentValue(100))
            .SetBorder(Border.NO_BORDER);

        var nameCell = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(PeachHeader).SetPadding(22 * s);
        nameCell.Add(new Paragraph(resume.FullName)
            .SetFontColor(White).SetFontSize(24 * s).SetBold().SetMarginBottom(3 * s));
        nameCell.Add(new Paragraph(resume.JobTitle)
            .SetFontColor(PeachLight).SetFontSize(11 * s).SetMarginBottom(6 * s));

        var infoCell = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(new DeviceRgb(220, 80, 70)).SetPadding(22 * s)
            .SetVerticalAlignment(VerticalAlignment.MIDDLE);

        if (!string.IsNullOrEmpty(resume.ProfileImagePath) && File.Exists(resume.ProfileImagePath))
        {
            try
            {
                var imgData = ImageDataFactory.Create(resume.ProfileImagePath);
                var img = new iText.Layout.Element.Image(imgData)
                    .SetWidth(70 * s).SetHeight(70 * s)
                    .SetBorderRadius(new BorderRadius(35 * s))
                    .SetBorder(new SolidBorder(White, 2))
                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                    .SetMarginBottom(8 * s);
                infoCell.Add(img);
            }
            catch { }
        }
        if (!string.IsNullOrEmpty(resume.Email))
            infoCell.Add(new Paragraph(resume.Email)
                .SetFontColor(White).SetFontSize(8 * s).SetMarginBottom(2 * s));
        if (!string.IsNullOrEmpty(resume.Phone))
            infoCell.Add(new Paragraph(resume.Phone)
                .SetFontColor(White).SetFontSize(8 * s).SetMarginBottom(2 * s));

        header.AddCell(nameCell);
        header.AddCell(infoCell);
        doc.Add(header);

        // ── BODY: sidebar + main ─────────────────────────────────────────────
        var body = new Table(new float[] { 32f, 68f })
            .SetWidth(UnitValue.CreatePercentValue(100))
            .SetBorder(Border.NO_BORDER);

        // Sidebar
        var sidebar = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(TealLight).SetPadding(14 * s);

        AddSidebarHeading(sidebar, "CONTACT", TealSidebar, s);
        if (!string.IsNullOrEmpty(resume.Address))
            AddSidebarText(sidebar, resume.Address, s);
        if (!string.IsNullOrEmpty(resume.Phone))
            AddSidebarText(sidebar, resume.Phone, s);
        if (!string.IsNullOrEmpty(resume.Email))
            AddSidebarText(sidebar, resume.Email, s);

        if (!string.IsNullOrEmpty(resume.Languages))
        {
            AddSidebarHeading(sidebar, "LANGUAGES", TealSidebar, s);
            foreach (var lang in resume.Languages.Split(',', StringSplitOptions.RemoveEmptyEntries))
                AddSidebarText(sidebar, "• " + lang.Trim(), s);
        }

        if (resume.SkillList.Count > 0)
        {
            AddSidebarHeading(sidebar, "SKILLS", TealSidebar, s);
            foreach (var skill in resume.SkillList)
            {
                sidebar.Add(new Paragraph(skill.Name)
                    .SetFontSize(8.5f * s).SetFontColor(DarkText).SetMarginBottom(1 * s));
                int dots = skill.Proficiency switch
                {
                    ProficiencyLevel.Beginner => 1,
                    ProficiencyLevel.Intermediate => 2,
                    ProficiencyLevel.Advanced => 3,
                    _ => 4
                };
                var dot = new Paragraph();
                for (int i = 0; i < 4; i++)
                    dot.Add(new Text("● ").SetFontColor(i < dots ? TealSidebar : new DeviceRgb(180, 220, 215))
                        .SetFontSize(9 * s));
                sidebar.Add(dot.SetMarginBottom(5 * s));
            }
        }
        body.AddCell(sidebar);

        // Main content
        var main = new Cell().SetBorder(Border.NO_BORDER)
            .SetBackgroundColor(LightGray).SetPadding(18 * s);

        if (!string.IsNullOrEmpty(resume.Summary))
        {
            AddMainHeading(main, "ABOUT ME", PeachHeader, s);
            main.Add(new Paragraph(resume.Summary)
                .SetFontSize(9.5f * s).SetFontColor(DarkText).SetMarginBottom(10 * s));
        }

        if (resume.EducationList.Count > 0)
        {
            AddMainHeading(main, "EDUCATION", PeachHeader, s);
            foreach (var edu in resume.EducationList)
            {
                main.Add(new Paragraph($"{edu.Degree}" + (string.IsNullOrEmpty(edu.FieldOfStudy) ? "" : $" – {edu.FieldOfStudy}"))
                    .SetFontSize(10 * s).SetBold().SetFontColor(DarkText).SetMarginBottom(1 * s));
                var row = new Table(new float[] { 60f, 40f })
                    .SetWidth(UnitValue.CreatePercentValue(100)).SetBorder(Border.NO_BORDER)
                    .SetMarginBottom(6 * s);
                row.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph(edu.Institution).SetFontSize(9 * s)
                        .SetFontColor(TealSidebar)));
                row.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph($"{edu.StartDate} – {edu.EndDate}")
                        .SetFontSize(8 * s).SetFontColor(new DeviceRgb(130,130,130))));
                main.Add(row);
            }
        }

        if (resume.ExperienceList.Count > 0)
        {
            AddMainHeading(main, "EXPERIENCE", PeachHeader, s);
            foreach (var exp in resume.ExperienceList)
            {
                main.Add(new Paragraph(exp.JobTitle)
                    .SetFontSize(10 * s).SetBold().SetFontColor(DarkText).SetMarginBottom(1 * s));
                var row = new Table(new float[] { 60f, 40f })
                    .SetWidth(UnitValue.CreatePercentValue(100)).SetBorder(Border.NO_BORDER)
                    .SetMarginBottom(2 * s);
                row.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph(exp.Company).SetFontSize(9 * s).SetFontColor(TealSidebar)));
                row.AddCell(new Cell().SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new Paragraph($"{exp.StartDate} – {exp.EndDate}")
                        .SetFontSize(8 * s).SetFontColor(new DeviceRgb(130,130,130))));
                main.Add(row);
                if (!string.IsNullOrEmpty(exp.Description))
                    main.Add(new Paragraph(exp.Description)
                        .SetFontSize(9 * s).SetFontColor(DarkText).SetMarginBottom(8 * s));
            }
        }

        body.AddCell(main);
        doc.Add(body);
    }

    private static void AddSidebarHeading(Cell cell, string title, DeviceRgb color, float s)
    {
        cell.Add(new Paragraph(title)
            .SetFontColor(color).SetFontSize(9 * s).SetBold()
            .SetMarginTop(10 * s).SetMarginBottom(3 * s));
        cell.Add(new LineSeparator(new SolidLine(0.5f))
            .SetStrokeColor(color).SetMarginBottom(4 * s));
    }

    private static void AddSidebarText(Cell cell, string text, float s)
    {
        cell.Add(new Paragraph(text)
            .SetFontSize(8.5f * s).SetFontColor(new DeviceRgb(60, 60, 60)).SetMarginBottom(2 * s));
    }

    private static void AddMainHeading(Cell cell, string title, DeviceRgb color, float s)
    {
        cell.Add(new Paragraph(title)
            .SetFontSize(11 * s).SetBold().SetFontColor(color).SetMarginBottom(4 * s));
        cell.Add(new LineSeparator(new SolidLine(1f))
            .SetStrokeColor(color).SetMarginBottom(6 * s));
    }
}
