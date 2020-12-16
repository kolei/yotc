using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public Products CurrentProduct { get; set; }
        public List<Manufacturers> ManufacturersList { get; set; }
        public string WindowTitle {
            get {
                if (CurrentProduct.Id == 0) return "Добавлние товара";
                return "Редактирование товара";
            }
        }

        public ProductWindow(Products Product)
        {
            InitializeComponent();
            this.DataContext = this;
            
            CurrentProduct = Product;
            ManufacturersList = Core.DB.Manufacturers.ToList();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // добавляем только новые
            if (CurrentProduct.Id == 0)
                Core.DB.Products.Add(CurrentProduct);
            // сохранение в БД
            Core.DB.SaveChanges();
            DialogResult = true;
        }
    }
}
