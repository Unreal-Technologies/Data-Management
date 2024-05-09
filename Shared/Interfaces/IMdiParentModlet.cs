namespace Shared.Interfaces
{
    public interface IMdiParentModlet
    {
        public event EventHandler? Load;

        public event EventHandler? OnFullscreenChanged;
        public bool IsFullScreen { get; }
    }
}
