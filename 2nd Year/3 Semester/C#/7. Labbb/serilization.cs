using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace _7.Labbb
{
    public static class FileHandler
    {
        public static void SaveToXml(string filePath, List<Student> students)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, students);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to XML: {ex.Message}");
            }
        }

        public static List<Student> LoadFromXml(string filePath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    return (List<Student>)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from XML: {ex.Message}");
                return new List<Student>();
            }
        }

        public static void SaveToBinary(string filePath, List<Student> students)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                  //  JsonSerializer.Serialize(fs, students); // Use System.Text.Json for binary format
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to binary: {ex.Message}");
            }
        }

        //public static List<Student> LoadFromBinary(string filePath)
        //{
        //    try
        //    {
        //        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        //        {
        //            return JsonSerializer.Deserialize<List<Student>>(fs);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error loading from binary: {ex.Message}");
        //        return new List<Student>();
        //    }
        //}

        public static void SaveToText(string filePath, List<Student> students)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var student in students)
                    {
                        writer.WriteLine($"{student.FullName};{student.Course};{student.Group};{student.Form}");
                        foreach (var session in student.Grades)
                        {
                            writer.WriteLine(string.Join(",", session));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to text: {ex.Message}");
            }
        }

        public static List<Student> LoadFromText(string filePath)
        {
            try
            {
                var students = new List<Student>();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] studentData = line.Split(';');
                        if (studentData.Length < 4) throw new FormatException("Invalid student data format.");

                        string fullName = studentData[0];
                        int course = int.Parse(studentData[1]);
                        int group = int.Parse(studentData[2]);
                        string form = studentData[3];

                        var grades = new List<List<int>>();
                        for (int i = 0; i < 8; i++) // Adjust this if dynamic session support is needed
                        {
                            string gradesLine = reader.ReadLine();
                            if (string.IsNullOrWhiteSpace(gradesLine)) throw new FormatException("Missing grades for session.");
                            var sessionGrades = gradesLine.Split(',').Select(int.Parse).ToList();
                            grades.Add(sessionGrades);
                        }

                        students.Add(new Student(fullName, course, group, grades, form));
                    }
                }
                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from text: {ex.Message}");
                return new List<Student>();
            }
        }
    }
}
