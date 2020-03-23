using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsApp9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int[,] vertexesMatrix;
        public int[,] edgesMatrix;
        public int[,] incidenceMatrix;
        public string str;
        public int vertCount = 0;
        public int edgesCount = 0;

        interface testInterface
        {
            int TransmitterVertex { get; set; }
            int TransmitterDirection { get; set; }
            int ReceiverVertex { get; set; }
            int ReceiverDirection { get; set; }
            void EdgeSetUp(int TransmitterValue, int ReceiverValue, bool isOriented);
        }
        public struct testEdge:testInterface
        {
            public int transmitterVertex;
            public int transmitterDirection;
            public int receiverVertex;
            public int receiverDirection;
            public testEdge(int TransmitterValue, int ReceiverValue, bool isOriented)
            {
                this.transmitterVertex = TransmitterValue;
                this.receiverVertex = ReceiverValue;
                this.transmitterDirection = 1;
                this.receiverDirection = 1;
                if (isOriented)
                {
                    this.receiverDirection = -1;
                }
            }
            public int TransmitterVertex { get => this.transmitterVertex; set => this.transmitterVertex = value; }
            public int TransmitterDirection { get => this.transmitterDirection; set => this.transmitterDirection = value; }
            public int ReceiverVertex { get => this.receiverVertex; set => this.receiverVertex = value; }
            public int ReceiverDirection { get => this.receiverDirection;  set => this.receiverDirection = value;  }
            public void EdgeSetUp(int transmitterValue, int receiverValue, bool isOriented)
            {
                this.transmitterVertex = transmitterValue;
                this.receiverVertex = receiverValue;
                this.transmitterDirection = 1;
                this.receiverDirection = 1;
                if (isOriented)
                {
                    this.receiverDirection = -1;
                }
            }

        }
        List<testInterface> test = new List <testInterface>();
        public void InitMatrix(int[,] matrix, int str, int col)
        {
            for (int i = 0; i < str; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }
        public void DeleteEdges()//поиск неориентированного ребра
        {
            for (int i = 0; i < test.Count; i++)
            {
                for (int j = i + 1; j < test.Count; j++) 
                {
                    if (test[i].TransmitterVertex == test[j].ReceiverVertex && test[j].TransmitterVertex == test[i].ReceiverVertex)
                    {
                        test.RemoveAt(j);
                        test[i].ReceiverDirection = 1;
                    }
                }
            }
        }
        public void SetMatrixSize()//из списка смежности создание списка ребер
        {
            str = richTextBox1.Text;
            int edgeNum = 0;
            vertCount++;
            for (int i = 0; i < str.Length; i++)
            {
                if(str[i] == '\n')
                {
                    str = str.Remove(i, 1);
                    vertCount++;
                }
                if (str[i] == ':' && str[i + 1] != '\n') 
                {
                    test.Add(new testEdge((int)Char.GetNumericValue(str[i - 1]), (int)Char.GetNumericValue(str[i + 1]), true));
                    edgeNum = (int)Char.GetNumericValue(str[i - 1]);
                }
                if (str[i] == ',')
                {
                    test.Add(new testEdge(edgeNum, (int)Char.GetNumericValue(str[i + 1]), true));
                }
            }
            DeleteEdges();
        }
        public void FillVertexesMatrix()// заполнение матрицы вершин из списка смежности
        {
            vertexesMatrix = new int[vertCount, vertCount];
            InitMatrix(vertexesMatrix, vertCount, vertCount);
            int k = 0;
            for (int i = 0; i < vertCount; i++)
            {
                for (int j = i+1; j < vertCount; j++)
                {
                    if (k < test.Count)
                    {

                        vertexesMatrix[test[k].TransmitterVertex, test[k].ReceiverVertex] = 1;

                        if (test[k].ReceiverDirection == 1)
                        {
                            vertexesMatrix[test[k].ReceiverVertex, test[k].TransmitterVertex] = 1;
                        }
                        k++;
                    }
                }

                for (int l = 0; l < vertCount; l++)
                {
                    richTextBox2.Text += Convert.ToString(vertexesMatrix[i, l]) + ' ';
                }
                richTextBox2.Text += "\n";
            }
        }
        public void SetVertexesMatrix()//заполнение матрицы вершин из текстбокса
        {
            string str = richTextBox2.Text;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].Equals('\n'))
                {
                    vertCount++;
                }
            }
            vertCount++;
            vertexesMatrix = new int[vertCount, vertCount];
            InitMatrix(vertexesMatrix, vertCount, vertCount);
            int k = 0, j = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (!str[i].Equals('\n'))
                {
                    vertexesMatrix[k, j] = (int)Char.GetNumericValue(str[i]);
                    j++;
                }
                else
                {
                    k++;
                    j = 0;
                }
            }
        }
        public void FillEdgeStruct()//заполнение списка ребер из матрицы вершин
        {
            for (int i = 0; i < vertCount; i++)
            {
                for (int j = 0; j < vertCount; j++)
                {
                    if(vertexesMatrix[i,j] == 1)
                    {
                        test.Add( new testEdge(i, j, true));
                    }
                }
            }
            DeleteEdges();
        }
        public void FillEdgesMatrix()//заполнение матрицы ребер из списка ребер
        {
            edgesMatrix = new int[test.Count, test.Count];
            InitMatrix(edgesMatrix, test.Count, test.Count);

            for (int i = 0; i < test.Count; i++)
            {
                for (int k = 0; k < test.Count; k++)
                {
                    if (i != k) 
                    {
                        if (test[i].ReceiverDirection == -1 && test[k].ReceiverDirection == -1)
                        {
                            if (test[i].TransmitterVertex == test[k].TransmitterVertex ||
                                test[i].ReceiverVertex == test[k].ReceiverVertex)
                            {
                                edgesMatrix[i, k] = 1;
                            }
                        }
                        else
                        {
                            if (test[i].TransmitterVertex == test[k].TransmitterVertex ||
                                test[i].TransmitterVertex == test[k].ReceiverVertex ||
                                test[i].ReceiverVertex == test[k].TransmitterVertex ||
                                test[i].ReceiverVertex == test[k].ReceiverVertex)
                            {
                                edgesMatrix[i, k] = 1;
                            }
                        }
                    }
                }

                for (int j = 0; j < test.Count; j++)
                {
                    richTextBox3.Text += Convert.ToString(edgesMatrix[i, j]) + ' ';
                }
                if (i + 1 != test.Count)
                    richTextBox3.Text += "\n";
            }
        }
        public void SetEdgesMatrix()//заполнение матрицы из текстбокса// не понятно, какие ребра смежны, матрица бесполезна
        {
            string str = richTextBox3.Text;
            for (int i = 0; i < str.Length && !str[i].Equals('\n'); i++)
            { 
                edgesCount++;
            }
            edgesMatrix = new int[edgesCount, edgesCount];
            int k = 0;
            for (int i = 0; i < edgesCount; i++)
            {
                for (int j = 0; j < edgesCount; j++)
                {
                    if (str[k].Equals(Convert.ToChar(10)))
                    {
                        str = str.Remove(k, 1);
                    }
                    edgesMatrix[i, j] = (int)Char.GetNumericValue(str[k]);
                    k++;
                }

            }
        }
        public void FillIncedenceMatrix()//заполнение матрицы инцидентности из списка ребер
        {
            incidenceMatrix = new int[vertCount, test.Count];
            InitMatrix(incidenceMatrix, vertCount, test.Count);

            for (int j = 0; j < test.Count; j++)
            {
                    incidenceMatrix[test[j].TransmitterVertex, j] = test[j].TransmitterDirection;
                    incidenceMatrix[test[j].ReceiverVertex, j] = test[j].ReceiverDirection;
            }

            for (int i = 0; i < vertCount; i++)
            {
                for (int k = 0; k < test.Count; k++)
                {
                    richTextBox4.Text += Convert.ToString(incidenceMatrix[i, k]) + ' ';
                }
                richTextBox4.Text += "\n";
            }

        }
        public void SetIncidenceMatrix()//заполнение матрицы инц из текстбокса
        {
            string str = richTextBox4.Text;
            int i = 0, k = 0;
            vertCount = 1;
            while (!str[i].Equals('\n'))
            {
                if (!str[i].Equals('-'))
                {
                    edgesCount++;
                }
                i++;
            }
            for (i=0; i < str.Length; i++,k++)
            {
                if(str[k + 1].Equals('\n'))
                {
                    vertCount++;
                    str = str.Remove(k+1, 1);
                    k--;
                } 
            }

            incidenceMatrix = new int[vertCount, edgesCount];

            k = 0;
            for (i = 0; i < vertCount; i++)
            {
                for (int j = 0; j < edgesCount; j++)
                {
                    if (k < str.Length)
                    {
                        if (str[k] != '-')
                        {
                            incidenceMatrix[i, j] = (int)Char.GetNumericValue(str[k++]);
                        }
                        else
                        {
                            k++;
                            incidenceMatrix[i, j] = -(int)Char.GetNumericValue(str[k++]);
                        }
                    }
                }

            }
        }
        public void FillAdjList()//заполнение списка смежности (и списка ребер) из матрицы инцедентности
        {
            string[] strMas = new string[vertCount];

            for (int i = 0; i < vertCount; i++)
            {
                for (int j = 0; j < edgesCount; j++)
                {
                    if (incidenceMatrix[i, j] == 1)
                    {
                        for (int k = 0; k < vertCount; k++)
                        {
                            if (k != i)
                            {
                                if (incidenceMatrix[k, j] == 1)
                                {
                                    test.Add(new testEdge(i, k, false));
                                }
                                if (incidenceMatrix[k, j] == -1)
                                {
                                    test.Add(new testEdge(i, k, true));
                                }
                            }
                        }
                    }
                }
            }
            DeleteEdges();
            for (int i = 0; i < test.Count; i++)
            {
                strMas[test[i].TransmitterVertex] += Convert.ToString(test[i].ReceiverVertex)+',';
                if (test[i].ReceiverDirection == 1)
                {
                    strMas[test[i].ReceiverVertex] += Convert.ToString(test[i].TransmitterVertex)+',';
                }
            }
            for (int i = 0; i < vertCount; i++)
            {
                strMas[i] = strMas[i].Remove(strMas[i].Length-1);
                richTextBox1.Text += Convert.ToString(i) + ':' + strMas[i] + '\n';
            }
        }
        public void EdgesList()//вывод списка ребер в текстбокс
        {
            for (int i = 0; i < test.Count; i++)
            {
                richTextBox5.Text += i + ":" + test[i].TransmitterVertex + test[i].ReceiverVertex + "  " + test[i].ReceiverDirection + "\r\n";
            }
        }     
        private void button1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text != "")
            {
                richTextBox2.Clear();
                richTextBox3.Clear();
                richTextBox4.Clear();
                richTextBox5.Clear();

                SetMatrixSize();
                FillVertexesMatrix();
                FillEdgesMatrix();
                FillIncedenceMatrix();
                EdgesList();
            }
            else
            {
                if (richTextBox2.Text != "")
                {
                    richTextBox3.Clear();
                    richTextBox4.Clear();
                    richTextBox5.Clear();

                    SetVertexesMatrix();
                    FillEdgeStruct();
                    FillEdgesMatrix();
                    FillIncedenceMatrix();
                    FillAdjList();
                    EdgesList();
                }
                else {
                    if (richTextBox3.Text != "")
                    {
                        richTextBox4.Clear();
                        richTextBox5.Clear();

                        SetEdgesMatrix();
                        FillIncedenceMatrix();
                        FillAdjList();
                        SetMatrixSize();
                        FillVertexesMatrix();
                        EdgesList();
                    }
                    else
                    {
                        if (richTextBox4.Text != "")
                        {
                            richTextBox5.Clear();

                            SetIncidenceMatrix();
                            FillAdjList();
                            EdgesList();
                            FillVertexesMatrix();
                            FillEdgesMatrix();
                        }
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            richTextBox3.Clear();
            richTextBox4.Clear();
            richTextBox5.Clear();
        }
    }
}