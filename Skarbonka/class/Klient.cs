using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skarbonka
{
    class Klient
    {
        public Klient(string userID, string userLoginID, string userDataID, string imie, string nazwisko, double saldo, string data_rejestracji)
        {
            UserID = userID;
            UserLoginID = userLoginID;
            UserDataID = userDataID;
            Imie = imie;
            Nazwisko = nazwisko;
            Saldo = saldo;
            Data_Rejesracji = data_rejestracji;
        }

        public string UserID { get; }   
        public string UserLoginID { get; }
        public string UserDataID { get; }
        public string Imie { get; }
        public string Nazwisko { get; }
        public double Saldo { get; set; }
        public string Data_Rejesracji { get; }
    }
}
