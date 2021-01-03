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

namespace AutoService.windows
{
    /// <summary>
    /// Логика взаимодействия для InputBoxWindow.xaml
    /// </summary>
    public partial class InputBoxWindow : Window
    {
        public string WindowCaption { get; set; }
        public string InputText { get; set; }
        public InputBoxWindow(string Caption)
        {
            InitializeComponent();
            WindowCaption = Caption;
            this.DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
