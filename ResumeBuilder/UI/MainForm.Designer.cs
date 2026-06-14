namespace ResumeBuilder.UI
{
    partial class MainForm
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
            this.SuspendLayout();

            // ─── Form Settings ───
            this.Text = "Resume Builder";
            this.Size = new System.Drawing.Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(24, 28, 36);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new System.Drawing.Font("Segoe UI", 10F);

            // ─── Header Panel ───
            pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 180;
            pnlHeader.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);

            lblTitle = new Label();
            lblTitle.Text = "📄 Resume Builder";
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(82, 183, 136);
            lblTitle.AutoSize = true;
            lblTitle.Location = new System.Drawing.Point(270, 40);

            lblSubtitle = new Label();
            lblSubtitle.Text = "Create professional resumes in minutes";
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(160, 170, 185);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new System.Drawing.Point(290, 100);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSubtitle);

            // ─── Button Panel ───
            pnlButtons = new Panel();
            pnlButtons.Size = new System.Drawing.Size(700, 200);
            pnlButtons.Location = new System.Drawing.Point(100, 240);
            pnlButtons.BackColor = System.Drawing.Color.Transparent;

            // Create New Button
            btnCreateNew = new Button();
            btnCreateNew.Text = "✨  Create New Resume";
            btnCreateNew.Size = new System.Drawing.Size(200, 120);
            btnCreateNew.Location = new System.Drawing.Point(20, 20);
            StyleButton(btnCreateNew, System.Drawing.Color.FromArgb(82, 183, 136));

            // Load Existing Button
            btnLoadExisting = new Button();
            btnLoadExisting.Text = "📂  Load Existing";
            btnLoadExisting.Size = new System.Drawing.Size(200, 120);
            btnLoadExisting.Location = new System.Drawing.Point(250, 20);
            StyleButton(btnLoadExisting, System.Drawing.Color.FromArgb(86, 156, 214));

            // Exit Button
            btnExit = new Button();
            btnExit.Text = "🚪  Exit";
            btnExit.Size = new System.Drawing.Size(200, 120);
            btnExit.Location = new System.Drawing.Point(480, 20);
            StyleButton(btnExit, System.Drawing.Color.FromArgb(190, 85, 85));

            pnlButtons.Controls.Add(btnCreateNew);
            pnlButtons.Controls.Add(btnLoadExisting);
            pnlButtons.Controls.Add(btnExit);

            // ─── Footer ───
            lblFooter = new Label();
            lblFooter.Text = "© 2026 Resume Builder  |  Built with ❤️";
            lblFooter.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblFooter.ForeColor = System.Drawing.Color.FromArgb(100, 110, 125);
            lblFooter.AutoSize = true;
            lblFooter.Location = new System.Drawing.Point(330, 530);

            // ─── Add to Form ───
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlButtons);
            this.Controls.Add(lblFooter);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void StyleButton(Button btn, System.Drawing.Color accentColor)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = accentColor;
            btn.BackColor = System.Drawing.Color.FromArgb(36, 42, 54);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(accentColor.R / 3, accentColor.G / 3, accentColor.B / 3);
        }

        private Panel pnlHeader;
        private Panel pnlButtons;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblFooter;
        private Button btnCreateNew;
        private Button btnLoadExisting;
        private Button btnExit;
    }
}
