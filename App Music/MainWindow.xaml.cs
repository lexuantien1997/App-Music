using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using App_Music.Chart;
using System.Windows.Controls.Primitives;

namespace App_Music
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window/*, INotifyPropertyChanged*/
    {

        public MainWindow()
        {
            InitializeComponent();
            btnHome_Click(null,null);        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        //#region Set Binding with INotifyPropertyChanged
        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //}
        //#endregion

        //#region Set mouse Click of TextBlock

        //private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DockPanel dp = (DockPanel)(sender as TextBlock).Parent;

        //    if (previous != null)
        //        previous.Background = new SolidColorBrush(Color.FromRgb(43, 43, 43));
        //    dp.Background = new SolidColorBrush(Color.FromRgb(28, 115, 151));
        //    previous = dp;
        //    if (dp.Name == "dpChart")
        //    {
        //        ucChart Chart = new ucChart();
        //        GridMain.Children.Clear();
        //        GridMain.Children.Add(Chart);
        //    }
        //}

        //#endregion

        //#region Set mouse Click of Image
        //private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    DockPanel dp = (DockPanel)(sender as Image).Parent;

        //    if (previous != null)
        //        previous.Background = new SolidColorBrush(Color.FromRgb(43, 43, 43));
        //    dp.Background = new SolidColorBrush(Color.FromRgb(28, 115, 151));
        //    previous = dp;
        //    if (dp.Name == "dpChart")
        //    {
        //        ucChart Chart = new ucChart();
        //        GridMain.Children.Clear();
        //        GridMain.Children.Add(Chart);
        //    }

        //}
        //#endregion

        //#region Set DockPanel Enter - Leave - Click
        //private void DockPanel_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    DockPanel dp = (DockPanel)sender;
        //    if ((dp.Background as SolidColorBrush).Color != Color.FromRgb(28, 115, 151))
        //        dp.Background = new SolidColorBrush(Color.FromRgb(53, 53, 53));
        //}

        //private void DockPanel_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    DockPanel dp = (DockPanel)sender;
        //    if ((dp.Background as SolidColorBrush).Color != Color.FromRgb(28, 115, 151))
        //        dp.Background = new SolidColorBrush(Color.FromRgb(43, 43, 43));
        //}
        //private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    DockPanel dp = (DockPanel)sender;

        //    if (previous != null)
        //        previous.Background = new SolidColorBrush(Color.FromRgb(43, 43, 43));
        //    dp.Background = new SolidColorBrush(Color.FromRgb(28, 115, 151));
        //    previous = dp;

        //    if (dp.Name=="dpChart")
        //    {
        //        ucChart Chart = new ucChart();
        //        GridMain.Children.Clear();
        //        GridMain.Children.Add(Chart);
        //    }

        //}
        //#endregion

        #region Click into every toggle button in menu option

       // int Isclicked;
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 1;
            BindingMouseClick(btnHome);
        }

        private void btnTopics_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 2;
            BindingMouseClick(btnTopics);
        }

        private void btnCharts_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 3;
            BindingMouseClick(btnCharts);
            ucChart Charts = new ucChart();
            GridMain.Children.Clear();
            GridMain.Children.Add(Charts);
        }

        private void btnSongs_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 4;
            BindingMouseClick(btnSongs);
        }

        private void btnPlaylists_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 5;
            BindingMouseClick(btnPlaylists);
        }

        private void btnVideos_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 6;
            BindingMouseClick(btnVideos);
        }

        private void btnArtists_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 7;
            BindingMouseClick(btnArtists);
        }

        private void btnMusicFile_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 8;
            BindingMouseClick(btnMusicFile);
        }

        private void btnDownloading_Click(object sender, RoutedEventArgs e)
        {
           // Isclicked = 9;
            BindingMouseClick(btnDownloading);
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            BindingMouseClick(btnSetting);
        }

        ToggleButton tggPrevious = null;
        private void BindingMouseClick(ToggleButton tgg)
        {
            if (tggPrevious == tgg)
            {
                tgg.IsChecked = true;
                return;
            }
            if (tggPrevious != null)
                tggPrevious.IsChecked = false;
            tgg.IsChecked = true;
            tggPrevious = tgg;
            //if (Isclicked == 1)
            //    btnHome.IsChecked = true;
            //else
            //    btnHome.IsChecked = false;
            //if (Isclicked == 2)
            //    btnTopics.IsChecked = true;
            //else
            //    btnTopics.IsChecked = false;
            //if (Isclicked == 3)
            //    btnCharts.IsChecked = true;
            //else
            //    btnCharts.IsChecked = false;
            //if (Isclicked == 4)
            //    btnSongs.IsChecked = true;
            //else
            //    btnSongs.IsChecked = false;
            //if (Isclicked == 5)
            //    btnPlaylists.IsChecked = true;
            //else
            //    btnPlaylists.IsChecked = false;
            //if (Isclicked == 6)
            //    btnVideos.IsChecked = true;
            //else
            //    btnVideos.IsChecked = false;
            //if (Isclicked == 7)
            //    btnArtists.IsChecked = true;
            //else
            //    btnArtists.IsChecked = false;
            //if (Isclicked == 8)
            //    btnMusicFile.IsChecked = true;
            //else
            //    btnMusicFile.IsChecked = false;
            //if (Isclicked == 9)
            //    btnDownloading.IsChecked = true;
            //else
            //    btnDownloading.IsChecked = false;
        }


        #endregion


    }




    #region Convert value in UI
    public class ChangeValue : IValueConverter
    {
        private int valueChange;

        public int ValueChange { get => valueChange; set => valueChange = value; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) + valueChange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
    #endregion


}
