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

namespace Chromatic_number__ind_1._3_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.MaximumSize = new System.Drawing.Size(550, 550);
            this.WindowState = FormWindowState.Maximized;
        }
        public bool[,] vertexMatrix;
        public int matrixSize;

        public int FindVertexDegree(int i)//возвращает степень вершины
        {
            int degree = 0;
            for (int j = 0; j < matrixSize; j++)
            {
                if (vertexMatrix[i, j])
                {
                    degree++;
                }
            }
            return degree;
        }
        public int MaxDegreeVertex(int i)//возвращает вершину максимальной степени, смежную с вершиной под номером i
        {
            int vertex = 0;
            int degree = 0;
            for (int j = 0; j < matrixSize; j++)
            {
                if (!vertexMatrix[i, j] && degree < FindVertexDegree(j))
                {
                    vertex = j;
                    degree = FindVertexDegree(j);
                }
            }
            return vertex;
        }
        public int FindChromaticNumber()
        {

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
            t = 0;
            vertexMatrix = new bool[matrixSize, matrixSize];

            for (int i = 0; i < matrixSize; i++)
            {
                vertexMatrix[i, i] = true;
                for (int j = 0; j < matrixSize; j++)
                {
                    if ((int)Char.GetNumericValue(richTextBox1.Text[t++]) == 1)
                    {
                        vertexMatrix[i, j] = true;
                    }
                }
                t++;
            }
            int chromatic = FindChromaticNumber();
            StreamWriter ffstream = new StreamWriter("result.txt", false, System.Text.Encoding.Default);
            ffstream.Write(chromatic);//запись результата в файл
            ffstream.Close();
            label3.Text = Convert.ToString(chromatic);
        }
    }
}
