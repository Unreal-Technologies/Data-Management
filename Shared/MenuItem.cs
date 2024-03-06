using UT.Data.Extensions;

namespace Shared
{
    public class MenuItem
    {
        #region Members
        private readonly Dictionary<string, MenuItem> children;
        private readonly Types type;
        private readonly bool canHaveChildren;
        private readonly bool isRoot;
        #endregion //Members

        #region Enums
        public enum Types
        {
            Submenu, Line, Button, Root
        }
        #endregion //Enums

        #region Delegates
        public delegate void OnButtonClick();
        #endregion //Delegates

        #region Properties
        public Dictionary<string, MenuItem> Children { get { return this.children; } }
        public Types Type { get { return this.type; } }
        public OnButtonClick? OnClick { get; set; }
        #endregion //Properties

        #region Constructors
        private MenuItem(Types type)
        {
            this.isRoot = type == Types.Root;
            this.canHaveChildren = this.isRoot || type == Types.Submenu;
            this.children = [];
            this.type = type;
        }
        #endregion //Constructors

        #region Public Methods
        public static MenuItem Root()
        {
            return new(Types.Root);
        }

        public static MenuItem Submenu()
        {
            return new(Types.Submenu);
        }

        public static MenuItem Line()
        {
            return new(Types.Line);
        }

        public static MenuItem Button(OnButtonClick click)
        {
            return new(Types.Button)
            {
                OnClick = click
            };
        }

        public MenuItem? Search(string tree, char seperator = '/')
        {
            return this.Search(tree.Split(seperator));
        }

        #region AddBefore
        public bool AddBefore(string before, string text, MenuItem value)
        {
            if(this.children.ContainsKey(text))
            {
                return false;
            }
            this.children.AddBefore(before, text, value);
            return true;
        }

        public bool AddBefore(MenuItem before, string text, MenuItem value)
        {
            if (this.children.ContainsKey(text))
            {
                return false;
            }
            this.children.AddBefore(before, text, value);
            return true;
        }
        #endregion //AddBefore

        #region AddAfter
        public bool AddAfter(string after, string text, MenuItem value)
        {
            if (this.children.ContainsKey(text))
            {
                return false;
            }
            this.children.AddAfter(after, text, value);
            return true;
        }

        public bool AddAfter(MenuItem after, string text, MenuItem value)
        {
            if(this.children.ContainsKey(text))
            {
                return false;
            }
            this.children.AddAfter(after, text, value);
            return true;
        }
        #endregion //AddAfter

        public bool Add(string text, MenuItem menuItem, int? position = null)
        {
            if(!this.canHaveChildren || menuItem.isRoot)
            {
                return false;
            }

            if(this.children.ContainsKey(text))
            {
                this.children[text] = menuItem;
                return true;
            }

            this.children.AddAt(text, menuItem, position);
            return true;
        }
        #endregion //Public Methods

        #region Private Methods
        private MenuItem? Search(string[] tree)
        {
            if (tree.Length == 0)
            {
                return null;
            }
            string selected = tree[0];
            if (!children.TryGetValue(selected, out MenuItem? value))
            {
                return null;
            }
            string[] remainder = tree.Skip(1).ToArray();
            if (remainder.Length > 0)
            {
                return value.Search(remainder);
            }

            return value;
        }
        #endregion //Private Methods
    }
}
