using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.RemissionNotes;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models.TypesCatalogs;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class RemissionNotesService : BasicService
    {


        public IQueryable<Remission> GetRemissionsByLoggedClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.Remission.Where(w => w.Consult.Appointment.ClinicID == clinicID);
        }


        public PagedRemissionNotes GetPagedRemisionNotes(PaginationVM pagination, DateTime? startDate, DateTime? endDate, Guid? patientID)
        {
            var pagedRemNotes = new PagedRemissionNotes();
            
            var clinicID = GetLoggedCurrentClinic().ID;
            var remissionsQry = DatabaseInstance.Remission.Where(w => w.ClinicID == clinicID);

            if (startDate.HasValue)
                remissionsQry = remissionsQry.Where(w => w.CreationDate >= startDate);

            if (endDate.HasValue)
            {
                var realEndDate = endDate.Value.AddDays(1).AddMilliseconds(-1);
                remissionsQry = remissionsQry.Where(w => w.CreationDate <= realEndDate);
            }

            if (patientID.HasValue)
                remissionsQry = remissionsQry.Where(w => w.PatientID == patientID);

            var remissionList = remissionsQry.OrderByDescending(o => o.CreationDate).Select(s => new RemissionNotesItems
            {
                ClinicMen = s.Consult.Appointment.ClinicMen.Name + " " + s.Consult.Appointment.ClinicMen.LastName1 + " " + s.Consult.Appointment.ClinicMen.LastName2,
                PatientName = s.Consult.Appointment.Patients.Name + " " + s.Consult.Appointment.Patients.LastName1 + " " + s.Consult.Appointment.Patients.LastName2,
                CreationDate = s.CreationDate,
                Payed = s.Payed,
                Pending = s.GrandTotal - s.Payed,
                GrandTotal = s.GrandTotal,
                ConsultNumber = s.Consult.Number.ToString(),
                Number = s.Number,
                ID = s.Id,
            });

            var allItemsCount = remissionList.Count();
            pagedRemNotes.TotalPages = Math.Ceiling((decimal)(allItemsCount / pagination.PageSize));
            pagedRemNotes.RemissionList = remissionList.Skip(pagination.Page * pagination.PageSize).Take(15).ToList();
            pagedRemNotes.TotalGrandAmount = remissionList.Count() > 0 ? remissionList.Sum(s => s.GrandTotal) : 0m;
            pagedRemNotes.TotalPayed = remissionList.Count() > 0 ?  remissionList.Sum(s => s.Payed): 0m;
            pagedRemNotes.TotalPending = pagedRemNotes.TotalGrandAmount - pagedRemNotes.TotalPayed;
            return pagedRemNotes;
        }

        public PatientRemissionNoteBackendDataVM GetRemissonInfoByID(Guid remissionID)
        {
            var remission = new PatientRemissionNoteBackendDataVM();
            remission.RemissionID = remissionID;
            var remissionInDB = GetRemissionsByLoggedClinic().Where(w => w.Id == remissionID).FirstOrDefault();
            if (remissionInDB == null)
                throw new Exception("La remisión solicitada no fue encontrada, favor de no copiar y pegar los vinvulos");
            var patient = new PatientService().GetPatientByID(remissionInDB.PatientID);
            var clinicMen = new ClinicMenService().GetClinicMenByID(remissionInDB.ClinicMenID);

            remission.PatientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            remission.ClinicManName = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;

            remission.RemissionNumber = remissionInDB.Number;
            remission.CreationDate = remissionInDB.CreationDate;
            remission.ConsultNumber = remissionInDB.Consult.Number.ToString();
            remission.GrandTotal = remissionInDB.GrandTotal;
            remission.Payed = remissionInDB.Payed;
            remission.Pending = remissionInDB.GrandTotal - remissionInDB.Payed;
            remission.Notes = remissionInDB.Notes;
            remission.RemissionID = remissionInDB.Id;


            return remission;
        }

        public void UpdateRemissionAfterPayment(RemissionPayment remissionPmt, bool isDeletedPayment = false)
        {
            var remission = remissionPmt.Remission;
            remission.Payed = isDeletedPayment? remission.Payed - remissionPmt.PaymentAmount:  remission.Payed + remissionPmt.PaymentAmount;
            if (remission.Payed == remission.GrandTotal)
                remission.Status = RemissionStatusCodes.Payed;

            DatabaseInstance.SaveChanges();
        }

        public void AddPatientPayment(RemissionNotesItems remission, string comments, DateTime date)
        {
            var ammountToPay = remission.Pending; // be carefull, dont use this var any more

            var currentRemission = GetRemissionsByLoggedClinic().Where(w => w.Id == remission.ID).FirstOrDefault();
            if (currentRemission == null)
                throw new Exception("No se encontró la remision: " + remission.ID + " ClinicID: " + GetLoggedCurrentClinic().ID);

            if (ammountToPay > (currentRemission.GrandTotal - currentRemission.Payed))
                throw new Exception("El monto a pagar supera al monto pendiente de la nota");

            if ((currentRemission.GrandTotal - currentRemission.Payed <= 0))
                throw new Exception("Esta nota ha sido pagada en su totalidad");

            RemissionPayment newPmt = new RemissionPayment();
            newPmt.Id = Guid.NewGuid();
            newPmt.RemissionID = currentRemission.Id;
            newPmt.PaymentAmount = ammountToPay;
            newPmt.ClinicID = GetLoggedCurrentClinic().ID;
            newPmt.CreationDate = date;
            newPmt.Comments = comments;
            newPmt.ExchangeRate = 1.0m;

            DatabaseInstance.RemissionPayment.Add(newPmt);
            DatabaseInstance.SaveChanges();

            UpdateRemissionAfterPayment(newPmt);
        }

        public MemoryStream GetPDFPayment(Guid id)
        {
            var payment = GetRemissionPaymentByID(id);
            var remission = payment.Remission;

            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;

            doc.AddTitle("Pago de Nota #" + remission.Number);
            doc.AddCreator("Sihapp Team");

            doc.Open();
            iTextSharp.text.Font smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            new PdfDocumentsService().InsertPaymentHeader(remission, payment, doc, smallFont);

            var patient = remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            var clinicMen = remission.Consult.Appointment.ClinicMen;
            var clinicMenName = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;

            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("_____________________________________________________________________________"));

            doc.Add(new Paragraph("Concepto: " + payment.Comments));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------"));

            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("                                                                                                                          Pago : $ " + payment.PaymentAmount.ToString("#0.00")));
            doc.Add(new Paragraph("                                                                                                                 Total de Nota : $ " + payment.Remission.GrandTotal.ToString("#0.00")));



            doc.Close();
            return workStream;

        }

        public MemoryStream GetPDFRemissionNote(Guid id)
        {
            var remission = GetRemissionsByLoggedClinic().FirstOrDefault(f => f.Id == id);

            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;

            doc.AddTitle("Nota #" + remission.Number);
            doc.AddCreator("Sihapp Team");

            doc.Open();
            iTextSharp.text.Font smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            new PdfDocumentsService().InsertRemissionHeader(remission, doc, smallFont);
            var clinicMen = remission.Consult.Appointment.ClinicMen;
            var clinicMenName = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;


            doc.Add(new Paragraph("Profesional de salud: " + clinicMenName));
            doc.Add(new Paragraph("Notas: " + remission.Notes));
            doc.Add(new Paragraph("# Consulta: " + remission.Consult.Number));

            var patient = remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;


            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("_____________________________________________________________________________"));

            doc.Add(new Paragraph("Concepto: Nota por consulta # " + remission.Consult.Number +
                 " de " + patientName + " con " + clinicMenName + " en la fecha " + remission.Consult.ConsultStartTime));
            doc.Add(Chunk.NEWLINE);
            doc.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------"));

            doc.Add(Chunk.NEWLINE);

            doc.Add(new Paragraph("                                                                                                                          Total: $ " + remission.GrandTotal.ToString("#0.00")));
            doc.Add(new Paragraph("                                                                                                                         Pagado: $ " + remission.Payed.ToString("#0.00")));
            doc.Add(new Paragraph("                                                                                                                      Pendiente: $ " + (remission.GrandTotal - remission.Payed).ToString("#0.00")));

            doc.Close();
            return workStream;
        }

        public RemissionPayment GetRemissionPaymentByID(Guid PaymentID)
        {

            var clinicID = GetLoggedCurrentClinic().ID;
            var remissionPmt = DatabaseInstance.RemissionPayment.FirstOrDefault(f => f.ClinicID == clinicID && f.Id == PaymentID);
            return remissionPmt;
        }

        public void DeletePatientPayment(Guid remissionPaymentID)
        {
            var remissionPayment = GetRemissionPaymentByID(remissionPaymentID);

            if (remissionPayment == null)
                throw new Exception("El pago seleccionado fue borrado en transacción, no hay forma de recuperarlo");

            UpdateRemissionAfterPayment(remissionPayment, isDeletedPayment: true);

            DatabaseInstance.RemissionPayment.Remove(remissionPayment);
            DatabaseInstance.SaveChanges();
        }

        public PagedPatientPaymentVM GetPagedPatientPayments(PaginationVM pagination, Guid remissionID)
        {
            var result = new PagedPatientPaymentVM();
            var remission = GetRemissionsByLoggedClinic().FirstOrDefault(f => f.Id == remissionID);

            var payments = remission.RemissionPayment;

            var items = payments.Skip(pagination.Page * pagination.PageSize).Take(pagination.PageSize)
                .Select(s => new PatientPaymentItemVM
                {
                    ApplicationDate = s.CreationDate.ToString(),
                    PaymentAmount = s.PaymentAmount,
                    PaymentID = s.Id,
                    Notes = s.Comments,
                }).OrderByDescending(o => o.ApplicationDate).ToList();

            result.PaymentItems = items;
            result.TotalPages = Math.Ceiling((decimal)(payments.Count() / pagination.PageSize));
            result.GrandTotal = remission.GrandTotal;
            result.TotalPayed = payments.Sum(s => s.PaymentAmount);

            return result;
        }
    }
}