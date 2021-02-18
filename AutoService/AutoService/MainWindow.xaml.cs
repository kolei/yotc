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

    public partial class Service
    {
        public float DiscountFloat
        {
            get
            {
                return Convert.ToSingle(Discount ?? 0);
            }
        }

        public string DescriptionString
        {
            get {
                return Description ?? "";
            }
        }

        public Uri ImageUri
        {
            get
            {
                return new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, MainImagePath ?? ""));
            }
        }
        public Boolean HasDiscount
        {
            get
            {
                return Discount > 0;
            }
        }

        public string CostString
        {
            get
            {
                return Cost.ToString("#.##");
            }
        }

        public string CostWithDiscount
        {
            get
            {
                return (Cost * Convert.ToDecimal(1 - Discount ?? 0)).ToString("#.##");
            }
        }

        public string CostTextDecoration
        {
            get
            {
                return HasDiscount ? "Strikethrough" : "None";
            }
        }

    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<Tuple<string, float, float>> FilterByDiscountValuesList = new List<Tuple<string, float, float>>() {
            Tuple.Create("Все записи", 0f, 1f),
            Tuple.Create("от 0% до 5%", 0f, 0.05f),
            Tuple.Create("от 5% до 15%", 0.05f, 0.15f),
            Tuple.Create("от 15% до 30%", 0.15f, 0.3f),
            Tuple.Create("от 30% до 70%", 0.3f, 0.7f),
            Tuple.Create("от 70% до 100%", 0.7f, 1f)
        };

        public List<string> FilterByDiscountNamesList {
            get {
                return FilterByDiscountValuesList.Select(item => item.Item1).ToList();
            }
        }

        private Tuple<float, float> _CurrentDiscountFilter = Tuple.Create(float.MinValue, float.MaxValue);

        public Tuple<float, float> CurrentDiscountFilter { 
            get
            {
                return _CurrentDiscountFilter;
            }
            set
            {
                _CurrentDiscountFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ServiceList"));
                }
            }
        }

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
                var FilteredServiceList = _ServiceList.FindAll(item =>
                    item.DiscountFloat >= CurrentDiscountFilter.Item1 &&
                    item.DiscountFloat < CurrentDiscountFilter.Item2);

                if (SearchFilter != "")
                    FilteredServiceList = FilteredServiceList.Where(item =>
                        item.Title.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1 ||
                        item.DescriptionString.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) != -1).ToList();

                if (SortPriceAscending)
                    return FilteredServiceList.OrderBy(item => Double.Parse(item.CostWithDiscount)).ToList();
                else
                    return FilteredServiceList.OrderByDescending(item => Double.Parse(item.CostWithDiscount)).ToList();
            }
            set { 
                _ServiceList = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ServiceList"));
            }
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
            var SelectedService = MainDataGrid.SelectedItem as Service;
            var EditServiceWindow = new windows.ServiceWindow(SelectedService);
            if ((bool)EditServiceWindow.ShowDialog())
            {
                PropertyChanged(this, new PropertyChangedEventArgs("ServiceList"));
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as Service;
            if (item.ClientService.Count > 0) {
                MessageBox.Show("Нельзя удалять услугу, она уже оказана");
                return;
            }
            Core.DB.Service.Remove(item);
            Core.DB.SaveChanges();
            ServiceList = Core.DB.Service.ToList();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SortPriceAscending = ((sender as RadioButton).Tag.ToString() == "1");
        }

        private void DiscountFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentDiscountFilter = Tuple.Create(
                FilterByDiscountValuesList[DiscountFilterComboBox.SelectedIndex].Item2,
                FilterByDiscountValuesList[DiscountFilterComboBox.SelectedIndex].Item3
            );
        }

        private string _SearchFilter = "";
        public string SearchFilter
        {
            get
            {
                return _SearchFilter;
            }
            set
            {
                _SearchFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ServiceList"));
                    // PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchTextBox.Text;
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            // создаем новую услугу
            var NewService = new Service();

            var NewServiceWindow = new windows.ServiceWindow(NewService);
            if ((bool)NewServiceWindow.ShowDialog())
            {
                ServiceList = Core.DB.Service.ToList();
                PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                PropertyChanged(this, new PropertyChangedEventArgs("ProductsCount"));
            }
        }
    }
}
