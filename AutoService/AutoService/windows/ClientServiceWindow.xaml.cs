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

namespace AutoService
{
    public partial class Client
    {
        public string FullName
        {
            get
            {
                return FirstName + ' ' + LastName;
            }
        }
    }

    public partial class ClientService
    {
        public string StartTimeText
        {
            get
            {
                return StartTime.ToString();
            }
            set
            {
                StartTime = new DateTime(2021, 2, 25, 9, 9, 9);
            }
        }
    }
}


namespace AutoService.windows
{

    /// <summary>
    /// Логика взаимодействия для ClientServiceWindow.xaml
    /// </summary>
    public partial class ClientServiceWindow : Window
    {
        public List<Client> ClientList { get; set; }
        public ClientService CurrentClientService { get; set; }
        public List<Service> ServiceList { get; set; }

        public ClientServiceWindow(List<Service> serviceList)
        {
            InitializeComponent();
            DataContext = this;
            ClientList = classes.Core.DB.Client.ToList();
            ServiceList = serviceList;
            CurrentClientService = new ClientService();
            CurrentClientService.StartTime = DateTime.Now;
        }
    }
}
