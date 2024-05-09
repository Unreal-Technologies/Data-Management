using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Modules.Dtos
{
    internal sealed class ContentDto
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Extension { get; set; }
        public Efc.Tables.Content.Types Type { get; set; }
        public DateTime TransStartDate { get; set; }
    }
}
