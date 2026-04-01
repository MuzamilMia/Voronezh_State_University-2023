//using System;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

//namespace FileProcessorApp
//{
//    public partial class Form1 : Form
//    {
//        private string fileName = "";
//        private StreamReader f_In;
//        private StreamWriter f_Out;

//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void openToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            if (openFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                // Open the file and read its content into textBox1
//                try
//                {
//                    using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
//                    {
//                        textBox1.Text = reader.ReadToEnd();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Error opening the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            if (fileName == "")
//            {
//                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//                {
//                    fileName = saveFileDialog1.FileName;
//                    f_Out = new StreamWriter(saveFileDialog1.FileName);
//                    f_Out.WriteLine(textBox1.Text);
//                    f_Out.Close();
//                }
//            }
//        }

//        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            // Check if there's content in textBox1 to process
//            if (string.IsNullOrWhiteSpace(textBox1.Text))
//            {
//                MessageBox.Show("Please open a file first.", "No File Opened", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // Initialize a StringBuilder to hold the new content
//            StringBuilder result = new StringBuilder();

//            // Process each line from the input (textBox1 contains the content of the opened file)
//            string[] lines = textBox1.Lines;

//            foreach (var line in lines)
//            {
//                if (string.IsNullOrWhiteSpace(line))
//                {
//                    // If the line is empty (or just spaces), add a blank line to the result
//                    result.AppendLine();
//                }
//                else
//                {
//                    // Split the line into words based on spaces (also handles multiple spaces)
//                    string[] words = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

//                    foreach (var word in words)
//                    {
//                        result.AppendLine(word);
//                    }
//                }
//            }

//            // Show the result in textBox2 for preview
//            textBox2.Text = result.ToString();

//            // Prompt the user to save the result to a new file
//            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
//            {
//                try
//                {
//                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
//                    {
//                        writer.Write(result.ToString());
//                    }
//                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                }
//            }
//        }

//        private void условиеЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Дан текстовый файл, содержащий слова, разделенные одним или несколькими пробелами.\n" +
//                "Реализовать программу, которая формирует новый файл, где каждое слово расположено на отдельной строке.\n" +
//                "Переход к новой строке в исходном файле соответствует пустой строке в новом файле.", "Условие задачи");
//        }

//        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
//        {
//            Close();
//        }
//    }
//}


using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FileProcessorApp
{
    public partial class Form1 : Form
    {
        private string fileName = "";
        private StreamReader f_In;
        private StreamWriter f_Out;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Open the file and read its content into textBox1
                    using (StreamReader reader = new StreamReader(openFileDialog1.FileName))
                    {
                        textBox1.Text = reader.ReadToEnd();  // Load file content into textBox1
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fileName = saveFileDialog1.FileName;
                    f_Out = new StreamWriter(saveFileDialog1.FileName);
                    f_Out.WriteLine(textBox1.Text);  // Save the raw text content of textBox1
                    f_Out.Close();
                }
            }
        }

        private void taskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if there's content in textBox1 to process
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please open a file first.", "No File Opened", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Initialize a StringBuilder to hold the new content
            StringBuilder result = new StringBuilder();

            // Process each line from the input (textBox1 contains the content of the opened file)
            string[] lines = textBox1.Lines;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // If the line is empty (or just spaces), add a blank line to the result
                    result.AppendLine();
                }
                else
                {
                    // Split the line into words based on spaces (also handles multiple spaces)
                    string[] words = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // For each word, add it on a new line in the result
                    foreach (var word in words)
                    {
                        result.AppendLine(word);
                    }
                }
            }

            // Show the result in textBox2 for preview
            textBox2.Text = result.ToString();

            // Prompt the user to save the result to a new file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Write the processed result to the new file
                    using (StreamWriter writer = new StreamWriter(saveFileDialog1.FileName))
                    {
                        writer.Write(result.ToString());  // Write the processed result
                    }
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void условиеЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Дан текстовый файл, содержащий слова, разделенные одним или несколькими пробелами.\n" +
                "Реализовать программу, которая формирует новый файл, где каждое слово расположено на отдельной строке.\n" +
                "Переход к новой строке в исходном файле соответствует пустой строке в новом файле.", "Условие задачи");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void conditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Дан текстовый файл, содержащий слова, разделенные одним или несколькими пробелами.\n" +
                            "Реализовать программу, которая формирует новый файл, где каждое слово расположено на отдельной строке.\n" +
                            "Переход к новой строке в исходном файле соответствует пустой строке в новом файле.",
                            "Условие задачи");
        }
    }
}
