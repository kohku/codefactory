using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.flajaxian;
using System.Web;
using CodeFactory.Web.Storage;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace CodeFactory.ContentManager.Flajaxian
{
    public class UploadStorageAdapter : FileUploaderAdapter
    {
        private string _folderName;

        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }

        public override void ProcessFile(HttpPostedFile file)
        {
            ManualResetEvent e = HttpContext.Current.Items["SyncUploadStorageAdapter"] as ManualResetEvent;

            try
            {
                UploadedFile item = UploadStorageService.CreateFile4Storage(file.ContentType);

                item.FileName = Path.GetFileName(file.FileName);
                item.ContentLength = file.ContentLength;
                item.InputStream = file.InputStream;

                if (!string.IsNullOrEmpty(HttpContext.Current.Request.Form["sectionId"]))
                {
                    Section section = Section.Load(new Guid(HttpContext.Current.Request.Form["sectionId"]));

                    if (section != null)
                        section.AddFile(item);
                }

                item.AcceptChanges();

                HttpContext.Current.Items["fileId"] = item.ID;
            }
            finally
            {
                if (e != null)
                {
                    Trace.WriteLine(string.Format("Signaling event for file in section {0}.", HttpContext.Current.Request.Form["sectionId"]));
                    e.Set();
                }
            }
        }
    }
}
