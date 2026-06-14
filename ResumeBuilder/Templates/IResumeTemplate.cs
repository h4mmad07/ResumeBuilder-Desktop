using iText.Layout;
using ResumeBuilder.Models;

namespace ResumeBuilder.Templates
{
    /// <summary>
    /// Interface that all resume templates must implement.
    /// </summary>
    public interface IResumeTemplate
    {
        string TemplateName { get; }
        string Description { get; }

        /// <summary>
        /// Apply the template layout to the PDF document using resume data.
        /// </summary>
        void ApplyTemplate(Document document, Resume resume);
    }

    /// <summary>
    /// Interface for templates that support dynamic layout scaling.
    /// </summary>
    public interface IScalableTemplate : IResumeTemplate
    {
        float Scale { get; set; }
    }
}
