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
using StockPriceWinodw.ViewModel;

namespace StockPriceWinodw.View
{
    /// <summary>
    /// Interaction logic for CurrencyHistoryUC.xaml
    /// </summary>
    public partial class CurrencyHistoryUC : UserControl
    {
        public CurrencyHistoryUC()
        {
            InitializeComponent();
            LineGraph.DataContext = new CoinHistoryViewModel();
            //YAxisName.Text = LineGraph.DataContext.ToString();
        }

        public CurrencyHistoryUC(string coin)
        {
            InitializeComponent();
            LineGraph.DataContext = new CoinHistoryViewModel(coin,"day");
            //YAxisName.Text = LineGraph.DataContext.ToString();
        }

        private void setData(string coin)
        {

        }

        public void ChangeCoin(string coin,string format = "day")
        {
            LineGraph.DataContext = new CoinHistoryViewModel(coin, format);
        }
    }
}
