using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace graph_algorithms__full_ind_1_
{
    class GraphAlgorythms:Graph
    {
        ~GraphAlgorythms()
        {
        }
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

        public bool MatrixIsCorrect(RichTextBox richTextBox, int i)
        {
            switch (i)
            {
                case 1:
                    if (richTextBox.Text == "" || richTextBox.TextLength == 1 ||
                richTextBox.TextLength != VertexCount * VertexCount || HaveSymbols(richTextBox))
                    {
                        return false;
                    }
                    return true;
                case 4:
                    if (richTextBox.Text != "" || richTextBox.TextLength == VertexCount * VertexCount)
                    {
                        for (int j = 0; j < richTextBox.TextLength; j++)
                        {
                            if (richTextBox.Text[j] >= '0' && richTextBox.Text[j] <= '9' || richTextBox.Text[j] == ',' || richTextBox.Text[j] == '\n')
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                default:
                    if (richTextBox.Text == "" || richTextBox.TextLength == 1 ||
                 richTextBox.TextLength != VertexCount * VertexCount || !SymmetricMatr(richTextBox) || HaveSymbols(richTextBox))
                    {
                        return false;
                    }
                    return true;
            }

        }
        public bool SymmetricMatr(RichTextBox richTextBox)
        {
            string str = richTextBox.Text;
            str = str.Replace("\n", "");

            for (int i = 0; i < str.Length; i += VertexCount + 1)
            {
                if (str[i] != '0')
                {
                    return false;
                }
            }
            return true;
        }
        public bool HaveSymbols(RichTextBox richTextBox)
        {
            for (int i = 0; i < richTextBox.TextLength; i++)
            {
                if (richTextBox.Text[i] != '0' && richTextBox.Text[i] != '1' && richTextBox.Text[i] != '\n')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
