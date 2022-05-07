using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BMP1
{
    public partial class Form2 : Form
    {
        public int[][] color;
        public Form2(int[][] c)
        {
            InitializeComponent();

            color = c;

            //создаем элемент Chart
            Chart myChart = new Chart();
            //кладем его на форму и растягиваем на все окно.
            myChart.Parent = this;
            myChart.Dock = DockStyle.Fill;
            //добавляем в Chart область для рисования графиков, их может быть
            //много, поэтому даем ей имя.
            myChart.ChartAreas.Add(new ChartArea("Math functions"));
            //Создаем и настраиваем набор точек для рисования графика, в том
            //не забыв указать имя области на которой хотим отобразить этот
            //набор точек.
            Series mySeriesOfPoint = new Series("Gist");

            mySeriesOfPoint.ChartType = SeriesChartType.Column;
            mySeriesOfPoint.Palette = ChartColorPalette.Chocolate;
            mySeriesOfPoint.ChartArea = "Math functions";

            int[] points = new int[256];

            for (int i = 0; i < color.Length; i++)
                for (int j = 0; j < color[i].Length; j++)
                    points[color[i][j]]++;


            for (int x = 0; x <= 255; x++)
                mySeriesOfPoint.Points.AddXY(x, points[x]);

            //Добавляем созданный набор точек в Chart
            myChart.Series.Add(mySeriesOfPoint);
        }
    }
}
