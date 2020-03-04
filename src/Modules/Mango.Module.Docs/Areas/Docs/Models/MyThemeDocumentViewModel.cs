using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Docs.Areas.Docs.Models
{
    public class MyThemeDocumentViewModel
    {
        public int TotalCount { get; set; }
        public List<DocumentDataModel> ListData { get; set; }
    }
}
