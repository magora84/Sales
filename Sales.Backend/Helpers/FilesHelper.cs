﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sales.Backend.Helpers {
    public static class FilesHelper {
        //metodo upload foto
        public static string UploadPhoto(HttpPostedFileBase file,string folder) {
            string path = string.Empty;
            string pic = string.Empty;
            if (file != null) {
                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                file.SaveAs(path);

            }
            return pic;
        }
    }
}