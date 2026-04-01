using System;
using System.Data;
using System.Windows.Forms;

namespace _6.lab
{
    public partial class Form1 : Form
    {
        // Use MatrMake class to store and process the matrix
        private MatrMake mt;

        public Form1()
        {
            InitializeComponent();
        }

        // Event handler for when button2 is clicked
        private void button2_Click(object sender, EventArgs e)
        {
            int N = Int32.Parse(textBox1.Text);
            dataGridView2.Visible = false;
            MatrMake mt = new MatrMake(N);
            mt.GridToMatrix(dataGridView1);

            // Remove rows where all elements are equal
            mt.RemoveEqualRows();

            // Remove rows containing only zeroes
            if (mt.DelStr())
                MessageBox.Show("Все строки содержат нули");
            else
            {
                dataGridView2.Visible = true;
                mt.MatrixToGrid(dataGridView2);
            }
        }


        // Event handler for when the text in textBox1 changes
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int N) && N > 0)
            {
                DataTable matr = new DataTable("matr");
                DataColumn[] cols = new DataColumn[N];
                for (int i = 0; i < N; i++)
                {
                    cols[i] = new DataColumn(i.ToString());
                    matr.Columns.Add(cols[i]);
                }
                for (int i = 0; i < N; i++)
                {
                    DataRow newRow = matr.NewRow();
                    matr.Rows.Add(newRow);
                }
                dataGridView1.DataSource = matr;

                // Set column width for better readability
                for (int i = 0; i < N; i++)
                {
                    dataGridView1.Columns[i].Width = 50;
                }
            }
            else
            {
                // Handle case for invalid input
                MessageBox.Show("Enter the right size for matrix ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Удалить строки, содержащие все равные элементы", "MY TASK ");
        }
    }
}
