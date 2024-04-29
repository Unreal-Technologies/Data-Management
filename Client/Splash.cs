using Shared;
using Shared.Extensions;
using Shared.Graphics;
using System.Reflection;
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
            this.RadialTransform(
                15,
                x => x.GetType() != typeof(GdiText)
            ).BorderTransform(
                BorderStyle.FixedSingle, 
                Color.Gray
            );

            gdiContent.RadialTransform(
                15, 
                null, 
                BackColor
            ).BorderTransform(
                BorderStyle.FixedSingle, 
                Color.Gray
            );

            Version? version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");

            gdiVersion.Text = string.Format(SharedResources.Version, version.ToString());
            gdiCopyright.Text = string.Format(SharedResources.Copyright, DateTime.Now.Year);
        }

        #endregion //Constructors
    }
}
