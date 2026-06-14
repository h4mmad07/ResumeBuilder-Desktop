using System.Collections.Generic;
using ResumeBuilder.Helpers;
using ResumeBuilder.Models;

namespace ResumeBuilder.BLL
{
    /// <summary>
    /// Business logic for managing Resume objects.
    /// </summary>
    public class ResumeManager
    {
        /// <summary>
        /// Build a Resume object from individual field values.
        /// </summary>
        public Resume CreateResume(
            string fullName, string jobTitle, string email, string phone,
            string address, string summary, string languages, string profileImagePath,
            List<Education> educations, List<Experience> experiences, List<Skill> skills)
        {
            return new Resume
            {
                FullName = fullName,
                JobTitle = jobTitle,
                Email = email,
                Phone = phone,
                Address = address,
                Summary = summary,
                Languages = languages,
                ProfileImagePath = profileImagePath,
                EducationList = educations ?? new List<Education>(),
                ExperienceList = experiences ?? new List<Experience>(),
                SkillList = skills ?? new List<Skill>()
            };
        }

        /// <summary>
        /// Validate a resume and return error messages.
        /// </summary>
        public List<string> ValidateResume(Resume resume)
        {
            return ValidationHelper.ValidateResume(resume);
        }
    }
}
