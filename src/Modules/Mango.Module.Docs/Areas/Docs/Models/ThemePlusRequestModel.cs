using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class ThemePlusRequestModel
    {
        public int ThemeId { get; set; }
        public int ToAccountId { get; set; }
        public string Title { get; set; }
    }
}
