using Client.ServerConfiguration;
using Shared;
using Shared.Controls;
using Shared.Modlet;
using System.Net;
using System.Reflection;
using System.Text;
using UT.Data;
using UT.Data.Extensions;
using UT.Data.Forms;
using UT.Data.IO;
using UT.Data.Modlet;

namespace Client
{
    public partial class Splash : CustomForm
    {
        #region Constructors
        public Splash()
        {
            InitializeComponent();
        }
        #endregion //Constructors

        #region Events
        private void Splash_Load(object sender, EventArgs e)
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");
            this.lbl_copyright.Text = this.lbl_copyright.Text.Replace("xxxx", DateTime.Now.Year.ToString());
            this.lbl_version.Text = this.lbl_version.Text.Replace("x.x.x.x", version.ToString());

            SequentialExecution se = new(this);
            se.Output += Se_Output;
            se.Add(this.ServerCommunication, "Starting Server Communication");
            se.Add(this.ModuleLoader, "Loading Modules");
            se.Add(this.Startup, "Starting..");
            se.Start();
        }

        private void Se_Output(string text, bool isValid)
        {
            Invoker<Label>.Invoke(this.lbl_progression, delegate (Label label, object[]? data)
            {
                label.Text = text;
                label.Size = label.PreferredSize;
                label.Location = new Point((this.Width - label.Width) / 2, label.Location.Y);
                if(!isValid)
                {
                    label.ForeColor = Color.Red;
                }
            });
            if(!isValid)
            {
                ApplicationState.Reset = true;
                Invoker<CustomForm>.Invoke(this, delegate (CustomForm form, object[]? data)
                {
                    form.Close();
                });
            }
        }
        #endregion //Events

        #region Sequential Execution
        private bool ServerCommunication(SequentialExecution self)
        {
            self.SetOutput("Loading Server Config.");
            LocalConfig config = new("Server");
            if (!config.Exists)
            {
                ServerConfiguration.Form form = new();
                self.SetOutput("Starting Setup");
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (form.Set == null || !config.Save(form.Set))
                    {
                        throw new NotImplementedException("Error saving config.");
                    }
                }
                else if(result == DialogResult.Abort)
                {
                    Application.Exit();
                }
            }
            if(!config.Exists)
            {
                self.SetOutput("Configuration not finished.");
                return false;
            }

            ServerConfigurationSet? set = config.Load<ServerConfigurationSet>();
            if(set == null || set.Server == null)
            {
                throw new NotImplementedException("Error loading config.");
            }

            self.SetOutput("Setup connection.");
            ApplicationState.Client = new AuthenticatedModletClient(set.Server, set.Port);
            try
            {
                if(ApplicationState.Client.Send(ModletCommands.Commands.Connect, null) == null)
                {
                    return false;
                }
                self.SetOutput("Get Transmission Key.");

                byte[]? key = ApplicationState.Client.Send(ASCIIEncoding.UTF8.GetBytes(Dns.GetHostName()), ModletCommands.Commands.Serverkey, null);
                if(key == null)
                {
                    return false;
                }
                ApplicationState.Client.Aes = key.AsString();

                self.SetOutput("Key received, enableling aes.");
                return true;
            }
            catch(Exception)
            {
                self.IsValid = false;
                self.SetOutput("Connection Error.");
                return false;
            }
        }

        private bool ModuleLoader(SequentialExecution self)
        {
            IModlet[] list = Modlet.Load<IMainFormModlet>(self);
            foreach (IModlet mod in list)
            {
                mod.OnClientConfiguration(this);
            }
            self.SetOutput("Loaded " + list.Length + " module(s).");

            return true;
        }

        private bool Startup(SequentialExecution self)
        {
            Invoker<Splash>.Invoke(this, delegate(Splash control, object[]? data)
            {
                control.TopMost = false;
            });
            return true;
        }
        #endregion //Sequential Execution
    }
}
