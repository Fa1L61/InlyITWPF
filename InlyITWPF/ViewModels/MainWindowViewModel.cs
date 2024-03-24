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
using System.Windows.Data;
using System;
using System.Windows.Controls;

namespace InlyITWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private Visibility _stackPanel { get; set; }
        public string MyText { get; }
        private Valute _selectedValute;
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
        public MainWindowViewModel()
        {
            
            DataWorker dataReader = new DataWorker();
            LocalSettings localSettings = new LocalSettings();

            MyText = localSettings.DateSetter();
            Valutes = dataReader.GetData();

            localSettings.DateWriter(localSettings);

            if (Valutes.Count > 0)
            {
                SelectedValute = Valutes[0];
            }
        }

        public Valute SelectedValute
        {
            get { return _selectedValute; }
            set
            {
                _selectedValute = value;
                OnPropertyChanged("SelectedValute");
            }
        }

        private void OpenAddValuteWindow()
        {
            AddValuteWindow viewModel = new AddValuteWindow();
            viewModel.Show();
            
        }

        private RelayCommand _updateDataWeb;
        public RelayCommand UpdateDataWeb
        {
            get
            {
                return _updateDataWeb ?? new RelayCommand(() => 
                {
                    UpdateDataFromWeb();
                }
                );
            }
        }
        private void UpdateDataFromWeb()
        {
            DataWorker dataReader = new DataWorker();
            
            Valutes = new ObservableCollection<Valute>(dataReader.WebData());
            
            if (Valutes.Count > 0)
            {
                SelectedValute = Valutes[0];
            }

            ICollectionView view = CollectionViewSource.GetDefaultView(_valutes);
            view.Refresh();
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
