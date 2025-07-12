using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Common
{
    [TsClass]
    public class TextValueVM
    {

        public TextValueVM()
        {

        }

        public TextValueVM(string text, object value)
        {
            this.Text = text;
            this.Value = value;
        }
        public string Text { get; set; }
        public object Value { get; set; }
    }
}