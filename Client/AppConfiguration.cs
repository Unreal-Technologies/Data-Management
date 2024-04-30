using UT.Data.Controls;

namespace Client
{
    public partial class AppConfiguration : ExtendedForm
    {
        public AppConfiguration()
        {
            InitializeComponent();
            Title = App.Configuration.Title;
        }
    }
}
