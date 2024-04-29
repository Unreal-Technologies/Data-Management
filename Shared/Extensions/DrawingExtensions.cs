using Shared.Graphics;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Extensions
{
    public static class DrawingExtensions
    {
        #region RadialTransform
        public static RadialTransform RadialTransform(
            this Control control,
            SizeF topLeftRadial,
            SizeF topRightRadial,
            SizeF bottomLeftRadial,
            SizeF bottomRightRadial
        )
        {
            return new RadialTransform(control, topLeftRadial, topRightRadial, bottomLeftRadial, bottomRightRadial);
        }

        public static RadialTransform RadialTransform(
            this Control control,
            float topLeftRadial,
            float topRightRadial,
            float bottomLeftRadial,
            float bottomRightRadial
        )
        {
            return new RadialTransform(control, topLeftRadial, topRightRadial, bottomLeftRadial, bottomRightRadial);
        }

        public static RadialTransform RadialTransform(
            this Control control,
            float radial
        )
        {
            return new RadialTransform(control, radial);
        }

        public static RadialTransform RadialTransform(
            this Control control,
            SizeF radial
        )
        {
            return new RadialTransform(control, radial);
        }
        #endregion //RadialTransform

        #region BorderTransform
        public static BorderTransform BorderTransform(this RadialTransform radialTransform, BorderStyle borderStyle, Color color)
        {
            return new BorderTransform(radialTransform, borderStyle, color);
        }

        public static BorderTransform BorderTransform(this Control control, BorderStyle borderStyle, Color color)
        {
            return control.RadialTransform(0).BorderTransform(borderStyle, color);
        }
        #endregion //BorderTransform

        #region IncrementX
        public static PointF IncrementX(this PointF point, int value)
        {
            point.X += value;
            return point;
        }

        public static IEnumerable<PointF> IncrementX(this IEnumerable<PointF> list, int value)
        {
            return list.Select(x => new PointF(x.X + value, x.Y));
        }
        #endregion //IncrementX

        #region IncrementY
        public static IEnumerable<PointF> IncrementY(this IEnumerable<PointF> list, int value)
        {
            return list.Select(x => new PointF(x.X, x.Y + value));
        }

        public static PointF IncrementY(this PointF point, int value)
        {
            point.Y += value;
            return point;
        }
        #endregion //IncrementY

        #region Increment
        public static IEnumerable<PointF> Increment(this IEnumerable<PointF> list, int value)
        {
            return list.IncrementX(value).IncrementY(value);
        }

        public static PointF Increment(this PointF point, int value)
        {
            return point.IncrementX(value).IncrementY(value);
        }
        #endregion //Increment
    }
}
