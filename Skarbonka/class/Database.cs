using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace Skarbonka
{
    class Database
    {

        private SQLiteConnection _connection;

        public Database()
        {
            _connection = new SQLiteConnection("data source=db.db");
        }

        public void LoadTable(DataGrid tabela, Klient client)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                List<Dlug> lista = new List<Dlug>();
                Connect();

                cmd.CommandText = "SELECT * FROM debt WHERE id_kto = '" + client.UserID + "'";
                SQLiteDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    Dlug temp = new Dlug(
                        r["id"].ToString(),
                        r["komu"].ToString(),
                        r["kwota"].ToString(),
                        r["data_pozyczki"].ToString(),
                        r["data_zwrotu"].ToString(),
                        r["status"].ToString(),
                        r["komentarz"].ToString()
                        );
                    lista.Add(temp);
                }
                tabela.ItemsSource = lista;
                Disconnect();
            }
        }

        public void SetSaldo(string saldo, string id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                Connect();
                cmd.CommandText = "UPDATE user_data SET saldo = '" + saldo + "' WHERE id = '" + id + "'";
                cmd.ExecuteNonQuery();
                Disconnect();
            }
        }

        public void AddDebt(Klient cl, string adresat, string kwota, string komentarz, string data)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                Connect();

                cmd.CommandText = "INSERT INTO debt (id_kto, komu, kwota, data_pozyczki, status, komentarz) VALUES ('" + cl.UserID + "', '" + adresat + "', '" + kwota + "', '" + data + "', '0', '" + komentarz + "'); ";
                cmd.ExecuteNonQuery();
             
                cl.Saldo = cl.Saldo - Convert.ToDouble(kwota);

                Disconnect();
                SetSaldo(cl.Saldo.ToString(), cl.UserDataID);
   
            }
        }

        public void CloseDebt(string id, Klient cl)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                Connect();
                cmd.CommandText = "SELECT * FROM debt WHERE id = '" + id + "' AND id_kto = '"+cl.UserID+"'";
                SQLiteDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    int temp = Convert.ToInt16(r["status"]);
                    if (temp == 1)
                    {
                        MessageBox.Show("Ten dług został już zamknięty!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                        r.Close();
                        Disconnect();
                    }
                    else
                    {
                        double kwota = (double)r["kwota"];
                        r.Close();
                        cl.Saldo = cl.Saldo + kwota;
                        cmd.CommandText = "UPDATE debt SET status = '1' , data_zwrotu = '" + DateTime.Now.ToString() + "' WHERE id = '" + id + "'";
                        cmd.ExecuteNonQuery();
                        Disconnect();

                        SetSaldo(cl.Saldo.ToString(), cl.UserDataID);
                    }                                   
                }
                else
                {
                    MessageBox.Show("Nie ma długu z takim ID lub nie należy on do Ciebie!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
                    Disconnect();
                }

            }
        }

        public Klient Login(string login, string pass)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(_connection))
            {
                Connect();
                cmd.CommandText = "SELECT * FROM login_data WHERE login_data.login = '" + login + "' AND login_data.haslo = '" + pass + "'";
                SQLiteDataReader r =  cmd.ExecuteReader();

                if(r.Read())
                {                 
                    string id_user_login = r["id"].ToString();
                    r.Close();
                    cmd.CommandText =  "UPDATE login_data SET ostatnie_logowanie = '"+DateTime.Now.ToString()+"' WHERE id = '" + id_user_login + "'" ;
                    cmd.ExecuteNonQuery();
                    
                    cmd.CommandText = "SELECT * FROM users WHERE id_login_data = '"+id_user_login+"'";                
                    r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        string user_id = r["id"].ToString();
                        string id_user_data = r["id_user_data"].ToString();
                        r.Close();

                        cmd.CommandText = "SELECT * FROM user_data WHERE id = '" + id_user_data + "'";
                        r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            string imie = r["imie"].ToString();
                            string nazwisko = r["nazwisko"].ToString();
                            double saldo = Convert.ToDouble(r["saldo"].ToString());
                            string data_rej = r["data_rejestracji"].ToString();
                            r.Close();
                            Disconnect();
                            return new Klient(user_id, id_user_login, id_user_data, imie, nazwisko, saldo, data_rej);
                        }
                    }
                }
                
                Disconnect();
                return null;
            }
            
        }



        private void Connect()
        {
            _connection.Open();
        }

        private void Disconnect()
        {
            _connection.Close();
        }

    }
}
