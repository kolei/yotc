using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using AutoService.classes;
using AutoService.windows;

namespace AutoService
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Boolean _SortPriceAscending = true;
        public Boolean SortPriceAscending {
            get { return _SortPriceAscending;  }
            set
            {
                _SortPriceAscending = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ServiceList"));
                }
            }
        }

        private List<Service> _ServiceList;
        public List<Service> ServiceList {
            get { 
                if (SortPriceAscending)
                    return _ServiceList.OrderBy(item => Double.Parse(item.CostWithDiscount)).ToList();
                else
                    return _ServiceList.OrderByDescending(item => Double.Parse(item.CostWithDiscount)).ToList();
            }
            set { _ServiceList = value; }
        }

        private Boolean _IsAdminMode = false;
        public Boolean IsAdminMode
        {
            get { return _IsAdminMode; }
            set
            {
                _IsAdminMode = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AdminModeCaption"));
                    PropertyChanged(this, new PropertyChangedEventArgs("AdminVisibility"));
                }
            }
        }

        public string AdminModeCaption {
            get {
                if (IsAdminMode) return "Выйти из режима\nАдминистратора";
                return "Войти в режим\nАдминистратора";
            }
        }

        public string AdminVisibility {
            get {
                if (IsAdminMode) return "Visible";
                return "Collapsed";
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ServiceList = Core.DB.Service.ToList();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdminMode) IsAdminMode = false;
            else {
                var InputBox = new InputBoxWindow("Введите пароль Администратора");
                if ((bool)InputBox.ShowDialog())
                {
                    IsAdminMode = InputBox.InputText == "0000";
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortPriceAscending = ((sender as RadioButton).Tag.ToString() == "1");
        }
    }
}
