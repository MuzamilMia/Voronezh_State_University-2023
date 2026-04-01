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
    public class Student : IComparable<Student>
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
            FormStudy = tmp == "договор" ? FormOfStudy.Contract : FormOfStudy.Budget;
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

        public int CompareTo(Student other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));
            int result = Course.CompareTo(other.Course);
            if (result != 0) return result;
            result = Group.CompareTo(other.Group);
            if (result != 0) return result;
            return string.Compare(FullName, other.FullName, StringComparison.Ordinal);   
        }
        public override string ToString()
        {
            string result = FullName + "\n" + Course.ToString() +
            "\n" + Group.ToString() + "\n" + (FormStudy == FormOfStudy.Contract ? "договор" : "бюджет") + "\n";
            for (int i = 0; i < Course * 2; ++i)
            {
                result += string.Format("{0} session:\n", i + 1);
                for (int j = 0; j < 5; ++j)
                    result += Grades[i].Exams[j].ToString() + "\n";
            }
            return result;
        }
        public bool IsExcellent()
        {
            bool flag = true;
            for (int i = 0; flag && i < Course * 2; ++i)
                for (int j = 0; j < 5; ++j)
                    if (this.Grades[i].Exams[j].Grade != 5)
                    {
                        flag = false;
                        break;
                    }
            return flag;
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
    }
}