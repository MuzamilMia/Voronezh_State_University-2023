using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters;

namespace WindowsFormsApp1
{
    [Serializable]
    public class Exam
    {
        [XmlElement]
        public string NameExam { get; set; }
        [XmlIgnore]
        private ushort grade;
        [XmlElement]
        public ushort Grade
        {
            get => grade;
            set
            {
                if (value <= 5 && value >= 2)
                    grade = value;
                else
                    throw new ArgumentException("Invalid grade\n");
            }
        }
        public Exam() 
        {
            NameExam = string.Empty;
            grade = 0;
        }
        public Exam(string nameExam, ushort grade)
        {
            NameExam = nameExam;
            Grade = grade;
        }
        public Exam(StreamReader rs)
        {
            string[] line = rs.ReadLine().Trim().Split('-');
            NameExam = line[0];
            if (!ushort.TryParse(line[1], out ushort tmpGrade))
                throw new FormatException("Invalid grade");
            Grade = tmpGrade;
        }
        public override string ToString() => NameExam + "-" + grade;
    }
}
