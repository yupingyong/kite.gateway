using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Areas.Account.Models
{
    public class MyMessageViewModel
    {
        public int TotalCount { get; set; }
        public List<MessageModel> ListData { get; set; }
    }
}
