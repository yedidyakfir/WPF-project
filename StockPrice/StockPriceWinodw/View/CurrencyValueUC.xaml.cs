using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StockPriceWinodw.ViewModel;

namespace StockPriceWinodw.View
{
    /// <summary>
    /// Interaction logic for CurrencyValueUC.xaml
    /// </summary>
    public partial class CurrencyValueUC : UserControl, INotifyPropertyChanged
    {
        public CurrencyValueUC()
        {
            InitializeComponent();
            this.DataContext = new CoinsViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void syncgrid_SelectionChanged(object sender, Syncfusion.UI.Xaml.Grid.GridSelectionChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(((CoinsViewModel)DataContext).SelectedCoin, new PropertyChangedEventArgs("selction changed"));
        }
    }
}
