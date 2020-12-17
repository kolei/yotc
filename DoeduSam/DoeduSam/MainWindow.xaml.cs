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

namespace DoeduSam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private List<vw_ProductDetails> _MyProducts;

        private List<Manufacturers> _ManufacturersList;
        public List<Manufacturers> ManufacturersList { get; set; }

        private int _ManufacturerFilterId = 0;
        public int ManufacturerFilterId
        {
            get { return _ManufacturerFilterId; }
            set {
                _ManufacturerFilterId = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        private string _ProductNameFilter = "";
        public string ProductNameFilter
        {
            get {
                return _ProductNameFilter;
            }
            set
            {
                _ProductNameFilter = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<vw_ProductDetails> MyProducts
        {
            get
            {
                return _MyProducts.FindAll(item => 
                    (ManufacturerFilterId==0 || item.ManufacturerId==ManufacturerFilterId) && 
                    (ProductNameFilter=="" || item.Name.IndexOf(ProductNameFilter, StringComparison.OrdinalIgnoreCase)!=-1)
                );
            }
            set
            {
                _MyProducts = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductsCount"));
                    PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
                }
            }
        }

        public int FilteredProductsCount {
            get
            {
                // показываю размер ФИЛЬТРОВАННОЙ таблицы
                return MyProducts.Count;
            }
        }

        public int ProductsCount {
            get 
            {
                return _MyProducts.Count;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            ManufacturersList = Core.DB.Manufacturers.ToList();

            var newManufacturer = new Manufacturers();
            newManufacturer.Name = "Все производители";

            ManufacturersList.Insert(0, newManufacturer);

            MyProducts = Core.DB.vw_ProductDetails.ToList();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as vw_ProductDetails;
            var DeletedProduct = Core.DB.Products.Find(item.Id);

            if (DeletedProduct != null)
            try
            {
                // связи не дадут удалить товар
                if (DeletedProduct.AdditionalProducts.Count > 0) {
                    MessageBox.Show("Нельзя удалять товар, есть дополнительные товары");
                    return;
                }

                if (DeletedProduct.Images.Count > 0)
                {
                    MessageBox.Show("Нельзя удалять товар, есть дополнительные изображения");
                    return;
                }

                if (DeletedProduct.ProductSales.Count > 0)
                {
                    MessageBox.Show("Нельзя удалять товар, есть продажи");
                   return;
                }

                Core.DB.Products.Remove(DeletedProduct);
                Core.DB.SaveChanges();
                MyProducts = Core.DB.vw_ProductDetails.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не смог удалить товар: "+ex.Message);
                //throw;
            }
                
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as vw_ProductDetails;
            var EditProduct = Core.DB.Products.Find(item.Id);

            if (EditProduct != null) {
                var NewProduct = new ProductWindow( EditProduct );
                if ((bool)NewProduct.ShowDialog())
                {
                    MyProducts = Core.DB.vw_ProductDetails.ToList();
                }
            }
        }

        // для создания/редактирования товара создаем окно
        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            var NewProduct = new ProductWindow( new Products() );
            if ((bool)NewProduct.ShowDialog())
            {
                MyProducts = Core.DB.vw_ProductDetails.ToList();
            }
        }

        private void ComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ManufacturerFilterId = (ManufacturersFilter.SelectedItem as Manufacturers).id;
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            ProductNameFilter = SearchTextBox.Text;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var item = MainDataGrid.SelectedItem as vw_ProductDetails;
            var HistoryProduct = Core.DB.Products.Find(item.Id);
            var NewHistoryWindow = new HistoryWindow(HistoryProduct);
            NewHistoryWindow.ShowDialog();
        }
    }
}
