using GalaSoft.MvvmLight.Command;
using InlyITWPF.Models;
using InlyITWPF.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace InlyITWPF.ViewModels
{
    public class AddValuteViewModel : BaseViewModel
    {
        public string NameTextBox { get; set; }
        public string ValueTextBox { get; set; }
        public string NominalTextBox { get; set; }
        public string PreviousTextBox { get; set; }
        public string IdTextBox { get; set; }
        public string CharCodeTextBox { get; set; }
        public string NumCodeTextBox  { get; set; }

        private RelayCommand _addValute;
        public RelayCommand AddValute
        {
            get
            {
                return _addValute ?? new RelayCommand(() =>
                {
                    AddValuteItem();
                }
                );
            }
        }
        private void AddValuteItem()
        {
            try
            {
                Valute valute = new Valute();

                valute.Value = double.Parse(ValueTextBox);
                valute.Previous = double.Parse(PreviousTextBox);
                valute.NumCode = NumCodeTextBox;
                valute.CharCode = CharCodeTextBox;
                valute.ID = IdTextBox;
                valute.Name = NameTextBox;
                valute.Nominal = int.Parse(NominalTextBox);
                
                var currWin = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

                MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
                mainWindowViewModel.UpdateValutes(valute, currWin);
            }
            catch
            {
                MessageBox.Show("Возможно, вы ввели не верный формат данных или не заполнили все поля");
            }
        }
    }
}
