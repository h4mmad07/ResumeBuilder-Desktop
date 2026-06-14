using System.IO;
using System.Windows.Forms;

namespace ResumeBuilder.Helpers
{
    public static class FileHelper
    {
        /// <summary>
        /// Opens a SaveFileDialog and returns the selected path.
        /// </summary>
        public static string? GetSavePath(string filter = "PDF Files|*.pdf", string defaultExt = "pdf")
        {
            using var dialog = new SaveFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = "Save File"
            };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

        /// <summary>
        /// Opens an OpenFileDialog and returns the selected path.
        /// </summary>
        public static string? GetOpenPath(string filter = "JSON Files|*.json", string defaultExt = "json")
        {
            using var dialog = new OpenFileDialog
            {
                Filter = filter,
                DefaultExt = defaultExt,
                Title = "Open File"
            };
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

        /// <summary>
        /// Ensures a directory exists, creating it if necessary.
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            string? dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
