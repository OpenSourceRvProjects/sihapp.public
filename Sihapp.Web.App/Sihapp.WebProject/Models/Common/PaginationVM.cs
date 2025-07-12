using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Common
{
    [TsClass]
    public class PaginationVM
    {
        public int Page { get; set; }
        public int PageSize  { get; set; }
        public string SearchTerm { get; set; }
    }
}