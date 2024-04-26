using UT.Data.Controls;
using Shared;
using System.Drawing.Drawing2D;
using Shared.Helpers;
using System.Reflection;

namespace Client
{
    public partial class Splash : ExtendedForm
    {
        class InternalLogo : Logo
        {
            private readonly string copyright;
            private readonly string version;

            public InternalLogo()
            {
                copyright = string.Format(SharedResources.Copyright, DateTime.Now.Year.ToString());

                Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");
                this.version = string.Format(SharedResources.Version, assemblyVersion.ToString());
            }

            protected override void OnPaint(PaintEventArgs pe)
            {
                base.OnPaint(pe);

                Font f1 = Font;
                Font f2 = new(f1.FontFamily, 14, FontStyle.Bold);

                Graphics graphics = pe.Graphics;

                RectangleF copyrightbounds = DrawText(copyright, f1, graphics, new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Left,
                    AlignmentHelper.Settings.Vertical.Bottom,
                    5,
                    -5
                ));
                DrawText(version, f1, graphics, new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Right,
                    AlignmentHelper.Settings.Vertical.Bottom,
                    -5,
                    -5
                ));

                DrawText("--text--", f1, graphics, new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Center,
                    AlignmentHelper.Settings.Vertical.Bottom,
                    0,
                    -(copyrightbounds.Height + 5)
                ));

                DrawText("--title--", f2, graphics, new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Center,
                    AlignmentHelper.Settings.Vertical.Top,
                    0,
                    10
                ));
            }

            private RectangleF DrawText(string text, Font font, Graphics graphics, AlignmentHelper.Settings settings)
            {
                Brush textBrush = Brushes.White;
                Brush shadowBrush = Brushes.Black;
                Brush panelBrush = new SolidBrush(Color.FromArgb(0x7F, Color.Gray));

                SizeF size = graphics.MeasureString(text, font);

                RectangleF textBounds = new(new PointF(), size);
                textBounds.Location = AlignmentHelper.CalculatePosition(textBounds, DisplayRectangle, settings);

                RectangleF shadowBounds = new(textBounds.Location, textBounds.Size);
                shadowBounds.X += 1;
                shadowBounds.Y += 1;

                graphics.FillRectangle(panelBrush, textBounds);
                graphics.DrawString(text, font, shadowBrush, shadowBounds);
                graphics.DrawString(text, font, textBrush, textBounds);

                return textBounds;
            }
        }

        public Splash() : base()
        {
            InternalLogo logo = new()
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(logo);

            InitializeComponent();
            this.Invalidate();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var sb = new SolidBrush(Color.FromArgb(100, 100, 100, 100));
            e.Graphics.FillRectangle(sb, this.DisplayRectangle);
        }
    }
}
