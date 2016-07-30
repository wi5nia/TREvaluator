using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TREvalKiosk
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MediaCapture _captureManager = null;
        private DeviceInformationCollection availableCameras;
        private int selectedCamera = 0;

        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            availableCameras = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);

            await InitMediaCapture();
            await InitCameraSwitch();
        }

        async Task InitMediaCapture(string cameraIndex = "0")
        {
            _captureManager = new MediaCapture();
            await _captureManager.InitializeAsync(
                new MediaCaptureInitializationSettings()
                {
                    VideoDeviceId = cameraIndex == "0" ? availableCameras.FirstOrDefault().Id.ToString() : cameraIndex.ToString()
                }
                );
            captureElement.Source = _captureManager;
            captureElement.FlowDirection = FlowDirection.LeftToRight;
            await _captureManager.StartPreviewAsync();

        }

        async Task InitCameraSwitch()
        {
            if (availableCameras.Count == 2)
            {
                cameraSwitch.Visibility = Visibility.Visible;
            }
            else if (availableCameras.Count == 1)
            {
                cameraSwitch.Visibility = Visibility.Collapsed;
            }
        }

        private async void cameraSwitch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (selectedCamera == 0)
            {
                await InitMediaCapture(availableCameras[1].Id);
                selectedCamera = 1;
            }
            else
            {
                await InitMediaCapture(availableCameras[0].Id);
                selectedCamera = 0;
            }
        }
    }
}
