using Shared.Modules;
using System.Windows.Forms;
using Shared.Data;

namespace Shared.Interfaces
{
    public interface IMainMenuContainer
    {
        public MenuStack MenuStack { get; }
        public MenuStrip MenuStrip { get; }
    }
}
