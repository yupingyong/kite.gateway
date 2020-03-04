using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class ThemeViewModel
    {
        public int TotalCount { get; set; }
        public List<ThemeDataModel> ThemeListData { get; set; }
    }
}
