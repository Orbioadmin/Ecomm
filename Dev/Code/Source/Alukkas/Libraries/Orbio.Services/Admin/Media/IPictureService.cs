using Orbio.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbio.Services.Admin.Media
{
    public interface IPictureService
    {
        /// <summary>
        /// Insert a new picture
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        Picture InsertPicture(string mimeType);

        /// <summary>
        ///  uploading image file to ftp
        /// </summary>
        /// <param name="source"></param>
        /// <param name="baseimageurl"></param>
        /// <param name="ftpurl"></param>
        /// <param name="ftpusername"></param>
        /// <param name="ftppassword"></param>
        /// <returns></returns>
        string UploadFileToFTP(string source);


        /// <summary>
        /// Update picture
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="relativeUrl"></param>
        /// <param name="seoFilename"></param>
        void UpdatePicture(int pictureId, string relativeUrl,string seoFilename);

        /// <summary>
        /// Get picture url from locale and adjust image size
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        string GetPictureUrl(Picture picture, int targetSize = 0);

        /// <summary>
        /// Returns the file extension from mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>File extension</returns>
        string GetFileExtensionFromMimeType(string mimeType);

        /// <summary>
        /// Get local path for thump images
        /// </summary>
        /// <param name="picture"></param>
        /// <param name="targetSize"></param>
        /// <returns></returns>
        string GetThumbLocalPath(Picture picture, int targetSize = 0);

        /// <summary>
        /// Remove uploaded images from local and ftp
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filePath_thumb"></param>
        void RemoveUploadPicture(string filePath, string filePath_thumb);

        /// <summary>
        /// Get all picture details by picture id
        /// </summary>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        Picture GetPictureById(int pictureId);

        /// <summary>
        /// Delete product picture
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        int DeletePicture(int Id);
    }
}
