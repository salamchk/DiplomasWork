using DiplomasWork.Model;
using DiplomasWork.ViewModel;
using Plugin.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiplomasWork.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceConfigurationView : ContentPage
    {
        private RobotControl _control;
        private static DeviceConfigurationView _view;
        private DeviceConfigurationView()
        {
            _control = new RobotControl(DeviceConnection.GetConnection());
            InitializeComponent();
            this.BindingContext = _control;
        }

        public static DeviceConfigurationView GetDeviceConfigurationView()
        {
            if (_view == null) _view = new DeviceConfigurationView();
            return _view;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
            _control.SaveConfigurations();
        }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _view = null;
                _control = null;
            }
        }
        private async void ResetButton_Clicked(object sender, EventArgs e)
        {
            try
            {
            _control.SetDefaultConfigrations();
        }
            catch (Exception)
            {
                await Navigation.PopModalAsync();
                CrossToastPopUp.Current.ShowToastMessage("Lost Connection");
                _view = null;
                _control = null;
            }
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void MaxLeft_DragCompleted(object sender, EventArgs e)
        {
            try
            {
                MaxLeft.Value = Math.Round(MaxLeft.Value);
                _control.ConfigureAngle(Model.Command.MaxTurnLeft, (int)MaxLeft.Value);
            }
            catch(Exception)
            {
                CrossToastPopUp.Current.ShowToastMessage("Something broken");
            }
        }

        private void MaxRight_DragCompleted(object sender, EventArgs e)
        {
            try
            {
                MaxRight.Value = Math.Round(MaxRight.Value);
                _control.ConfigureAngle(Model.Command.MaxTurnRight, (int)MaxRight.Value);
            }
            catch (Exception)
            {
                CrossToastPopUp.Current.ShowToastMessage("Something broken");
            }
        }

        private void DirectPosition_DragCompleted(object sender, EventArgs e)
        {
            try
            {
                DirectPosition.Value = Math.Round(DirectPosition.Value);
                _control.ConfigureAngle(Model.Command.DirectPosition, (int)DirectPosition.Value);
            }
            catch (Exception)
            {
                CrossToastPopUp.Current.ShowToastMessage("Something broken");
            }
        }
    }
}