using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Number_of_spanning_trees__ind_1._1_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public int[,] vertexMatrix;
        public int[,] kirchhoffMatrix;
        public int matrixSize;

        public void CreateKirchhoffMatrix() 
        {
            kirchhoffMatrix = new int[matrixSize, matrixSize];
            for (int i = 0; i < matrixSize; i++)
            {
                int temp = 0;
                for (int j = 0; j < matrixSize; j++)
                {
                    if (vertexMatrix[i, j] == 1)
                    {
                        kirchhoffMatrix[i, j] = -1;
                        temp++;
                    }
                }
                kirchhoffMatrix[i, i] = temp;
            }
        }
        public void TransformMatrix() 
        {
        
        }
        public int FindDeterminant(int[,] kirchhoffMatrix, int matrixSize) 
        {
            int sum = 0, a = 1;
            int[,] minor = new int[matrixSize, matrixSize];
            if (matrixSize > 0)
            {
                if (matrixSize == 1)
                {
                    return kirchhoffMatrix[0, 0];
                }
                if (matrixSize == 2)
                {
                    return kirchhoffMatrix[0, 0] * kirchhoffMatrix[1, 1] - kirchhoffMatrix[0, 1] * kirchhoffMatrix[1, 0];
                }
                if (matrixSize >= 2)
                {
                    for (int i = 0; i < matrixSize; i++)
                    {
                        TransformMatrix();
                        sum += a * kirchhoffMatrix[i, 0] * FindDeterminant(minor, matrixSize - 1);
                        a = -a;
                    }
                }
            }
            else return 0;
            return sum;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int t = 0;
            if (richTextBox1.Text.Equals(""))
            {
                StreamReader ifstream = new StreamReader("matrix.txt", System.Text.Encoding.Default);
                richTextBox1.Text += ifstream.ReadToEnd();
                ifstream.Close();
            }

            while (!richTextBox1.Text[t++].Equals('\n'))
            {
                matrixSize++;
            }
            vertexMatrix = new int[matrixSize, matrixSize];
            t = 0;

            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    vertexMatrix[i, j] = (int)Char.GetNumericValue(richTextBox1.Text[t++]);
                }
                t++;
            }

            CreateKirchhoffMatrix();
            int det = FindDeterminant();
            StreamWriter ffstream = new StreamWriter("result.txt", false, System.Text.Encoding.Default);
            ffstream.Write(det);
            ffstream.Close();
            label3.Text = Convert.ToString(det);
        }
    }
}
