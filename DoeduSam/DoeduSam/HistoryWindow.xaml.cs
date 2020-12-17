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
using System.Windows.Shapes;

namespace DoeduSam
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window, INotifyPropertyChanged
    {

        private List<ProductSales> _HistoryList;
        public List<ProductSales> HistoryList { 
            get {
                return _HistoryList.FindAll(history => history.ProductId == SelectedProduct.Id).OrderByDescending(f => f.DateSale).ToList();
            }
            set {
                _HistoryList = value;
            }
        }


        public List<Products> ProductsList { get; set; }

        public int SelectedProductIndex {
            get; set;
        }

        private Products _SelectedProduct;

        public event PropertyChangedEventHandler PropertyChanged;

        public Products SelectedProduct {
            get {
                return _SelectedProduct;
            }
            set {
                _SelectedProduct = value;
                if (PropertyChanged != null) { 
                    PropertyChanged(this, new PropertyChangedEventArgs("HistoryList"));
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedProduct"));
                }
            }
        }

        public HistoryWindow(Products Product)
        {
            InitializeComponent();
            DataContext = this;

            SelectedProduct = Product;

            HistoryList = Core.DB.ProductSales.ToList();
            ProductsList = Core.DB.Products.ToList();
            SelectedProductIndex = ProductsList.FindIndex(product => product.Id == SelectedProduct.Id);
        }

        private void ProductsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedProduct = ProductsComboBox.SelectedItem as Products;
        }
    }
}
