using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.Efc;
using Shared.Efc.Tables;
using Shared.Interfaces;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MaxValue)]
    public partial class Content : ExtendedMdiModletForm
    {
        #region Members
        private States state;
        #endregion //Members

        #region Constructors
        public Content()
            : base()
        {
            Text = "Content";
            InitializeComponent();
        }
        #endregion //Constructors

        #region Enums
        public enum States
        {
            Upload, List
        }

        private enum Actions
        {
            Upload
        }
        #endregion //Enums

        #region Public Methods
        public override void OnMenuCreation()
        {
            if (Root is IMainFormModlet && Root is IMainMenuContainer mmc)
            {
                mmc.MenuStack.Add(this, ["Application", "Content"], OpenEdit);
            }
        }

        public void SetState(States state)
        {
            this.state = state;
            UpdateState();
        }
        #endregion //Public Methods

        #region Private Methods
        private void OpenEdit()
        {
            Content? cu = ShowMdi<Content>();
            cu?.SetState(States.List);
        }

        private void UpdateState()
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;

            if (InfoBar == null)
            {
                return;
            }

            switch (state)
            {
                case States.Upload:
                    InfoBar.Subtitle = "* Upload";
                    tabControl.SelectTab(tabPage_upload);
                    RenderUpload();
                    break;
                case States.List:
                    InfoBar.Subtitle = "* Select Content";
                    tabControl.SelectTab(tabPage_list);
                    break;
            }
        }

        public override byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            DbContext? modContext = Context?.Select(this);
            Actions? action = ModletStream.GetInputType<Actions>(stream);
            if (modContext is not SharedModContext smc || action == null)
            {
                return null;
            }

            switch (action)
            {
                case Actions.Upload:
                    return OnLocalServerAction_Upload(smc, stream);
                default:
                    break;
            }
            return null;
        }

        public static byte[]? OnLocalServerAction_Upload(SharedModContext smc, byte[]? stream)
        {
            Efc.Tables.Content? content = ModletStream.GetContent<Actions, Efc.Tables.Content>(stream);
            if(content == null)
            {
                return null;
            }
            Guid userId = content.User?.Id ?? Guid.Empty;

            content.User = smc.Users.FirstOrDefault(x => x.Id == userId);

            smc.Add(content);
            smc.SaveChanges();


            return ModletStream.CreatePacket(true, content);
        }

        private void RenderUpload()
        {
            if (ofdUpload.ShowDialog() != DialogResult.OK)
            {
                DialogResult = DialogResult.Abort;
                Close();
            }
            string path = ofdUpload.FileName;
            Efc.Tables.Content.Types type = DetermenType(path);

            RenderUpload(path, type);
        }

        private void RenderUpload(string path, Efc.Tables.Content.Types type)
        {
            tabPage_upload_vtb_description.Control.Text = path.Md5();
            tabPage_upload_lbl_type_v.Text = type.ToString();
            tabPage_upload_pb_image.Visible = false;
            tabPage_upload_tb_path.Text = path;

            switch (type)
            {
                case Efc.Tables.Content.Types.Image:
                    tabPage_upload_pb_image.Image = Bitmap.FromFile(path);
                    tabPage_upload_pb_image.Visible = true;
                    tabPage_upload_pb_image.SizeMode = PictureBoxSizeMode.StretchImage;
                    break;
                default:
                    break;
            }
        }

        private void TabPage_upload_btn_save_Click(object sender, EventArgs e)
        {
            Efc.Tables.Content.Types? type = tabPage_upload_lbl_type_v.Text.AsEnum<Efc.Tables.Content.Types>();
            if (type == null)
            {
                return;
            }

            Validator validator = new();
            validator.Add(tabPage_upload_vtb_description);
            validator.Validate();
            if (
                validator.IsValid &&
                Session != null &&
                Session.TryGetValue("User-Authentication", out object? value) &&
                value is User user
            )
            {
                Efc.Tables.Content input = new()
                {
                    Description = tabPage_upload_vtb_description.Control.Text,
                    Type = type.Value,
                    User = user,
                    Stream = File.ReadAllBytes(tabPage_upload_tb_path.Text).ToBase64().AsString()
                };
                Efc.Tables.Content? output = ModletStream.GetContent<bool, Efc.Tables.Content>(
                    Client?.Send(
                    ModletStream.CreatePacket(
                            Actions.Upload,
                            input
                        ),
                        ModletCommands.Commands.Action,
                        this
                    )
                );
                if(output != null)
                {
                    SetState(States.List);
                }
            }
        }

        private void TabPage_upload_btn_changeFile_Click(object sender, EventArgs e)
        {
            RenderUpload();
        }

        private static Efc.Tables.Content.Types DetermenType(string path)
        {
            try
            {
                using Image img = Bitmap.FromFile(path);
                return Efc.Tables.Content.Types.Image;
            }
            catch (Exception)
            {
                return Efc.Tables.Content.Types.Undefined;
            }
        }
        #endregion //Private Methods
    }
}
