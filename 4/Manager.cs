using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MediaDevices;

namespace Laba_4
{
    class Manager
    {
        public List<Usb> DeviseListCreate()
        {
            List<Usb> usbDevices = new List<Usb>();
            List<DriveInfo> diskDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Removable).ToList();
            List<MediaDevice> mtpDrives = MediaDevice.GetDevices().ToList();
            foreach (MediaDevice device in mtpDrives)
            {
                device.Connect();
                if (device.DeviceType != DeviceType.Generic)
                {
                    usbDevices.Add(new Usb()
                    {
                        DeviceName = device.FriendlyName + "  " + device.Description,
                        FreeSpace = null,
                        UsedSpace = null,
                        TotalSpace = null,
                        IsMtpDevice = true
                    });
                }
            }
            foreach (DriveInfo drive in diskDrives)
            {
                usbDevices.Add(new Usb()
                {
                    DeviceName = drive.Name,
                    FreeSpace = Convert(drive.TotalFreeSpace),
                    UsedSpace = Convert(drive.TotalSize - drive.TotalFreeSpace),
                    TotalSpace = Convert(drive.TotalSize),
                    IsMtpDevice = false
                });
            }
            return usbDevices;
        }

        private string Convert(long value)
        {
            double megaBytes = (value / 1024) / 1024;
            return megaBytes.ToString() + " mb";
        }
    }
}
