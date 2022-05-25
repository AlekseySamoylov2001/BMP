using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

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

        private void GetColors()
        {
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


        private int FillTabl()
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

            void FillColumn(int[][] color, int column, int min, int max)
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

            FillColumn(Red, 1, minR, maxR);
            FillColumn(Green, 2, minG, maxG);
            FillColumn(Blue, 3, minB, maxB);


            return 0;
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

        private void ChangeColor(ref int[][] color, ref int[][] filteredColor, ref int[][] h, int height, int width)
        {
            int sum = 0;
            int borderH = h.Length / 2;
            int borderW = h[0].Length / 2;
            int halfH = height / 2;
            int halfW = width / 2;

            for (int i = 0; i < h.Length; i++)
                for (int j = 0; j < h[i].Length; j++)
                    sum += h[i][j];

            if (sum == 0)
                sum = 1;

            for (int i = borderH; i < color.Length + borderH - height; i++)
            {
                for (int j = borderW; j < color[0].Length + borderW - width; j++)
                {
                    double value = 0;

                    int row = i - borderH + halfH;
                    int col = j - borderW + halfW;

                    for (int k = 0; k < h.Length; k++)
                        for (int l = 0; l < h[k].Length; l++)
                            value += filteredColor[row + k - halfH][col + l - halfW] * h[k][l];

                    int result = Convert.ToInt32(value / sum);

                    color[row][col] = result > 0 ? result < 256 ? result : 255 : 0;
                }
            }
        }

        private void PSchemeColor(ref int[][] color, ref int[][] h)
        {
            int[][] filteredColor = new int[color.Length][];

            for (int i = 0; i < color.Length; i++)
            {
                filteredColor[i] = new int[color[i].Length];

                for (int j = 0; j < color[i].Length; j++)
                    filteredColor[i][j] = color[i][j];
            }

            ChangeColor(ref color, ref filteredColor, ref h, h.Length, h[0].Length);
        }

        private Bitmap PSchemeBitmap(ref int[][] h)
        {
            Bitmap bitmap = new Bitmap(Red[0].Length, Red.Length);

            PSchemeColor(ref Red, ref h);
            PSchemeColor(ref Green, ref h);
            PSchemeColor(ref Blue, ref h);

            for (int i = 0; i < Red[0].Length; i++)
                for (int j = 0; j < Red.Length; j++)
                    bitmap.SetPixel(i, j, Color.FromArgb(Red[j][i], Green[j][i], Blue[j][i]));

            return bitmap;
        }

        private void SSchemeColor(ref int[][] color, ref int[][] h)
        {
            int bigHeight = color.Length + h.Length - 1;
            int bigWidth = color[0].Length + h[0].Length - 1;
            int borderH = h.Length / 2;
            int borderW = h[0].Length / 2;

            int[][] filteredColor = new int[bigHeight][];

            for (int i = 0; i < bigHeight; i++)
                filteredColor[i] = new int[bigWidth];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    filteredColor[borderH + i][borderW + j] = color[i][j];

            ChangeColor(ref color, ref filteredColor, ref h, 0, 0);
        }

        private Bitmap SSchemeBitmap(ref int[][] h)
        {
            Bitmap bitmap = new Bitmap(Red[0].Length, Red.Length);

            SSchemeColor(ref Red, ref h);
            SSchemeColor(ref Green, ref h);
            SSchemeColor(ref Blue, ref h);

            for (int i = 0; i < Red[0].Length; i++)
                for (int j = 0; j < Red.Length; j++)
                    bitmap.SetPixel(i, j, Color.FromArgb(Red[j][i], Green[j][i], Blue[j][i]));

            return bitmap;
        }

        private void TSchemeColor(ref int[][] color, ref int[][] h)
        {
            int bigHeight = color.Length + h.Length - 1;
            int bigWidth = color[0].Length + h[0].Length - 1;
            int borderH = h.Length / 2;
            int borderW = h[0].Length / 2;

            int[][] filteredColor = new int[bigHeight][];

            for (int i = 0; i < bigHeight; i++)
                filteredColor[i] = new int[bigWidth];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    filteredColor[borderH + i][borderW + j] = color[i][j];

            int tempSize = color.Length * color[0].Length;
            int[] temp = new int[tempSize];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    temp[i * color[0].Length + j] = color[i][j];

            int index = tempSize - color[0].Length * borderH - borderW;

            for (int i = 0; i < bigHeight; i++)
            {
                for (int j = 0; j < bigWidth; j++)
                {
                    filteredColor[i][j] = temp[index % tempSize];
                    index++;
                }

                index -= 2 * borderW;
            }

            ChangeColor(ref color, ref filteredColor, ref h, 0, 0);
        }

        private Bitmap TSchemeBitmap(ref int[][] h)
        {
            Bitmap bitmap = new Bitmap(Red[0].Length, Red.Length);

            TSchemeColor(ref Red, ref h);
            TSchemeColor(ref Green, ref h);
            TSchemeColor(ref Blue, ref h);

            for (int i = 0; i < Red[0].Length; i++)
                for (int j = 0; j < Red.Length; j++)
                    bitmap.SetPixel(i, j, Color.FromArgb(Red[j][i], Green[j][i], Blue[j][i]));

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

            try
            {
                srcImage = new Bitmap(filePath);

                Bitmap img = new Bitmap(srcImage, new Size(300, 300));
                pictureBox1.Image = img;

                label1.Text = "Размер: " + srcImage.Width.ToString() + "x" + srcImage.Height.ToString() + "pi";

                Red = new int[srcImage.Height][];
                Green = new int[srcImage.Height][];
                Blue = new int[srcImage.Height][];

                GetColors();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 gist1 = new Form2(Red, "Red histogram");
            gist1.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 gist2 = new Form2(Green, "Green histogram");
            gist2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 gist3 = new Form2(Blue, "Blue histogram");
            gist3.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    int[][] Middle = new int[srcImage.Height][];

                    for (int i = 0; i < srcImage.Height; i++)
                    {
                        Middle[i] = new int[srcImage.Width];

                        for (int j = 0; j < srcImage.Width; j++)
                        {
                            Middle[i][j] = Convert.ToInt32(
                                0.2125 * Convert.ToDouble(Red[i][j]) +
                                0.7154 * Convert.ToDouble(Green[i][j]) +
                                0.0721 * Convert.ToDouble(Blue[i][j]));
                        }
                    }
                    Bitmap bitmap = new Bitmap(srcImage.Width, srcImage.Height);

                    for (int i = 0; i < srcImage.Width; i++)
                        for (int j = 0; j < srcImage.Height; j++)
                            bitmap.SetPixel(i, j, Color.FromArgb(Middle[j][i], Middle[j][i], Middle[j][i]));

                    Bitmap img = new Bitmap(bitmap, new Size(300, 300));
                    pictureBox1.Image = img;
                    break;
                default:
                    break;
            }

            int[][] h = new int[dataGridView2.Rows.Count][];

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                h[i] = new int[dataGridView2.Columns.Count];

                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    h[i][j] = Convert.ToInt32(dataGridView2.Rows[i].Cells[j].Value);
            }

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    pictureBox1.Image = new Bitmap(PSchemeBitmap(ref h), new Size(300, 300));
                    break;

                case 1:
                    pictureBox1.Image = new Bitmap(SSchemeBitmap(ref h), new Size(300, 300));
                    break;

                case 2:
                    pictureBox1.Image = new Bitmap(TSchemeBitmap(ref h), new Size(300, 300));
                    break;

                default:
                    break;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GetColors();
            Bitmap img = new Bitmap(srcImage, new Size(300, 300));
            pictureBox1.Image = img;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text == "Расчитать данные")
            {
                if (srcImage != null)
                {
                    FillTabl();

                    button7.Text = "Сбросить таблицу";

                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                }
                else
                    MessageBox.Show("Изображение не выбрано");
            }
            else
            {
                ClearTabl();
                button7.Text = "Расчитать данные";

                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(comboBox3.SelectedIndex - 1, 3 + comboBox2.SelectedIndex * 2);
            form3.Owner = this;
            form3.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DefaultDataGrid();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = "";
                saveFileDialog.Filter = "BMP files (*.BMP)|*.BMP";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap = new Bitmap(Red[0].Length, Red.Length);

                    for (int i = 0; i < Red[0].Length; i++)
                        for (int j = 0; j < Red.Length; j++)
                            bitmap.SetPixel(i, j, Color.FromArgb(Red[j][i], Green[j][i], Blue[j][i]));

                    bitmap.Save(saveFileDialog.FileName);
                }
            }
        }



        private void button10_Click(object sender, EventArgs e)
        {
            BinaryWriter binaryWriter = new BinaryWriter(File.Open("test.dat", FileMode.OpenOrCreate));

            byte size = 4;

            binaryWriter.Write(srcImage.Height);
            binaryWriter.Write(srcImage.Width);
            binaryWriter.Write(size);

            void Compression(int y, int x)
            {
                bool[] result = new bool[size * size];

                int sizeY = srcImage.Height - y < size ? srcImage.Height - y : size;
                int sizeX = srcImage.Width - x < size ? srcImage.Width - x : size;

                int[][] Middle = new int[size][];

                for (int i = 0; i < size; i++)
                    Middle[i] = new int[size];

                for (int i = 0; i < sizeY; i++)
                    for (int j = 0; j < sizeX; j++)
                        Middle[i][j] = Convert.ToInt32(
                            0.2125 * Convert.ToDouble(Red[y + i][x + j]) +
                            0.7154 * Convert.ToDouble(Green[y + i][x + j]) +
                            0.0721 * Convert.ToDouble(Blue[y + i][x + j]));


                double C = 0;
                double E = 0;

                for (int i = 0; i < sizeY; i++)
                    for (int j = 0; j < sizeX; j++)
                    {
                        C += Middle[i][j];
                        E += Math.Pow(Middle[i][j], 2);
                    }

                C /= sizeY * sizeX;
                E /= sizeY * sizeX;

                double delta = Math.Sqrt(E - Math.Pow(C, 2));

                int q = 0;

                for (int i = 0; i < sizeY; i++)
                    for (int j = 0; j < sizeX; j++)
                        q += Middle[i][j] > C ? 1 : 0;

                byte a;
                byte b;

                int temp = Convert.ToInt32(C - Convert.ToByte(delta * Math.Sqrt(Convert.ToDouble(q) / (sizeY * sizeX - q))));

                if (temp <= 0)
                    a = 0;
                else
                    a = Convert.ToByte(temp);

                if (q == 0)
                    b = Convert.ToByte(C);
                else
                    b = Convert.ToByte(C + q == 0 ? 0 : Convert.ToByte(delta * Math.Sqrt((sizeY * sizeX - q) / Convert.ToDouble(q))));

                binaryWriter.Write(a);
                binaryWriter.Write(b);

                ushort number = 0;

                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        number += Convert.ToUInt16(Math.Pow(2, i * size + j));

                binaryWriter.Write(number);
            }

            for (int i = 0; i < srcImage.Height; i += size)
                for (int j = 0; j < srcImage.Width; j += size)
                    Compression(i, j);

            binaryWriter.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BinaryReader binaryReader = new BinaryReader(File.Open("test.dat", FileMode.OpenOrCreate));

            int height = binaryReader.ReadInt32();
            int width = binaryReader.ReadInt32();
            byte size = binaryReader.ReadByte();

            byte[][] color = new byte[height][];

            for (int i = 0; i < height; i++)
                color[i] = new byte[width];


            void Uncompression(int y, int x)
            {
                byte a = binaryReader.ReadByte();
                byte b = binaryReader.ReadByte();

                ushort number = binaryReader.ReadUInt16();

                int sizeY = height - y < size ? height - y : size;
                int sizeX = width - x < size ? width - x : size;

                byte[] ab = new byte[size * size];

                for (int i = ab.Length - 1; i >= 0; i--)
                {
                    ab[i] = Convert.ToByte(number % 2);
                    number /= 2;
                }

                for (int i = 0; i < sizeY; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        color[y + i][x + j] = ab[i * size + j] == 0 ? a : b;
                    }
                }
            }

            for (int i = 0; i < height; i += size)
                for (int j = 0; j < width; j += size)
                    Uncompression(i, j);

            binaryReader.Close();

            Bitmap bitmap = new Bitmap(width, height);

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    bitmap.SetPixel(i, j, Color.FromArgb(color[j][i], color[j][i], color[j][i]));

            Bitmap img = new Bitmap(bitmap, new Size(300, 300));
            pictureBox1.Image = img;
        }
    }
}