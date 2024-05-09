using Shared;
using Shared.Interfaces;
using System.Net;
using System.Reflection;
using System.Text;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Extensions;
using UT.Data.Forms;
using UT.Data.IO;
using UT.Data.Modlet;

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
            UpdateConfiguration();
            Text = string.Empty;

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
            TitleChanged += Splash_TitleChanged;

            App.MainModlets = Modlet.Load<IMainFormModlet>();
            App.SubModlets = Modlet.Load<IMdiFormModlet>();

            this.RadialTransform(
                25,
                x => x.GetType() != typeof(GdiLabel) && x.GetType() != typeof(Label)
            ).BorderTransform(
                BorderStyle.FixedSingle,
                Color.Gray
            );
        }
        #endregion //Constructors

        #region Private Methods
        private void Splash_TitleChanged(object? sender, EventArgs e)
        {
            if (App.MainModlets != null)
            {
                foreach (IModlet mod in App.MainModlets)
                {
                    if (mod is ExtendedForm eForm)
                    {
                        eForm.Title = Title;
                    }
                }
            }
        }

        private void Splash_Load(object? sender, EventArgs e)
        {
            SequentialExecution sequentialExecution = new(this);
            sequentialExecution.Output += Se_Output;
            sequentialExecution.Add(LoadConfig, "Configuration");
            sequentialExecution.Add(SetupCommunication, "Setup Communication");
            sequentialExecution.Add(Startup, "Starting..");
            sequentialExecution.Start();
        }

        private void Se_Output(string text, bool isValid)
        {
            gdiContent.Text = text;
            if(!isValid)
            {
                gdiContent.ForeColor = Color.Red;
            }
        }

        private bool Startup(SequentialExecution sequentialExecution, ManualResetEvent resetEvent)
        {
            if(App.Client == null)
            {
                return false;
            }

            bool state = true;
            Invoker<Splash>.Invoke(this, (Splash control, object[]? data) =>
            {
                control.Hide();
                if (App.MainModlets == null)
                {
                    state = false;
                    sequentialExecution.Unpause();
                    return;
                }
                foreach (IModlet mod in App.MainModlets)
                {
                    mod.OnClientConfiguration(this, App.Client, App.Session);
                    if (mod is IMdiParentModlet mpm)
                    {
                        mpm.Load += Mpm_Load;
                    }

                    if (mod is IMainFormModlet mfm)
                    {
                        DialogResult result = mfm.ShowDialog();
                        if (result == DialogResult.Abort)
                        {
                            state = false;
                            sequentialExecution.Unpause();
                            return;
                        }
                        else if(result == DialogResult.Retry)
                        {
                            App.AutoRestart = true;
                            state = true;
                            sequentialExecution.Unpause();
                            return;
                        }
                    }
                }
            });
            sequentialExecution.Pause(); //Stop SequentialExecution to wait for appConfiguration to finish
            resetEvent.WaitOne();
            return state;
        }

        private void Mpm_Load(object? sender, EventArgs e)
        {
            if(App.SubModlets == null || sender is not Form form || App.Client == null)
            {
                return;
            }
            foreach(IModlet mod in App.SubModlets)
            {
                mod.OnClientConfiguration(form, App.Client, App.Session);
                if (mod is IMdiFormModlet mfm)
                {
                    mfm.MdiParent = form;
                    mfm.OnMenuCreation();
                }

                if(mod is ExtendedForm eForm)
                {
                    eForm.Title = Title;
                }
            }

            if(form is IMainMenuContainer mmc)
            {
                mmc.MenuStack.ConvertTo(mmc.MenuStrip);
            }
        }

        private bool LoadConfig(SequentialExecution sequentialExecution, ManualResetEvent resetEvent)
        {
            LocalConfig lc = new("Configuration");
            if(!lc.Exists)
            {
                sequentialExecution.SetOutput("Setup");
                bool isChanged = false;
                Invoker<Form>.Invoke(this, (Form control, object[]? data) =>
                {
                    control.SendToBack();
                    if (appConfiguration.ShowDialog() == DialogResult.OK)
                    {
                        isChanged = true;
                    }
                    sequentialExecution.Unpause(); //Continue SequentialExecution
                });
                sequentialExecution.Pause(); //Stop SequentialExecution to wait for appConfiguration to finish
                resetEvent.WaitOne();

#pragma warning disable S2583 // Conditionally executed code should be reachable
                if(isChanged) //Is Reachable, worked through invoke handler
                {
                    lc.Save(App.Configuration);
                    sequentialExecution.SetOutput("Saved \"" + lc.Path + "\"");
                }
#pragma warning restore S2583 // Conditionally executed code should be reachable

                UpdateConfiguration();
                return true;
            }
            else
            {
                Configuration? configuration = lc.Load<Configuration>();
                if(configuration == null) //Configuration corrupted, delete & re-initialize config screen
                {
                    File.Delete(lc.FullPath);
                    return LoadConfig(sequentialExecution, resetEvent);
                }
                sequentialExecution.SetOutput("Loaded \"" + lc.Path + "\"");

                App.Configuration = configuration;
                Invoker<Splash>.Invoke(this, (Splash splash, object[]? data) =>
                {
                    splash.Title = configuration.Title.Replace("&", "&&");
                });
                
                UpdateConfiguration();
                return true;
            }
        }

        private bool SetupCommunication(SequentialExecution sequentialExecution, ManualResetEvent resetEvent)
        {
            App.Client = new ModletClient(App.Configuration.Server, App.Configuration.Port);
            if (App.Client.Send(ModletCommands.Commands.Connect, null) == null)
            {
                return false;
            }

            sequentialExecution.SetOutput("Get Transmission Key.");
            byte[]? key = App.Client.Send(
                Encoding.UTF8.GetBytes(Dns.GetHostName()), 
                ModletCommands.Commands.Serverkey, 
                null
            );

            if (key == null)
            {
                return false;
            }

            App.Client.Aes = key.AsString();
            sequentialExecution.SetOutput("Key received, enabling aes.");

            return true;
        }

        private void UpdateConfiguration()
        {
            Invoker<ExtendedForm>.Invoke(this, (ExtendedForm control, object[]? data) =>
            {
                control.Title = App.Configuration.Title.Replace("&", "&&");
            });
        }
        #endregion //Private Methods
    }
}
