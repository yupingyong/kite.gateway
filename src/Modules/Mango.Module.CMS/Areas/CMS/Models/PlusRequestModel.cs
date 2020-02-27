using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.CMS.Areas.CMS.Models
{
    public class PlusRequestModel
    {
        public int ContentsId { get; set; }
        public string Title { get; set; }
        public int ToAccountId { get; set; }
    }
}
