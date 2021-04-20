using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WavePathAlg
{
    public partial class Grid : UserControl
    {
        private EBlockType[,] _blockTypes;
        private int[,] _blockDistances;
        private int _size;
        private int _blockSize;

        private Dictionary<int, Font> _lenToFont = new Dictionary<int, Font>();

        private Point _dragPoint;
        private bool _dragEnabled;
        
        public Point Origin { get; set; }

        private Grid()
        {
            InitializeComponent();

            _panel.SetProperty(nameof(DoubleBuffered), true);

            _panel.Paint += OnPanelPaint;
            _panel.MouseDown += OnMouseDown;
            _panel.MouseMove += OnMouseMove;
            _panel.MouseLeave += OnMouseLeave;
            _panel.MouseUp += OnMouseUp;
        }

        public Grid(int size, int blockSize) : this()
        {
            _size = size;
            _blockSize = blockSize;
            ResetAll();
        }

        public void StartSearch(Point start, Point end, int delay = 100)
        {
            ThreadPool.QueueUserWorkItem((state) => Search(start, end, delay));
        }

        public void Search(Point start, Point end,int delay)
        {
            Queue<Point> path = new Queue<Point>();
            path.Enqueue(start);

            Point result = Point.Empty;
            Point p;
            int count;
            int dist = 1;
            while ((count = path.Count) > 0)
            {
                while (count>0)
                {
                    p = path.Dequeue();
                    if (SearchAt(p.X, p.Y + 1, dist, path, out result) ||
                        SearchAt(p.X + 1, p.Y, dist, path, out result) ||
                        SearchAt(p.X, p.Y - 1, dist, path, out result) ||
                        SearchAt(p.X - 1, p.Y, dist, path, out result))
                    {
                        path.Clear();
                        break;
                    }


                    count--;
                }
                this.Invoke(Invalidate);
                Thread.Sleep(delay);

                dist++;
            }

            GetResultPath(result, delay);

        }

        private List<Point> GetResultPath(Point point,int delay)
        {
            List<Point> path = new List<Point>();
            while (true)
            {
                Point[] points = new[]
                {
                    new Point(point.X + 1, point.Y), new Point(point.X - 1, point.Y), new Point(point.X, point.Y + 1),
                    new Point(point.X, point.Y - 1)
                };
                int min = int.MaxValue;
                Point step = Point.Empty;
                for (int i = 0; i < 4; i++)
                {
                    int temp = GetValue(points[i]);
                    if (temp<min)
                    {
                        min = temp;
                        step = points[i];
                    }
                }

                if (_blockTypes[step.X,step.Y]==EBlockType.Start)
                {
                    break;
                }
                else
                {
                    _blockTypes[step.X, step.Y] = EBlockType.ReversePath;
                    this.Invoke(Invalidate);
                    Thread.Sleep(delay/3);

                }

                path.Add(step);
                point = step;
            }

            return path;
        }

        private int GetValue(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X >= _blockSize || p.Y >= _blockSize || _blockTypes[p.X,p.Y] == EBlockType.Wall)
            {
                return int.MaxValue;
            }

            return _blockDistances[p.X, p.Y];
        }

        private bool SearchAt(int x, int y, int dist, Queue<Point> queue, out Point result)    
        {
            if (x < 0 || y < 0 || x >= _blockSize || y >= _blockSize)
            {
                result  = Point.Empty;
                return false;
            }

            switch (_blockTypes[x,y])
            {
                case EBlockType.Finish:
                    _blockDistances[x, y] = dist;
                    result = new Point(x, y);
                    return true;
                case EBlockType.None:
                    _blockTypes[x, y] = EBlockType.Path;
                    queue.Enqueue(new Point(x, y));
                    _blockDistances[x, y] = dist;
                    break;
            }
            result = Point.Empty;
            return false;
        }

        public void SetBlock(int x, int y, EBlockType type)
        {
            _blockTypes[x, y] = type;
        }

        public void ResetPath(bool invalidate = true)
        {
            DeleteType(EBlockType.Path, (i, j) => _blockDistances[i, j] = 0);
            DeleteType(EBlockType.ReversePath);
            if (invalidate) Invalidate();
        }

        public void ResetWalls(bool invalidate = true)
        {
            ResetPath(false);
            DeleteType(EBlockType.Wall);
            if (invalidate) Invalidate();
        }

        private void DeleteType(EBlockType type, Action<int, int> callback = null)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_blockTypes[i, j] == type)
                    {
                        _blockTypes[i, j] = EBlockType.None;
                    }

                    callback?.Invoke(i, j);
                }
            }
        }

        public void ResetAll()
        {
            _blockTypes = new EBlockType[_size, _size];
            _blockDistances = new int[_size, _size];
            Invalidate();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            _panel.Invalidate();
            base.OnInvalidated(e);
        }

        private Font GetFont(string s,Graphics g,int fSize = 16)
        {
            if (_lenToFont.TryGetValue(s.Length,out Font r))
            {
                return r;
            }
            SizeF size;
            Font font;
            do
            {
                font = new Font(FontFamily.GenericMonospace, fSize);
                size = g.MeasureString(s, font);
                fSize--;

            } while (size.Width > _blockSize && fSize > 1);

            _lenToFont.Add(s.Length, font);
            return font;
        }

        private void OnPanelPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Size panelSize = _panel.Size;

            int xMax = Math.Min(_size * _blockSize + Origin.X,panelSize.Width);
            int yMax = Math.Min(_size * _blockSize + Origin.Y,panelSize.Height);
            int xMin = Math.Max(Origin.X, Origin.X % _blockSize);
            int yMin = Math.Max(Origin.Y, Origin.Y % _blockSize);

            int xMinIndex = (xMin - Origin.X) / _blockSize;
            int yMinIndex = (yMin - Origin.Y) / _blockSize;

            for (int x = xMin, xIndex = xMinIndex; x < xMax; x += _blockSize, xIndex++)
            {
                for (int y = yMin, yIndex = yMinIndex; y < yMax; y+=_blockSize,yIndex++)
                {
                    var type = _blockTypes[xIndex, yIndex];
                    var dist = _blockDistances[xIndex, yIndex];
                    Color color = Color.Transparent;
                    switch (type)
                    {
                        case EBlockType.None:
                            color = Color.Gray;
                            break;
                        case EBlockType.Start:
                            color = Color.Blue;
                            break;
                        case EBlockType.Finish:
                            color = Color.Red;
                            break;
                        case EBlockType.Path:
                            color = Color.LightBlue;
                            break;
                        case EBlockType.Wall:
                            color = Color.Black;
                            break;
                        case EBlockType.ReversePath:
                            color = Color.DarkBlue;
                            break;
                    }

                    
                    g.FillRectangle(new SolidBrush(color), x, y, _blockSize, _blockSize);
                    if(dist>0)
                    {
                        string s = dist.ToString();
                        g.DrawString(s, GetFont(s, g), Brushes.Black,
                            new RectangleF(x, y, _blockSize, _blockSize));
                    }
                }
            }

            for (int x = xMin; x <= xMax; x+=_blockSize)
            {
                g.DrawLine(Pens.Black, x,Origin.Y,x,yMax);
            }
            for (int y = yMin; y <= yMax; y += _blockSize)
            {
                g.DrawLine(Pens.Black,Origin.X,y,xMax,y);
            }
        }


        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            _dragPoint = Point.Empty;
            _dragEnabled = false;
        }

        private void OnMouseLeave(object? sender, EventArgs e)
        {
            _dragPoint = Point.Empty;
            _dragEnabled = false;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragEnabled)
            {
                int xDiff = e.Location.X - _dragPoint.X;
                int yDiff = e.Location.Y - _dragPoint.Y;
                Origin += new Size(xDiff, yDiff);
                _dragPoint = e.Location;
                Invalidate();
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    _dragPoint = e.Location;
                    _dragEnabled = true;
                    break;
                case MouseButtons.Left:
                    int x = e.Location.X - Origin.X;
                    int y = e.Location.Y - Origin.Y;
                    x /= _blockSize;
                    y /= _blockSize;
                    if (x >= 0 && y >= 0 && x < _size && y < _size && _blockTypes[x, y] == EBlockType.None)
                    {

                        _blockTypes[x, y] = EBlockType.Wall;
                    }

                    break;
            }
            Invalidate();
        }



        public enum EBlockType
        {
            None,
            Start,
            Finish,
            Wall,
            Path,
            ReversePath
        }
    }
}
