using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public class CustomForm : Form
    {
        #region Constructors
        public CustomForm()
            : base()
        {
            this.Font = new Font(FontFamily.GenericMonospace, 9);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Icon = Resource.favicon;
        }
        #endregion //Constructors
    }
}
