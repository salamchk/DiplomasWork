using DiplomasWork.Model;
using DiplomasWork.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiplomasWork.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceConnectView : ContentPage
    {
        private static DeviceConnection _connection;
        public DeviceConnectView()
        {
            InitializeComponent();
            var searcher = DeviceSearcher.GetSearcher();
            searcher.Search();
            devicesList.ItemsSource = new ObservableCollection<Model.Device>(searcher.Devices);
            if (_connection != null)
            {
                devicesList.IsEnabled = false;
                DisconnectButton.IsEnabled = true;
            }
            else
            {
                devicesList.IsEnabled = true;
                DisconnectButton.IsEnabled = false;
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                devicesList.IsEnabled = false;
                _connection = DeviceConnection.GetConnection((Model.Device)e.Item);
                _connection.Connect();
                DisconnectButton.IsEnabled = true;
            }
            catch (Exception)
            {

                throw;
            }

            devicesList.SelectedItem = null;
        }

        private void DisconnectButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                _connection.Disconect();
                devicesList.IsEnabled = true;
                DisconnectButton.IsEnabled = false;
                _connection = null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}