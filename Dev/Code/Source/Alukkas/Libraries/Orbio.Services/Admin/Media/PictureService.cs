using Orbio.Core;
using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Orbio.Services.Admin.Media
{
    public partial class PictureService:IPictureService
    {
        #region Fields
        private readonly IWebHelper webHelper;
        #endregion

        public PictureService(IWebHelper _webHelper)
        {
            this.webHelper = _webHelper;
        }

        #region Utilities

        #region ftp details
        string baseimgurl = System.Configuration.ConfigurationManager.AppSettings["ImageServerBaseftpUrl"];
        string ftpserverurl = System.Configuration.ConfigurationManager.AppSettings["ImageServerPath"];
        string ftpusername = System.Configuration.ConfigurationManager.AppSettings["FtpUserName"];
        string ftppassword = System.Configuration.ConfigurationManager.AppSettings["FtpPassword"];
        #endregion

        #endregion

        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize)
        {
            var newSize = new Size();
            if (originalSize.Height > originalSize.Width) // portrait 
            {
                newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
                newSize.Height = targetSize;
            }
            else // landscape or square
            {
                newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
                newSize.Width = targetSize;
            }
            return newSize;
        }
        
        /// <summary>
        /// Insert a new picture
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public Picture InsertPicture(string mimeType)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                        var picture = context.Pictures.FirstOrDefault();
                        picture.MimeType = mimeType;
                        context.Pictures.Add(picture);
                        context.SaveChanges();
                        return picture;
                }
                catch (Exception)
                {
                    return new Picture();
                }
            }
        }

        /// <summary>
        ///  uploading image file to ftp
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public string UploadFileToFTP(string source)
        {
            string ftppath;
            try
            {

                string filename = Path.GetFileName(source);
                ftppath = ftpserverurl + "/" + filename;
                string ftpfullpath = baseimgurl + ftppath;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpusername, ftppassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(source);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (WebException ex)
            {
                throw ex;

            }
            return ftppath;
        }

        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="relativeUrl"></param>
        /// <param name="seoFilename"></param>
        public void UpdatePicture(int pictureId, string relativeUrl, string seoFilename)
        {
            using (var context = new OrbioAdminContext())
            {
                var picture = context.Pictures.Where(m => m.Id == pictureId).FirstOrDefault();
                picture.RelativeUrl = relativeUrl;
                picture.SeoFilename = seoFilename;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get all picture details by picture id
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        public Picture GetPictureById(int pictureId)
        {
            using (var context = new OrbioAdminContext())
            {
                var picture = context.Pictures.Where(m => m.Id == pictureId).FirstOrDefault();
                return picture;
            }
        }

        /// <summary>
        /// Get picture url from locale and adjust image size
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public virtual string GetPictureUrl(Picture picture, int targetSize = 0)
        {
            string url = string.Empty;
            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            string filePath = GetPictureLocalPath(picture,"");
            if (!File.Exists(filePath))
            {
                return "";
            }
            string target = "";
            if (targetSize == 0)
            {
                url = filePath;
                target = targetSize.ToString();
                return url;
            }
            else
            {
                target = "tb";
                string fileExtension = Path.GetExtension(filePath);
                thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    target,
                    fileExtension);
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!File.Exists(thumbFilePath))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        var newSize = CalculateDimensions(b.Size, targetSize);

                        if (newSize.Width < 1)
                            newSize.Width = 1;
                        if (newSize.Height < 1)
                            newSize.Height = 1;

                        using (var newBitMap = new Bitmap(newSize.Width, newSize.Height))
                        {
                            using (var g = Graphics.FromImage(newBitMap))
                            {
                                g.SmoothingMode = SmoothingMode.HighQuality;
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.CompositingQuality = CompositingQuality.HighQuality;
                                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                                g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                                var ep = new EncoderParameters(1);
                                ep.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
                                ImageCodecInfo ici = GetImageCodecInfoFromExtension(fileExtension);
                                if (ici == null)
                                    ici = GetImageCodecInfoFromMimeType("image/jpeg");
                                newBitMap.Save(thumbFilePath, ici, ep);
                            }
                        }
                    }
                }
                url = GetThumbUrl(thumbFileName);
                return url;
            }
        }

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        public virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;

            //also see System.Web.MimeMapping for more mime types

            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }

        /// <summary>
        /// Get picture (thumb) local path
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = webHelper.MapPath("~/Areas/Admin/images/thumbs");
            if (!System.IO.Directory.Exists(thumbsDirectoryPath))
               {
                  System.IO.Directory.CreateDirectory(thumbsDirectoryPath);
               }
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }

        /// <summary>
        /// Get local path for thump images
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        public virtual string GetThumbLocalPath(Picture picture, int targetSize = 0)
        {
            string url = GetPictureUrl(picture, targetSize);
            if (String.IsNullOrEmpty(url))
                return String.Empty;
            else
                return Path.GetFileName(url);
        }

        /// <summary>
        /// Get picture (thumb) URL 
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbUrl(string thumbFileName)
        {
            string storeLocation = webHelper.GetStoreLocation();
            var url = storeLocation + "/Areas/Admin/images/thumbs/";

            //get the first two letters of the file name
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(thumbFileName);
            url = url + thumbFileName;
            return url;
        }

        /// <summary>
        /// Get local path for picture
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual string GetPictureLocalPath(Picture picture,string fileName)
        {
            var imagesDirectoryPath = webHelper.MapPath("~/Areas/Admin/images/");
            if (picture == null)
            {
                var filePath = Path.Combine(imagesDirectoryPath, fileName);
                return filePath;
            }
            else
            {
                string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
                fileName = string.Format("{0}_0.{1}", picture.Id.ToString("0000000"), lastPart);
                var filePath = Path.Combine(imagesDirectoryPath, fileName);
                return filePath;
            }
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified extension.
        /// </summary>
        /// <param name="fileExt">File extension</param>
        /// <returns>ImageCodecInfo</returns>
        protected virtual ImageCodecInfo GetImageCodecInfoFromExtension(string fileExt)
        {
            fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    return GetImageCodecInfoFromMimeType("image/jpeg");
                case "png":
                    return GetImageCodecInfoFromMimeType("image/png");
                case "gif":
                    //use png codec for gif to preserve transparency
                    //return GetImageCodecInfoFromMimeType("image/gif");
                    return GetImageCodecInfoFromMimeType("image/png");
                default:
                    return GetImageCodecInfoFromMimeType("image/jpeg");
            }
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>ImageCodecInfo</returns>
        protected virtual ImageCodecInfo GetImageCodecInfoFromMimeType(string mimeType)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            foreach (var ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    return ici;
            return null;
        }

        /// <summary>
        /// Remove uploaded images from local and ftp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePath_thumb"></param>
        public void RemoveUploadPicture(string filePath, string filePath_thumb)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                RemovePictureFromFtp(filePath);
            }
            if (System.IO.File.Exists(filePath_thumb))
            {
                System.IO.File.Delete(filePath_thumb);
                RemovePictureFromFtp(filePath_thumb);
            }
        }
        /// <summary>
        /// Remove picture from ftp
        /// </summary>
        /// <param name="source"></param>
        protected void RemovePictureFromFtp(string source)
        {
            string filename = Path.GetFileName(source);
            string ftppath = ftpserverurl + "/" + filename;
            string ftpfullpath = baseimgurl + ftppath;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpfullpath);
            request.Method = WebRequestMethods.Ftp.DeleteFile;

            request.Credentials = new NetworkCredential(ftpusername, ftppassword);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }

        /// <summary>
        /// Delete product picture
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DeletePicture(int Id)
        {
            using (var context = new OrbioAdminContext())
            {
                try
                {
                    var query = context.Pictures.Where(m => m.Id == Id).FirstOrDefault();
                    if (query != null)
                    {


                        context.Pictures.Remove(query);
                        context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}
