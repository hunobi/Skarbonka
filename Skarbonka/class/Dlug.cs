using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skarbonka
{ 
    class Dlug
    {
        public Dlug(string iD, string dluznik, string kwota, string data_Pozyczki, string data_Zwrotu, string status, string komentarz)
        {
            ID = iD;
            Dluznik = dluznik;
            Kwota = kwota;
            Data_Pozyczki = data_Pozyczki;
            Data_Zwrotu = data_Zwrotu;
            Status = status;
            Komentarz = komentarz;
        }

        public string ID { get; }
        public string Dluznik { get; }
        public string Kwota { get; }
        public string Data_Pozyczki { get; }
        public string Data_Zwrotu { get; }
        public string Status { get; }
        public string Komentarz { get; }
    }
}
