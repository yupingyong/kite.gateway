using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class MyThemeViewModel
    {
        public int TotalCount { get; set; }
        public List<ThemeDataModel> ListData { get; set; }
    }
}
