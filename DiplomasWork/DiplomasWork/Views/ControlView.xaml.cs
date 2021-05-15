using DiplomasWork.Model;
using DiplomasWork.ViewModel;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiplomasWork.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ControlView : ContentPage
    {
        private static DeviceConnection _connection;
        private static RobotControl _control;
        private byte speed = 0;

        public ControlView()
        {
            InitializeComponent();
            Content.IsVisible = true;
            if (_connection == null)
            {
                try
                {
                    _connection = DeviceConnection.GetConnection();
                    _control = new RobotControl(_connection);
                  
                }
                catch (Exception)
                {
                    CrossToastPopUp.Current.ShowToastMessage("You are not connected to any device");

                }
            }
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                _control.TurnOnTheLight(Lights.IsToggled);
                if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }

        private async void AutoSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                _control.AutomaticMode(AutoSwitch.IsToggled);
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
            _connection.Disconect();
            _connection = null;
            _control = null;
        }

        private async void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;
            try
            {
                await Navigation.PushModalAsync(DeviceConfigurationView.GetDeviceConfigurationView());
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }

        private async void TurnLeftButton_Pressed(object sender, EventArgs e)
        {
            try
            {
                _control.Turn(false, _control.MaxTurnLeft);
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }

        private async void TurnRightButton_Pressed(object sender, EventArgs e)
        {
            if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;
            try
            {
                _control.Turn(true, _control.MaxTurnRight);
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }

        private async void OnSetForwardPosition(object sender, EventArgs e)
        {
            try
            {
                _control.ToDirectPosition();
                if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection = null;
                _control = null;
            }
        }

        private async void OnStopMoving(object sender, EventArgs e)
        {
            try
            {
                _control.HardStop();
                speed = 0;
                if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection.Disconect();
                _connection = null;
                _control = null;
            }
        }


        private async void MoveBackButton_Pressed(object sender, EventArgs e)
        {
            if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;

            try
            {
                await Task.Run(() =>
                {
                    while (MoveBackButton.IsPressed && speed < 10)
                    {
                        _control.Move(allowMoving: true, forward: false, speed: speed);
                        speed++;
                        Thread.Sleep(150);
                    }
                });
                _control.ToDirectPosition();
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection = null;
                _control = null;
            }
        }

        private async void MoveForwardButton_Pressed(object sender, EventArgs e)
        {
            if (AutoSwitch.IsToggled) AutoSwitch.IsToggled = false;

            try
            {
                await Task.Run(() =>
                {
                    while(MoveForwardButton.IsPressed && speed < 10)
                    {
                        _control.Move(allowMoving: true, forward: true, speed: speed);
                        speed++;
                        Thread.Sleep(150);
                    }
                });
                _control.ToDirectPosition();
            }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _connection = null;
                _control = null;
            }
        }
    }
}