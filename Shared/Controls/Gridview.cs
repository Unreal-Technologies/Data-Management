using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public class Gridview<Tid> : Panel
        where Tid : struct
    {
        #region Constants
        private new const int Padding = 3;
        #endregion //Constants

        #region Enums
        public enum ControlLocations
        {
            Left, Right
        }
        #endregion //Enums

        #region Members
        private readonly List<Column> columns;
        private readonly List<Tuple<Point, Control?>> fields;
        private readonly List<Row> rows;
        private Dictionary<int, int> columnSizes;
        private Dictionary<int, int> rowSizes;
        #endregion //Members

        #region Properties
        public ControlLocations ControlLocation { get; set; }
        #endregion //Properties

        #region Constructors
        public Gridview(): base()
        {
            this.columns = [];
            this.fields = [];
            this.columnSizes = [];
            this.rowSizes = [];

            this.rows = [];
            this.ControlLocation = ControlLocations.Left;
            this.MinimumSize = new Size(20, 20);
        }
        #endregion //Constructors

        #region Public Methods
        public void AddRow(Row row)
        {
            this.rows.Add(row);
            this.ComposeRows();
        }

        public void SetRows(Row[] rows)
        {
            this.rows.Clear();
            this.rows.AddRange(rows);
            this.ComposeRows();
        }

        public void SetColumns(Column[] columns)
        {
            this.columns.Clear();
            this.columns.AddRange(columns);
            this.ComposeHeader();
        }

        public void AddColumn(Column column)
        {
            this.columns.Add(column);
            this.ComposeHeader();
        }
        #endregion //Public Methods

        #region Private Methods
        private void ComposeRows()
        {
            int i = 1;
            int offset = this.ControlLocation == ControlLocations.Left ? 2 : 0;

            for(int r = 0; r<this.rows.Count; r++)
            {
                Row row = this.rows[r];

                if (offset != 0)
                {
                    Button edit = new();
                    edit.Click += delegate (object? sender, EventArgs e) { row.OnEdit?.Invoke(row.ID); };

                    Button remove = new();
                    remove.Click += delegate (object? sender, EventArgs e) { row.OnRemove?.Invoke(row.ID); };

                    this.fields.Add(new Tuple<Point, Control?>(new Point(0, r + i), edit));
                    this.Controls.Add(edit);

                    this.fields.Add(new Tuple<Point, Control?>(new Point(1, r + i), remove));
                    this.Controls.Add(remove);
                }

                int length = row.Cells.Count;
                for (int c=0; c<length; c++)
                {
                    Cell cell = row.Cells[c];
                    int index = c + offset;

                    Label l = new()
                    {
                        Text = cell.Text
                    };
                    if (cell.FontStyle != null)
                    {
                        l.Font = new Font(this.Font, cell.FontStyle.Value);
                    }
                    if (cell.Color != null)
                    {
                        l.ForeColor = cell.Color.Value;
                    }

                    this.fields.Add(new Tuple<Point, Control?>(new Point(index, r + i), l));
                    this.Controls.Add(l);
                }

                if (offset == 0)
                {
                    Button edit = new();
                    edit.Click += delegate (object? sender, EventArgs e) { row.OnEdit?.Invoke(row.ID); };

                    Button remove = new();
                    remove.Click += delegate (object? sender, EventArgs e) { row.OnRemove?.Invoke(row.ID); };

                    this.fields.Add(new Tuple<Point, Control?>(new Point(length + 0, r + i), edit));
                    this.Controls.Add(edit);

                    this.fields.Add(new Tuple<Point, Control?>(new Point(length + 1, r + i), remove));
                    this.Controls.Add(remove);
                }
            }

            this.ComposeSizes();
        }

        private void ComposeHeader()
        {
            this.fields.Clear();
            this.Controls.Clear();
            int offset = this.ControlLocation == ControlLocations.Left ? 2 : 0;
            if (offset != 0)
            {
                this.fields.Add(new Tuple<Point, Control?>(new Point(0, 0), null));
                this.fields.Add(new Tuple<Point, Control?>(new Point(1, 0), null));
            }

            int i = 0;
            foreach (Column column in this.columns)
            {
                int index = i + offset;

                Label l = new()
                {
                    Text = column.Text,
                    Font = new Font(this.Font, FontStyle.Bold)
                };
                this.fields.Add(new Tuple<Point, Control?>(new Point(index, 0), l));
                this.Controls.Add(l);

                i++;
            }

            if (offset == 0)
            {
                this.fields.Add(new Tuple<Point, Control?>(new Point(i + 0, 0), null));
                this.fields.Add(new Tuple<Point, Control?>(new Point(i + 1, 0), null));
            }

            this.ComposeSizes();
        }

        private void ComposeSizes()
        {
            int maxY = this.fields.Select(x => x.Item1.Y).Max();
            int maxX = this.fields.Select(x => x.Item1.X).Max();

            Dictionary<int, int> columns = [];
            Dictionary<int, int> rows = [];

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    Point location = new(x, y);
                    Control? control = this.fields.Where(x => x.Item1.X == location.X && x.Item1.Y == location.Y).Select(x => x.Item2).FirstOrDefault();

                    Size size = new(0, 0);
                    if(control != null)
                    {
                        control.Size = control.PreferredSize;
                        size = control.Size;
                    }

                    if(columns.TryGetValue(x, out int valueX))
                    {
                        columns[x] = Math.Max(valueX, size.Width);
                    }
                    else
                    {
                        columns.Add(x, size.Width);
                    }

                    if(rows.TryGetValue(y, out int valueY))
                    {
                        rows[y] = Math.Max(valueY, size.Height);
                    }
                    else
                    {
                        rows.Add(y, size.Height);
                    }
                }
            }

            this.columnSizes = columns;
            this.rowSizes = rows;

            this.PositionControls();
        }

        private void PositionControls()
        {
            Dictionary<int, int> rows = this.rowSizes;
            Dictionary<int, int> columns = this.columnSizes;

            int maxY = rows.Keys.Max();
            int maxX = columns.Keys.Max();

            int offsetY = 0;
            for(int y=0; y<=maxY; y++)
            {
                int offsetX = 0;
                for(int x=0; x<=maxX; x++)
                {
                    Point location = new(x, y);
                    Size size = new(columns[x], rows[y]);
                    Control? control = this.fields.Where(x => x.Item1.X == location.X && x.Item1.Y == location.Y).Select(x => x.Item2).FirstOrDefault();

                    if(control != null)
                    {
                        control.Location = new Point(offsetX, offsetY);
                        control.Size = control.PreferredSize;
                    }
                    offsetX += size.Width + Gridview<Guid>.Padding;
                }
                offsetY += rows.Values.Max() + Gridview<Guid>.Padding;
            }
            this.Size = this.PreferredSize;
        }

        private void DrawHorizontalLines(Graphics g)
        {
            Pen p = Pens.Black;
            int w = this.columnSizes.Values.Sum() + (this.columnSizes.Count * Gridview<Tid>.Padding);

            for (int yI = 0; yI < this.rowSizes.Count; yI++)
            {
                int y = this.rowSizes.Values.Take(yI).Sum() + (yI * Gridview<Tid>.Padding) - (Gridview<Tid>.Padding / 2);

                g.DrawLine(p, new PointF(0, y), new PointF(w, y));
            }
        }

        private void DrawVerticalLines(Graphics g)
        {
            Pen p = Pens.Black;
            int h = this.rowSizes.Values.Sum() + (this.rowSizes.Count * Gridview<Tid>.Padding);

            int offset = this.ControlLocation == ControlLocations.Left ? 2 : 0;
            int offsetInverse = 2 - offset;

            for (int xI = offset; xI < this.columnSizes.Count - offsetInverse; xI++)
            {
                int x = this.columnSizes.Values.Take(xI).Sum() + (xI * Gridview<Tid>.Padding) - (Gridview<Tid>.Padding / 2);

                g.DrawLine(p, new PointF(x, 0), new PointF(x, h));
            }
        }
        #endregion //Private Methods

        #region Overrides
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            this.DrawHorizontalLines(g);
            this.DrawVerticalLines(g);
        }
        #endregion //Overrides

        #region Classes
        public class Row
        {
            #region Properties
            public List<Cell> Cells { get; set; }
            public Tid? ID { get; set; }
            #endregion //Properties

            #region Delegates
            public delegate void ClickHandler(Tid? id);
            #endregion //Delegates

            #region Events
            public ClickHandler? OnEdit { get; set; }
            public ClickHandler? OnRemove { get; set; }
            #endregion //Events

            #region Constructors
            public Row()
            {
                this.Cells = [];
            }
            #endregion //Constructors
        }

        public class Cell
        {
            public string? Text { get; set; }
            public Color? Color { get; set; }
            public FontStyle? FontStyle { get; set; }
        }

        public class Column
        {
            #region Properties
            public string? Text { get; set; }
            #endregion //Properties
        }
        #endregion //Classes
    }
}
