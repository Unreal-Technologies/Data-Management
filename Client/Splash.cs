using Shared;
using System.Reflection;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Controls.Gdi;
using UT.Data.Extensions;
using UT.Data.IO;

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
                x => x.GetType() != typeof(GdiLabel)
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
            gdiContent.Text = string.Empty;

            Version? version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");

            gdiVersion.Text = string.Format(SharedResources.Version, version.ToString());
            gdiCopyright.Text = string.Format(SharedResources.Copyright, DateTime.Now.Year);

            InfoBar.Close.Click += Close_Click;

            Load += Splash_Load;
        }
        #endregion //Constructors

        #region Private Methods
        private void Close_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void Splash_Load(object? sender, EventArgs e)
        {
            SequentialExecution sequentialExecution = new(this);
            sequentialExecution.Output += Se_Output;
            sequentialExecution.Add(LoadConfig, "Configuration");
            sequentialExecution.Start();
        }

        private void Se_Output(string text, bool isValid)
        {
            gdiContent.Text = text;
        }

        private bool LoadConfig(SequentialExecution sequentialExecution)
        {
            LocalConfig lc = new("Configuration");
            if(!lc.Exists)
            {
                sequentialExecution.SetOutput("Configuration Missing");
                Thread.Sleep(2500);
            }
            sequentialExecution.SetOutput("HELP!");

            return false;
        }
        #endregion //Private Methods
    }
}
