using UT.Data.Modlet;

namespace Shared.Modlet
{
    public interface IMdiFormModlet : IModlet
    {
        public void OnMenuCreation(MenuItem menu);
    }
}
