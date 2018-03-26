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
using StockPriceWinodw.Model;
using StockPriceWinodw.ViewModel;
using BL;

namespace StockPriceWinodw.View
{
    /// <summary>
    /// Interaction logic for MainViewUC.xaml
    /// </summary>
    public partial class MainViewUC : UserControl
    {
        public MainViewUC()
        {
            InitializeComponent();
            CurrencyList.PropertyChanged += ChangeCurrency; //add event to switch graph
            originCoin.DataContext = new CoinsViewModel();
            destinationCoin.DataContext = originCoin.DataContext;
        }

        //switch graph data
        internal void ChangeCurrency(object coinObj, PropertyChangedEventArgs arg)
        {
            if (!(coinObj is CoinModel))
                return;
            CoinModel coin = (CoinModel)coinObj;

            History.ChangeCoin(coin.coin);
        }

        private void Year_Click(object sender, RoutedEventArgs e)
        {
            Year.IsChecked = true;
            Day.IsChecked = false;
            Month.IsChecked = false;
            History.ChangeFormat("year");
        }

        private void Month_Click(object sender, RoutedEventArgs e)
        {
            Year.IsChecked = false;
            Day.IsChecked = false;
            Month.IsChecked = true;
            History.ChangeFormat("month");
        }

        private void Day_Click(object sender, RoutedEventArgs e)
        {
            Year.IsChecked = false;
            Day.IsChecked = true;
            Month.IsChecked = false;
            History.ChangeFormat("day");
        }

        private void IntegerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(originCoin.SelectedItem is CoinModel && destinationCoin.SelectedItem is CoinModel))
                return;

            destinationValue.Text = FactoryBL.get().Relation(((CoinModel)originCoin.SelectedItem).coin, ((CoinModel)destinationCoin.SelectedItem).coin, (double)originValue.Value).ToString();
        }
    }
}
