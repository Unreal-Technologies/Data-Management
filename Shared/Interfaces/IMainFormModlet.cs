using System.Windows.Forms;
using UT.Data.Modlet;

namespace Shared.Interfaces
{
    public interface IMainFormModlet : IModlet
    {
        public DialogResult ShowDialog();
    }
}
