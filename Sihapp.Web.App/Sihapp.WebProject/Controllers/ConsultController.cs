using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models.TypesCatalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace Sihapp.WebProject.Controllers
{
    public class ConsultController : BasicController
    {
        // GET: Consult
        public ActionResult OpenConsult(Guid appointmentID)
        {
            var consultID = new ConsultService().OpenConsult(appointmentID);
            return GetJsonNetResult(consultID);
        }

        public ActionResult Consult(Guid consultID)
        {

            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType == UserTypeCodes.Patient)
                return RedirectToAction("Login", "Account");

            var consult = new ConsultService().GetConsulVMById(consultID);
            ViewBag.backendData = SerializeJSONData(consult);

            return View();
        }

        [HttpPost]
        public ActionResult CloseConsult(Guid consultID, string notes, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(notes))
                return GetJsonNetResult("No se puede guardar la consulta si no hay notas");

            Action addConsult = () =>
            {
                new ConsultService().CloseConsult(consultID, notes, amount);
            };

            new UtilService().RunTransaction(addConsult);
           
            return GetJsonNetResult(null);
        }

        [HttpPost]
        public ActionResult SaveConsultImage(Guid consultId, HttpPostedFileBase file)
        {
            
            var webImage = new System.Web.Helpers.WebImage(Request.Files[0].InputStream);
            var imgBytes = webImage.GetBytes();
            Bitmap bmp;
            using (var memS = new MemoryStream(imgBytes))
            {
                bmp = new Bitmap(memS);
            }

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            byte[] b = ms.ToArray();
            Array.Copy(b, 54, b, 0, b.Length - 54);
            byte[] jpegBytes = new UtilService().ConvertBytestoJpegBytes(b, bmp.Width, bmp.Height);
            new ConsultService().SaveConsultImage(jpegBytes, consultId);


            return GetJsonNetResult(jpegBytes);

        }

        [HttpPost]
        public ActionResult GetConsultImgesQuantity(Guid consultId)
        {
            return GetJsonNetResult(new ConsultService().GetConsultImages(consultId).Count() >= 2);
        }

        [HttpPost]
        public ActionResult SavePatientImg(Guid consultID, byte[] img, string notes)
        {
            new ConsultService().SaveConsultPatientImg(consultID, img, notes);
            return GetJsonNetResult(null);
        }

        [HttpPost]
        public ActionResult SaveNotes(Guid consultID, string notes)
        {
            new ConsultService().SaveConsultMessage(consultID, notes);
            return GetJsonNetResult(null);
        }

        [HttpPost]
        public ActionResult GetConsultImagesByConsultId(Guid id)
        {
            var images = new ConsultService().GetConsultImagesByConsultID(id);
            return GetJsonNetResult(images);
        }
    }
}