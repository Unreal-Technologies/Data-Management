using Shared.Helpers;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Graphics
{
    public class RadialTransform
    {
        #region Members
        private readonly SizeF topLeftRadial;
        private readonly SizeF topRightRadial;
        private readonly SizeF bottomLeftRadial;
        private readonly SizeF bottomRightRadial;
        private readonly Control control;
        private readonly Color color;
        private PointF[]? topLeft;
        private PointF[]? topRight;
        private PointF[]? bottomLeft;
        private PointF[]? bottomRight;
        #endregion //Members

        #region Constructors
        public RadialTransform(
            Control control,
            float topleftRadial,
            float topRightRadial,
            float bottomLeftRadial,
            float bottomRightRadial,
            Color color
        )
        : this(
                control,
                new SizeF(topleftRadial, topleftRadial),
                new SizeF(topRightRadial, topRightRadial),
                new SizeF(bottomLeftRadial, bottomLeftRadial),
                new SizeF(bottomRightRadial, bottomRightRadial),
                color
        )
        { }

        public RadialTransform(Control control, float radial, Color color)
            : this(control, new SizeF(radial, radial), color) { }

        public RadialTransform(
            Control control, 
            SizeF radial, 
            Color color
        )
        : this(
              control, 
              radial, 
              radial, 
              radial, 
              radial, 
              color
        ) { }

        public RadialTransform(
            Control control, 
            SizeF topLeftRadial, 
            SizeF topRightRadial, 
            SizeF bottomLeftRadial, 
            SizeF bottomRightRadial, 
            Color color
        )
        {
            this.topLeftRadial = topLeftRadial;
            this.topRightRadial = topRightRadial;
            this.bottomLeftRadial = bottomLeftRadial;
            this.bottomRightRadial = bottomRightRadial;
            this.control = control;
            this.color = color;

            control.Paint += Control_Paint;
            control.Resize += Control_Resize;

            CalculatePoints();
        }
        #endregion //Constructors

        private void CalculatePoints()
        {
            bool tlSet = !topLeftRadial.IsEmpty;
            bool trSet = !topRightRadial.IsEmpty;
            bool blSet = !bottomLeftRadial.IsEmpty;
            bool brSet = !bottomRightRadial.IsEmpty;

            RectangleF bounds = control.DisplayRectangle;
            topLeft = tlSet ? AlignmentHelper.CalculateArcCorner(
                topLeftRadial,
                bounds,
                new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Left,
                    AlignmentHelper.Settings.Vertical.Top
                )
            ) : [];
            topRight = trSet ? AlignmentHelper.CalculateArcCorner(
                topRightRadial,
                bounds,
                new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Right,
                    AlignmentHelper.Settings.Vertical.Top
                )
            ) : [];
            bottomLeft = blSet ? AlignmentHelper.CalculateArcCorner(
                bottomLeftRadial,
                bounds,
                new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Left,
                    AlignmentHelper.Settings.Vertical.Bottom
                )
            ) : [];
            bottomRight = brSet ? AlignmentHelper.CalculateArcCorner(
                bottomRightRadial,
                bounds,
                new AlignmentHelper.Settings(
                    AlignmentHelper.Settings.Horizontal.Right,
                    AlignmentHelper.Settings.Vertical.Bottom
                )
            ) : [];
        }

        #region Events
        private void Control_Resize(object? sender, EventArgs e)
        {
            CalculatePoints();
        }

        private void Control_Paint(object? sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;

            if (topLeft != null && topRight != null && bottomLeft != null && bottomRight != null)
            {
                Brush brush = new SolidBrush(color);

                graphics.FillPolygon(brush, topLeft);
                graphics.FillPolygon(brush, topRight);
                graphics.FillPolygon(brush, bottomLeft);
                graphics.FillPolygon(brush, bottomRight);
            }
        }
        #endregion //Events
    }
}
