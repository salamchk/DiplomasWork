using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomasWork.Model
{
    public class DeviceSearcher
    {
        public List<Device> Devices { get; private set; }
        private static BluetoothAdapter _adapter;
        private static DeviceSearcher _searcher;

        private DeviceSearcher()
        {
        }

        public static DeviceSearcher GetSearcher()
        {
            if (_searcher == null) _searcher = new DeviceSearcher();
            return _searcher;
        }

        public void Search()
        {
            _adapter = BluetoothAdapter.DefaultAdapter;
            Devices = _adapter.BondedDevices.Select(device =>
            new Device(name: device.Name, address: device.Address)).ToList();
        }
    }
}
