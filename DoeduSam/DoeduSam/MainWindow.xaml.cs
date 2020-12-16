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

        public event PropertyChangedEventHandler PropertyChanged;

        public List<vw_ProductDetails> MyProducts
        {
            get
            {
                return _MyProducts;
            }
            set
            {
                _MyProducts = value;
            }
        }

        public int FilteredProductsCount {
            get
            {
                // фильтра пока нет - показываю полный размер
                return _MyProducts.Count;
            }
        }

        public int ProductsCount {
            get 
            {
                return _MyProducts.Count;
            }
        }

        private void UpdateValues() {
            PropertyChanged(this, new PropertyChangedEventArgs("MyProducts"));
            PropertyChanged(this, new PropertyChangedEventArgs("ProductsCount"));
            PropertyChanged(this, new PropertyChangedEventArgs("FilteredProductsCount"));
        }

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

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
                UpdateValues();
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
                    UpdateValues();
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
                UpdateValues();
            }
        }
    }
}
