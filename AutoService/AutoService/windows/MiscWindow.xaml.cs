﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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

    public class Rootobject
    {
        public bool success { get; set; }
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public int Num { get; set; }
        public string Adress { get; set; }
        public string Vid { get; set; }
        public int Rast { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для MiscWindow.xaml
    /// </summary>
    public partial class MiscWindow : Window
    {
        public MiscWindow()
        {
            InitializeComponent();
        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new WebClient();
            // вам в URL-е нужно прописать свои названия баз и таблиц
            var ResponseBuffer = client.DownloadData("http://kolei.ru/api/ekolesnikov/Sklad");
            var ResponseString = Encoding.UTF8.GetString(ResponseBuffer);
            ResponseTextBox.Text = ResponseString;

            var serializer = new DataContractJsonSerializer(typeof(Rootobject));

            // сериализуем полученный ответ сервера
            var respObj = serializer.ReadObject(new MemoryStream(ResponseBuffer));
            if((respObj as Rootobject).success)
            {
                foreach(Datum item in (respObj as Rootobject).data)
                {
                    ResponseTextBox.Text += $"{item.Adress}\n";
                }
            }
        }
    }
}
