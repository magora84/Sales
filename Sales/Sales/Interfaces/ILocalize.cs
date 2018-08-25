using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Sales.Interfaces
{
   public interface ILocalize
    {
        //trae el idioma del telefono
        CultureInfo GetCurrentCultureInfo();
        void SetLocale(CultureInfo ci);

    }
}
