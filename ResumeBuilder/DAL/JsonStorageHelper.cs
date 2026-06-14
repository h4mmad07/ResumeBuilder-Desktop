using System;
using System.IO;
using System.Text.Json;
using ResumeBuilder.Models;

namespace ResumeBuilder.DAL
{
    public static class JsonStorageHelper
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Save a Resume object to a JSON file.
        /// </summary>
        public static void SaveResume(Resume resume, string filePath)
        {
            resume.ModifiedDate = DateTime.Now;
            string json = JsonSerializer.Serialize(resume, _options);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Load a Resume object from a JSON file.
        /// </summary>
        public static Resume? LoadResume(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Resume>(json, _options);
        }
    }
}
