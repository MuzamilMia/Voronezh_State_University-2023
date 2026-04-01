/* 18. На заданном курсе сравнить успеваемость студентов бюджетной и 
договорной форм обучения (по среднему баллу).*/
using System;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
/*
Структура записи файла с информацией о студентах:
 ФИО;
 номер курса;
 номер группы;
 успеваемость — 5 экзаменов в каждой из 8 сессий;
 форма обучения.
*/

namespace WindowsFormsApp1
{
    [Serializable]
    public class Student
    {
        [XmlElement]
        public string FullName { get; set; }

        [XmlIgnore]
        private ushort course;
        [XmlElement]
        public ushort Course
        {
            get => course;
            set
            {
                if (value < 1 || value > kMaxCourse)
                    throw new ArgumentException("Course can't be 0 or more than 4\n");
                course = value;
            }
        }
        
        [XmlIgnore]
        private uint group;
        [XmlElement]
        public uint Group
        {
            get => group;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Group can't be 0\n");
                group = value;
            }
        }

        public enum FormOfStudy { Contract, Budget }
        [XmlElement]
        public FormOfStudy FormStudy { get; set; }

        [XmlIgnore]
        static public readonly ushort kExamPerSession = 5;
        [XmlIgnore]
        static public readonly ushort kSessionPerCourse = 2;
        [XmlIgnore]
        static public readonly ushort kMaxCourse = 4;

        [Serializable]
        public class GradeRow
        {
            [XmlArray("Exams")]
            [XmlArrayItem("Exam")]
            public Exam[] Exams { get; set; }

            public GradeRow()
            {
                Exams = new Exam[kExamPerSession];
                for (int i = 0; i < kExamPerSession; ++i)
                    Exams[i] = new Exam();
            }
        }
        [XmlArray("Grades")]
        [XmlArrayItem("Session")]
        public GradeRow[] Grades { get; set; }

        public Student() 
        {
            FullName = string.Empty;
            Course = 1;
            Group = 1;
            FormStudy = FormOfStudy.Budget;
            initGrades();
        }
        public Student(Student other)
        {
            FullName = other.FullName;
            Course = other.Course;
            Group = other.Group;
            FormStudy = other.FormStudy;
            initGrades();
            if (other.Grades != null)
            {
                for (int i = 0; i < kSessionPerCourse * kMaxCourse; ++i)
                {
                    if (other.Grades.Length <= i)
                        break;
                    if (other.Grades[i].Exams.Length == 0 || other.Grades[i].Exams[0] == null)
                        break;
                    for (int j = 0; j < kExamPerSession; ++j)
                        Grades[i].Exams[j] = other.Grades[i].Exams[j];
                }
            }
        }
        public Student(StreamReader sr)
        {
            FullName = sr.ReadLine().Trim();
            if (!ushort.TryParse(sr.ReadLine(), out ushort tmpCourse))
                throw new FormatException("Invalid Course\n");
            Course = tmpCourse;
            if (!uint.TryParse(sr.ReadLine(), out uint tmpGroup))
                throw new FormatException("Invalid Group\n");
            Group = tmpGroup;
            string tmp = sr.ReadLine().Trim().ToLower();
            FormStudy = tmp == "contract" ? FormOfStudy.Contract : FormOfStudy.Budget;
            initGrades();

            for (ushort j = 0; j < Course * kSessionPerCourse; ++j) 
            {
                sr.ReadLine();
                for (ushort k = 0; k < kExamPerSession; ++k)
                {
                    Grades[j].Exams[k] = new Exam(sr);
                }
            }
        }

       
        public override string ToString()
        {
            string result = $"{FullName}\nCourse: {Course}\nGroup: {Group}\n" +
                            $"Form of Education: {(FormStudy == FormOfStudy.Contract ? "Contract" : "Budget")}\n" +
                            $"Average Grade: {AverageGrade():F2}\n";

            for (int i = 0; i < Course * 2; ++i)
            {
                result += $"{i + 1} session:\n";
                for (int j = 0; j < 5; ++j)
                    result += Grades[i].Exams[j].ToString() + "\n";
            }

            return result;
        }

       
        private void initGrades()
        {
            Grades = new GradeRow[kSessionPerCourse * kMaxCourse];
            for (int i = 0; i < kSessionPerCourse * kMaxCourse; ++i)
            {
                Grades[i] = new GradeRow { Exams = new Exam[kExamPerSession] };
                for (int j = 0; j < kExamPerSession; ++j)
                    Grades[i].Exams[j] = new Exam();
            }
        }

        //-------------------------------Average--------------------------------
        public double AverageGrade()
        {
            int totalGrades = 0;
            int gradeCount = 0;

            for (int i = 0; i < Course * kSessionPerCourse; ++i)
            {
                for (int j = 0; j < kExamPerSession; ++j)
                {
                    if (Grades[i].Exams[j] != null)
                    {
                        totalGrades += Grades[i].Exams[j].Grade;
                        gradeCount++;
                    }
                }
            }

            return gradeCount > 0 ? (double)totalGrades / gradeCount : 0.0;
        }
        //-------------------------------------------
        public string GetFormOfEducationString()
        {
            return FormStudy == FormOfStudy.Contract ? "Contract" : "Budget";
        }



    }
}