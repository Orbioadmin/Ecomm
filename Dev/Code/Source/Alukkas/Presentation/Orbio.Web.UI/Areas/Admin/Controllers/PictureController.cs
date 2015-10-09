using Orbio.Services.Admin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orbio.Web.UI.Areas.Admin.Controllers
{
    public class PictureController : Controller
    {
        #region Fields
        public readonly IPictureService _pictureService;
        #endregion

        #region Constructors
        public PictureController(IPictureService pictureService)
        {
            this._pictureService = pictureService;
        }
        #endregion

        #region Methods

        public ActionResult PictureUpload()
        {

            return PartialView("_PictureUpload");
        }

        [HttpPost]
        public JsonResult uploadImg()
        {
            var fileName = "";
            var contentType = "";
            HttpPostedFileBase httpPostedFile = Request.Files[0];
            if (String.IsNullOrEmpty(Request["UploadedImage"]))
            {
                // IE
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                fileName = Request["UploadedImage"];
            }
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
            var picture = _pictureService.InsertPicture(contentType);
            string imagePath = SavePictureLocal(picture.Id, contentType, httpPostedFile);
            string imageUrl = _pictureService.GetPictureUrl(picture, 100);
            string Url_for_Ftp_thumb = System.IO.Path.Combine(Server.MapPath("~/Areas/Admin/images/thumbs/"), _pictureService.GetThumbLocalPath(picture, 400));
            //saving image in ftp
            string url_db = _pictureService.UploadFileToFTP(imagePath);
            string url_thumb = _pictureService.UploadFileToFTP(Url_for_Ftp_thumb);
            _pictureService.UpdatePicture(picture.Id, url_db, fileName.Replace(" ", "_"));
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = imageUrl
            }, "text/plain");
        }


        protected string SavePictureLocal(int pictureId, string mimeType, HttpPostedFileBase httpPostedFile)
        {
            string lastPart = _pictureService.GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            string path = System.IO.Path.Combine(Server.MapPath("~/Areas/Admin/images/"), fileName);
            httpPostedFile.SaveAs(path);
            return path;
        }

        [HttpPost]
        public JsonResult RemoveImage(int id)
        {
            var picture = _pictureService.GetPictureById(id);
            string lastPart = _pictureService.GetFileExtensionFromMimeType(picture.MimeType);
            string fileName = string.Format("{0}_0.{1}", picture.Id.ToString("0000000"), lastPart);
            string filePath = System.IO.Path.Combine(Server.MapPath("~/Areas/Admin/images/"), fileName);
            string filePath_thumb = System.IO.Path.Combine(Server.MapPath("~/Areas/Admin/images/thumbs/"), _pictureService.GetThumbLocalPath(picture, 400));
            _pictureService.RemoveUploadPicture(filePath, filePath_thumb);
            _pictureService.DeletePicture(picture.Id);
            return Json(new { success = true});
        }
        #endregion
    }
}