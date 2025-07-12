using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models
{
    [TsClass]
    public class CustomFieldsVM
    {
        public string FieldText { get; set; }
        public string Value { get; set; }
        public string RelatedID { get; set; }
        public bool AllowAllEntities { get; set; }
        public int FieldType { get; set; }
    }
}