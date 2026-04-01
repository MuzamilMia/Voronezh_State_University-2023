        using System;
        using System.Collections.Generic;
        using System.Data;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks;
        using System.Windows.Forms;

namespace _6.lab
{
    class MatrMake
    {
        int n_str, n_col;
        int[,] matrix;

        public MatrMake(int n)
        {
            n_str = n;  // Square matrix
            n_col = n;
            matrix = new int[n, n];
        }

        // Fill matrix from DataGridView
        public void GridToMatrix(DataGridView dgv)
        {
            DataGridViewCell txtCell;
            for (int i = 0; i < n_str; i++)
            {
                for (int j = 0; j < n_col; j++)
                {
                    txtCell = dgv.Rows[i].Cells[j];
                    string s = txtCell.Value.ToString();
                    if (s == "")
                        matrix[i, j] = 0;
                    else
                        matrix[i, j] = Int32.Parse(s);
                }
            }
        }

        // Display matrix in DataGridView
        public void MatrixToGrid(DataGridView dgv)
        {
            int i;
            DataTable matr = new DataTable("matr");
            DataColumn[] cols = new DataColumn[n_col];
            for (i = 0; i < n_col; i++)
            {
                cols[i] = new DataColumn(i.ToString());
                matr.Columns.Add(cols[i]);
            }
            for (i = 0; i < n_str; i++)
            {
                DataRow newRow;
                newRow = matr.NewRow();
                matr.Rows.Add(newRow);
            }
            dgv.DataSource = matr;
            for (i = 0; i < n_col; i++)
                dgv.Columns[i].Width = 50;

            // Fill DataGridView with matrix values
            DataGridViewCell txtCell;
            for (i = 0; i < n_str; i++)
            {
                for (int j = 0; j < n_col; j++)
                {
                    txtCell = dgv.Rows[i].Cells[j];
                    txtCell.Value = matrix[i, j].ToString();
                }
            }
        }

        // Remove rows where all elements are equal
        public void RemoveEqualRows()
        {
            int i, j;
            for (i = 0; i < n_str; i++)
            {
                bool allEqual = true;
                // Check if all elements in the row are equal
                for (j = 1; j < n_col; j++)
                {
                    if (matrix[i, j] != matrix[i, 0])
                    {
                        allEqual = false;
                        break;
                    }
                }
                if (allEqual)
                {
                    // If all elements in the row are equal, shift rows down
                    for (int k = i; k < n_str - 1; k++)
                    {
                        for (j = 0; j < n_col; j++)
                        {
                            matrix[k, j] = matrix[k + 1, j];
                        }
                    }
                    n_str--;  // Decrease row count
                    i--;  // Check the new row that shifted down
                }
            }
        }

        // Delete rows containing only zeroes
        public bool DelStr()
        {
            int i, j;
            bool ok;
            i = 0;
            while (i < n_str)
            {
                ok = true;
                for (j = 0; j < n_col && ok; j++)
                    if (matrix[i, j] == 0)
                        ok = false;
                if (!ok)
                {
                    // Remove row i
                    for (int k = i; k < n_str - 1; k++)
                        for (j = 0; j < n_col; j++)
                            matrix[k, j] = matrix[k + 1, j];
                    n_str--;
                }
                else i++;
            }
            return n_str == 0;
        }
    }
}
