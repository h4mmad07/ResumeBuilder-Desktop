using System.Collections.Generic;
using ResumeBuilder.Templates;

namespace ResumeBuilder.BLL
{
    /// <summary>
    /// Manages available resume templates.
    /// </summary>
    public class TemplateManager
    {
        private readonly List<IResumeTemplate> _templates;

        public TemplateManager()
        {
            _templates = new List<IResumeTemplate>
            {
                new Template1(),
                new Template2(),
                new Template3(),
                new Template4(),
                new Template5(),
                new Template6()
            };
        }

        /// <summary>
        /// Get all available templates.
        /// </summary>
        public List<IResumeTemplate> GetAvailableTemplates()
        {
            return _templates;
        }

        /// <summary>
        /// Get a template by name.
        /// </summary>
        public IResumeTemplate? GetTemplate(string name)
        {
            return _templates.Find(t => t.TemplateName.Equals(name, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}
