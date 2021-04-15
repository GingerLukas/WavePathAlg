using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WavePathAlg
{
    public partial class Form1 : Form
    {
        private Grid _grid = new Grid(16, 16);
        public Form1()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            _grid.SetBlock(0,0,Grid.EBlockType.Start);
            _grid.SetBlock(15,15,Grid.EBlockType.Finish);

            this.Controls.Add(_grid);
            _grid.Dock = DockStyle.Fill;

            _grid.Origin = new Point(50, 50);

            _toolResetAll.Click += ToolResetAllOnClick;
            _toolResetPath.Click += ToolResetPathOnClick;
            _toolSearch.Click += ToolSearchOnClick;
            _toolResetWalls.Click += ToolResetWallsOnClick;
        }

        private void ToolResetWallsOnClick(object? sender, EventArgs e)
        {
            _grid.ResetWalls();
        }

        private void ToolSearchOnClick(object? sender, EventArgs e)
        {
            _grid.StartSearch(new Point(0, 0), new Point(15, 15));
        }

        private void ToolResetPathOnClick(object? sender, EventArgs e)
        {
            _grid.ResetPath();
        }

        private void ToolResetAllOnClick(object? sender, EventArgs e)
        {
            _grid.ResetAll();
        }
    }
}
