using Shared;
using Shared.Helpers;
using System.Reflection;
using UT.Data.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    class InternalLogo : Logo
    {
        #region Members
        private readonly string copyright;
        private readonly string version;
        #endregion //Members

        #region Properties
        public new string Text { get; set; }
        #endregion //Properties

        #region Constructors
        public InternalLogo()
        {
            copyright = string.Format(SharedResources.Copyright, DateTime.Now.Year.ToString());
            Text = string.Empty;

            Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");
            this.version = string.Format(SharedResources.Version, assemblyVersion.ToString());
        }
        #endregion //Constructors

        #region Overrides
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (Parent == null)
            {
                return;
            }

            Font f1 = Parent.Font;
            Font f2 = new(f1.FontFamily, 14, FontStyle.Bold);

            System.Drawing.Graphics graphics = pe.Graphics;

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

            DrawText(Text, f1, graphics, new AlignmentHelper.Settings(
                AlignmentHelper.Settings.Horizontal.Center,
            AlignmentHelper.Settings.Vertical.Bottom,
                0,
                -(copyrightbounds.Height + 5)
            ));

            DrawText(Parent.Text, f2, graphics, new AlignmentHelper.Settings(
                AlignmentHelper.Settings.Horizontal.Center,
                AlignmentHelper.Settings.Vertical.Top,
                0,
                10
            ));
        }
        #endregion //Overrides

        #region Private Methods
        private RectangleF DrawText(string text, Font font, System.Drawing.Graphics graphics, AlignmentHelper.Settings settings)
        {
            Brush textBrush = Brushes.White;
            Brush shadowBrush = Brushes.Black;
            Brush panelBrush = new SolidBrush(Color.FromArgb(0x33, Color.Gray));

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
        #endregion //Private Methods
    }
}
