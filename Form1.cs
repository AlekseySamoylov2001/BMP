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
            dataGridView1.Rows.Add("Распределение");
            dataGridView1.Rows.Add("Энтропия");
            dataGridView1.Rows.Add("Отн энтропия");
            dataGridView1.Rows.Add("Избыточность");
        }

        static double M_k(int[][] color, int k)
        {
            double sum = 0;

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    sum += Math.Pow(color[i][j], k);


            return sum / (color.Length * color[0].Length);
        }

        static double AnM_k(int[][] color, int k)
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

        static double U_2(int[][] color)
        {
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_2 - Math.Pow(m_1, 2);
        }

        static double U_3(int[][] color)
        {
            double m_3 = M_k(color, 3);
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_3 - 3 * m_1 * m_2 + 2 * Math.Pow(m_1, 3);
        }

        static double U_4(int[][] color)
        {
            double m_4 = M_k(color, 4);
            double m_3 = M_k(color, 3);
            double m_2 = M_k(color, 2);
            double m_1 = M_k(color, 1);

            return m_4 - 4 * m_1 * m_3 + 6 * Math.Pow(m_1, 2) * m_2 - 3 * Math.Pow(m_1, 4);
        }

        static double G_1(int[][] color)
        {
            double u_3 = U_3(color);
            double u_2 = U_2(color);

            return u_3 / Math.Pow(u_2, 1.5);
        }

        static double G_2(int[][] color)
        {
            double u_4 = U_4(color);
            double u_2 = U_2(color);

            return u_4 / Math.Pow(u_2, 2) - 3;
        }

        static double Entropy(int[][] color)
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


        static double RelativeEntropy(int[][] color, int h)
        {
            return Entropy(color) / Math.Log(h + 1, 2);
        }

        static double Redundancy(int[][] color, int h)
        {
            double entropy = Entropy(color);

            return entropy - entropy / Math.Log(h + 1, 2);
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

            var srcImage = new Bitmap(filePath);

            label1.Text = "Размер: " + srcImage.Width.ToString() + "x" + srcImage.Height.ToString() + "pi";
            label2.Text = "Кол-во пикселей: " + (srcImage.Width*srcImage.Height).ToString();

            label1.Visible = true;
            label2.Visible = true;

            Red = new int[srcImage.Height][];
            Green = new int[srcImage.Height][];
            Blue = new int[srcImage.Height][];

            int minR = 256, maxR = -1;
            int minG = 256, maxG = -1;
            int minB = 256, maxB = -1;

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

                    minR = ((int)srcPixel.R).CompareTo(minR) < 0 ? srcPixel.R : minR;
                    minG = ((int)srcPixel.G).CompareTo(minG) < 0 ? srcPixel.G : minG;
                    minB = ((int)srcPixel.B).CompareTo(minB) < 0 ? srcPixel.B : minB;
                    maxR = ((int)srcPixel.R).CompareTo(maxR) > 0 ? srcPixel.R : maxR;
                    maxG = ((int)srcPixel.G).CompareTo(maxG) > 0 ? srcPixel.G : maxG;
                    maxB = ((int)srcPixel.B).CompareTo(maxB) > 0 ? srcPixel.B : maxB;
                }
            }

            Bitmap img = new Bitmap(srcImage, new Size(308, 308));
            pictureBox1.Image = img;

            FillColumn(Red, 1, minR, maxR);
            FillColumn(Green, 2, minG, maxG);
            FillColumn(Blue, 3, minB, maxB);

            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
        }

        private void FillColumn(int[][] color, int column, int min, int max)
        {
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
            dataGridView1[column, 16].Value =
                Math.Abs(G_1(color) / Math.Sqrt(6 / color.Length * color[0].Length)) <= 3 &&
                Math.Abs(G_2(color) / Math.Sqrt(24 / color.Length * color[0].Length)) <= 3 ?
                "Нормальное" : "Не нормальное";
            dataGridView1[column, 17].Value = Math.Round(Entropy(color), 2);
            dataGridView1[column, 18].Value = Math.Round(RelativeEntropy(color, max - min), 2);
            dataGridView1[column, 19].Value = Math.Round(Redundancy(color, max - min), 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 gist1;
            gist1 = new Form2(Red);
            gist1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 gist2;
            gist2 = new Form2(Green);
            gist2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 gist3;
            gist3 = new Form2(Blue);
            gist3.Show();
        }
    }
}
