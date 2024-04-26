using System.Drawing;
using System.Windows.Forms;
using UT.Data;

namespace Shared.Controls
{
    public class ExtendedForm : Form
    {
        public ExtendedForm() : base()
        {
            Font = new Font(FontFamily.GenericMonospace, 9);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            Icon = Resources.Favicon;
        }
    }
}
