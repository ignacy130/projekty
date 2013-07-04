using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace slowkolory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public Dictionary<string, SolidColorBrush> kolory = new Dictionary<string, SolidColorBrush> { };
        public ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        DispatcherTimer stoper = new DispatcherTimer();
        public string poprzedni = "";
        public bool czy = false;
        public int wynik = 0;
        public int czas = 60;
        bool Snapped = false;


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            button.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            kolory.Add("BLUE", new SolidColorBrush(Windows.UI.Colors.Blue));
            kolory.Add("ORANGE", new SolidColorBrush(Windows.UI.Colors.OrangeRed));
            kolory.Add("GREEN", new SolidColorBrush(Windows.UI.Colors.LawnGreen));
            kolory.Add("YELLOW", new SolidColorBrush(Windows.UI.Colors.Gold));
            kolory.Add("PINK", new SolidColorBrush(Windows.UI.Colors.Magenta));
            if (localSettings.Values["best"] == null) localSettings.Values["best"] = 0;
            best.Text = "Best: " + localSettings.Values["best"];
            stoper.Tick += stoper_Tick;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            wynik = 0;
            stoper.Interval = System.TimeSpan.FromMilliseconds(1000);
            stoper.Start();
            start.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            start.Foreground = new SolidColorBrush(Windows.UI.Colors.Gray);
            score.Text = "Score: 0";
            start.Opacity = 0;
            start.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            button.Visibility = Windows.UI.Xaml.Visibility.Visible;
            button.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            info.Opacity = 0;
            kolor.Opacity = 1;
        }

        void stoper_Tick(object sender, object e)
        {
            var r = new Random();
            var los = r.Next(0, kolory.Count);
            var kolorText = kolory.Keys.ElementAt(los).ToString();

            var los2 = r.Next(0, kolory.Count);
            var kolorColor = kolory.Values.ElementAt(los2);

            if (kolory[kolorText] == kolorColor) czy = true;
            else czy = false;

            kolor.Foreground = kolorColor;
            kolor.Text = kolorText;

            czas--;
            time.Text = "Time: " + czas;
            if (czas == 00)
            {
                stoper.Stop();
                if (wynik > Convert.ToInt32(localSettings.Values["best"].ToString())) localSettings.Values["best"] = wynik;
                best.Text = "Best: " + localSettings.Values["best"];
                czas = 60;
                time.Text = "Time: " + czas;
                info.Opacity = 1;
                kolor.Opacity = 0;
                start.Opacity = 1;
                button.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                start.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (czy == true) wynik += 100;
            else if (czy == false) wynik -= 100;
            score.Text = "Score: " + wynik;
        }

        private void Button_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Z && czy == true) wynik += 100;
            else if (e.Key == VirtualKey.Z && czy == false) wynik -= 100;
            System.Diagnostics.Debug.WriteLine(wynik);
            score.Text = "Score: " + wynik;
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window.Current.SizeChanged += (object sender2, Windows.UI.Core.WindowSizeChangedEventArgs e2) =>
            {
                ApplicationViewState myViewState = ApplicationView.Value;

                if (myViewState == ApplicationViewState.Snapped)
                {
                    this.Frame.Navigate(typeof(BlankPage1));
                    Snapped = true;
                }
                else if (myViewState != ApplicationViewState.Snapped)
                {
                    if (Snapped)
                    {
                        this.Frame.Navigate(typeof(MainPage));
                        Snapped = false;
                    }
                }
            };
        }



    }
}
