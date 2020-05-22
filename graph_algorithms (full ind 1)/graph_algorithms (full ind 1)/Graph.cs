using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace graph_algorithms__full_ind_1_
{
    class Graph
    {
        protected int vertexCount;
        protected int[,] vertexesMatrix;
        public int VertexCount
        {
            get => this.vertexCount;
            set
            {
                if (value >= 0)
                { this.vertexCount = value; }
            }
        }
        public Graph() { }
        //public Graph(int size)
        //{
        //    vertexCount = size;
        //    vertexesMatrix = new int[vertexCount, vertexCount];
        //}
        //public Graph(RichTextBox richTextBox)
        //{
        //    vertexCount = richTextBox.Lines.Length;
        //    vertexesMatrix = new int[vertexCount, vertexCount];
        //    string[] splitTxtBox = richTextBox.Text.Split('\n');
        //    for (int i = 0; i < vertexCount; i++)
        //    {
        //        for (int j = 0; j < vertexCount; j++)
        //        {
        //            vertexesMatrix[i, j] = (int)Char.GetNumericValue(splitTxtBox[i][j]);
        //        }
        //    }
        //}
        //public Graph(RichTextBox richTextBox, int size)
        //{
        //    vertexCount = size;
        //    vertexesMatrix = new int[vertexCount, vertexCount];
        //    string[] splitTxtBox = richTextBox.Text.Split('\n');
        //    for (int i = 0; i < vertexCount; i++)
        //    {
        //        for (int j = 0; j < vertexCount; j++)
        //        {
        //            vertexesMatrix[i, j] = (int)Char.GetNumericValue(splitTxtBox[i][j]);
        //        }
        //    }
        //}
        ~Graph()
        {
        }
        public int[,] GetMatrix()
        {
            return vertexesMatrix;
        }
        public void SetMatrix(int[,] matrix)
        {
            vertexesMatrix = matrix;
        }
        public int GetEdge(int str, int row)
        {
            return vertexesMatrix[str, row];
        }
        public void SetEdge(int value, int str, int row)
        {
            vertexesMatrix[str, row] = value;
        }
        public void FillMatrix(RichTextBox richTextBox, ToolStripComboBox toolStripComboBox)
        {
            vertexesMatrix = new int[vertexCount, vertexCount];
            char[] sep = new char[2]{ '\n', ',' };
            string[] splitTxtBox = richTextBox.Text.Split(sep);

            if (toolStripComboBox.SelectedIndex == 4)
            {
                int t = 0;
                for (int i = 0; i < vertexCount; i++)
                {
                    for (int j = 0; j < vertexCount; j++)
                    {
                        vertexesMatrix[i, j] = Convert.ToInt32(splitTxtBox[t]);
                        t++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < vertexCount; i++)
                {
                    for (int j = 0; j < vertexCount; j++)
                    {
                        vertexesMatrix[i, j] = (int)Char.GetNumericValue(splitTxtBox[i][j]);
                    }
                    if (toolStripComboBox.SelectedIndex == 1 || toolStripComboBox.SelectedIndex == 2)
                    {
                        vertexesMatrix[i, i] = 1;
                    }

                }
            }
        }
    }
}
