using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _7.Labbb
{
    public partial class Form1 : Form
    {
        private List<Student> students = new List<Student>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files|*.xml|Binary Files|*.bin|Text Files|*.txt";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (filePath.EndsWith(".xml"))
                    students = FileHandler.LoadFromXml(filePath);
               // else if (filePath.EndsWith(".bin"))
                   // students = FileHandler.LoadFromBinary(filePath);
                else
                    students = FileHandler.LoadFromText(filePath);

                RefreshGrid();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            students.Clear();
            RefreshGrid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Files|*.xml|Binary Files|*.bin|Text Files|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                if (filePath.EndsWith(".xml"))
                    FileHandler.SaveToXml(filePath, students);
                else if (filePath.EndsWith(".bin"))
                    FileHandler.SaveToBinary(filePath, students);
                else
                    FileHandler.SaveToText(filePath, students);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddStudent addForm = new AddStudent();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                students.Add(addForm.Student);
                RefreshGrid();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int course = int.Parse(textBox1.Text);
            var budgetStudents = students.Where(s => s.Course == course && s.Form == "Budget").ToList();
            var contractStudents = students.Where(s => s.Course == course && s.Form == "Contract").ToList();

            double budgetAvg = budgetStudents.Any() ? budgetStudents.Average(s => s.AverageGrade()) : 0;
            double contractAvg = contractStudents.Any() ? contractStudents.Average(s => s.AverageGrade()) : 0;

            MessageBox.Show($"Budget Avg: {budgetAvg:F2}\nContract Avg: {contractAvg:F2}", "Comparison Result");
        }
        private void RefreshGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var student in students)
            {
                dataGridView1.Rows.Add(student.FullName, student.Course, student.Group, student.Form, student.AverageGrade());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
