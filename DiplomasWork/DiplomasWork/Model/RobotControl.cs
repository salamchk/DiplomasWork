using DiplomasWork.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DiplomasWork.Model
{
    [Serializable]
    public class RobotControl : INotifyPropertyChanged
    {
        private static int _maxTurnRight;
        private static int _maxTurnLeft;
        private static int _directPosition;
        private static DeviceConnection _connection;

        public event PropertyChangedEventHandler PropertyChanged;

        public int MaxTurnRight
        {
            get => _maxTurnRight;
            private set
            {
                if (value > 9)
                    _maxTurnRight = 9;
                if (value < 0)
                    _maxTurnRight = 0;
                else _maxTurnRight = value;
                OnPropertyChanged();
            }
        }
        public int MaxTurnLeft
        {
            get => _maxTurnLeft;
            private set
            {
                if (value > 9)
                    _maxTurnLeft = 9;
                if (value < 0)
                    _maxTurnLeft = 0;
                else _maxTurnLeft = value;
                OnPropertyChanged();
            }
        }
        public int DirectPosition
        {
            get => _directPosition;
            private set
            {
                if (value > 180)
                    _directPosition = 180;
                if (value < 0)
                    _directPosition = 0;
                else _directPosition = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public RobotControl(DeviceConnection connection)
        {
            _connection = connection;
            LoadConfigurations();
            TurnOnTheLight(false);
        }

        public void ConfigureAngle(Command typeOfAConfiguration, int position)
        {
            if (typeOfAConfiguration != Command.MaxTurnRight && typeOfAConfiguration != Command.MaxTurnLeft && typeOfAConfiguration != Command.DirectPosition)
                throw new Exception("wrong command");
            if (position > 9) position = 9;
            else if (position < 0) position = 0;
            char symbolToTransmit = position.ToString().First();

            if (typeOfAConfiguration == Command.DirectPosition && position != DirectPosition)
                DirectPosition = position;

            else if (typeOfAConfiguration == Command.MaxTurnRight && position != MaxTurnRight)
                MaxTurnRight = position;

            else if (typeOfAConfiguration == Command.MaxTurnLeft && position != MaxTurnLeft)
                MaxTurnLeft = position;

            _connection.SendMessage(new byte[] { (byte)typeOfAConfiguration, (byte)symbolToTransmit });
        }


        public void Move(bool allowMoving, bool forward, int speed)
        {
            char speedToTransmit = speed > 9 ? '9' : speed < 0 ? '0' : speed.ToString().First();

            if (!allowMoving)
                _connection.SendMessage(new byte[] { (byte)Command.StopMoving, (byte)'0' });
            if (forward)
                _connection.SendMessage(new byte[] { (byte)Command.MoveForward, (byte)speedToTransmit });
            else _connection.SendMessage(new byte[] { (byte)Command.MoveBack, (byte)speedToTransmit });
        }


        public void TurnOnTheLight(bool turnOn)
        {
            _connection.SendMessage(new byte[] { (byte)Command.TurnOnTheLight,
                (byte)(turnOn == true ? '1' :'0')});
        }

        public void AutomaticMode(bool automatic)
        {
            _connection.SendMessage(new byte[] { (byte)Command.AutomaticMode,
                (byte)(automatic == true ? '1' :'0')});
        }

        public void Turn(bool right, int angle)
        {
            if (right)
            {
                if (angle > MaxTurnRight && angle <= 9) angle = MaxTurnRight;
                _connection.SendMessage(new byte[] { (byte)Command.TurnRight, (byte)angle.ToString().First() });

            }
            else
            {
                if (angle > MaxTurnLeft && angle <= 9) angle = MaxTurnLeft;
                _connection.SendMessage(new byte[] { (byte)Command.TurnLeft, (byte)angle.ToString().First() });

            }
        }

        public void ToDirectPosition()
        {
            _connection.SendMessage(new byte[] { (byte)Command.SetDirectPosition, (byte)'0' });
        }

        public void HardStop()
        {
            _connection.SendMessage(new byte[] { (byte)Command.StopMoving, (byte)'0' });
        }

        public void SaveConfigurations()
        {
            Preferences.Clear();
            Preferences.Set(nameof(MaxTurnRight), (int)MaxTurnRight);
            Preferences.Set(nameof(MaxTurnLeft), (int)MaxTurnLeft);
            Preferences.Set(nameof(DirectPosition), (int)DirectPosition);
        }

        private void LoadConfigurations()
        {
            ConfigureAngle(Model.Command.MaxTurnRight, (int)Preferences.Get(nameof(MaxTurnRight), 6));
            ConfigureAngle(Model.Command.MaxTurnLeft, (int)Preferences.Get(nameof(MaxTurnLeft), 4));
            ConfigureAngle(Model.Command.DirectPosition, (int)Preferences.Get(nameof(DirectPosition), 3));

        }

        public void SetDefaultConfigrations()
        {
            MaxTurnRight = 6;
            MaxTurnLeft = 4;
            DirectPosition = 3;
        }
    }
}
