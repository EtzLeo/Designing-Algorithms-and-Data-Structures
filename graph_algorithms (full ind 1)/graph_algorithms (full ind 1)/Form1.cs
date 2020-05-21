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

namespace graph_algorithms__full_ind_1_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new Size(620, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        RichTextBox matrixTextBox = new RichTextBox();
        Label textBoxTitle = new Label();
        Label resultText = new Label();
        Button resultButton = new Button();
        Button clearButton = new Button();

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;

            openFileDialog1.InitialDirectory = "С:\\";
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;

                Stream fileStream = openFileDialog1.OpenFile();

                StreamReader reader = new StreamReader(fileStream);
                fileContent = reader.ReadToEnd();

                toolStripTextBox1.Text = filePath;
                MessageBox.Show(fileContent, "File path:" + filePath, MessageBoxButtons.OK);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать задачу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                toolStripContainer1.ContentPanel.Controls.Add(matrixTextBox);
                matrixTextBox.Location = new Point(68, 36);
                matrixTextBox.Size = new Size(348, 282);

                toolStripContainer1.ContentPanel.Controls.Add(textBoxTitle);
                textBoxTitle.AutoSize = true;
                textBoxTitle.Text = "Матрица смежности вершин графа:";
                textBoxTitle.Location = new Point(64, 14);

                toolStripContainer1.ContentPanel.Controls.Add(resultButton);
                resultButton.Text = "OK";
                resultButton.Location = new Point(68, 344);
                resultButton.Size = new Size(110, 35);
                resultButton.BackColor = Color.DarkSeaGreen;
                resultButton.FlatStyle = FlatStyle.Flat;
                resultButton.Click += ResultButton_Click;

                toolStripContainer1.ContentPanel.Controls.Add(clearButton);
                clearButton.Text = "Clear";
                clearButton.Location = new Point(306, 344);
                clearButton.Size = new Size(110, 35);
                clearButton.BackColor = Color.IndianRed;
                clearButton.FlatStyle = FlatStyle.Flat;
                clearButton.Click += ClearButton_Click;

                toolStripContainer1.ContentPanel.Controls.Add(resultText);
                resultText.AutoSize = true;
                resultText.Text = "";
                resultText.ForeColor = Color.Maroon;
                resultText.Location = new Point(64, 422);
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            matrixTextBox.Text = "";
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
