using iText.IO.Font.Constants;
using iText.IO.Image;
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
    /// Creative template - matches the two-column portfolio style with
    /// teal sidebar on the left, white body on the right, and peach header.
    /// </summary>
    public class Template3 : IScalableTemplate
    {
        public string TemplateName => "Creative";
        public string Description => "Vibrant peach-and-teal portfolio layout with circular profile crop.";
        public float Scale { get; set; } = 1.0f;

        // Colors matching the reference screenshot
        private static readonly iText.Kernel.Colors.Color TealSidebarBg = new DeviceRgb(106, 149, 150);
        private static readonly iText.Kernel.Colors.Color PeachBg = new DeviceRgb(232, 196, 184);
        private static readonly iText.Kernel.Colors.Color TealAccent = new DeviceRgb(61, 139, 122);
        private static readonly iText.Kernel.Colors.Color DarkText = new DeviceRgb(51, 51, 51);
        private static readonly iText.Kernel.Colors.Color WhiteColor = new DeviceRgb(255, 255, 255);
        private static readonly iText.Kernel.Colors.Color GrayText = new DeviceRgb(130, 130, 130);

        public void ApplyTemplate(Document document, Resume resume)
        {
            float s = Scale;
            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);

            document.SetMargins(0, 0, 0, 0);

            // Two-column table layout - fully scaled to A4 dimensions (595x842 points)
            float[] columnWidths = { 208f * s, 387f * s };
            var mainTable = new Table(columnWidths)
                .UseAllAvailableWidth()
                .SetBorder(Border.NO_BORDER)
                .SetMinHeight(842f * s); // Stretch table to full page height

            // ---------------------------------------
            // LEFT SIDEBAR (Teal background, white text)
            // ---------------------------------------
            var sidebarCell = new Cell()
                .SetBackgroundColor(TealSidebarBg)
                .SetPadding(20f * s)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.TOP);

            // -- Profile Photo (cropped to circle with white border) --
            if (!string.IsNullOrEmpty(resume.ProfileImagePath) && File.Exists(resume.ProfileImagePath))
            {
                try
                {
                    string croppedPath = CropImageToCircle(resume.ProfileImagePath);
                    if (File.Exists(croppedPath))
                    {
                        var imageData = ImageDataFactory.Create(croppedPath);
                        var photo = new iText.Layout.Element.Image(imageData)
                            .SetWidth(120f * s)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        // Table wrapper to center and apply white border
                        var photoTable = new Table(1)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER)
                            .SetBorder(Border.NO_BORDER)
                            .SetWidth(120f * s)
                            .SetMarginBottom(20f * s)
                            .SetMarginTop(15f * s);

                        var photoCell = new Cell()
                            .SetWidth(120f * s)
                            .SetHeight(120f * s)
                            .SetBorder(new SolidBorder(WhiteColor, 4f * s))
                            .SetBorderRadius(new BorderRadius(60f * s))
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
                        var imageData = ImageDataFactory.Create(resume.ProfileImagePath);
                        var photo = new iText.Layout.Element.Image(imageData)
                            .SetWidth(120f * s)
                            .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        sidebarCell.Add(photo);
                    }
                    catch { }
                }
            }
            else
            {
                // Space offset at top if no image is provided
                sidebarCell.Add(new Paragraph("").SetMarginTop(40f * s));
            }

            // -- PROFILE Section --
            sidebarCell.Add(new Paragraph("PROFILE")
                .SetFont(boldFont)
                .SetFontSize(13f * s)
                .SetFontColor(WhiteColor)
                .SetMarginBottom(8f * s)
                .SetMarginTop(10f * s));

            if (!string.IsNullOrWhiteSpace(resume.Summary))
            {
                sidebarCell.Add(new Paragraph(resume.Summary)
                    .SetFont(normalFont)
                    .SetFontSize(9f * s)
                    .SetFontColor(WhiteColor)
                    .SetMultipliedLeading(1.3f)
                    .SetMarginBottom(20f * s));
            }

            // -- CONTACT ME Section --
            sidebarCell.Add(new Paragraph("CONTACT ME")
                .SetFont(boldFont)
                .SetFontSize(13f * s)
                .SetFontColor(WhiteColor)
                .SetMarginBottom(8f * s)
                .SetMarginTop(10f * s));

            if (!string.IsNullOrEmpty(resume.Phone))
                AddContactRow(sidebarCell, "P", resume.Phone, boldFont, normalFont);

            if (!string.IsNullOrEmpty(resume.Email))
                AddContactRow(sidebarCell, "@", resume.Email, boldFont, normalFont);

            if (!string.IsNullOrEmpty(resume.Address))
                AddContactRow(sidebarCell, "L", resume.Address, boldFont, normalFont);

            mainTable.AddCell(sidebarCell);

            // ---------------------------------------
            // RIGHT MAIN CONTENT (White background + Peach header)
            // ---------------------------------------
            var mainCell = new Cell()
                .SetBackgroundColor(WhiteColor)
                .SetPadding(0) // 0 padding to let peach banner span to boundaries
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment(VerticalAlignment.TOP);

            // -- Peach Header Banner (Name & Title) --
            var peachBannerTable = new Table(1)
                .SetBorder(Border.NO_BORDER)
                .UseAllAvailableWidth();

            var peachBannerCell = new Cell()
                .SetBackgroundColor(PeachBg)
                .SetBorder(Border.NO_BORDER)
                .SetHeight(120f * s)
                .SetPaddingLeft(25f * s)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            peachBannerCell.Add(new Paragraph(resume.FullName.ToUpper())
                .SetFont(boldFont)
                .SetFontSize(28f * s)
                .SetFontColor(DarkText)
                .SetMargin(0)
                .SetMarginBottom(4f * s));

            if (!string.IsNullOrEmpty(resume.JobTitle))
            {
                peachBannerCell.Add(new Paragraph(resume.JobTitle)
                    .SetFont(normalFont)
                    .SetFontSize(12f * s)
                    .SetFontColor(DarkText)
                    .SetMargin(0));
            }
            else
            {
                peachBannerCell.Add(new Paragraph("Student")
                    .SetFont(normalFont)
                    .SetFontSize(12f * s)
                    .SetFontColor(DarkText)
                    .SetMargin(0));
            }

            peachBannerTable.AddCell(peachBannerCell);
            mainCell.Add(peachBannerTable);

            // -- Content Container (Margins and Padding) --
            var contentCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPaddingLeft(25f * s)
                .SetPaddingRight(25f * s)
                .SetPaddingTop(20f * s)
                .SetPaddingBottom(20f * s)
                .SetVerticalAlignment(VerticalAlignment.TOP);

            // -- EDUCATION --
            if (resume.EducationList != null && resume.EducationList.Count > 0)
            {
                AddSectionHeader(contentCell, "EDUCATION", boldFont);
                foreach (var edu in resume.EducationList)
                {
                    contentCell.Add(new Paragraph(edu.Institution.ToUpper())
                        .SetFont(boldFont)
                        .SetFontSize(10f * s)
                        .SetFontColor(DarkText)
                        .SetMarginBottom(1f * s)
                        .SetMarginTop(2f * s));

                    contentCell.Add(new Paragraph(edu.Degree + " " + edu.FieldOfStudy)
                        .SetFont(normalFont)
                        .SetFontSize(9f * s)
                        .SetFontColor(DarkText)
                        .SetMarginBottom(1f * s));

                    contentCell.Add(new Paragraph(edu.StartDate + " - " + edu.EndDate)
                        .SetFont(normalFont)
                        .SetFontSize(8.5f * s)
                        .SetFontColor(GrayText)
                        .SetMarginBottom(8f * s));
                }
            }

            // -- LANGUAGE --
            if (!string.IsNullOrEmpty(resume.Languages))
            {
                AddSectionHeader(contentCell, "LANGUAGE", boldFont);
                var langs = resume.Languages.Split(',');
                foreach (var lang in langs)
                {
                    var trimmed = lang.Trim();
                    if (!string.IsNullOrEmpty(trimmed))
                    {
                        contentCell.Add(new Paragraph(trimmed)
                            .SetFont(normalFont)
                            .SetFontSize(9f * s)
                            .SetFontColor(DarkText)
                            .SetMarginBottom(3f * s));
                    }
                }
            }

            // -- TECHNICAL SKILLS --
            if (resume.SkillList != null && resume.SkillList.Count > 0)
            {
                AddSectionHeader(contentCell, "TECHNICAL SKILLS", boldFont);
                foreach (var skill in resume.SkillList)
                {
                    var skillPara = new Paragraph()
                        .SetFontColor(DarkText)
                        .SetFontSize(9f * s)
                        .SetMultipliedLeading(1.2f);

                    string skillName = skill.Name.Trim();
                    int colonIndex = skillName.IndexOf(':');

                    if (colonIndex > 0)
                    {
                        // Split title and details for bold formatting
                        string category = skillName.Substring(0, colonIndex).Trim();
                        string details = skillName.Substring(colonIndex).Trim();

                        skillPara.Add(new Text(category).SetFont(boldFont));
                        skillPara.Add(new Text(details).SetFont(normalFont));
                    }
                    else
                    {
                        // Fallback: standard skill display
                        skillPara.Add(new Text(skillName).SetFont(boldFont));
                        skillPara.Add(new Text(" (" + skill.Proficiency.ToString() + ")").SetFont(normalFont).SetFontSize(8f * s));
                    }

                    AddBulletPoint(contentCell, skillPara, TealAccent);
                }
            }

            // -- EXPERIENCE --
            if (resume.ExperienceList != null && resume.ExperienceList.Count > 0)
            {
                AddSectionHeader(contentCell, "EXPERIENCE", boldFont);
                foreach (var exp in resume.ExperienceList)
                {
                    var expPara = new Paragraph()
                        .SetFontColor(DarkText)
                        .SetFontSize(9f * s)
                        .SetMultipliedLeading(1.2f);

                    if (string.IsNullOrEmpty(exp.Company) && string.IsNullOrEmpty(exp.StartDate))
                    {
                        // Single-line bullet point style (e.g. Worked MS Office: description)
                        string jobTitle = exp.JobTitle.Trim();
                        string desc = exp.Description.Trim();

                        expPara.Add(new Text(jobTitle).SetFont(boldFont));
                        if (!string.IsNullOrEmpty(desc))
                        {
                            if (!desc.StartsWith(":") && !jobTitle.EndsWith(":"))
                                expPara.Add(new Text(": ").SetFont(normalFont));
                            expPara.Add(new Text(desc).SetFont(normalFont));
                        }
                        AddBulletPoint(contentCell, expPara, TealAccent);
                    }
                    else
                    {
                        // Standard multi-line job experience with a bullet
                        var stdExpPara = new Paragraph()
                            .SetFontColor(DarkText)
                            .SetFontSize(9f * s)
                            .SetMultipliedLeading(1.2f);

                        string headerText = exp.JobTitle;
                        if (!string.IsNullOrEmpty(exp.Company))
                            headerText += " at " + exp.Company;

                        stdExpPara.Add(new Text(headerText).SetFont(boldFont).SetFontSize(9.5f * s));

                        if (!string.IsNullOrEmpty(exp.StartDate) || !string.IsNullOrEmpty(exp.EndDate))
                        {
                            string dateRange = "\n" + exp.StartDate + " - " + exp.EndDate;
                            stdExpPara.Add(new Text(dateRange).SetFont(italicFont).SetFontSize(8f * s).SetFontColor(GrayText));
                        }

                        if (!string.IsNullOrEmpty(exp.Description))
                        {
                            stdExpPara.Add(new Text("\n" + exp.Description).SetFont(normalFont).SetFontSize(9f * s));
                        }

                        AddBulletPoint(contentCell, stdExpPara, TealAccent);
                    }
                }
            }

            var contentTable = new Table(1)
                .SetBorder(Border.NO_BORDER)
                .UseAllAvailableWidth();
            contentTable.AddCell(contentCell);
            mainCell.Add(contentTable);

            mainTable.AddCell(mainCell);
            document.Add(mainTable);
        }

        /// <summary>
        /// Crops a source image into a perfect circle using GDI+ and outputs a temporary file path.
        /// </summary>
        private string CropImageToCircle(string imagePath)
        {
            try
            {
                using (var srcImage = System.Drawing.Image.FromFile(imagePath))
                {
                    int minSize = Math.Min(srcImage.Width, srcImage.Height);
                    var dstImage = new System.Drawing.Bitmap(minSize, minSize);
                    using (var g = System.Drawing.Graphics.FromImage(dstImage))
                    {
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        using (var brush = new System.Drawing.TextureBrush(srcImage))
                        {
                            // Center texture
                            brush.TranslateTransform((minSize - srcImage.Width) / 2f, (minSize - srcImage.Height) / 2f);
                            var path = new System.Drawing.Drawing2D.GraphicsPath();
                            path.AddEllipse(0, 0, minSize, minSize);
                            g.FillPath(brush, path);
                        }
                    }
                    string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(imagePath) + "_circle.png");
                    dstImage.Save(tempPath, System.Drawing.Imaging.ImageFormat.Png);
                    return tempPath;
                }
            }
            catch
            {
                return imagePath; // Fallback to original image if failed
            }
        }

        /// <summary>
        /// Contact row with a white circular/square badge with teal abbreviation letter and white text.
        /// </summary>
        private void AddContactRow(Cell cell, string icon, string text, PdfFont boldFont, PdfFont normalFont)
        {
            float s = Scale;
            var rowTable = new Table(new float[] { 18f * s, 150f * s })
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(8f * s)
                .UseAllAvailableWidth();

            var iconCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetBackgroundColor(WhiteColor)
                .SetPadding(2 * s)
                .SetWidth(15f * s)
                .SetHeight(15f * s)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            iconCell.Add(new Paragraph(icon)
                .SetFont(boldFont)
                .SetFontSize(8f * s)
                .SetFontColor(TealSidebarBg)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMargin(0));

            var textCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPaddingLeft(8f * s)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            textCell.Add(new Paragraph(text)
                .SetFont(normalFont)
                .SetFontSize(8.5f * s)
                .SetFontColor(WhiteColor)
                .SetMultipliedLeading(1.2f)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetMargin(0));

            rowTable.AddCell(iconCell);
            rowTable.AddCell(textCell);
            cell.Add(rowTable);
        }

        /// <summary>
        /// Section header with teal chevron icon and title text.
        /// </summary>
        private void AddSectionHeader(Cell cell, string title, PdfFont font)
        {
            float s = Scale;
            var headerTable = new Table(new float[] { 12f * s, 330f * s })
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(6f * s)
                .SetMarginTop(12f * s)
                .UseAllAvailableWidth();

            var chevronCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            chevronCell.Add(new Paragraph(">")
                .SetFont(font)
                .SetFontSize(13f * s)
                .SetFontColor(TealAccent)
                .SetMargin(0));

            var titleCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPaddingLeft(6f * s)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            titleCell.Add(new Paragraph(title)
                .SetFont(font)
                .SetFontSize(12f * s)
                .SetFontColor(TealAccent)
                .SetMargin(0));

            headerTable.AddCell(chevronCell);
            headerTable.AddCell(titleCell);
            cell.Add(headerTable);
        }

        /// <summary>
        /// Bullet point row using a tiny square teal box cell as bullet for font compatibility.
        /// </summary>
        private void AddBulletPoint(Cell cell, Paragraph textParagraph, iText.Kernel.Colors.Color bulletColor)
        {
            float s = Scale;
            var bulletTable = new Table(new float[] { 12f * s, 330f * s })
                .SetBorder(Border.NO_BORDER)
                .SetMarginBottom(5f * s)
                .UseAllAvailableWidth();

            var bulletContainerCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0)
                .SetPaddingTop(4f * s) // Align bullet vertically with the first line of text
                .SetVerticalAlignment(VerticalAlignment.TOP)
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            // Tiny square bullet
            var square = new Table(1)
                .SetWidth(4f * s)
                .SetHeight(4f * s)
                .SetBorder(Border.NO_BORDER)
                .SetMargin(0)
                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

            var squareCell = new Cell()
                .SetBackgroundColor(bulletColor)
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0);

            square.AddCell(squareCell);
            bulletContainerCell.Add(square);

            var textCell = new Cell()
                .SetBorder(Border.NO_BORDER)
                .SetPadding(0)
                .SetPaddingLeft(6f * s)
                .SetVerticalAlignment(VerticalAlignment.TOP);

            textCell.Add(textParagraph.SetMargin(0));

            bulletTable.AddCell(bulletContainerCell);
            bulletTable.AddCell(textCell);
            cell.Add(bulletTable);
        }
    }
}
