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
using System.Reflection;

namespace InlyITWPF.Models
{
    public class DataWorker : BaseViewModel
    {
        public static RootObject _rootObject { get; set; }

        public ObservableCollection<Valute> UpdateValutes(Valute valute)
        {
            _rootObject.Valute.Add(valute.CharCode,valute);
            
            return OverwriteLocalData(_rootObject);
        }

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
            var web = JsonConvert.DeserializeObject<RootObject>(jTok.ToString());

            _rootObject.Date = web.Date;
            _rootObject.PreviousDate = web.PreviousDate;
            _rootObject.PreviousURL = web.PreviousURL;
            _rootObject.Timestamp = web.Timestamp;


            List<Valute> listValute = new List<Valute>();
            List<string> itemsToDelete = new List<string>();
            var webList = web.Valute.Values.ToList();

            for (int i = 0; i < web.Valute.Count; i++)
            {
                foreach (var valute in _rootObject.Valute)
                {
                    if (valute.Value.CharCode == webList[i].CharCode)
                    {
                        var fields = typeof(Valute).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                        foreach (var field in fields)
                        {
                            var flag = false;
                            var value1 = field.GetValue(valute.Value);
                            var value2 = field.GetValue(webList[i]);
                            if (!value1.Equals(value2))
                            {
                                //_rootObject.Valute.Remove(valute.Value.CharCode);
                                //_rootObject.Valute.Add(valute.Value.CharCode, valute.Value);
                                listValute.Add(webList[i]);
                                flag = true;
                            }
                            if (flag)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            foreach (var valute in listValute)
            {
                _rootObject.Valute.Remove(valute.CharCode);
                _rootObject.Valute.Add(valute.CharCode, valute);
            }

            _valutes = new ObservableCollection<Valute>(_rootObject.Valute.Select(x => x.Value));
            MessageBox.Show("Файл успешно перезагружен с сайта");
            return OverwriteLocalData(_rootObject);
        }

        public ObservableCollection<Valute> OverwriteLocalData(RootObject rootObject)
        {
            string ser = JsonConvert.SerializeObject(rootObject);

            if (ser.Length > 0)
            {
                File.Create("test.json").Close();
                File.WriteAllText("test.json", ser, Encoding.UTF8);
            }
            _valutes = new ObservableCollection<Valute>(rootObject.Valute.Select(x => x.Value));

            return _valutes;
        }

        public ObservableCollection<Valute> DeleteLocalData(string charCode)
        {
            _rootObject.Valute.Remove(charCode);
            string ser = JsonConvert.SerializeObject(_rootObject);

            return OverwriteLocalData(_rootObject);

        }

        public ObservableCollection<Valute> LocalData()
        {


            string jsonString = File.ReadAllText("test.json");
            _rootObject = JsonConvert.DeserializeObject<RootObject>(jsonString);
            _valutes = new ObservableCollection<Valute>(_rootObject.Valute.Select(x => x.Value));

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
                return DeleteLocalData(charCode);
            }
            else
            {
                return WebData();
            }
        }
        
        
    }
}
