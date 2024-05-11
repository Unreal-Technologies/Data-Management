using Microsoft.EntityFrameworkCore;
using Shared.Modules;
using Shared.Modules.Dtos;
using UT.Data.Efc;
using UT.Data.Modlet;

namespace Shared.Efc
{
    public class DataHandler
    {
        #region Enums
        public enum SharedActions
        {
            UploadContentByContent, ListContentDtoByUserId, SelectContentById, DeleteContentById
        }
        #endregion //Enums

        #region Public Methods
        public static byte[]? OnLocalServerAction(byte[]? stream, IModlet mod, ServerContext? serverContext)
        {
            DbContext? modContext = serverContext?.Select(mod);
            SharedActions? action = ModletStream.GetInputType<SharedActions>(stream);
            if (modContext is not SharedModContext smc || action == null)
            {
                return null;
            }

            switch (action)
            {
                case SharedActions.DeleteContentById:
                    return OnLocalServerAction_DeleteContentById(smc, stream);
                case SharedActions.UploadContentByContent:
                    return OnLocalServerAction_UploadContentByContent(smc, stream);
                case SharedActions.ListContentDtoByUserId:
                    return OnLocalServerAction_ListContentDtoByUserId(smc, stream);
                case SharedActions.SelectContentById:
                    return OnLocalServerAction_SelectContentById(smc, stream);
                default:
                    break;
            }
            return null;
        }
        #endregion //Public Methods

        #region Private Methods
        private static byte[]? OnLocalServerAction_DeleteContentById(SharedModContext smc, byte[]? stream)
        {
            Guid contentId = ModletStream.GetContent<SharedActions, Guid>(stream);
            if (contentId == Guid.Empty)
            {
                return null;
            }

            Tables.Content? content = smc.Contents.FirstOrDefault(x => x.Id == contentId);
            if(content != null)
            {
                smc.Remove(content);
                smc.SaveChanges();
            }

            return null;
        }

        private static byte[]? OnLocalServerAction_ListContentDtoByUserId(SharedModContext smc, byte[]? stream)
        {
            Guid userId = ModletStream.GetContent<SharedActions, Guid>(stream);
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

        private static byte[]? OnLocalServerAction_SelectContentById(SharedModContext smc, byte[]? stream)
        {
            Guid contentId = ModletStream.GetContent<SharedActions, Guid>(stream);
            if (contentId == Guid.Empty)
            {
                return null;
            }

            Tables.Content? content = smc.Contents.FirstOrDefault(x => x.Id == contentId);
            return ModletStream.CreatePacket(true, content);
        }

        private static byte[]? OnLocalServerAction_UploadContentByContent(SharedModContext smc, byte[]? stream)
        {
            Tables.Content? content = ModletStream.GetContent<SharedActions, Tables.Content>(stream);
            if (content == null)
            {
                return null;
            }
            Guid userId = content.User?.Id ?? Guid.Empty;

            if (smc.Contents.Any(x =>
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
        #endregion //Private Methods
    }
}
