using System.Drawing;

namespace Shared.Helpers
{
    public class AlignmentHelper
    {
        #region Classes
        public struct Settings(Settings.Horizontal? hAlign = null, Settings.Vertical? vAlign = null, float? xOffset = null, float? yOffset = null)
        {
            #region Enums
            public enum Horizontal
            {
                Left, Right, Center
            }

            public enum Vertical
            {
                Top, Center, Bottom
            }
            #endregion //Enums

            #region Properties
            public Horizontal? HAlign { get; set; } = hAlign;
            public Vertical? VAlign { get; set; } = vAlign;
            public float? XOffset { get; set; } = xOffset;
            public float? YOffset { get; set; } = yOffset;
            #endregion //Properties
        }
        #endregion //Classes

        #region Public Methods
        public static PointF CalculatePosition(RectangleF source, RectangleF destination, Settings settings)
        {
            float x = settings.HAlign switch
            {
                Settings.Horizontal.Right => destination.Width - source.Width,
                Settings.Horizontal.Center => (destination.Width - source.Width) / 2,
                _ => 0,
            };

            float y = settings.VAlign switch
            {
                Settings.Vertical.Center => (destination.Height - source.Height) / 2,
                Settings.Vertical.Bottom => destination.Height - source.Height,
                _ => 0,
            };

            if (settings.XOffset.HasValue)
            {
                x += settings.XOffset.Value;
            }
            if (settings.YOffset.HasValue)
            {
                y += settings.YOffset.Value;
            }

            return new(x, y);
        }
        #endregion //Public Methods
    }
}
