
using DiplomasWork.Views;
using Java.Lang;
using System;

using Xamarin.Forms;

namespace DiplomasWork
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ControlView());
        }

        private async void btnConnect_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DeviceConnectView());
        }

        private void btnClose_Clicked(object sender, EventArgs e)
        {
            JavaSystem.Exit(0);
        }
    }
}
