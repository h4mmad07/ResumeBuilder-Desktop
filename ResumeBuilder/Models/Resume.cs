using System;
using System.Collections.Generic;

namespace ResumeBuilder.Models
{
    public class Resume
    {
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public string ProfileImagePath { get; set; } = string.Empty;

        public List<Education> EducationList { get; set; } = new List<Education>();
        public List<Experience> ExperienceList { get; set; } = new List<Experience>();
        public List<Skill> SkillList { get; set; } = new List<Skill>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
