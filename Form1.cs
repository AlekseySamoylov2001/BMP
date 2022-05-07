using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMP1
{
    public partial class Form1 : Form
    {
        int[][] Red;
        int[][] Green;
        int[][] Blue;
        Bitmap srcImage;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.Rows.Add("Мин яркость");
            dataGridView1.Rows.Add("Макс яркость");
            dataGridView1.Rows.Add("Размах яркости");
            dataGridView1.Rows.Add("m1");
            dataGridView1.Rows.Add("m2");
            dataGridView1.Rows.Add("m3");
            dataGridView1.Rows.Add("m4");
            dataGridView1.Rows.Add("аналог m1");
            dataGridView1.Rows.Add("аналог m2");
            dataGridView1.Rows.Add("аналог m3");
            dataGridView1.Rows.Add("аналог m4");
            dataGridView1.Rows.Add("u2");
            dataGridView1.Rows.Add("u3");
            dataGridView1.Rows.Add("u4");
            dataGridView1.Rows.Add("g1");
            dataGridView1.Rows.Add("g2");
            dataGridView1.Rows.Add("K_g1");
            dataGridView1.Rows.Add("K_g2");
            dataGridView1.Rows.Add("Распределение");
            dataGridView1.Rows.Add("Энтропия");
            dataGridView1.Rows.Add("Отн энтропия");
            dataGridView1.Rows.Add("Избыточность");

            DefaultDataGrid();
        }

        private double M_k(int[][] color, int k)
        {
            double sum = 0;

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    sum += Math.Pow(color[i][j], k);


            return sum / (color.Length * color[0].Length);
        }

        private double AnM_k(int[][] color, int k)
        {
            int[] points = new int[256];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    points[color[i][j]]++;

            double sum = 0;

            for (int i = 0; i < 256; i++)
                sum += Math.Pow(i, k) * points[i];

            return sum / (color.Length * color[0].Length);
        }

        private double U_2(int[][] color)
        {
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_2 - Math.Pow(m_1, 2);
        }

        private double U_3(int[][] color)
        {
            double m_3 = M_k(color, 3);
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_3 - 3 * m_1 * m_2 + 2 * Math.Pow(m_1, 3);
        }

        private double U_4(int[][] color)
        {
            double m_4 = M_k(color, 4);
            double m_3 = M_k(color, 3);
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_4 - 4 * m_1 * m_3 + 6 * Math.Pow(m_1, 2) * m_2 - 3 * Math.Pow(m_1, 4);
        }

        private double G_1(int[][] color)
        {
            double u_3 = U_3(color);
            double u_2 = U_2(color);

            return u_3 / Math.Pow(u_2, 1.5);
        }

        private double G_2(int[][] color)
        {
            double u_4 = U_4(color);
            double u_2 = U_2(color);

            return u_4 / Math.Pow(u_2, 2) - 3;
        }

        private double Entropy(int[][] color)
        {
            int[] points = new int[256];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    points[color[i][j]]++;

            double entropy = 0;

            for (int i = 0; i < 256; i++)
                entropy += points[i] > 0 ? points[i] * Math.Log(points[i], 2) : 0;

            entropy = Math.Log(color.Length * color[0].Length, 2) - entropy / (color.Length * color[0].Length);

            return entropy;
        }

        private double RelativeEntropy(int[][] color, int h)
        {
            return Entropy(color) / Math.Log(h + 1, 2);
        }

        private double Redundancy(int[][] color, int h)
        {
            double entropy = Entropy(color);

            return entropy - entropy / Math.Log(h + 1, 2);
        }

        private void FillColumn(int[][] color, int column, int min, int max)
        {
            double K1 = Math.Abs(G_1(color) / Math.Sqrt((double)6 / (color.Length * color[0].Length)));
            double K2 = Math.Abs(G_2(color) / Math.Sqrt((double)24 / (color.Length * color[0].Length)));

            dataGridView1[column, 0].Value = min;
            dataGridView1[column, 1].Value = max;
            dataGridView1[column, 2].Value = max - min;
            dataGridView1[column, 3].Value = Math.Round(M_k(color, 1), 2);
            dataGridView1[column, 4].Value = Math.Round(M_k(color, 2), 2);
            dataGridView1[column, 5].Value = Math.Round(M_k(color, 3), 2);
            dataGridView1[column, 6].Value = Math.Round(M_k(color, 4), 2);
            dataGridView1[column, 7].Value = Math.Round(AnM_k(color, 1), 2);
            dataGridView1[column, 8].Value = Math.Round(AnM_k(color, 2), 2);
            dataGridView1[column, 9].Value = Math.Round(AnM_k(color, 3), 2);
            dataGridView1[column, 10].Value = Math.Round(AnM_k(color, 4), 2);
            dataGridView1[column, 11].Value = Math.Round(U_2(color), 2);
            dataGridView1[column, 12].Value = Math.Round(U_3(color), 2);
            dataGridView1[column, 13].Value = Math.Round(U_4(color), 2);
            dataGridView1[column, 14].Value = Math.Round(G_1(color), 2);
            dataGridView1[column, 15].Value = Math.Round(G_2(color), 2);
            dataGridView1[column, 16].Value = K1;
            dataGridView1[column, 17].Value = K2;
            dataGridView1[column, 18].Value = K1 <= 3 && K2 <= 3 ? "Нормальное" : "Не нормальное";
            dataGridView1[column, 19].Value = Math.Round(Entropy(color), 2);
            dataGridView1[column, 20].Value = Math.Round(RelativeEntropy(color, max - min), 2);
            dataGridView1[column, 21].Value = Math.Round(Redundancy(color, max - min), 2);
        }

        private int FillTabl()
        {
            if (srcImage != null)
            {
                int minR = 256, maxR = -1;
                int minG = 256, maxG = -1;
                int minB = 256, maxB = -1;

                for (var y = 0; y < srcImage.Height; y++)
                {
                    for (var x = 0; x < srcImage.Width; x++)
                    {
                        minR = Red[y][x].CompareTo(minR) < 0 ? Red[y][x] : minR;
                        minG = Green[y][x].CompareTo(minG) < 0 ? Green[y][x] : minG;
                        minB = Blue[y][x].CompareTo(minB) < 0 ? Blue[y][x] : minB;
                        maxR = Red[y][x].CompareTo(maxR) > 0 ? Red[y][x] : maxR;
                        maxG = Green[y][x].CompareTo(maxG) > 0 ? Green[y][x] : maxG;
                        maxB = Blue[y][x].CompareTo(maxB) > 0 ? Blue[y][x] : maxB;
                    }
                }

                FillColumn(Red, 1, minR, maxR);
                FillColumn(Green, 2, minG, maxG);
                FillColumn(Blue, 3, minB, maxB);

                button2.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;

                return 0;
            }
            else
            {
                MessageBox.Show("Изображение не выбрано");
                return -1;
            }

        }

        private void ClearTabl()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                    dataGridView1.Rows[i].Cells[j].Value = "";
        }

        private void DefaultDataGrid()
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            dataGridView2.Columns.Add("1", "1");
            dataGridView2.Columns.Add("2", "2");
            dataGridView2.Columns.Add("3", "3");
            dataGridView2.Rows.Add();
            dataGridView2.Rows.Add();

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
                dataGridView2.Rows[i].Height = 35;
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
                dataGridView2.Columns[i].Width = 35;

            dataGridView2.CurrentCell = null;
        }

        public void GridChange(DataGridView dataGridView)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            int rowSize = 105 / dataGridView.Rows.Count;
            int columnSize = 105 / dataGridView.Columns.Count;

            dataGridView2.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font(FontFamily.GenericSansSerif, rowSize / 2)
            };

            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                dataGridView2.Columns.Add(i.ToString(), "1");
                dataGridView2.Columns[i].Width = columnSize;
            }

            dataGridView2.Rows[0].Height = rowSize;

            for (int i = 0; i < dataGridView.Rows.Count - 1; i++)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Height = rowSize;
            }

            for (int i = 0; i < dataGridView.Rows.Count; i++)
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                    dataGridView2.Rows[i].Cells[j].Value = dataGridView.Rows[i].Cells[j].Value;

            dataGridView2.CurrentCell = null;
        }

        private int[][] PSchemeColor(int[][] color, int[][] h)
        {
            int[][] filteredColor = new int[color.Length][];

            for (int i = 0; i < color.Length; i++)
            {
                filteredColor[i] = new int[color[i].Length];

                for (int j = 0; j < color[i].Length; j++)
                    filteredColor[i][j] = color[i][j];
            }

            int sum = 0;

            for (int i = 0; i < h.Length; i++)
                for (int j = 0; j < h[i].Length; j++)
                    sum += h[i][j];

            if (sum == 0)
                sum = 1;

            for (int i = h.Length; i < color.Length - h.Length; i++)
            {
                for (int j = h[0].Length; j < color[i].Length - h[0].Length; j++)
                {
                    double value = 0;
                    int row = i - h.Length / 2;
                    int col = j - h[0].Length / 2;

                    for (int k = 0; k < h.Length; k++)
                        for (int l = 0; l < h[k].Length; l++)
                            value += color[row + k][col + l] * h[k][l];

                    filteredColor[i][j] = Convert.ToInt32(value / sum);
                }
            }

            return filteredColor;
        }

        private Bitmap PSchemeBitmap(int[][] h)
        {
            Bitmap bitmap = new Bitmap(Red[0].Length, Red.Length);

            int[][] filteredRed = PSchemeColor(Red, h);
            int[][] filteredGreen = PSchemeColor(Green, h);
            int[][] filteredBlue = PSchemeColor(Blue, h);

            Red = filteredRed;
            Green = filteredGreen;
            Blue = filteredBlue;

            for (int i = 0; i < Red[0].Length; i++)
                for (int j = 0; j < Red.Length; j++)
                    bitmap.SetPixel(i, j, Color.FromArgb(filteredRed[j][i], filteredGreen[j][i], filteredBlue[j][i]));

            return bitmap;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "";
                openFileDialog.Filter = "BMP files (*.BMP)|*.BMP";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    filePath = openFileDialog.FileName;
            }

            srcImage = new Bitmap(filePath);

            Bitmap img = new Bitmap(srcImage, new Size(300, 300));
            pictureBox1.Image = img;

            label1.Text = "Размер: " + srcImage.Width.ToString() + "x" + srcImage.Height.ToString() + "pi";

            Red = new int[srcImage.Height][];
            Green = new int[srcImage.Height][];
            Blue = new int[srcImage.Height][];

            for (var y = 0; y < srcImage.Height; y++)
            {
                Red[y] = new int[srcImage.Width];
                Green[y] = new int[srcImage.Width];
                Blue[y] = new int[srcImage.Width];

                for (var x = 0; x < srcImage.Width; x++)
                {
                    var srcPixel = srcImage.GetPixel(x, y);

                    Red[y][x] = srcPixel.R;
                    Green[y][x] = srcPixel.G;
                    Blue[y][x] = srcPixel.B;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 gist1 = new Form2(Red);
            gist1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 gist2 = new Form2(Green);
            gist2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 gist3 = new Form2(Blue);
            gist3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int[][] h = new int[dataGridView2.Rows.Count][];

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                h[i] = new int[dataGridView2.Columns.Count];

                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    h[i][j] = Convert.ToInt32(dataGridView2.Rows[i].Cells[j].Value);
            }
            //new Bitmap(PSchemeBitmap(h), new Size(300, 300));
            
            pictureBox1.Image = new Bitmap(PSchemeBitmap(h), new Size(300, 300));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(srcImage, new Size(300, 300));
            pictureBox1.Image = img;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "Расчитать данные")
            {
                if (FillTabl() == 0)
                    button7.Text = "Сбросить таблицу";
            }
            else
            {
                ClearTabl();
                button7.Text = "Расчитать данные";
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(comboBox3.SelectedIndex, 3 + comboBox2.SelectedIndex * 2);
            form3.Owner = this;
            form3.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefaultDataGrid();
        }
    }
}
