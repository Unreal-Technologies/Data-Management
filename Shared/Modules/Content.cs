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
        private GridviewGuid? gridviewList;
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
            Upload, List
        }

        private enum Actions
        {
            Upload, ListContent
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
                case Actions.ListContent:
                    return OnLocalServerAction_ListContent(smc, stream);
                default:
                    break;
            }
            return null;
        }
        #endregion //Public Methods

        #region Private Methods
        private static byte[]? OnLocalServerAction_ListContent(SharedModContext smc, byte[]? stream)
        {
            Guid userId = ModletStream.GetContent<Actions, Guid>(stream);
            if (userId == Guid.Empty)
            {
                return null;
            }

            ContentDto[] contents = [.. smc.Contents
                .Where(x => x.User != null && x.User.Id == userId)
                .OrderByDescending(x => x.TransStartDate)
                .Select(x => new ContentDto()
                {
                    Description = x.Description,
                    Id = x.Id,
                    TransStartDate = x.TransStartDate,
                    Type = x.Type,
                    Extension = x.Extension
                }
            )];

            return ModletStream.CreatePacket(true, contents);
        }

        private void Content_Load(object? sender, EventArgs e)
        {
            gridviewList = new GridviewGuid();
            gridviewList.SetColumns([
                new GridviewGuid.Column()
                {
                    Text = "Description"
                },
                new GridviewGuid.Column()
                {
                    Text = "Extension"
                },
                new GridviewGuid.Column()
                {
                    Text = "Type"
                },
                new GridviewGuid.Column()
                {
                    Text = "Last Update"
                }
            ]);

            gridviewList.OnAdd += OnAdd;
            //gridviewList.OnEdit += OnEdit;
            //gridviewList.OnRemove += OnRemove;

            tabPage_list.Controls.Add(gridviewList);
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
            }
        }

        private Tdata? Request<Tdata, Tkey, Tinput>(Tkey key, Tinput input)
            where Tkey: struct
        {
            Tdata? result = ModletStream.GetContent<bool, Tdata>(
                Client?.Send(
                    ModletStream.CreatePacket(
                        key,
                        input
                    ),
                    ModletCommands.Commands.Action,
                    this
                )
            );
            return result;
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
                ContentDto[]? content = Request<ContentDto[], Actions, Guid>(Actions.ListContent, user.Id);
                if(content != null)
                {
                    gridviewList.Clear();
                    foreach(ContentDto dto in content)
                    {
                        Gridview<Guid>.Row row = new()
                        {
                            ID = dto.Id
                        };
                        row.Cells.Add(new Gridview<Guid>.Cell()
                        {
                            Text = dto.Description
                        });
                        row.Cells.Add(new Gridview<Guid>.Cell()
                        {
                            Text = dto.Extension
                        });
                        row.Cells.Add(new Gridview<Guid>.Cell()
                        {
                            Text = dto.Type.ToString()
                        });
                        row.Cells.Add(new Gridview<Guid>.Cell()
                        {
                            Text = dto.TransStartDate.ToString("dd-MM-yyyy HH:mm")
                        });

                        gridviewList.AddRow(row);
                    }
                    gridviewList.Render();
                }
            }
        }

        private static byte[]? OnLocalServerAction_Upload(SharedModContext smc, byte[]? stream)
        {
            Efc.Tables.Content? content = ModletStream.GetContent<Actions, Efc.Tables.Content>(stream);
            if(content == null)
            {
                return null;
            }
            Guid userId = content.User?.Id ?? Guid.Empty;

            if(smc.Contents.Any(x => 
                x.Description == content.Description && 
                x.Extension == content.Extension && 
                x.User != null && 
                x.User.Id == userId
            ))
            {
                return ModletStream.CreatePacket(
                    false, 
                    string.Format("Description \"{0}\" is already in use.", content.Description)
                );
            }

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
                        Actions.Upload,
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
        #endregion //Private Methods

        #region Classes
        private sealed class ContentDto
        {
            public Guid Id { get; set; }
            public string? Description { get; set; }
            public string? Extension { get; set; }
            public Efc.Tables.Content.Types Type { get; set; }
            public DateTime TransStartDate { get; set; }
        }
        #endregion //Classes
    }
}
