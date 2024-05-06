using System.Windows.Forms;
using UT.Data.Modlet;

namespace Shared.Interfaces
{
    public interface IMdiFormModlet : IModlet
    {
        public Form? MdiParent { get; set; }

        public void OnMenuCreation();
    }
}
