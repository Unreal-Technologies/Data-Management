using Shared;
using System.Reflection;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Controls.Gdi;
using UT.Data.Extensions;
using UT.Data.Forms;
using UT.Data.IO;

namespace Client
{
    public partial class Splash : ExtendedForm
    {
        #region Members
        private readonly AppConfiguration appConfiguration;
        #endregion //Members

        #region Constructors
        public Splash() : base()
        {
            appConfiguration = new();

            InitializeComponent();
            Title = App.Configuration.Title;
            Text = string.Empty;

            TransparencyKey = RadialTransform.TransparencyKey;
            this.RadialTransform(
                15,
                x => x.GetType() != typeof(GdiLabel) && x.GetType() != typeof(Label)
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

            Load += Splash_Load;
        }
        #endregion //Constructors

        #region Private Methods
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

        private bool LoadConfig(SequentialExecution sequentialExecution, ManualResetEvent resetEvent)
        {
            LocalConfig lc = new("Configuration");
            if(!lc.Exists)
            {
                Invoker<Form>.Invoke(this, (Form control, object[]? data) =>
                {
                    control.SendToBack();
                    if (appConfiguration.ShowDialog() == DialogResult.OK)
                    {

                    }
                    sequentialExecution.Unpause(); //Continue SequentialExecution
                });
                sequentialExecution.Pause(); //Stop SequentialExecution to wait for appConfiguration to finish
                resetEvent.WaitOne();
            }
            sequentialExecution.SetOutput("HELP!");
            Thread.Sleep(2500);

            return false;
        }
        #endregion //Private Methods
    }
}
