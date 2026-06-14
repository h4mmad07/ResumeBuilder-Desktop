using System.Collections.Generic;
using System.Text.RegularExpressions;
using ResumeBuilder.Models;

namespace ResumeBuilder.Helpers
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Checks if a string is not null or whitespace.
        /// </summary>
        public static bool IsNotEmpty(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Validates an email address format.
        /// </summary>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Validates an entire Resume object and returns a list of error messages.
        /// </summary>
        public static List<string> ValidateResume(Resume resume)
        {
            var errors = new List<string>();

            if (!IsNotEmpty(resume.FullName))
                errors.Add("Full Name is required.");

            if (!IsValidEmail(resume.Email))
                errors.Add("A valid Email address is required.");

            if (!IsNotEmpty(resume.Phone))
                errors.Add("Phone number is required.");

            if (!IsNotEmpty(resume.Summary))
                errors.Add("Professional Summary is required.");

            if (resume.EducationList.Count == 0)
                errors.Add("At least one Education entry is required.");

            if (resume.SkillList.Count == 0)
                errors.Add("At least one Skill is required.");

            return errors;
        }
    }
}
