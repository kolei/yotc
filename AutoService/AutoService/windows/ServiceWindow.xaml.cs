using Microsoft.Win32;
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

namespace AutoService.windows
{
    /// <summary>
    /// Логика взаимодействия для ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window, INotifyPropertyChanged
    {
        public Service CurrentService { get; set; }

        public string WindowName {
            get {
                return CurrentService.ID == 0 ? "Новая услуга" : "Редактирование услуги";
            }
        }

        public ServiceWindow(Service service)
        {
            InitializeComponent();
            CurrentService = service;
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentService.Cost <= 0)
            {
                MessageBox.Show("Стоимость услуги должна быть больше ноля");
                return;
            }

            if (CurrentService.Discount < 0 || CurrentService.Discount>1)
            {
                MessageBox.Show("Скидка на услугу должна быть в диапазоне от 0 до 1");
                return;
            }

            // если запись новая, то добавляем ее в список
            if (CurrentService.ID == 0)
                classes.Core.DB.Service.Add(CurrentService);

            // сохранение в БД
            try
            {
                classes.Core.DB.SaveChanges();
            }
            catch
            { 
            }
            DialogResult = true;
        }

        private void GetImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog GetImageDialog = new OpenFileDialog();
            GetImageDialog.Filter = "Файлы изображений: (*.png, *.jpg)|*.png;*.jpg";
            GetImageDialog.InitialDirectory = Environment.CurrentDirectory;
            if (GetImageDialog.ShowDialog() == true)
            {
                CurrentService.MainImagePath = GetImageDialog.FileName.Substring(Environment.CurrentDirectory.Length+1);
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentService"));
                }
            }
        }
    }
}
