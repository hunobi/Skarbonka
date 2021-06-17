using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Skarbonka
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Klient _client = null;

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists("db.db"))
            {
                MessageBox.Show("Brak podpiętej bazy danych!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
            
        }

        private void Logowanie_Zaloguj_Click(object sender, RoutedEventArgs e)
        {
            var obj = new Database().Login(Logowanie_Login.Text, Logowanie_Haslo.Password);
            if (obj == null)
            {
                Logowanie_Error.Visibility = Visibility.Visible;
                Task.Run(() => 
                {
                    Task.WaitAll(Task.Delay(5000));
                    Dispatcher.Invoke(() =>
                    {
                        Logowanie_Error.Visibility = Visibility.Hidden;
                    });
                });
            }
            else
            {
                _client = obj;
                Logowanie_Login.Text = "";
                Logowanie_Haslo.Password = "";
                Logowanie.Visibility = Visibility.Hidden;
                Main.Visibility = Visibility.Visible;
            }
        }

        private void Main_Wyloguj_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Hidden;
            Logowanie.Visibility = Visibility.Visible;
        }



        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Saldo_Back_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            Saldo.Visibility = Visibility.Hidden;
        }

        private void Saldo_OK_Click(object sender, RoutedEventArgs e)
        {
            new Database().SetSaldo(Saldo_Texbox.Text, _client.UserDataID);
            _client.Saldo = Convert.ToDouble(Saldo_Texbox.Text);
        }

        private void Main_Saldo_Click(object sender, RoutedEventArgs e)
        {
            Saldo_Texbox.Text = _client.Saldo.ToString();
            Main.Visibility = Visibility.Hidden;
            Saldo.Visibility = Visibility.Visible;
        }

        private void Main_Info_Click(object sender, RoutedEventArgs e)
        {
            Info_Imie.Content = _client.Imie;
            Info_Nazwisko.Content = _client.Nazwisko;
            Info_Saldo.Content = _client.Saldo;
            Info_Data.Content = _client.Data_Rejesracji;
            Main.Visibility = Visibility.Hidden;
            Info.Visibility = Visibility.Visible;
        }

        private void Info_Back_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            Info.Visibility = Visibility.Hidden;
        }


        private void AddDebt_Back_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            AddDebt.Visibility = Visibility.Hidden;
        }

        private void Main_Add_Debt_Click(object sender, RoutedEventArgs e)
        {
            AddDebt_Data_Pozyczki.Text = DateTime.Now.ToString();
            Main.Visibility = Visibility.Hidden;
            AddDebt.Visibility = Visibility.Visible;
        }

        private void AddDebt_OK_Click(object sender, RoutedEventArgs e)
        {
            if(AddDebt_Adresat.Text == "" || AddDebt_Data_Pozyczki.Text == "" || AddDebt_Kwota.Text == "")
            {
                MessageBox.Show("Pola nie mogą być puste!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                new Database().AddDebt(_client, AddDebt_Adresat.Text, AddDebt_Kwota.Text, AddDebt_Komentarz.Text, AddDebt_Data_Pozyczki.Text);
                AddDebt_Adresat.Text = "";
                AddDebt_Data_Pozyczki.Text = DateTime.Now.ToString();
                AddDebt_Kwota.Text = "";
                AddDebt_Komentarz.Text = "";
            }
        }


        private void Main_Modify_Debt_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Hidden;
            ModifyDebt.Visibility = Visibility.Visible;
        }


        private void ModifyDebt_OK_Click(object sender, RoutedEventArgs e)
        {
            if(ModifyDebt_ID.Text == "")
            {
                MessageBox.Show("Pole nie może być puste!", "Uwaga!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                new Database().CloseDebt(ModifyDebt_ID.Text, _client);
                ModifyDebt_ID.Text = "";
            }
        }


        private void ModifyDebt_Back_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            ModifyDebt.Visibility = Visibility.Hidden;
            ModifyDebt_ID.Text = "";
        }



        private void Main_Show_Debt_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Hidden;
            ShowDebt.Visibility = Visibility.Visible;
            new Database().LoadTable(Tabela,_client);
        }

        private void ShowDebt_Back_Click(object sender, RoutedEventArgs e)
        {
            Main.Visibility = Visibility.Visible;
            ShowDebt.Visibility = Visibility.Hidden;
        }
    }
}
