using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_4
{
    public partial class USBView : Form
    {
        private const int WM_DEVICECHANGE = 0X219;
        private static readonly Manager _manager = new Manager();
        private List<Usb> _deviceList;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_DEVICECHANGE)
            {
                ReloadForm();
            }
        }

        public USBView()
        {
            InitializeComponent();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            _deviceList = new List<Usb>();
            ReloadForm();         
            timer.Enabled = true;
        }
        private void ReloadForm()
        {
            _deviceList = _manager.DeviseListCreate();
            usbList.Items.Clear();

            foreach (Usb device in _deviceList)
            {
                var deviceInfo = new ListViewItem(device.DeviceName);
                deviceInfo.SubItems.AddRange(new[]
                {
                    device.FreeSpace,
                    device.UsedSpace,
                    device.TotalSpace
                });
                usbList.Items.Add(deviceInfo);
            }           
        }

        private void TickTimer(object sender, EventArgs e)
        {
            ReloadForm();
        }

        private void ExecuteDevice(object sender, MouseEventArgs e)
        {
            bool isEjected = _deviceList[usbList.HitTest(e.Location).Item.Index].EjectDevice();
            if (!isEjected)
            {
                MessageBox.Show("Can't eject","Message",MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Congratulations", "Message", MessageBoxButtons.OK);
            }
        }
    }
}
