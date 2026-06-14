using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using ResumeBuilder.Models;
using System.Collections.Generic;
using System.Linq;

namespace ResumeBuilder.Templates
{
    /// <summary>
    /// Professional template – classic single-column layout with serif styling.
    /// </summary>
    public class Template1 : IScalableTemplate
    {
        public string TemplateName => "Classic";
        public string Description => "Traditional single-column layout with clean lines and classic formatting.";
        public float Scale { get; set; } = 1.0f;

        // Color palette
        private static readonly iText.Kernel.Colors.Color PrimaryColor = new DeviceRgb(15, 23, 42);       // Dark Navy (#0f172a)
        private static readonly iText.Kernel.Colors.Color AccentColor = new DeviceRgb(37, 99, 235);       // Steel Blue (#2563eb)
        private static readonly iText.Kernel.Colors.Color TextColor = new DeviceRgb(51, 65, 85);          // Charcoal (#334155)
        private static readonly iText.Kernel.Colors.Color LightGray = new DeviceRgb(203, 213, 225);       // Slate Divider (#cbd5e1)

        public void ApplyTemplate(Document document, Resume resume)
        {
            PdfFont titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            document.SetMargins(30f * Scale, 40f * Scale, 30f * Scale, 40f * Scale);

            // ─── HEADER ───
            var nameText = new Paragraph(resume.FullName.ToUpper())
                .SetFont(titleFont)
                .SetFontSize(22f * Scale)
                .SetFontColor(PrimaryColor)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(2f * Scale);
            document.Add(nameText);

            // Job Title
            if (!string.IsNullOrEmpty(resume.JobTitle))
            {
                var jobPara = new Paragraph(resume.JobTitle.ToUpper())
                    .SetFont(titleFont)
                    .SetFontSize(10f * Scale)
                    .SetFontColor(AccentColor)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(6f * Scale);
                document.Add(jobPara);
            }

            // Contact info line (compact single-row style)
            var contactParts = new List<string>();
            if (!string.IsNullOrEmpty(resume.Email)) contactParts.Add(resume.Email);
            if (!string.IsNullOrEmpty(resume.Phone)) contactParts.Add(resume.Phone);
            if (!string.IsNullOrEmpty(resume.Address)) contactParts.Add(resume.Address);

            var contactLine = new Paragraph(string.Join("   |   ", contactParts))
                .SetFont(normalFont)
                .SetFontSize(8.5f * Scale)
                .SetFontColor(TextColor)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginBottom(10f * Scale);
            document.Add(contactLine);

            // ─── SUMMARY ───
            if (!string.IsNullOrWhiteSpace(resume.Summary))
            {
                AddSectionTitle(document, "PROFESSIONAL SUMMARY", titleFont);
                document.Add(new Paragraph(resume.Summary)
                    .SetFont(normalFont)
                    .SetFontSize(9.5f * Scale)
                    .SetFontColor(TextColor)
                    .SetMarginBottom(10f * Scale)
                    .SetMultipliedLeading(1.3f));
            }

            // ─── EXPERIENCE ───
            if (resume.ExperienceList.Count > 0)
            {
                AddSectionTitle(document, "WORK EXPERIENCE", titleFont);
                foreach (var exp in resume.ExperienceList)
                {
                    var expTable = new Table(UnitValue.CreatePercentArray(new float[] { 70f, 30f }))
                        .UseAllAvailableWidth()
                        .SetBorder(Border.NO_BORDER)
                        .SetMarginBottom(2f * Scale);

                    var leftPara = new Paragraph()
                        .Add(new Text(exp.JobTitle).SetFont(titleFont).SetFontSize(10f * Scale).SetFontColor(PrimaryColor))
                        .Add(new Text("  |  ").SetFont(normalFont).SetFontSize(9.5f * Scale).SetFontColor(LightGray))
                        .Add(new Text(exp.Company).SetFont(titleFont).SetFontSize(9.5f * Scale).SetFontColor(AccentColor));

                    var rightPara = new Paragraph($"{exp.StartDate} - {exp.EndDate}")
                        .SetFont(italicFont)
                        .SetFontSize(8.5f * Scale)
                        .SetFontColor(TextColor)
                        .SetTextAlignment(TextAlignment.RIGHT);

                    expTable.AddCell(new Cell().Add(leftPara).SetBorder(Border.NO_BORDER).SetPadding(0));
                    expTable.AddCell(new Cell().Add(rightPara).SetBorder(Border.NO_BORDER).SetPadding(0));
                    document.Add(expTable);

                    if (!string.IsNullOrWhiteSpace(exp.Description))
                    {
                        document.Add(new Paragraph(exp.Description)
                            .SetFont(normalFont)
                            .SetFontSize(9f * Scale)
                            .SetFontColor(TextColor)
                            .SetMarginBottom(8f * Scale)
                            .SetMultipliedLeading(1.25f));
                    }
                }
            }

            // ─── EDUCATION ───
            if (resume.EducationList.Count > 0)
            {
                AddSectionTitle(document, "EDUCATION", titleFont);
                foreach (var edu in resume.EducationList)
                {
                    var eduTable = new Table(UnitValue.CreatePercentArray(new float[] { 70f, 30f }))
                        .UseAllAvailableWidth()
                        .SetBorder(Border.NO_BORDER)
                        .SetMarginBottom(2f * Scale);

                    var eduLeft = new Paragraph()
                        .Add(new Text(edu.Degree).SetFont(titleFont).SetFontSize(10f * Scale).SetFontColor(PrimaryColor))
                        .Add(new Text($" in {edu.FieldOfStudy}").SetFont(normalFont).SetFontSize(9.5f * Scale).SetFontColor(TextColor))
                        .Add(new Text("  |  ").SetFont(normalFont).SetFontSize(9.5f * Scale).SetFontColor(LightGray))
                        .Add(new Text(edu.Institution).SetFont(italicFont).SetFontSize(9.5f * Scale).SetFontColor(AccentColor));

                    var eduRight = new Paragraph($"{edu.StartDate} - {edu.EndDate}")
                        .SetFont(italicFont)
                        .SetFontSize(8.5f * Scale)
                        .SetFontColor(TextColor)
                        .SetTextAlignment(TextAlignment.RIGHT);

                    eduTable.AddCell(new Cell().Add(eduLeft).SetBorder(Border.NO_BORDER).SetPadding(0));
                    eduTable.AddCell(new Cell().Add(eduRight).SetBorder(Border.NO_BORDER).SetPadding(0));
                    document.Add(eduTable);

                    if (!string.IsNullOrWhiteSpace(edu.Grade))
                    {
                        document.Add(new Paragraph($"Grade: {edu.Grade}")
                            .SetFont(normalFont)
                            .SetFontSize(8.5f * Scale)
                            .SetFontColor(TextColor)
                            .SetMarginBottom(8f * Scale));
                    }
                }
            }

            // ─── SKILLS ───
            if (resume.SkillList.Count > 0)
            {
                AddSectionTitle(document, "SKILLS", titleFont);
                var skillsPara = new Paragraph()
                    .SetMarginBottom(10f * Scale);

                for (int i = 0; i < resume.SkillList.Count; i++)
                {
                    var skill = resume.SkillList[i];
                    skillsPara.Add(new Text(skill.Name).SetFont(titleFont).SetFontSize(9f * Scale).SetFontColor(PrimaryColor));
                    skillsPara.Add(new Text($" ({skill.Proficiency})").SetFont(normalFont).SetFontSize(8.5f * Scale).SetFontColor(TextColor));

                    if (i < resume.SkillList.Count - 1)
                    {
                        skillsPara.Add(new Text("   |   ").SetFont(normalFont).SetFontSize(9f * Scale).SetFontColor(LightGray));
                    }
                }
                document.Add(skillsPara);
            }
        }

        private void AddSectionTitle(Document doc, string title, PdfFont font)
        {
            doc.Add(new Paragraph(title)
                .SetFont(font)
                .SetFontSize(11f * Scale)
                .SetFontColor(PrimaryColor)
                .SetMarginBottom(2f * Scale)
                .SetMarginTop(12f * Scale));

            doc.Add(new LineSeparator(new SolidLine(1f * Scale))
                .SetFontColor(LightGray)
                .SetMarginBottom(6f * Scale)
                .SetMarginTop(0));
        }
    }
}
