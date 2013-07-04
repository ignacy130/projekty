using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NotificationsExtensions.ToastContent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;
using Windows.UI.Core;
using Windows.UI.Popups;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238



namespace przypominacz
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

        List<ScheduledToastNotification> lista_zwin = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().ToList();
        Dictionary<string, ScheduledToastNotification> tosty = new Dictionary<string, ScheduledToastNotification>() { };
        
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
           //lista_tostow.ItemsSource
        }

        private void Slider_ValueChanged_0(object sender, RangeBaseValueChangedEventArgs e)
        {
            godziny.Text = godziny_ustaw.Value.ToString();
        }

        private void Slider_ValueChanged_1(object sender, RangeBaseValueChangedEventArgs e)
        {
            minuty.Text = minuty_ustaw.Value.ToString();
        }

        private void Slider_ValueChanged_2(object sender, RangeBaseValueChangedEventArgs e)
        {
            sekundy.Text = sekundy_ustaw.Value.ToString();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void godziny_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                godziny_ustaw.Value = Convert.ToDouble(godziny.Text);
            }
            catch (FormatException)
            {

            }
        }

        private void minuty_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                minuty_ustaw.Value = Convert.ToDouble(minuty.Text);
            }
            catch (FormatException)
            {

            }
        }

        private void sekundy_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                sekundy_ustaw.Value = Convert.ToDouble(sekundy.Text);
            }
            catch (FormatException)
            {

            }

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ToastTemplateType toastType = ToastTemplateType.ToastText01;
            XmlDocument toastXML = ToastNotificationManager.GetTemplateContent(toastType);
            XmlNodeList toastText = toastXML.GetElementsByTagName("text");

            var wiadomosc = tekst.Text;
            toastText[0].InnerText = wiadomosc;
            var random = new Random();
            Int32 dueTimeInSeconds = 0;

            dueTimeInSeconds = Convert.ToInt32(godziny_ustaw.Value * 3600 + minuty_ustaw.Value * 60 + sekundy_ustaw.Value + 1);

            DateTime dueTime = DateTime.Now.AddSeconds(dueTimeInSeconds);
            
            ScheduledToastNotification toast = new ScheduledToastNotification(toastXML, dueTime, TimeSpan.FromSeconds(300), 5);
            
            var idNumber = Math.Floor((double)(random.Next() * 100000000)).ToString();
            toast.Id = idNumber;
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            
            odswiez_liste();
         }

        private void lista_tostow_ItemClick(object sender, ItemClickEventArgs e)
        {
            var _item = e.ClickedItem;

            ScheduledToastNotification do_usuniecia = null;
            string[] lista_z_listy = new string[tosty.Keys.Count];
            tosty.Keys.CopyTo(lista_z_listy, 0);
            for (int i = 0; i < lista_z_listy.Length; i++)
            {
                if (lista_z_listy[i] == _item.ToString()) do_usuniecia = tosty[lista_z_listy[i]];
            }
            
            if(ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().Select(_ => do_usuniecia.Id) != null) ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(do_usuniecia);
            odswiez_liste();
        }

        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            odswiez_liste();
        }

        public void odswiez_liste()
        {
            tosty.Clear();
            lista_tostow.Items.Clear();
            var klucz = "";
            var zastap = " ";
            foreach (var tost in ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications())
	        {
                lista_tostow.Items.Add(tost.Content.InnerText + ", " + tost.DeliveryTime);
                klucz = tost.Content.InnerText + ", " + tost.DeliveryTime;
                System.Diagnostics.Debug.WriteLine("a");
                if (tosty.Keys.Contains(klucz)) { zastap = klucz + zastap; tosty.Add(zastap, tost); zastap += " "; } else tosty.Add(klucz, tost);
	        }
        }

        private void tekst_KeyDown(object sender, KeyRoutedEventArgs e)
        {
          // if(e.Key.ToString() == "Enter") odswiez_liste();
        }

        private void tekst_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


    }
}
