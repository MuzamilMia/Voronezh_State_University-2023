using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public event Action UpdateListRequested;
        public Student student = new Student();
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(ref Student ostudent)
        {
            InitializeComponent();
            this.student = ostudent;
            FillForm();
        }

        Student GetStudent()
        {
            return student;
        }

        //In second form we are saving. The button. 
        private void saveButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Введите Имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            student.FullName = textBox1.Text;
            if (ushort.TryParse(textBox2.Text, out ushort course))
            {
                try
                {
                    student.Course = course;
                }
                catch (ArgumentException exc)
                {
                    MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else 
            {
                MessageBox.Show("Невалидный курс", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (uint.TryParse(textBox3.Text, out uint group))
            {
                try
                {
                    student.Group = group;
                }
                catch (ArgumentException exc)
                {
                    MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Невалидная группа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (checkedListBox1.GetItemChecked(0) && checkedListBox1.GetItemChecked(1))
            {
                MessageBox.Show("Нельзя выбрать две формы обучения", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (checkedListBox1.GetItemChecked(0))
                student.FormStudy = Student.FormOfStudy.Contract;
            else
                student.FormStudy = Student.FormOfStudy.Budget;

            int j = 0;
            for (int i = 0; i < student.Course * Student.kSessionPerCourse; ++i)
            {
                for (int k = 0; k < Student.kExamPerSession; ++k)
                {
                    string tmp = dataGridView1.Rows[j].Cells[2].Value.ToString();
                    if (tmp == string.Empty)
                    {
                        MessageBox.Show(string.Format("Введите название предмета в строке {0}", j + 1), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                        student.Grades[i].Exams[k].NameExam = tmp;
                    tmp = dataGridView1.Rows[j].Cells[3].Value.ToString();
                    if (ushort.TryParse(tmp, out ushort grade))
                    {
                        try
                        {
                            student.Grades[i].Exams[k].Grade = grade;
                        }
                        catch (ArgumentException exc)
                        {
                            MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Enter the mark to the row {0}", j + 1), "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ++j;
                }
            }
            UpdateListRequested?.Invoke();
            this.student = null;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ushort.TryParse(textBox2.Text, out ushort newCourse) && newCourse > 0 && newCourse <= 4)
            {
                student.Course = newCourse;
                UpdateDataGridViewRows(newCourse);
            }

        }

        private void FillForm()
        {
            textBox1.Text = student.FullName;
            textBox2.Text = student.Course.ToString();
            textBox3.Text = student.Group.ToString();
            UpdateDataGridViewRows(student.Course);

            if (student.FormStudy == Student.FormOfStudy.Contract)
                checkedListBox1.SetItemChecked(0, true);
            else
                checkedListBox1.SetItemChecked(1, true);
        }

        private void UpdateDataGridViewRows(int course)
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < course; i++)
            {
                for (int j = i * Student.kSessionPerCourse; j < (i + 1) * Student.kSessionPerCourse; ++j)
                {
                    for (int k = 0; k < Student.kExamPerSession; ++k)
                    {
                        dataGridView1.Rows.Add(
                            (i + 1).ToString(),
                            (j + 1).ToString(),
                            student.Grades[j].Exams[k].NameExam,
                            student.Grades[j].Exams[k].Grade.ToString());
                    }
                }
            }
        }

        
    }
}
