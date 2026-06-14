namespace ResumeBuilder.UI
{
    partial class TemplateSelectionForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            btnPreview = new Button();
            btnGenerate = new Button();
            pnlTemplate1 = CreateTemplateCard(50, 100, "Modern", "Modern two-column layout with dark slate sidebar & progress bars.", Color.FromArgb(82, 183, 136));
            pnlTemplate2 = CreateTemplateCard(350, 100, "Classic", "Traditional single-column layout with clean lines & classic structure.", Color.FromArgb(255, 193, 7));
            pnlTemplate3 = CreateTemplateCard(650, 100, "Elegant", "Elegant single-column layout with navy & steel-blue accents.", Color.FromArgb(52, 152, 219));
            pnlTemplate4 = CreateTemplateCard(50, 370, "Minimal", "Sleek two-column layout with peach banner & teal sidebar.", Color.FromArgb(231, 76, 60));
            pnlTemplate5 = CreateTemplateCard(350, 370, "Professional", "Professional business two-column layout with slate-blue sidebar.", Color.FromArgb(155, 89, 182));
            pnlTemplate6 = CreateTemplateCard(650, 370, "Creative", "Vibrant peach-and-teal portfolio layout with circular profile crop.", Color.FromArgb(46, 204, 113));
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(82, 183, 136);
            lblTitle.Location = new Point(230, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(599, 54);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Choose Your Resume Template";
            // 
            // btnPreview
            // 
            btnPreview.Location = new Point(230, 700);
            btnPreview.Name = "btnPreview";
            btnPreview.Size = new Size(200, 50);
            btnPreview.TabIndex = 1;
            btnPreview.Text = "Preview Resume";
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(500, 700);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(200, 50);
            btnGenerate.TabIndex = 2;
            btnGenerate.Text = "Generate PDF";
            // 
            // TemplateSelectionForm
            // 
            BackColor = Color.FromArgb(24, 28, 36);
            ClientSize = new Size(938, 744);
            Controls.Add(lblTitle);
            Controls.Add(btnPreview);
            Controls.Add(btnGenerate);
            Controls.Add(pnlTemplate1);
            Controls.Add(pnlTemplate2);
            Controls.Add(pnlTemplate3);
            Controls.Add(pnlTemplate4);
            Controls.Add(pnlTemplate5);
            Controls.Add(pnlTemplate6);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "TemplateSelectionForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select a Template";
            Load += TemplateSelectionForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel CreateTemplateCard(int x, int y, string title, string desc, System.Drawing.Color accentColor)
        {
            var panel = new Panel();
            panel.Location = new System.Drawing.Point(x, y);
            panel.Size = new System.Drawing.Size(280, 240);
            panel.BackColor = System.Drawing.Color.FromArgb(36, 42, 54);
            panel.BorderStyle = BorderStyle.FixedSingle; // Always show a border, color will be set dynamically
            panel.Padding = new Padding(2);

            // Accent bar at top
            var accentBar = new Panel();
            accentBar.Dock = DockStyle.Top;
            accentBar.Height = 5;
            accentBar.BackColor = accentColor;
            panel.Controls.Add(accentBar);

            var lblName = new Label();
            lblName.Text = title;
            lblName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblName.ForeColor = accentColor;
            lblName.Location = new System.Drawing.Point(20, 25);
            lblName.AutoSize = true;
            panel.Controls.Add(lblName);

            var lblDesc = new Label();
            lblDesc.Text = desc;
            lblDesc.Font = new System.Drawing.Font("Segoe UI", 10F);
            lblDesc.ForeColor = System.Drawing.Color.FromArgb(180, 190, 205);
            lblDesc.Location = new System.Drawing.Point(20, 80);
            lblDesc.Size = new System.Drawing.Size(240, 140);
            panel.Controls.Add(lblDesc);

            return panel;
        }

        private void StyleButton(Button btn, System.Drawing.Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = color;
            btn.BackColor = System.Drawing.Color.FromArgb(36, 42, 54);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private Label lblTitle;
        private Panel pnlTemplate1;
        private Panel pnlTemplate2;
        private Panel pnlTemplate3;
        private Panel pnlTemplate4;
        private Panel pnlTemplate5;
        private Panel pnlTemplate6;
        private Button btnPreview;
        private Button btnGenerate;
    }
}
