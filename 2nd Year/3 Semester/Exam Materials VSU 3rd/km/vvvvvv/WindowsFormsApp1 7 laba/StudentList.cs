using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public class StudentList
    {
        private List<Student> students;

        public StudentList()
        {
            students = new List<Student>();
        }

        public void FromFile(string filePath)
        {
            StreamReader sr = new StreamReader(filePath);
            if (!int.TryParse(sr.ReadLine(), out int n))
                throw new FormatException("Invalid Count of Students\n");
            if (n <= 0)
                throw new ArgumentException("Invalid Count of Students. It can't be zero, or below zero\n");
            students = new List<Student>();
            for (int i = 0; i < n; ++i)
            {
                students.Add(new Student(sr));
                sr.ReadLine();
            }
            sr.Close();
        }
        public void WriteToFile(string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(this.ToString());
            sw.Close();
        }

        public void FromBinary(string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(filePath, FileMode.Open))
                students = formatter.Deserialize(file) as List<Student>;
            FixGrades();
        }
        public void WriteToBinary(string filePath)
        {
            FilterGrades();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream file = new FileStream(filePath, FileMode.Create))
                formatter.Serialize(file, students);
        }

        public void FromXML(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
            using (FileStream file = new FileStream(filePath, FileMode.Open))
                students = serializer.Deserialize(file) as List<Student>;
            FixGrades();
        }
        public void WriteToXML(string filePath)
        {
            FilterGrades();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));
            using (FileStream file = new FileStream(filePath, FileMode.Create))
                serializer.Serialize(file, students);
        }

        public void FilterGrades()
        {
            for (int i = 0; i < Length(); ++i)
            {
                int limit = At(i).Course * Student.kSessionPerCourse;
                At(i).Grades = At(i).Grades.Take(limit).ToArray();
            }
        }

        public void FixGrades()
        {
            for (int i = 0; i < Length(); ++i)
            {
                if (At(i).Grades.Length != Student.kSessionPerCourse * Student.kMaxCourse)
                {
                    Student.GradeRow[] tmp = At(i).Grades;
                    At(i).Grades = new Student.GradeRow[Student.kSessionPerCourse * Student.kMaxCourse];
                    for (int j = 0; j < At(i).Grades.Length; ++j)
                    {
                        if (j < tmp.Length && tmp[j] != null)
                        {
                            At(i).Grades[j] = tmp[j];
                            for (int k = 0; k < Student.kExamPerSession; ++k)
                                At(i).Grades[j].Exams[k] = tmp[j].Exams[k];
                        }
                        else
                        {
                            At(i).Grades[j] = new Student.GradeRow { Exams = new Exam[Student.kExamPerSession] };
                            for (int k = 0; k < Student.kExamPerSession; ++k)
                                At(i).Grades[j].Exams[k] = new Exam();
                        }
                    }
                }
            }
        }


        public void Add(Student student) => students.Add(student);
        public void EraseAt(int index)
        {
            students.RemoveAt(index);
            students.TrimExcess();
        }
        public int Length() => students.Count();
        public void Clear() => students.Clear();
        public void Sort() => students.Sort((x,y) => x.CompareTo(y));
        public Student At(int i)
        {
            if (i >= 0 && i < Length())
                return students[i];
            throw new IndexOutOfRangeException("AT index");
        }
        public StudentList CountExcellentStudents()
        {
            int count = 0;
            StudentList result = new StudentList();
            for (int i = 0; i < Length(); ++i)
                if (At(i).IsExcellent())
                {
                    ++count;
                    result.Add(new Student(At(i)));
                }
            return result;
        }
        public override string ToString()
        {
            string result = students.Count().ToString() + "\n";
            for (int i = 0; i < Length(); ++i)
                result += At(i).ToString() + '\n';
            return result;
        }
    }
}
