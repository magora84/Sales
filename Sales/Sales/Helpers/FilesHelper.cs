using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sales.Helpers {
    public class FilesHelper {
     
        public static byte[] ReadFully(Stream input) { 
        //convierte el string de imagen a byte para poder enviar por postman
        using (MemoryStream ms = new MemoryStream())
            {
                 input.CopyTo(ms);
                 return ms.ToArray();
            }
        }
    }
}