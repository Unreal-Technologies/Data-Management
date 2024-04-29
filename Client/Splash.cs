using Shared.Extensions;
using Shared.Graphics;
using UT.Data.Controls;

namespace Client
{
    public partial class Splash : ExtendedForm
    {
        #region Constructors
        public Splash() : base()
        {
            InitializeComponent();

            TransparencyKey = RadialTransform.TransparencyKey;
            this.RadialTransform(15).BorderTransform(BorderStyle.FixedSingle, Color.Gray);
        }

        #endregion //Constructors
    }
}
