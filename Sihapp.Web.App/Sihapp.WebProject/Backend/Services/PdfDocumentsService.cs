using iTextSharp.text;
using iTextSharp.text.pdf;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class PdfDocumentsService : BasicService
    {

        public void InsertExpedientHeader(Patients patient, Document doc, Font smallFont)
        {
            InsertHeaderImg(doc);
            var today = DateTime.Now;
            var expedientTitle = "Expediente " + patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            int age = (int)new PatientService().DiffYears(patient.Birthdate, today);
            var edad = "Edad: " + age;
            var fechaNacimiento = "Fecha de nacimiento:" + patient.Birthdate.ToString("dd/MM/yyyy");
            var expedientDate = "Fecha de actualización de expediente: " + today.ToString();
            var expedientSubtitle = "Clinica o consultorio: " + GetLoggedCurrentClinic().Name;
            var clinicParagraph = new Paragraph(expedientSubtitle, smallFont);
            clinicParagraph.Alignment = Element.ALIGN_CENTER;
            // doc.Add(new Paragraph(titelPragraph));
            doc.Add(clinicParagraph);
            doc.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase(expedientTitle));
            cell.Colspan = 2;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            table.AddCell(cell);
            table.AddCell(edad);
            table.AddCell(fechaNacimiento);

            table.AddCell(expedientDate);
            table.AddCell(patient.Email);

            doc.Add(table);
         


            //var titelPragraph = new Paragraph(expedientTitle);
            //titelPragraph.Alignment = Element.ALIGN_CENTER;



            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);
           
            
            doc.Add(Chunk.NEWLINE);
        }

        public static void InsertHeaderImg(Document doc)
        {
            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance("sihappOficial.png");
            ////Resize image depend upon your need
            //jpg.ScaleToFit(70f, 50f);

            ////Give space before image

            //jpg.SpacingBefore = 10f;
            //jpg.Border = 0;

            ////Give some space after the image

            //jpg.SpacingAfter = 1f;
            //jpg.Alignment = Element.ALIGN_CENTER;
            //jpg.ScalePercent(25f);
            //jpg.BorderWidth = 0;
            //jpg.SpacingBefore = 0;
            //jpg.Border = 0;
            //jpg.BorderWidthLeft = 0;

            //doc.Add(jpg);
        }

        public void InsertRemissionHeader(Remission remission, Document doc, Font smallFont)
        {
            InsertHeaderImg(doc);
            var today = DateTime.Now;
            var expedientTitle = "Nota " + remission.Number;
            var fechaEmision = "Emision:" + remission.CreationDate.ToString("dd/MM/yyyy hh:mm:ss tt");
            var expedientDate = "Fecha de impresión: " + today.ToString();
            var expedientSubtitle = "Clinica o consultorio: " + GetLoggedCurrentClinic().Name;
            var clinicParagraph = new Paragraph(expedientSubtitle, smallFont);
            clinicParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(clinicParagraph);
            doc.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase(expedientTitle));
            cell.Colspan = 2;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            table.AddCell(cell);
            table.AddCell(expedientDate);
            table.AddCell(fechaEmision);

            var patient = remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;

            table.AddCell(patient.Email);
            table.AddCell(patientName);

            doc.Add(table);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);


            doc.Add(Chunk.NEWLINE);
        }

        public void InsertPaymentHeader(Remission remission,RemissionPayment payment, Document doc, Font smallFont)
        {
            InsertHeaderImg(doc);
            var today = DateTime.Now;
            var expedientTitle = "Pago de Nota " + remission.Number;
            var fechaEmision = "Emision de pago:" + payment.CreationDate.ToString("dd/MM/yyyy hh:mm:ss tt");
            var expedientDate = "Fecha de impresión: " + today.ToString("dd/MM/yyyy hh:mm:ss tt");
            var expedientSubtitle = "Clinica o consultorio: " + GetLoggedCurrentClinic().Name;
            var clinicParagraph = new Paragraph(expedientSubtitle, smallFont);
            clinicParagraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(clinicParagraph);
            doc.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase(expedientTitle));
            cell.Colspan = 2;
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

            table.AddCell(cell);
            table.AddCell(expedientDate);
            table.AddCell(fechaEmision);

            var patient = remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;

            table.AddCell(patient.Email);
            table.AddCell(patientName);

            doc.Add(table);

            doc.Add(Chunk.NEWLINE);
            doc.Add(Chunk.NEWLINE);


            doc.Add(Chunk.NEWLINE);
        }
    }
}