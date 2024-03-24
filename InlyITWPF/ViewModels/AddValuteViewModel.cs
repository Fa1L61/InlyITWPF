using GalaSoft.MvvmLight.Command;
using InlyITWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InlyITWPF.ViewModels
{
    public class AddValuteViewModel : BaseViewModel
    {
        private RelayCommand _showMainWin;
        public RelayCommand ShowMainWin
        {
            get
            {
                return _showMainWin ?? new RelayCommand(() =>
                {
                    ShowMainWindow();
                }
                );
            }
        }

        private void ShowMainWindow()
        {
            var win = Application.Current.Windows.OfType<MainWindow>().SingleOrDefault();

            var currWin = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            currWin.Close();

            win.Show();
        }
    }
}
