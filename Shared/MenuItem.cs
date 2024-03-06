using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic;
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

            };
        }

        public MenuItem? Search(string tree, char seperator = '/')
        {
            return this.Search(tree.Split(seperator));
        }

        private MenuItem? Search(string[] tree)
        {
            if(tree.Length == 0)
            {
                return null;
            }
            string selected = tree[0];
            if(!children.TryGetValue(selected, out MenuItem? value))
            {
                return null;
            }
            string[] left = tree.Skip(1).ToArray();
            if (left.Length > 0)
            {
                return value.Search(left);
            }

            return value;
        }

        #region AddBefore
        public bool AddBefore(string before, string text, MenuItem value)
        {
            try
            {
                this.children.AddBefore(before, text, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddBefore(MenuItem before, string text, MenuItem value)
        {
            try
            {
                this.children.AddBefore(before, text, value);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        #endregion //AddBefore

        #region AddAfter
        public bool AddAfter(string after, string text, MenuItem value)
        {
            try
            {
                this.children.AddAfter(after, text, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool AddAfter(MenuItem after, string text, MenuItem value)
        {
            try
            {
                this.children.AddAfter(after, text, value);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
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
    }
}
