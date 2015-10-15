using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orbio.Core.Data;

namespace Orbio.Services.Admin.Seo
{
    public partial class UrlRecordService : IUrlRecordService
    {
        /// <summary>
        /// Get url record details by slug
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public UrlRecord GetBySlug(string slug)
        {
            using (var context = new OrbioAdminContext())
            {
                return context.UrlRecords.Where(u => u.Slug == slug).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get active slug by id and entity name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public string GetActiveSlug(int id, string entityName)
        {
            using (var context = new OrbioAdminContext())
            {
                var urlRecord = context.UrlRecords.Where(u => u.EntityId == id && u.EntityName == entityName && u.IsActive).FirstOrDefault();
                return urlRecord.Slug;
            }
        }

        /// <summary>
        /// Validate search engine name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="seName"></param>
        /// <param name="name"></param>
        /// <param name="entityName"></param>
        /// <returns>Valid seo name</returns>
        public string ValidateSeName(int id, string seName, string name,string entityName)
        {
            //use name if sename is not specified
            if (String.IsNullOrWhiteSpace(seName) && !String.IsNullOrWhiteSpace(name))
                seName = name;

            //validation
            seName = GetSeName(seName);

            //max length
            //For long URLs we can get the following error:
            //"the specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters"
            //that's why we limit it to 200 here (consider a store URL + probably added {0}-{1} below)
            seName = EnsureMaximumLength(seName, 200);

            if (String.IsNullOrWhiteSpace(seName))
            {
                return seName;
            }

            int i = 2;
            var tempSeName = seName;
            while (true)
            {
                //check whether such slug already exists (and that is not the current product)
                var urlRecord = GetBySlug(tempSeName);
                var reserved1 = urlRecord != null && !(urlRecord.EntityId == id && urlRecord.EntityName.Equals(entityName, StringComparison.InvariantCultureIgnoreCase));

                if (!reserved1)
                    break;

                tempSeName = string.Format("{0}-{1}", seName, i);
                i++;
            }
            seName = tempSeName;

            return seName;
        }

        /// <summary>
        /// Get SE name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Result</returns>
        public string GetSeName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return name;
            name = name.Trim().ToLowerInvariant();

            var sb = new StringBuilder();

            string name2 = sb.ToString();
            name2 = name.Replace(" ", "-");
            while (name.Contains("--"))
                name2 = name.Replace("--", "-");
            while (name.Contains("__"))
                name2 = name.Replace("__", "_");
            return name2;
        }

        /// <summary>
        /// Ensure that a string doesn't exceed maximum allowed length
        /// </summary>
        /// <param name="str">Input string</param>
        /// <param name="maxLength">Maximum length</param>
        /// <param name="postfix">A string to add to the end if the original string was shorten</param>
        /// <returns>Input string if its lengh is OK; otherwise, truncated input string</returns>
        public string EnsureMaximumLength(string str, int maxLength, string postfix = null)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            if (str.Length > maxLength)
            {
                var result = str.Substring(0, maxLength);
                if (!String.IsNullOrEmpty(postfix))
                {
                    result += postfix;
                }
                return result;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Get search engine name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entityName"></param>
        /// <returns>Search engine name</returns>
        public string GetSeName(int id,string entityName)
        {
            string result = string.Empty;
            //set default value if required
            if (String.IsNullOrEmpty(result))
            {
                result = GetActiveSlug(id, entityName);
            }

            return result;
        }
    }
}
