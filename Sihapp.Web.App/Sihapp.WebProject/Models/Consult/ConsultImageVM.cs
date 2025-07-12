using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Consult
{

    [TsClass]
    public class ConsultImageVM
    {
        public DateTime CreationDate { get; internal set; }
        public string Description { get; internal set; }
        public Guid ID { get; internal set; }
        public byte[] Image { get; internal set; }
    }
}