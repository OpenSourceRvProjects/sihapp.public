using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.CustomFields
{

    [TsClass]
    public class ExtraFieldsVM
    {
        public Guid ClinicID { get; internal set; }
        public int FieldType { get; internal set; }
        public string FieldText { get; internal set; }
        public Guid Id { get; internal set; }
        public string Value { get; internal set; }
    }
}