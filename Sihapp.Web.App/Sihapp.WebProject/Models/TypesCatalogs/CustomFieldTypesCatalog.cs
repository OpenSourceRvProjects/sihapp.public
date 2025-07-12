using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Models.TypesCatalogs
{
    public class CustomFieldTypesCatalog
    {
        public const int Number = 0;
        public const int TextBox = 1;
        public const int Bool = 2;
        public const int Dropdown = 3;


        public List<TextValueVM> GetAllCustomFieldTypes()
        {
            var allValues = new List<TextValueVM>();
            allValues.Add(new TextValueVM("Número", Number));
            allValues.Add(new TextValueVM("Caja de texto", TextBox));
            allValues.Add(new TextValueVM("Si/No", Bool));
            allValues.Add(new TextValueVM("Desplegable", Dropdown));
            return allValues;
        }
    }

}