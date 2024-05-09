using System.Drawing;
using System.Windows.Forms;

namespace Shared.Data
{
    public class MenuStack
    {
        #region Delegates
        public delegate void ClickHandler();
        #endregion //Delegates

        #region Members
        private readonly SortedDictionary<string, object> data;
        #endregion //Members

        #region Constructors
        public MenuStack()
        {
            data = [];
        }
        #endregion //Constructors

        #region Public Methods
        public void Add(Form form, string[] entries, ClickHandler? handler = null)
        {
            SortedDictionary<string, object> stack = data;

            int i = 0;
            int count = entries.Length;

            foreach (string entry in entries)
            {
                string parsedEntry = entry.Replace("&", "&&");
                bool isLast = i == count - 1;
                if (!stack.ContainsKey(parsedEntry) && !isLast)
                {
                    stack.Add(parsedEntry, AddStack());
                    stack = (SortedDictionary<string, object>)stack[parsedEntry];
                }
                else if (!stack.ContainsKey(parsedEntry) && isLast)
                {
                    stack.Add(parsedEntry, AddEntry(form, handler));
                }
                else if (stack.ContainsKey(parsedEntry) && !isLast)
                {
                    stack = (SortedDictionary<string, object>)stack[parsedEntry];
                }
                else
                {
                    throw new NotImplementedException();
                }
                i++;
            }
        }

        public void ConvertTo(MenuStrip menuStrip)
        {
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new SubColorTable(menuStrip.BackColor));
            ConvertTo(menuStrip, data);
        }
        #endregion //Public Methods

        #region Classes
        private sealed class SubColorTable(Color backColor) : ProfessionalColorTable
        {
            #region Members
            private readonly Color backColor = backColor;
            #endregion //Members

            #region Properties
            public override Color MenuItemSelected
            {
                get { return backColor; }
            }

            public override Color ButtonSelectedHighlight
            {
                get { return backColor; }
            }

            public override Color ButtonCheckedHighlight
            {
                get { return backColor; }
            }

            public override Color ButtonPressedHighlight
            {
                get { return backColor; }
            }

            public override Color ToolStripDropDownBackground
            {
                get { return backColor; }
            }

            public override Color CheckSelectedBackground
            {
                get { return backColor; }
            }
            #endregion //Properties
        }
        #endregion //Classes

        #region Private Methods
        private static void ConvertTo(MenuStrip menuStrip, SortedDictionary<string, object> stack)
        {
            foreach (KeyValuePair<string, object> kvp in stack)
            {
                if (kvp.Value is SortedDictionary<string, object> sub)
                {
                    ToolStripMenuItem tsmi = new(kvp.Key)
                    {
                        BackColor = menuStrip.BackColor
                    };
                    menuStrip.Items.Add(tsmi);
                    ConvertTo(tsmi, sub);
                }
                else if (kvp.Value is ClickHandler clickHandler)
                {
                    ToolStripMenuItem tsmi = new(kvp.Key)
                    {
                        BackColor = menuStrip.BackColor
                    };
                    tsmi.Click += (object? sender, EventArgs e) => { clickHandler(); };
                    menuStrip.Items.Add(tsmi);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static void ConvertTo(ToolStripMenuItem toolStripMenuItem, SortedDictionary<string, object> stack)
        {
            foreach (KeyValuePair<string, object> kvp in stack)
            {
                if (kvp.Value is SortedDictionary<string, object> sub)
                {
                    ToolStripMenuItem tsmi = new(kvp.Key)
                    {
                        BackColor = toolStripMenuItem.BackColor
                    };
                    toolStripMenuItem.DropDownItems.Add(tsmi);
                    ConvertTo(tsmi, sub);
                }
                else if (kvp.Value is ClickHandler clickHandler)
                {
                    ToolStripMenuItem tsmi = new(kvp.Key)
                    {
                        BackColor = toolStripMenuItem.BackColor
                    };
                    tsmi.Click += (object? sender, EventArgs e) => { clickHandler(); };
                    toolStripMenuItem.DropDownItems.Add(tsmi);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static ClickHandler AddEntry(Form form, ClickHandler? handler)
        {
            if (handler != null)
            {
                return handler;
            }

            return form.Show;
        }

        private static SortedDictionary<string, object> AddStack()
        {
            return [];
        }
        #endregion //Private Methods
    }
}
