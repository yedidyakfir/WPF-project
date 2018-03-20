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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using StockPriceWinodw.ViewModel;

namespace StockPriceWinodw.View
{
    /// <summary>
    /// Interaction logic for CoinHistoryUC.xaml
    /// </summary>
    public partial class CoinHistoryUC : UserControl
    {
        public CoinHistoryUC()
        {
            InitializeComponent();
            DataContext = new CoinHistoryViewModel();
        }

        public CoinHistoryUC(string coin)
        {
            InitializeComponent();
            DataContext = new CoinHistoryViewModel(coin, "day");
            //YAxisName.Text = LineGraph.DataContext.ToString();
        }

        public void ChangeCoin(string coin, string format = "day")
        {
            DataContext = new CoinHistoryViewModel(coin, format);
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true);
        }
    }
}
