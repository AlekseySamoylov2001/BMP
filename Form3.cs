using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BMP1
{
    public partial class Form3 : Form
    {
        List<DataGridView> dataGrids = new List<DataGridView>();
        Form1 main;

        public Form3(int mode, int aperture)
        {
            
            InitializeComponent();

            switch (mode)
            {
                case 0:
                    switch (aperture)
                    {
                        case 3:
                            Size = new Size(380, 240);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1 },
                                new int[] { 1, 1, 1 },
                                new int[] { 1, 1, 1 } },
                                aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1 },
                                new int[] { 1, 2, 1 },
                                new int[] { 1, 1, 1 } },
                                aperture, 1));

                            break;

                        case 5:
                            Size = new Size(780, 320);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1} },
                               aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1, 1, 1 },
                                new int[] { 1, 2, 2, 2, 1 },
                                new int[] { 1, 2, 4, 2, 1 },
                                new int[] { 1, 2, 2, 2, 1 },
                                new int[] { 1, 1, 1, 1, 1} },
                              aperture, 1)); 
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 0, 1, 1, 1, 0 },
                                new int[] { 1, 1, 2, 1, 1 },
                                new int[] { 1, 2, 4, 2, 1 },
                                new int[] { 1, 1, 2, 1, 1 },
                                new int[] { 0, 1, 1, 1, 0} },
                              aperture, 2));

                            break;

                        case 7:
                            Size = new Size(700, 400);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 } },
                               aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1, 1, 1, 1, 1 },
                                new int[] { 1, 2, 2, 2, 2, 2, 1 },
                                new int[] { 1, 2, 4, 4, 4, 2, 1 },
                                new int[] { 1, 2, 4, 8, 4, 2, 1 },
                                new int[] { 1, 2, 4, 4, 4, 2, 1 },
                                new int[] { 1, 2, 2, 2, 2, 2, 1 },
                                new int[] { 1, 1, 1, 1, 1, 1, 1 } },
                               aperture, 1));

                            break;

                        default:
                            break;
                    }
                    break;

                case 1:
                    switch (aperture)
                    {
                        case 3:
                            Size = new Size(380, 240);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 2, 1 },
                                new int[] { 2, 4, 2 },
                                new int[] { 1, 2, 1 } },
                                aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 2, 1, 2 },
                                new int[] { 1, 4, 1 },
                                new int[] { 2, 1, 2 } },
                                aperture, 1));

                            for (int i = 0; i < dataGrids.Count; i++)
                                Controls.Add(dataGrids[i]);

                            break;
                        default:
                            break;
                    }
                    break;

                case 2:
                    switch (aperture)
                    {
                        case 3:
                            Size = new Size(540, 240);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 0, -1, 0 },
                                new int[] { -1, 5, -1 },
                                new int[] { 0, -1, 0 } },
                                aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { -1, -1, -1 },
                                new int[] { -1, 9, -1 },
                                new int[] { -1, -1, -1 } },
                                aperture, 1));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, -2, 1 },
                                new int[] { -2, 5, -2 },
                                new int[] { 1, -2, 1 } },
                               aperture, 2));

                            for (int i = 0; i < dataGrids.Count; i++)
                                Controls.Add(dataGrids[i]);

                            break;
                        default:
                            break;
                    }
                    break;

                case 3:
                    switch (aperture)
                    {
                        case 3:
                            Size = new Size(1340, 240);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1 },
                                new int[] { 1, -2, 1 },
                                new int[] { -1, -1, -1 } },
                                aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1 },
                                new int[] { -1, -2, 1 },
                                new int[] { -1, -1, 1 } },
                                aperture, 1));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { -1, 1, 1 },
                                new int[] { -1, -2, 1 },
                                new int[] { -1, 1, 1 } },
                               aperture, 2));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { -1, -1, 1 },
                                new int[] { -1, -2, 1 },
                                new int[] { 1, 1, 1 } },
                               aperture, 3));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { -1, -1, -1 },
                                new int[] { 1, -2, 1 },
                                new int[] { 1, 1, 1 } },
                               aperture, 4));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, -1, -1 },
                                new int[] { 1, -2, -1 },
                                new int[] { 1, 1, 1 } },
                               aperture, 5));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, -1 },
                                new int[] { 1, -2, -1 },
                                new int[] { 1, 1, -1 } },
                               aperture, 6));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, 1, 1 },
                                new int[] { 1, -2, -1 },
                                new int[] { 1, -1, -1 } },
                               aperture, 7));

                            for (int i = 0; i < dataGrids.Count; i++)
                                Controls.Add(dataGrids[i]);

                            break;
                        default:
                            break;
                    }
                    break;

                case 4:
                    switch (aperture)
                    {
                        case 3:
                            Size = new Size(540, 240);

                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 0, -1, 0 },
                                new int[] { -1, 5, -1 },
                                new int[] { 0, -1, 0 } },
                                aperture, 0));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { -1, -1, -1 },
                                new int[] { -1, 9, -1 },
                                new int[] { -1, -1, -1 } },
                                aperture, 1));
                            dataGrids.Add(CreateMatrix(new int[][] {
                                new int[] { 1, -2, 1 },
                                new int[] { -2, 5, -2 },
                                new int[] { 1, -2, 1 } },
                               aperture, 2));

                            for (int i = 0; i < dataGrids.Count; i++)
                                Controls.Add(dataGrids[i]);

                            break;
                        default:
                            break;
                    }
                    break;

                default:
                    break;
            }

            for (int i = 0; i < dataGrids.Count; i++)
                Controls.Add(dataGrids[i]);
        }

        private DataGridView CreateMatrix(int[][] matrix, int aperture, int number)
        {
            int gridSize = aperture * 40;

            DataGridView dataGrid = new DataGridView();

            dataGrid.TabStop = false;
            dataGrid.Size = new Size(gridSize, gridSize);
            dataGrid.ScrollBars = 0;
            dataGrid.ReadOnly = true;
            dataGrid.Location = new Point(40 + number * (gridSize + 40), 40);
            dataGrid.ColumnHeadersVisible = false;
            dataGrid.RowHeadersVisible = false;
            dataGrid.AllowUserToResizeColumns = false;
            dataGrid.AllowUserToResizeRows = false;
            dataGrid.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font(FontFamily.GenericSansSerif, 14)
            };

            for (int i = 0; i < aperture; i++)
            {
                dataGrid.Columns.Add("column" + i.ToString(), i.ToString());
                dataGrid.Columns[i].Width = 40;
            }

            dataGrid.Rows[0].Height = 40;

            for (int i = 0; i < aperture - 1; i++)
            {
                dataGrid.Rows.Add();
                dataGrid.Rows[i].Height = 40;
            }

            for (int i = 0; i < dataGrid.Rows.Count; i++)
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                    dataGrid.Rows[i].Cells[j].Value = matrix[i][j];

            return dataGrid;
        }

        private void Form3_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGrids.Count; i++)
            {
                (Controls[Controls.GetChildIndex(dataGrids[i])] as DataGridView).CurrentCell = null;
                dataGrids[i].SelectionChanged += SelectionChanged;
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            main = Owner as Form1;

            if (main != null)
                main.GridChange(sender as DataGridView);

            Close();
        }
    }
}
