﻿using InlyITWPF.Models;
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
using System.Windows.Navigation;

namespace InlyITWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        private DataWorker _dataWorker = new DataWorker();
        private LocalSettings _localSettings = new LocalSettings();
        public string LastActiveDate { get; }
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
            LastActiveDate = _localSettings.DateSetter();
            Valutes = _dataWorker.GetData();

            _localSettings.DateWriter(_localSettings);

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

            ICollectionView view = CollectionViewSource.GetDefaultView(Valutes);
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

        private void OpenAddValuteWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            AddValuteWindow valuteWindow = new AddValuteWindow();
            window.Hide();
            valuteWindow.Show();
        }

        private RelayCommand _deleteValute;
        public RelayCommand DeleteSelectedValute
        {
            get
            {
                return _deleteValute ?? new RelayCommand(() =>
                {
                    DeleteValute();
                }
                );
            }
        }

        private void DeleteValute()
        {
            Valutes = _dataWorker.GetData(SelectedValute.CharCode);

            ICollectionView view = CollectionViewSource.GetDefaultView(Valutes);
            view.Refresh();
        }

        public void UpdateValutes(Valute valute, Window window)
        {
            window.Close();
            
            Valutes = _dataWorker.UpdateValutes(valute);
            
            ICollectionView view = CollectionViewSource.GetDefaultView(Valutes);
            view.Refresh();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }


        /// servise Который работает с данными 
        /// Сервис который работает с бд (Entaty можно в принципе и без сервиса)
    }
}
