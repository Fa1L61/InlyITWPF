using InlyITWPF.Models;
using InlyITWPF.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.ComponentModel;
using GalaSoft.MvvmLight.Command;
using System.IO;

namespace InlyITWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private MainWindow _mainViewModel;
        private RootObject RootObject { get; }
        private Visibility _stackPanel { get; }
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

        private Valute selectedValutes;
        private bool _isWeb = true;

        public MainWindowViewModel()
        {
            
            // переделать в класс, который будет дополнительно загружать данные

            byte[] jToken;

            if (_isWeb)
            {
                jToken = new WebClient().DownloadData("https://www.cbr-xml-daily.ru/daily_json.js");
                var jTok = Encoding.UTF8.GetString(jToken);
                RootObject = JsonConvert.DeserializeObject<RootObject>(jTok.ToString());
                Valutes = new ObservableCollection<Valute>(RootObject.Valute.Select(x => x.Value));
                if(Valutes.Count > 0)
                {
                    SelectedValutes = Valutes[0];
                }
                else
                {
                    _stackPanel = Visibility.Hidden;
                }
                string ser = JsonConvert.SerializeObject(RootObject);

                if(ser.Length > 0)
                {
                    File.Create("test.json").Close();
                    File.WriteAllText("test.json", jTok, Encoding.UTF8);
                }
                

            }
            else
            {
                //логика с бд...
            }           
        }

        public Valute SelectedValutes
        {
            get { return selectedValutes; }
            set
            {
                selectedValutes = value;
                OnPropertyChanged("SelectedValutes");
            }
        }

        private void OpenAddValuteWindow()
        {
            AddValuteWindow viewModel = new AddValuteWindow();
            viewModel.ShowDialog();
            
        }

        private RelayCommand _openAddValuteWnd;
        public RelayCommand OpenAddValuteWnd
        {
            get
            {
                return _openAddValuteWnd ?? new RelayCommand(() => 
                    {
                        OpenAddValuteWindow();
                    }
                );
            }
        }
        /// servise Который работает с данными 
        /// Сервис который работает с бд (Entaty можно в принципе и без сервиса)
    }
}
