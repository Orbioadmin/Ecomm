using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Nop.Services.Media;
using Nop.Web.Framework.Controllers;

namespace Nop.Admin.Controllers
{
    [AdminAuthorize]
    public partial class PictureController : BaseNopController
    {
        private readonly IPictureService _pictureService;

        public PictureController(IPictureService pictureService)
        {
            this._pictureService = pictureService;
        }

        [HttpPost]
        public ActionResult AsyncUpload()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);

            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = "image/tiff";
                        break;
                    default:
                        break;
                }
            }

            var picture = _pictureService.InsertPicture(fileBinary, contentType, null, true);
            //getting the image url
            var image_url = _pictureService.GetPictureUrl(picture, 100);
            string Url_for_Ftp = _pictureService.GetThumbLocalPath(picture, 0, true);
            string Url_for_Ftp_thumb = _pictureService.GetThumbLocalPath(picture, 400, true);

            string baseimgurl = System.Configuration.ConfigurationManager.AppSettings["ImageServerBaseftpUrl"];
            string ftpserverurl =System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];
            string ftpusername = System.Configuration.ConfigurationManager.AppSettings["FtpUserName"];
            string ftppassword = System.Configuration.ConfigurationManager.AppSettings["FtpPassword"];

             //saving image in ftp
            string url_db = _pictureService.UploadFileToFTP(Url_for_Ftp, baseimgurl, ftpserverurl, ftpusername, ftppassword);
            string url_thumb = _pictureService.UploadFileToFTP(Url_for_Ftp_thumb, baseimgurl, ftpserverurl, ftpusername, ftppassword);

             var url = _pictureService.UpdatePicture(picture.Id, fileBinary, contentType, null, false, url_db, true);
            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new { success = true, pictureId = picture.Id,
                              imageUrl = image_url}, "text/plain");
        }
    }
}
