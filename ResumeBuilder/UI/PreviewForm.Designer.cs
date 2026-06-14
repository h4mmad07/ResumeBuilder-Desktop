namespace ResumeBuilder.UI
{
    partial class PreviewForm
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

            this.Text = "Resume Preview";
            this.Size = new System.Drawing.Size(850, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(24, 28, 36);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);

            // ─── Top Bar ───
            pnlTopBar = new Panel();
            pnlTopBar.Dock = DockStyle.Top;
            pnlTopBar.Height = 60;
            pnlTopBar.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);
            pnlTopBar.Padding = new Padding(10);

            lblPreviewTitle = new Label();
            lblPreviewTitle.Text = "👁️ Resume Preview";
            lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            lblPreviewTitle.ForeColor = System.Drawing.Color.FromArgb(82, 183, 136);
            lblPreviewTitle.AutoSize = true;
            lblPreviewTitle.Location = new System.Drawing.Point(15, 15);
            pnlTopBar.Controls.Add(lblPreviewTitle);

            // ─── WebBrowser for PDF ───
            webBrowser = new WebBrowser();
            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true;

            // ─── Bottom Panel ───
            pnlBottom = new Panel();
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Height = 70;
            pnlBottom.BackColor = System.Drawing.Color.FromArgb(30, 35, 45);
            pnlBottom.Padding = new Padding(10);

            btnSaveAs = new Button();
            btnSaveAs.Text = "💾 Save As PDF";
            btnSaveAs.Size = new System.Drawing.Size(180, 45);
            btnSaveAs.Location = new System.Drawing.Point(15, 12);
            StyleButton(btnSaveAs, System.Drawing.Color.FromArgb(82, 183, 136));

            btnOpenExternal = new Button();
            btnOpenExternal.Text = "🔗 Open in Viewer";
            btnOpenExternal.Size = new System.Drawing.Size(180, 45);
            btnOpenExternal.Location = new System.Drawing.Point(210, 12);
            StyleButton(btnOpenExternal, System.Drawing.Color.FromArgb(86, 156, 214));

            btnClose = new Button();
            btnClose.Text = "✕ Close";
            btnClose.Size = new System.Drawing.Size(130, 45);
            btnClose.Location = new System.Drawing.Point(690, 12);
            StyleButton(btnClose, System.Drawing.Color.FromArgb(190, 85, 85));

            pnlBottom.Controls.Add(btnSaveAs);
            pnlBottom.Controls.Add(btnOpenExternal);
            pnlBottom.Controls.Add(btnClose);

            // ─── Add to Form ───
            this.Controls.Add(webBrowser);
            this.Controls.Add(pnlTopBar);
            this.Controls.Add(pnlBottom);

            this.ResumeLayout(false);
        }

        private void StyleButton(Button btn, System.Drawing.Color color)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.BorderColor = color;
            btn.BackColor = System.Drawing.Color.FromArgb(36, 42, 54);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }

        private Panel pnlTopBar;
        private Panel pnlBottom;
        private Label lblPreviewTitle;
        private WebBrowser webBrowser;
        private Button btnSaveAs;
        private Button btnOpenExternal;
        private Button btnClose;
    }
}
