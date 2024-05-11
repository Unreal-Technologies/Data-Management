using Shared.Controls;
using Shared.Efc;
using Shared.Efc.Tables;
using Shared.Interfaces;
using Shared.Modules.Dtos;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Controls.Custom;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MaxValue)]
    public partial class Content : ExtendedMdiModletForm
    {
        #region Members
        private States state;
        private Gridview<ContentDto>? gridviewList;
        private Guid tempContentId = Guid.Empty;
        #endregion //Members

        #region Constructors
        public Content()
            : base()
        {
            Text = "Content";
            InitializeComponent();

            Load += Content_Load;
        }
        #endregion //Constructors

        #region Enums
        public enum States
        {
            Upload, List, Delete
        }
        #endregion //Enums

        #region Constants
        private const string Description = "Description";
        private const string Extension = "Extension";
        private const string Type = "Type";
        private const string LastUpdate = "Last Update";
        #endregion //Constants

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

        public override byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            return DataHandler.OnLocalServerAction(stream, this, Context);
        }
        #endregion //Public Methods

        #region Private Methods
        private void Content_Load(object? sender, EventArgs e)
        {
            gridviewList = new Gridview<ContentDto>(x => x.Id);
            gridviewList.SetColumns([
                gridviewList.Column(Description, x => x.Description ?? "", Sorting.Container),
                gridviewList.Column(Extension, x => x.Extension ?? "", Sorting.Container),
                gridviewList.Column(Type, x => x.Type.ToString(), Gridview.Alignment.Center, Sorting.Container),
                gridviewList.Column(LastUpdate, x => x.TransStartDate.ToString("dd-MM-yyyy HH:mm"), Sorting.Container)
            ]);

            gridviewList.OnAdd += OnAdd;
            //gridviewList.OnEdit += OnEdit;
            gridviewList.OnRemove += OnRemove;

            tabPage_list.Controls.Add(gridviewList);
        }

        private void OnRemove(Guid? id)
        {
            if (id != null)
            {
                tempContentId = id.Value;
                SetState(States.Delete);
                tempContentId = Guid.Empty;
            }
        }

        private void OnAdd(Guid? id)
        {
            SetState(States.Upload);
        }

        private void OpenEdit()
        {
            Content? cu = ShowMdi<Content>();
            cu?.SetState(States.List);
        }

        private void ClearControls()
        {
            tabPage_upload_vtb_description.Control.Text = "";
            tabPage_upload_lbl_extension_v.Text = "";
            tabPage_upload_lbl_type_v.Text = "";
        }

        private void UpdateState()
        {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;

            ClearControls();

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
                    RenderList();
                    break;
                case States.Delete:
                    InfoBar.Subtitle = "Delete: " + tempContentId.ToString();
                    tabControl.SelectTab(tabPage_delete);
                    RenderDelete();
                    break;
            }
        }

        private void RenderDelete()
        {
            if (InfoBar == null)
            {
                return;
            }

            Efc.Tables.Content? content = Request<Efc.Tables.Content, DataHandler.SharedActions, Guid>(
                DataHandler.SharedActions.SelectContentById, 
                tempContentId
            );

            if (content == null)
            {
                return;
            }

            tabPage_delete_lbl_message.Text = string.Format(tabPage_delete_lbl_message.Text, content.Description);
            tabPage_delete_tb_id.Text = tempContentId.ToString();

            InfoBar.Subtitle = tabPage_delete_lbl_message.Text;
        }

        private void RenderList()
        {
            if (gridviewList == null)
            {
                return;
            }

            if (
                Session != null &&
                Session.TryGetValue("User-Authentication", out object? value) &&
                value is User user
            )
            {
                ContentDto[]? content = Request<ContentDto[], DataHandler.SharedActions, Guid>(
                    DataHandler.SharedActions.ListContentDtoByUserId, 
                    user.Id
                );
                if (content != null)
                {
                    gridviewList.Dataset(content);
                }
            }
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
            if (path == string.Empty)
            {
                return;
            }
            FileInfo fi = new(path);

            tabPage_upload_vtb_description.Control.Text = fi.ShortName();
            tabPage_upload_lbl_type_v.Text = type.ToString();
            tabPage_upload_pb_image.Visible = false;
            tabPage_upload_tb_path.Text = path;
            tabPage_upload_lbl_extension_v.Text = fi.Extension;

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
                FileInfo fi = new(tabPage_upload_tb_path.Text);
                Efc.Tables.Content input = new()
                {
                    Description = tabPage_upload_vtb_description.Control.Text,
                    Extension = fi.Extension,
                    Type = type.Value,
                    User = user,
                    Stream = File.ReadAllBytes(fi.FullName).ToBase64().AsString()
                };

                byte[]? result = Client?.Send(
                ModletStream.CreatePacket(
                        DataHandler.SharedActions.UploadContentByContent,
                        input
                    ),
                    ModletCommands.Commands.Action,
                    this
                );
                bool stateOk = ModletStream.GetKey<bool, object>(result);
                if (stateOk)
                {
                    Efc.Tables.Content? output = ModletStream.GetContent<bool, Efc.Tables.Content>(result);
                    if (output != null)
                    {
                        SetState(States.List);
                    }
                }
                else
                {
                    string? message = ModletStream.GetContent<bool, string>(result);
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                using Image img = Image.FromFile(path);
                return Efc.Tables.Content.Types.Image;
            }
            catch (Exception)
            {
                return Efc.Tables.Content.Types.Undefined;
            }
        }

        private void TabPage_delete_btn_no_Click(object sender, EventArgs e)
        {
            SetState(States.List);
        }

        private void TabPage_delete_btn_yes_Click(object sender, EventArgs e)
        {
            Guid contentId = Guid.Parse(tabPage_delete_tb_id.Text);
            ModletStream.GetContent<bool, Efc.Tables.Content>(
                Client?.Send(
                    ModletStream.CreatePacket(
                        DataHandler.SharedActions.DeleteContentById,
                        contentId
                    ),
                    ModletCommands.Commands.Action,
                    this
                )
            );
            SetState(States.List);
        }
        #endregion //Private Methods

        #region Classes
        private sealed class Sorting : Gridview<ContentDto>.GvSorting
        {
            #region Public Methods
            public static void Container(object? sender, EventArgs e)
            {
                var data = Data(sender, e);
                switch (data.Item2.Text)
                {
                    case Description:
                        data.Item1.Data = data.Item1.Data?.OrderBy(x => x.Description, data.Item4);
                        break;
                    case Extension:
                        data.Item1.Data = data.Item1.Data?.OrderBy(x => x.Extension, data.Item4);
                        break;
                    case Type:
                        data.Item1.Data = data.Item1.Data?.OrderBy(x => x.Type, data.Item4);
                        break;
                    case LastUpdate:
                        data.Item1.Data = data.Item1.Data?.OrderBy(x => x.TransStartDate, data.Item4);
                        break;
                    default:
                        break;
                }
            }
            #endregion //Public Methods
        }
        #endregion //Classes
    }
}
