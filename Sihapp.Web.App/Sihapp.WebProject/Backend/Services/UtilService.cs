using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class UtilService : BasicService
    {

        //info de transacciones https://msdn.microsoft.com/en-us/library/dn456843(v=vs.113).aspx

        public void RunTransaction(Func<object> function)
        {
            using (var transactionContext = DatabaseInstance)
            {
                using (var db_transactionContext = transactionContext.Database.BeginTransaction())
                {
                    try
                    {
                        function();
                        db_transactionContext.Commit();
                    }
                    catch (Exception ex)
                    {

                        db_transactionContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
        }

        public void RunTransaction(Action function)
        {
            using (var transactionContext = DatabaseInstance)
            {
                using (var db_transactionContext = transactionContext.Database.BeginTransaction())
                {
                    try
                    {
                        function();
                        db_transactionContext.Commit();
                    }
                    catch (Exception ex)
                    {

                        db_transactionContext.Rollback();
                        throw new Exception(ex.Message);
                    }
                }

            }
        }

        public void SendEmail(List<string> emails, string subject, string body)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sihapp.web@gmail.com");

            foreach (var e in emails)
            {
                mail.To.Add(e);
            }

            mail.Subject = subject;

            var htmlBody = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");
            mail.AlternateViews.Add(htmlBody);
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("sihapp.web@gmail.com", "_yout_password_");
          

            SmtpServer.Send(mail);
            string mails = string.Join(",", emails);
            AddLog("Email send to " + mails, false, "", null, null);

            //mail.Body = body;

        }


        public string GetDayNameByDayNumber(int day)
        {

            switch (day)
            {
                case 1:
                    return "Lunes";
                case 2:
                    return "Martes";
                case 3:
                    return "Miercoles";
                case 4:
                    return "Jueves";
                case 5:
                    return "Viernes";
                case 6:
                    return "Sabado";
                case 7:
                    return "Domíngo";
                default:
                    return "";

            }

        }


        public string GetMonthNameByNumber(int month)
        {

            switch (month)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";

                default:
                    return "";
            }
        }


        //https://social.msdn.microsoft.com/Forums/sqlserver/en-US/4726721e-5c28-4f76-bc58-9f4c3ca9a417/compressing-image-byte-array-to-jpeg?forum=csharplanguage

        public byte[] ConvertBytestoJpegBytes(byte[] pixels24bpp, int W, int H)
        {
            GCHandle gch = GCHandle.Alloc(pixels24bpp, GCHandleType.Pinned);
            int stride = 4 * ((24 * W + 31) / 32);
            Bitmap bmp = new Bitmap(W, H, stride, PixelFormat.Format24bppRgb, gch.AddrOfPinnedObject());
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Jpeg);
            gch.Free();
            return ms.ToArray();
        }

        public int GetAge(DateTime dateOfBirth)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;
            return age;
        }

    }
}