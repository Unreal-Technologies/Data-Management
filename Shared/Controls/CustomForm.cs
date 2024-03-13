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
            this.Icon = Resources.favicon;
        }
        #endregion //Constructors
    }

    public class CustomForm<T> : CustomForm
        where T : CustomForm
    {
        #region Delegates
        public delegate void ShowHandler(T? form);
        #endregion //Delegates

        #region Public Methods
        public void Show(Form? mdiParent, ShowHandler? handler = null)
        {
            if (mdiParent == null || Activator.CreateInstance(typeof(T)) is not T clone)
            {
                return;
            }
            clone.MdiParent = mdiParent;
            handler?.Invoke(clone);
            clone.Show();
        }
        #endregion //Public Methods
    }
}
