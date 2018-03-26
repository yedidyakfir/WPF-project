using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private string coin = "";
        private string format = "";
        public CoinHistoryUC()
        {
            InitializeComponent();
            DataContext = new CoinHistoryViewModel();
        }

        public CoinHistoryUC(string c)
        {
            InitializeComponent();
            ChangeCoin(c, "day");
            
            //DataContext = new CoinHistoryViewModel(coin, "day");
            //YAxisName.Text = LineGraph.DataContext.ToString();
        }

        private void changeView()
        {
            if (coin == "")
                DataContext = new CoinHistoryViewModel();
            else
               DataContext = new CoinHistoryViewModel(coin, format);
        }

        //syncronize function to change data or format
        public async void ChangeCoin(string c, string f = "day")
        {
            coin = c;
            format = f;
            changeView();
            //Thread getHistoryThread = new Thread(changeView);
            //getHistoryThread.Start();
        }

        public async void ChangeFormat(string f)
        {
            format = f;
            changeView();
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true);
        }
    }
}
