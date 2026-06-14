using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using ResumeBuilder.Models;
using System;
using System.IO;

namespace ResumeBuilder.Templates
{
    /// <summary>
    /// Modern template – two-column layout with accent sidebar.
    /// </summary>
    public class Template2 : IScalableTemplate
    {
        public string TemplateName => "Modern";
        public string Description => "Modern two-column layout with dark slate sidebar and progress bars.";
        public float Scale { get; set; } = 1.0f;

        // Color palette
        private static readonly iText.Kernel.Colors.Color SidebarBg = new DeviceRgb(30, 41, 59);        // Slate-800 (#1e293b)
        private static readonly iText.Kernel.Colors.Color SidebarText = new DeviceRgb(248, 250, 252);    // Off-white (#f8fafc)
        private static readonly iText.Kernel.Colors.Color AccentColor = new DeviceRgb(13, 148, 136);      // Premium Teal (#0d9488)
        private static readonly iText.Kernel.Colors.Color ProgressTrack = new DeviceRgb(51, 65, 85);      // Slate-700 (#334155) for bar background
        private static readonly iText.Kernel.Colors.Color MainText = new DeviceRgb(15, 23, 42);          // Dark Slate (#0f172a)
        private static readonly iText.Kernel.Colors.Color SubText = new DeviceRgb(71, 85, 105);          // Muted Slate (#475569)
        private static readonly iText.Kernel.Colors.Color LightGray = new DeviceRgb(226, 232, 240);      // Light Slate divider (#e2e8f0)

        public void ApplyTemplate(Document document, Resume resume)
        {
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            document.SetMargins(0, 0, 0, 0);

            // Create a two-column table layout
            float[] columnWidths = { 195f, 400f };
            var mainTable = new Table(columnWidths)
                .UseAllAvailableWidth()
                .SetBorder(Border.NO_BORDER)
                .SetMinHeight(842f * Scale);

            // ═══════════ LEFT SIDEBAR ═══════════
            var sidebarCell = new Cell()
                .SetBackgroundColor(SidebarBg)
                .SetPadding(20f * Scale)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.TOP);

            // Profile Photo (cropped to circle with white border)
            if (!string.IsNullOrEmpty(resume.ProfileImagePath) && File.Exists(resume.ProfileImagePath))
            {
                try
                {
                    string croppedPath = ResumeBuilder.Helpers.ImageHelper.CropImageToCircle(resume.ProfileImagePath);
                    if (File.Exists(croppedPath))
                    {
                        var imageData = iText.IO.Image.ImageDataFactory.Create(croppedPath);
                        var photo = new iText.Layout.Element.Image(imageData)
                            .SetWidth(90f * Scale)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        var photoTable = new Table(1)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER)
                            .SetWidth(90f * Scale)
                            .SetMarginBottom(15f * Scale)
                            .SetMarginTop(10f * Scale);

                        var photoCell = new Cell()
                            .SetWidth(90f * Scale)
                            .SetHeight(90f * Scale)
                            .SetBorder(new SolidBorder(ColorConstants.WHITE, 2f * Scale))
                            .SetBorderRadius(new BorderRadius(45f * Scale))
                            .SetPadding(0)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                        photoCell.Add(photo);
                        photoTable.AddCell(photoCell);
                        sidebarCell.Add(photoTable);
                    }
                }
                catch
                {
                    // Fallback to original image if cropping fails
                    try
                    {
                        var imageData = iText.IO.Image.ImageDataFactory.Create(resume.ProfileImagePath);
                        var photo = new iText.Layout.Element.Image(imageData)
                            .SetWidth(90f * Scale)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        sidebarCell.Add(photo);
                    }
                    catch { }
                }
            }
            else
            {
                sidebarCell.Add(new Paragraph("").SetMarginTop(20f * Scale));
            }

            // Name in sidebar
            sidebarCell.Add(new Paragraph(resume.FullName)
                .SetFont(boldFont)
                .SetFontSize(18f * Scale)
                .SetFontColor(ColorConstants.WHITE)
                .SetMarginBottom(4f * Scale));

            // Job Title
            if (!string.IsNullOrEmpty(resume.JobTitle))
            {
                sidebarCell.Add(new Paragraph(resume.JobTitle.ToUpper())
                    .SetFont(boldFont)
                    .SetFontSize(9f * Scale)
                    .SetFontColor(AccentColor)
                    .SetMarginBottom(20f * Scale));
            }
            else
            {
                sidebarCell.Add(new Paragraph("").SetMarginBottom(15f * Scale));
            }

            // Contact section
            AddSidebarSection(sidebarCell, "CONTACT", boldFont);

            if (!string.IsNullOrEmpty(resume.Email))
            {
                AddContactRow(sidebarCell, "@", resume.Email, boldFont, normalFont);
            }
            if (!string.IsNullOrEmpty(resume.Phone))
            {
                AddContactRow(sidebarCell, "P", resume.Phone, boldFont, normalFont);
            }
            if (!string.IsNullOrEmpty(resume.Address))
            {
                AddContactRow(sidebarCell, "L", resume.Address, boldFont, normalFont);
            }

            sidebarCell.Add(new Paragraph("").SetMarginBottom(10f * Scale));

            // Skills in sidebar (graphical progress bars)
            if (resume.SkillList.Count > 0)
            {
                AddSidebarSection(sidebarCell, "SKILLS", boldFont);
                foreach (var skill in resume.SkillList)
                {
                    AddSkillProgressBar(sidebarCell, skill.Name, skill.Proficiency, normalFont, boldFont);
                }
            }

            sidebarCell.Add(new Paragraph("").SetMarginBottom(10f * Scale));

            // Education in sidebar
            if (resume.EducationList.Count > 0)
            {
                AddSidebarSection(sidebarCell, "EDUCATION", boldFont);
                foreach (var edu in resume.EducationList)
                {
                    sidebarCell.Add(new Paragraph(edu.Degree)
                        .SetFont(boldFont).SetFontSize(9.5f * Scale).SetFontColor(SidebarText).SetMarginBottom(1f * Scale));

                    sidebarCell.Add(new Paragraph(edu.FieldOfStudy)
                        .SetFont(normalFont).SetFontSize(8f * Scale).SetFontColor(AccentColor).SetMarginBottom(1f * Scale));

                    sidebarCell.Add(new Paragraph(edu.Institution)
                        .SetFont(italicFont).SetFontSize(8f * Scale).SetFontColor(SidebarText).SetMarginBottom(1f * Scale));

                    string dateRange = $"{edu.StartDate} - {edu.EndDate}";
                    if (!string.IsNullOrWhiteSpace(edu.Grade))
                        dateRange += $" | Grade: {edu.Grade}";

                    sidebarCell.Add(new Paragraph(dateRange)
                        .SetFont(italicFont).SetFontSize(7.5f * Scale).SetFontColor(SidebarText).SetMarginBottom(8f * Scale));
                }
            }

            mainTable.AddCell(sidebarCell);

            // ═══════════ RIGHT MAIN CONTENT ═══════════
            var mainCell = new Cell()
                .SetPadding(24f * Scale)
                .SetPaddingTop(20f * Scale)
                .SetBackgroundColor(ColorConstants.WHITE)
                .SetBorder(Border.NO_BORDER);

            // Summary
            if (!string.IsNullOrWhiteSpace(resume.Summary))
            {
                AddMainSection(mainCell, "ABOUT ME", boldFont);
                mainCell.Add(new Paragraph(resume.Summary)
                    .SetFont(normalFont)
                    .SetFontSize(10f * Scale)
                    .SetFontColor(MainText)
                    .SetMultipliedLeading(1.4f)
                    .SetMarginBottom(16f * Scale));
            }

            // Experience
            if (resume.ExperienceList.Count > 0)
            {
                AddMainSection(mainCell, "WORK EXPERIENCE", boldFont);
                foreach (var exp in resume.ExperienceList)
                {
                    var titleParagraph = new Paragraph()
                        .Add(new Text(exp.JobTitle).SetFont(boldFont).SetFontSize(11f * Scale).SetFontColor(MainText))
                        .Add(new Text("  |  ").SetFont(normalFont).SetFontSize(10f * Scale).SetFontColor(LightGray))
                        .Add(new Text(exp.Company).SetFont(boldFont).SetFontSize(10f * Scale).SetFontColor(AccentColor))
                        .SetMarginBottom(2f * Scale);
                    mainCell.Add(titleParagraph);

                    var dateRange = new Paragraph($"{exp.StartDate} - {exp.EndDate}")
                        .SetFont(italicFont)
                        .SetFontSize(8f * Scale)
                        .SetFontColor(SubText)
                        .SetMarginBottom(6f * Scale);
                    mainCell.Add(dateRange);

                    if (!string.IsNullOrWhiteSpace(exp.Description))
                    {
                        mainCell.Add(new Paragraph(exp.Description)
                            .SetFont(normalFont)
                            .SetFontSize(9f * Scale)
                            .SetFontColor(MainText)
                            .SetMultipliedLeading(1.3f)
                            .SetMarginBottom(12f * Scale));
                    }
                }
            }

            mainTable.AddCell(mainCell);
            document.Add(mainTable);
        }

        private void AddSidebarSection(Cell cell, string title, PdfFont font)
        {
            cell.Add(new Paragraph(title)
                .SetFont(font)
                .SetFontSize(11f * Scale)
                .SetFontColor(AccentColor)
                .SetMarginTop(12f * Scale)
                .SetMarginBottom(6f * Scale)
                .SetBorderBottom(new SolidBorder(AccentColor, 1f * Scale)));
        }

        private void AddMainSection(Cell cell, string title, PdfFont font)
        {
            cell.Add(new Paragraph(title)
                .SetFont(font)
                .SetFontSize(13f * Scale)
                .SetFontColor(MainText)
                .SetMarginTop(14f * Scale)
                .SetMarginBottom(8f * Scale)
                .SetBorderBottom(new SolidBorder(AccentColor, 1.5f * Scale)));
        }

        private void AddContactRow(Cell cell, string icon, string text, PdfFont boldFont, PdfFont normalFont)
        {
            var rowTable = new Table(new float[] { 15f, 150f })
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(4f * Scale)
                .UseAllAvailableWidth();

            var iconCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetBackgroundColor(AccentColor)
                .SetWidth(12f * Scale)
                .SetHeight(12f * Scale)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            iconCell.Add(new Paragraph(icon)
                .SetFont(boldFont)
                .SetFontSize(8f * Scale)
                .SetFontColor(ColorConstants.WHITE)
                .SetMargin(0));

            var textCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPaddingLeft(6f * Scale)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            textCell.Add(new Paragraph(text)
                .SetFont(normalFont)
                .SetFontSize(8f * Scale)
                .SetFontColor(SidebarText)
                .SetMargin(0));

            rowTable.AddCell(iconCell);
            rowTable.AddCell(textCell);
            cell.Add(rowTable);
        }

        private void AddSkillProgressBar(Cell cell, string skillName, ProficiencyLevel level, PdfFont normalFont, PdfFont boldFont)
        {
            cell.Add(new Paragraph(skillName)
                .SetFont(boldFont)
                .SetFontSize(8.5f * Scale)
                .SetFontColor(SidebarText)
                .SetMarginTop(4f * Scale)
                .SetMarginBottom(2f * Scale));

            float percentage = level switch
            {
                ProficiencyLevel.Beginner => 30f,
                ProficiencyLevel.Intermediate => 60f,
                ProficiencyLevel.Advanced => 80f,
                ProficiencyLevel.Expert => 100f,
                _ => 30f
            };

            // Progress bar track container table
            var barTable = new Table(UnitValue.CreatePercentArray(new float[] { percentage, 100f - percentage }))
                .UseAllAvailableWidth()
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(8f * Scale);

            var filledCell = new Cell()
                .SetBackgroundColor(AccentColor)
                .SetHeight(4f * Scale)
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0);

            var emptyCell = new Cell()
                .SetBackgroundColor(ProgressTrack)
                .SetHeight(4f * Scale)
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0);

            barTable.AddCell(filledCell);
            barTable.AddCell(emptyCell);
            cell.Add(barTable);
        }
    }
}
