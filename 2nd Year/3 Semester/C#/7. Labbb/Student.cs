using System;
using System.Collections.Generic;
using System.Linq;

namespace _7.Labbb
{
    [Serializable]
    public class Student
    {
        public string FullName { get; set; }
        public int Course { get; set; }
        public int Group { get; set; }
        public List<List<int>> Grades { get; set; } = new List<List<int>>(); // Default to avoid null
        public string Form { get; set; } // Consider using enum for Budget/Contract

        public Student() { }

        public Student(string fullName, int course, int group, List<List<int>> grades, string form)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("FullName cannot be null or empty.");
            if (course <= 0)
                throw new ArgumentException("Course must be positive.");
            if (group <= 0)
                throw new ArgumentException("Group must be positive.");
            if (grades == null || grades.Count != 8 || grades.Any(s => s.Count != 5))
                throw new ArgumentException("Grades must have 8 sessions, each with 5 exams.");
            if (form != "Budget" && form != "Contract")
                throw new ArgumentException("Form must be 'Budget' or 'Contract'.");

            FullName = fullName;
            Course = course;
            Group = group;
            Grades = grades;
            Form = form;
        }

        public double AverageGrade()
        {
            if (Grades == null || Grades.Count == 0 || Grades.All(session => session.Count == 0))
                return 0.0; // Avoid divide by zero
            int totalGrades = Grades.Sum(session => session.Sum());
            int totalExams = Grades.Sum(session => session.Count);
            return (double)totalGrades / totalExams;
        }

        public override string ToString()
        {
            return $"{FullName} (Course: {Course}, Group: {Group}, Average Grade: {AverageGrade():F2}, Form: {Form})";
        }
    }
}
