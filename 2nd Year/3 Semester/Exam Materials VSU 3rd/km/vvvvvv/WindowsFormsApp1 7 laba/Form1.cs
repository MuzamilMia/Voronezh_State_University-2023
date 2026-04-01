//12.Задана целочисленная квадратная матрица n * n. Удалить все симметричные столбцы

using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Form2 form2 = null;
        private StudentList students = new StudentList();
        private StudentList result = new StudentList();
       
        private enum FileTypes{ None, txt, bin, xml };
        private string filePath = string.Empty;
        private FileTypes fileType = FileTypes.None;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ShowForm2();
        }

        private void showStudentsList()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < students.Length(); ++i)
                listBox1.Items.Add(students.At(i).FullName + " " 
                    + students.At(i).Course.ToString() + " к. " 
                    + students.At(i).Group.ToString() + " гр.");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            students.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool exc = false;
                if (new FileInfo(dialog.FileName).Length < 10)
                {
                    MessageBox.Show("Пустой Файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                filePath = dialog.FileName;
                string extension = filePath.Substring(filePath.LastIndexOf("."));
                try
                {
                    switch (extension)
                    {
                        case ".txt":
                            students.FromFile(filePath);
                            fileType = FileTypes.txt;
                            break;
                        case ".bin":
                            students.FromBinary(filePath);
                            fileType = FileTypes.bin;
                            break;
                        case ".xml":
                            students.FromXML(filePath);
                            fileType = FileTypes.xml;
                            break;
                        default:
                            MessageBox.Show("Неизвестный тип файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            exc = true;
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Проблема с Файлом " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                if (exc)
                {
                    students.Clear();
                    fileType = FileTypes.None;
                    filePath = string.Empty;
                }
                else
                {
                    showStudentsList();
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Visible = false;
                    button5.Visible = false;
                }
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";
 
            if (dialog.ShowDialog() != DialogResult.Cancel)
            {
                filePath = dialog.FileName;
                string extension = filePath.Substring(filePath.LastIndexOf("."));
                switch (extension)
                {
                    case ".txt":
                        fileType = FileTypes.txt;
                        break;
                    case ".bin":
                        fileType = FileTypes.bin;
                        break;
                    case ".xml":
                        fileType = FileTypes.xml;
                        break;
                    default:
                        fileType = FileTypes.None;
                        MessageBox.Show("Неподдерживаемый тип файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                StreamWriter file = new StreamWriter(filePath, false, Encoding.UTF8); ;
                file.Close();
                listBox1.Items.Clear();
                students.Clear();
                MessageBox.Show("Файл успешно создан", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileType == FileTypes.None)
                createToolStripMenuItem_Click(sender, e);
            bool exc = false;
            try
            {
                switch (fileType)
                {
                    case FileTypes.txt:
                        students.WriteToFile(filePath);
                        break;
                    case FileTypes.bin:
                        students.WriteToBinary(filePath);
                        break;
                    case FileTypes.xml:
                        students.WriteToXML(filePath);
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                exc = true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                exc = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Проблема с Файлом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                exc = true;
            }
            if (!exc)
                MessageBox.Show($"Файл {filePath.Substring(filePath.LastIndexOf("\\") + 1)} успешно сохранён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bool exc = false;
                string filename = dialog.FileName;
                string extension = filename.Substring(filename.LastIndexOf("."));
                try
                {
                    switch (extension)
                    {
                        case ".txt":
                            students.WriteToFile(filename);
                            fileType = FileTypes.txt;
                            break;
                        case ".bin":
                            students.WriteToBinary(filename);
                            fileType = FileTypes.bin;
                            break;
                        case ".xml":
                            students.WriteToXML(filename);
                            fileType = FileTypes.xml;
                            break;
                        default:
                            MessageBox.Show("Неизвестный тип файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            exc = true;
                            break;
                    }
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Проблема с Файлом", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exc = true;
                }
                if (!exc)
                {
                    filePath = filename;
                    MessageBox.Show($"Файл {filePath.Substring(filePath.LastIndexOf("\\") + 1)} успешно сохранён", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void solveTheTasskButton_Click(object sender, EventArgs e)
        {
            result.Clear();
            result = students.CountExcellentStudents();
            if (result.Length() != 0)
            {
                textBox1.Clear();
                label4.Visible = true;
                textBox1.Visible = true;
                button5.Visible = true;
                result.Sort();
                for (int i = 0; i < result.Length(); ++i)
                    textBox1.AppendText(result.At(i).FullName + " " + result.At(i).Course.ToString() +
                        " курс " + result.At(i).Group.ToString() + " группа" + Environment.NewLine);
            }
            else
            {
                label4.Visible = false;
                textBox1.Visible = false;
                button5.Visible = false;
            }
            textBox2.Text = result.Length().ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Результат пуст, нечего сохранять", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text (*.txt)|*.txt|Binary (*.bin)|*.bin|XML (*.xml)|*.xml";

            string filename = string.Empty;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filename = dialog.FileName;
                string extension = filename.Substring(filename.LastIndexOf("."));
                switch (extension)
                {
                    case ".txt":
                        result.WriteToFile(filename);
                        break;
                    case ".bin":
                        result.WriteToBinary(filename);
                        break;
                    case ".xml":
                        result.WriteToXML(filename);
                        break;
                    default:
                        MessageBox.Show("Неизвестный тип файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }
                MessageBox.Show(string.Format("Файл {0} успешно сохранён", filename), "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object seendr, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Выберите сначала студента, попробуйте еще раз", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            students.Add(new Student(students.At(index)) { FullName = students.At(index).FullName + "_copy"});
            UpdateList();
            button6.Visible = false;
            button3.Visible = false;
            panel1.Visible = false;
            label3.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            ShowForm2(ref student);
            students.Add(student);
            MessageBox.Show("Введите данные нового студента", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index < 0)
            {
                MessageBox.Show("Выберите сначала студента, попробуйте еще раз", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            students.EraseAt(index);
            UpdateList();
            button3.Visible = false;
            button6.Visible = false;
            panel1.Visible = false;
            label3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            students.Sort();
            UpdateList();
        }

        private void ShowForm2(ref Student student)
        {
            Form2 form2 = new Form2(ref student);
            form2.TopLevel = false;
            form2.UpdateListRequested += UpdateList;
            form2.FormBorderStyle = FormBorderStyle.None;
            form2.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Visible = true;
            label3.Visible = true;
            panel1.Controls.Add(form2);
            form2.Show();
        }
        private void UpdateList()
        {

            showStudentsList();
            MessageBox.Show("Изменения внесены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                button3.Visible = true;
                button6.Visible = true;
                Student student = students.At(index);
                ShowForm2(ref student);
            }
        }
    }
}