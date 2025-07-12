using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.CustomFields;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class CustomFieldsService : BasicService
    {

        public void AddCustomFieldsToEntity(Guid relatedEntityID, int entityType, List<CustomFieldsVM> customFields)
        {
            foreach (var cf in customFields)
            {
                ValidateCustomField(cf);
                var customFieldInDB = new CustomFields();
                customFieldInDB.AllowAllEntities = true;
                customFieldInDB.ClinicID = GetLoggedCurrentClinic().ID;
                customFieldInDB.CreationDate = DateTime.Now;
                customFieldInDB.EntityType = entityType;
                customFieldInDB.Id = Guid.NewGuid();
                customFieldInDB.IsActive = true;
                customFieldInDB.FieldText = cf.FieldText;
                customFieldInDB.FieldType = cf.FieldType;
                //customFieldInDB.Value = cf.Value;
                //customFieldInDB.RelatedID = relatedEntityID;
                DatabaseInstance.CustomFields.Add(customFieldInDB);
                DatabaseInstance.SaveChanges();
                AddCustomFieldValues(customFieldInDB.Id, relatedEntityID, cf.Value);
            }
        }

        private void AddCustomFieldValues(Guid customFieldID, Guid relatedEntityID, string value)
        {
            var customFieldValuesDB = new CustomFieldValues();
            customFieldValuesDB.Id = Guid.NewGuid();
            customFieldValuesDB.RelatedEntityID = relatedEntityID;
            customFieldValuesDB.CustomFieldId = customFieldID;
            customFieldValuesDB.Value = value;
            DatabaseInstance.CustomFieldValues.Add(customFieldValuesDB);
            DatabaseInstance.SaveChanges();
        }

        private void ValidateCustomField(CustomFieldsVM cf)
        {

            if (string.IsNullOrWhiteSpace(cf.FieldText))
                throw new Exception("No se definió el nombre del campo");

            if (string.IsNullOrWhiteSpace(cf.Value))
                throw new Exception("No se definió el valor del campo");
            //switch (cf.FieldType)
            //{
            //    case CustomFieldTypesCatalog.Number:

            //}
        }

        public List<ExtraFieldsVM> GetPatientCustomFieldsByClinic()
        {
            var clinicId = GetLoggedCurrentClinic().ID;

            var fieldsAlreadySet = new List<ExtraFieldsVM>();

            var customFields = DatabaseInstance.CustomFields.Where(w => w.ClinicID == clinicId);

            if (customFields != null && customFields.Count() > 0)
            {
                var customFieldsDTO = customFields.Select(s => new ExtraFieldsVM
                {
                    ClinicID = s.ClinicID,
                    FieldText = s.FieldText,
                    FieldType = s.FieldType,
                    Id = s.Id,
                    Value = "",
                }).ToList();

                fieldsAlreadySet = customFieldsDTO;
            }

            return fieldsAlreadySet;

        }
    }
}