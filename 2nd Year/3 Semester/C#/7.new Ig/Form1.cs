using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.VisualStyles;

namespace _7.new_Ig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
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
    }
}
