using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace graph_algorithms__full_ind_1_
{
    class GraphAlgorythms : Graph
    {
        ~GraphAlgorythms()
        {
        }

        public void TaskSolution(ToolStripComboBox task, Label resultText, RichTextBox matrixTextBox)
        {
            VertexCount = matrixTextBox.Lines.Length;
            FillMatrix(matrixTextBox, task);

            switch (task.SelectedIndex)
            {
                case 0:
                    NumberOfSpanningTrees(resultText);
                    break;
                case 1:
                    HamiltonianPath(resultText);
                    break;
                case 2:
                    ChromaticNum(resultText);
                    break;
                case 3:
                    LargestEdgeCover(resultText);
                    break;
                case 4:
                    MaxFlow(resultText);
                    break;
            }
        }

        // span trees
        public void NumberOfSpanningTrees(Label resultText)
        {
            int[,] kirchhoffMatrix = new int[VertexCount, VertexCount];
            CreateKirchhoffMatrix(ref kirchhoffMatrix);
            int det = FindDeterminant(kirchhoffMatrix, VertexCount - 1);
            resultText.Text = Convert.ToString(det);
            DataOutputToFile(resultText);
            resultText.Text = resultText.Text.Insert(0, "Число остовных деревьев: ");
        }
        public void CreateKirchhoffMatrix(ref int[,] kirchhoffMatrix) //создание матрицы Кирхгофа
        {
            kirchhoffMatrix = new int[VertexCount, VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                int temp = 0;
                for (int j = 0; j < VertexCount; j++)
                {
                    if (base.vertexesMatrix[i, j] == 1)
                    {
                        kirchhoffMatrix[i, j] = -1;
                        temp++;
                    }
                }
                kirchhoffMatrix[i, i] = temp;
            }
        }
        public void TransformMatrix(int[,] kirMatrix, int[,] minor, int row, int column, int size) //строит новую матрицу, удаляя i строку и j столбец исходной
        {
            int difi = 0, difj = 0;
            for (int i = 0; i < size - 1; i++)
            {
                if (i == row)
                {
                    difi = 1;
                }
                difj = 0;
                for (int j = 0; j < size - 1; j++)
                {
                    if (j == column)
                    {
                        difj = 1;
                    }
                    minor[i, j] = kirMatrix[i + difi, j + difj];
                }
            }
        }
        public int FindDeterminant(int[,] kirMatrix, int matrixSize) //поиск определителя матрицы Кирхгофа без i строки и j столбца
        {
            int sum = 0, a = 1;
            int[,] minor = new int[matrixSize, matrixSize];
            if (matrixSize > 0)
            {
                if (matrixSize == 1)
                {
                    return kirMatrix[0, 0];
                }
                if (matrixSize == 2)
                {
                    return kirMatrix[0, 0] * kirMatrix[1, 1] - kirMatrix[0, 1] * kirMatrix[1, 0];
                }
                if (matrixSize >= 2)
                {
                    for (int i = 0; i < matrixSize; i++)
                    {
                        TransformMatrix(kirMatrix, minor, i, 0, matrixSize);
                        sum += a * kirMatrix[i, 0] * FindDeterminant(minor, matrixSize - 1);
                        a = -a;
                    }
                }
            }
            else return 0;
            return sum;
        }

        // hamilton
        public void HamiltonianPath(Label resultText)
        {
            bool[] usedVertex = new bool[VertexCount];
            List<int> path = new List<int>(VertexCount);
            FindHamPath(VertexCount, base.vertexesMatrix, usedVertex, path, resultText);
            DataOutputToFile(resultText);
            resultText.Text = resultText.Text.Insert(0, "Гамильтонов путь:\n");
        }
        public bool HamiltonPathExistence(int vertex, int matrixSize, int[,] vertexMatrix, bool[] usedVertex, List<int> path)
        {
            path.Add(vertex);

            if (path.Count == matrixSize)
            {
                return true;
            }
            usedVertex[vertex] = true;
            for (int i = 0; i < matrixSize; i++)
            {
                if (!usedVertex[i] && vertexMatrix[vertex, i] == 1)
                {
                    if (HamiltonPathExistence(i, matrixSize, vertexMatrix, usedVertex, path))
                    {
                        return true;
                    }
                }
            }
            path.RemoveAt(path.Count - 1);
            usedVertex[vertex] = false;
            return false;
        }
        public void FindHamPath(int vertexCount, int[,] vertexMatrix, bool[] usedVertex, List<int> path, Label label)
        {
            bool flag = false;
            label.Text = "";
            for (int i = 0; i < vertexCount && !flag; i++)
            {
                if (HamiltonPathExistence(i, vertexCount, vertexMatrix, usedVertex, path))
                {
                    foreach (int item in path)
                    {
                        label.Text += Convert.ToString(item);

                        if (!item.Equals(path.Last()))
                        {
                            label.Text += "-";
                        }
                    }
                    flag = true;
                }
            }

            if (!flag || label.Text.Length == 1)
            {
                label.Text = "Гамильтонова пути не существует.";
            }
        }

        // chromatic number
        public void ChromaticNum(Label resultText)
        {
            int chromatic = FindChromaticNumber();
            resultText.Text = Convert.ToString(chromatic);
            DataOutputToFile(resultText);
            resultText.Text = resultText.Text.Insert(0, "Хроматическое число: ");
        }
        public int VertexDegree(int vertex)//возвращает степень вершины
        {
            int degree = 0;
            for (int j = 0; j < VertexCount; j++)
            {
                if (vertexesMatrix[vertex, j] == 1)
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
            for (int j = 0; j < VertexCount; j++)
            {
                if (vertexesMatrix[i, j] == 0 && degree < VertexDegree(j))
                {
                    vertex = j;
                    degree = VertexDegree(j);
                }
            }
            return vertex;
        }
        void DeleteVertex(int vertex)//удаление вершины
        {
            int[,] newMatrix = new int[VertexCount - 1, VertexCount - 1];
            int i1 = 0, j1 = 0;
            for (int k = 0; k < VertexCount; k++)
            {
                if (k != vertex)
                {
                    j1 = 0;
                    for (int j = 0; j < VertexCount; j++)
                    {
                        if (j != vertex)
                        {
                            newMatrix[i1, j1] = vertexesMatrix[k, j];
                            j1++;
                        }
                    }
                    i1++;
                }
            }
            vertexesMatrix = newMatrix;
            VertexCount--;
        }
        void AlterationOfRowsElemets(int vertex1, int vertex2)//логическое сложение элементов строк
        {
            for (int j = 0; j < VertexCount; j++)
            {
                vertexesMatrix[vertex1, j] = vertexesMatrix[vertex1, j] | vertexesMatrix[vertex2, j];
            }
            DeleteVertex(vertex2);
        }
        public int FindChromaticNumber()//поиск хроматического числа
        {
            int chrom = 0;
            for (int i = 0; i < VertexCount; i++)
            {
                while (VertexDegree(i) != VertexCount)
                {
                    AlterationOfRowsElemets(i, MaxDegreeVertex(i));
                }
                if (VertexDegree(i) == VertexCount)
                {
                    DeleteVertex(i--);
                    chrom++;
                }
            }
            if (VertexCount == 1)//осталась последняя неокрашенная вершина
            {
                return chrom + 1;
            }
            else return chrom;
        }

        //edge cover
        public void LargestEdgeCover(Label resultText)
        {
            bool[] usedVertex = new bool[VertexCount];
            FindLargestEdgeCover(usedVertex);
            OutputLabelResult(resultText);
            DataOutputToFile(resultText);
            resultText.Text = resultText.Text.Insert(0, "Наибольшее реберное покрытие:\n");
        }
        int VertexNumberOfMinimumDegree(bool[] used)
        {
            int min = vertexCount;
            int num = 0;

            for (int i = 0; i < vertexCount; i++)
            {
                if (VertexDegree(i) < min && !used[i])
                {
                    min = VertexDegree(i);
                    num = i;
                }
            }
            return num;
        }
        int FindMinAdjVertex(int vertex)
        {
            int min = vertexCount;
            int num = -1;
            for (int i = 0; i < vertexCount; i++)
            {
                if (vertexesMatrix[vertex, i] == 1 && VertexDegree(i) < min && VertexDegree(i) > 1)
                {
                    min = VertexDegree(i);
                    num = i;
                }
            }
            return num;
        }
        void FindLargestEdgeCover(bool[] used)
        {
            int minVertex = VertexNumberOfMinimumDegree(used);
            int minVertexDegree = VertexDegree(minVertex);
            int minAdjVert;
            while (!used[minVertex])
            {
                used[minVertex] = true;
                if (minVertexDegree != 1)
                {
                    for (int i = 0; i < minVertexDegree; i++)
                    {
                        minAdjVert = FindMinAdjVertex(minVertex);

                        if (minVertexDegree != 1 && minAdjVert != -1)
                        {
                            SetEdge(0, minVertex, minAdjVert);
                            SetEdge(0, minAdjVert, minVertex);
                            minVertexDegree = VertexDegree(minVertex);
                        }
                    }
                }
                minVertex = VertexNumberOfMinimumDegree(used);
                minVertexDegree = VertexDegree(minVertex);
            }

        }
        void OutputLabelResult(Label label)
        {
            label.Text = "";
            for (int i = 0; i < vertexCount; i++)
                for (int j = i; j < vertexCount; j++)
                {
                    if (vertexesMatrix[i, j] == 1)
                    {
                        label.Text += Convert.ToString(i) + "-" + Convert.ToString(j) + ",";
                    }
                }
            label.Text = label.Text.Remove(label.Text.Length - 1);
        }


        //max flow
        public void MaxFlow(Label resultText)
        {
            int source = AddSource(vertexesMatrix);
            int sink = AddSink(vertexesMatrix);
            int[,] flowMatrix = new int[VertexCount, VertexCount]; // текущий поток сети
            int[] flowInVertex = new int[VertexCount]; // поток из начальной вершины
            int[] vertexParent = new int[VertexCount]; // массив предков вершин
            bool[] usedVertex = new bool[VertexCount];
            int maxFlow = FindMaxFlow(source, sink, usedVertex, flowInVertex, vertexParent, vertexesMatrix, flowMatrix);
            resultText.Text = Convert.ToString(maxFlow);
            DataOutputToFile(resultText);
            resultText.Text = resultText.Text.Insert(0, "Максимальный поток: ");
        }
        void SetMasValue(bool[] used, int[] flowInVertex, int[] vertexParent)
        {
            for (int i = 1; i < VertexCount; i++)
            {
                used[i] = false;
                flowInVertex[i] = 0;
                vertexParent[i] = 0;
            }
        }
        bool CheckSource(int vert, int[,] vertexesMatrix)
        {
            for (int i = 0; i < VertexCount - 1; i++)
            {
                if (vertexesMatrix[i, vert] != 0)
                    return false;
            }
            return true;
        }
        bool CheckSink(int vert, int[,] vertexesMatrix)
        {
            for (int j = 0; j < VertexCount - 1; j++)
            {
                if (vertexesMatrix[vert, j] != 0)
                    return false;
            }
            return true;
        }
        int SourceCapacity(int vertex, int[,] tempMatr)
        {
            int capacity = 0;
            for (int j = 0; j < VertexCount; j++)
            {
                capacity += tempMatr[vertex, j];
            }
            return capacity;
        }
        int SinkCapacity(int vertex, int[,] temp)
        {
            int capacity = 0;
            for (int i = 0; i < VertexCount; i++)
            {
                capacity += temp[i, vertex];
            }
            return capacity;
        }
        int AddSource(int[,] vertexesMatrix)
        {
            int[,] tempMatrix = new int[++VertexCount, VertexCount];
            int source = VertexCount - 1;
            for (int i = 0; i < VertexCount - 1; i++)
            {
                for (int j = 0; j < VertexCount - 1; j++)
                {
                    tempMatrix[i, j] = vertexesMatrix[i, j];

                    if (CheckSource(j, vertexesMatrix))
                    {
                        tempMatrix[VertexCount - 1, j] = SourceCapacity(j, tempMatrix);
                    }
                }
            }
            SetMatrix(tempMatrix);
            return source;
        }
        int AddSink(int[,] vertexesMatrix)
        {
            int[,] tempMatrix = new int[++VertexCount, VertexCount];
            int sink = VertexCount - 1;
            for (int i = 0; i < VertexCount - 1; i++)
            {
                for (int j = 0; j < VertexCount - 1; j++)
                {
                    tempMatrix[i, j] = vertexesMatrix[i, j];
                }
                if (CheckSink(i, vertexesMatrix))
                {
                    tempMatrix[i, VertexCount - 1] = SinkCapacity(i, tempMatrix);
                }

            }
            SetMatrix(tempMatrix);
            return sink;
        }
        bool BFS(int source, int sink, bool[] used, int[] flowInVertex, int[] vertexParent, int[,] capacityMatrix, int[,] flowMatrix)
        {
            SetMasValue(used, flowInVertex, vertexParent);
            Queue<int> Q = new Queue<int>();
            used[source] = true;
            vertexParent[source] = source;
            flowInVertex[source] = int.MaxValue;

            Q.Enqueue(source);
            while (!used[sink] && Q.Count() != 0)
            {
                int u = Q.Peek(); Q.Dequeue();
                for (int v = 0; v < VertexCount; v++)
                    if (!used[v] && (capacityMatrix[u, v] - flowMatrix[u, v] > 0))
                    {
                        flowInVertex[v] = Math.Min(flowInVertex[u], capacityMatrix[u, v] - flowMatrix[u, v]);
                        used[v] = true;
                        vertexParent[v] = u;
                        Q.Enqueue(v);
                    }
            }

            return used[sink];
        }
        // Алгоритм Форда-Фалкерсона
        int FindMaxFlow(int source, int sink, bool[] used, int[] flowInVertex, int[] vertexParent, int[,] vertexesMatrix, int[,] flowMatrix)
        {
            int u, v, flow = 0;

            while (BFS(source, sink, used, flowInVertex, vertexParent, vertexesMatrix, flowMatrix))
            {
                int add = flowInVertex[sink];

                v = sink; u = vertexParent[v];
                while (v != source)
                {
                    flowMatrix[u, v] += add;
                    flowMatrix[v, u] -= add;
                    v = u; u = vertexParent[v];
                }
                flow += add;
            }

            return flow;
        }

        //other alg
        public void TextboxDataEntry(RichTextBox richTextBox, ToolStripTextBox textBox)
        {
            StreamReader ifstream = new StreamReader(textBox.Text, System.Text.Encoding.Default);
            richTextBox.Text += ifstream.ReadToEnd();
            ifstream.Close();
        }
        public void DataOutputToFile(Label label)
        {
            StreamWriter ffstream = new StreamWriter("result.txt", false, System.Text.Encoding.Default);
            ffstream.Write(label.Text);
            ffstream.Close();
        }

        public bool MatrixIsCorrect(RichTextBox richTextBox, ToolStripComboBox task)
        {
            if (richTextBox.Text.Length == 0)
            {
                return false;
            }

            if (task.SelectedIndex != 4)
            {
                int check = richTextBox.Lines[0].Length;

                if (check != richTextBox.Lines.Length)
                {
                    return false;
                }

                for (int i = 0; i < richTextBox.Lines.Length; i++)
                {
                    if (richTextBox.Lines[i].Length != check)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < richTextBox.Lines.Length; i++)
                {
                    if (richTextBox.Text[i] != '0' && richTextBox.Text[i] != '1' && richTextBox.Text[i] != '\n')
                    {
                        return false;
                    }
                }
            }
            else
            {
                int check = richTextBox.Lines[0].Length + 1;
                if (check != richTextBox.Lines.Length * 2)
                {
                    return false;
                }

                for (int i = 0; i < richTextBox.Lines.Length; i++)
                {
                    if (richTextBox.Lines[i].Length + 1 != check)
                    {
                        return false;
                    }
                }

                for (int i = 0; i < richTextBox.Lines.Length; i++)
                {
                    if (!(richTextBox.Text[i] >= '0' && richTextBox.Text[i] <= '9') && richTextBox.Text[i] != ',' && richTextBox.Text[i] != '\n')
                    {
                        return false;
                    }
                }
            }
            return true;

        }
    }
}