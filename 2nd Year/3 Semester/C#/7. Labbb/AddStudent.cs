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
    public partial class AddStudent : Form
    {
       // private StudentManager manager;
        public AddStudent()
        {
            InitializeComponent();
          //  this.manager = manager;
        }

        public Student Student { get; private set; }

        private void AddStudent_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

          /*  var student = new Student
            {
                FullName = txtName.Text,
                Course = int.Parse(txtcourse.Text),
                Group = int.Parse(txtGroup.Text),
                StudyForm = comboBox1.SelectedItem.ToString(),
                //Grades = ParseGrades()
            };

            manager.AddStudent(student);
            MessageBox.Show("Студент добавлен!");
            this.Close();
            */
              try
            {
                // Get inputs from the form controls
                string fullName = txtName.Text;
                int course = int.Parse(txtcourse.Text);
                int group = int.Parse(txtGroup.Text);
                string form = comboBox1.SelectedItem?.ToString();

                // Validate inputs
                if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(form))
                {
                    MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Initialize grades as an empty list (or add logic to collect grades)
                var grades = new List<List<int>>();
                for (int i = 0; i < 8; i++) // 8 sessions
                {
                    grades.Add(Enumerable.Repeat(0, 5).ToList()); // 5 exams per session, default to 0
                }

                // Create a Student object
                Student = new Student(fullName, course, group, grades, form);

                // Close the form with OK result
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
