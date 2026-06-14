namespace ResumeBuilder.Models
{
    public enum ProficiencyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }

    public class Skill
    {
        public string Name { get; set; } = string.Empty;
        public ProficiencyLevel Proficiency { get; set; } = ProficiencyLevel.Beginner;
    }
}
