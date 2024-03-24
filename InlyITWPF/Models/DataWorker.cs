using InlyITWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using InlyITWPF.ViewModels;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace InlyITWPF.Models
{
    public class DataWorker : BaseViewModel
    {
        private RootObject _rootObject { get; set; }
        private ObservableCollection<Valute> _valutes;
        public ObservableCollection<Valute> Valutes
        {
            get => _valutes;
            set
            {
                _valutes = value;
                OnPropertyChanged(nameof(Valutes));
            }
        }
        public ObservableCollection<Valute> WebData()
        {

            var jToken = new WebClient().DownloadData("https://www.cbr-xml-daily.ru/daily_json.js");
            var jTok = Encoding.UTF8.GetString(jToken);
            _rootObject = JsonConvert.DeserializeObject<RootObject>(jTok.ToString());
            _valutes = new ObservableCollection<Valute>(_rootObject.Valute.Select(x => x.Value));

            string ser = JsonConvert.SerializeObject(_rootObject);

            if (ser.Length > 0)
            {
                File.Create("test.json").Close();
                File.WriteAllText("test.json", ser, Encoding.UTF8);
            }


            return _valutes;
        }
        public ObservableCollection<Valute> OverwriteLocalData(string charCode)
        {
            _rootObject.Valute.Remove(charCode);
            string ser = JsonConvert.SerializeObject(_rootObject);

            if (ser.Length > 0)
            {
                File.Create("test.json").Close();
                File.WriteAllText("test.json", ser, Encoding.UTF8);
            }
            _valutes = new ObservableCollection<Valute>(_rootObject.Valute.Select(x => x.Value));
            return _valutes;

        }

        public ObservableCollection<Valute> LocalData()
        {


            string jsonString = File.ReadAllText("test.json");
            _rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
            _valutes = new ObservableCollection<Valute>(_rootObject.Valute.Select(x => x.Value));

            //string ser = JsonConvert.SerializeObject(_rootObject);

            //if (ser.Length > 0)
            //{
            //    File.Create("test.json").Close();
            //    File.WriteAllText("test.json", jsonString, Encoding.UTF8);
            //}

            return _valutes;

        }
        public ObservableCollection<Valute> GetData(string charCode = null)
        {
            if (File.Exists("test.json") && charCode is null)
            {
                return LocalData();
            }
            else if (charCode != null)
            {
                return OverwriteLocalData(charCode);
            }
            else
            {
                return WebData();
            }
        }
        
        
    }
}
