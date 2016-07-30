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
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SharedProject;
using TRMobEval;



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

        private BitmapImage _bmpImage = null;
        private StorageFile _file = null;
        private bool _isCaptureMode = true;

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

        private async void OnActionClick(object sender, RoutedEventArgs e)
        {
            if (_isCaptureMode == true)
            {
                await ImageCaptureAndDisplay();
                
                try
                {
                    float result = await Core.GetAvgEmotionScore(await _file.OpenStreamForReadAsync());
                    hapinessRatio.Text = Core.GetEmotionMessage(result);
                    previewImage.Visibility = Visibility.Visible;
                    // Commit to DB
                    SessionEval item = new TRMobEval.SessionEval
                    {
                        Id = Guid.NewGuid().ToString(),
                        SessionName = "TrDemoSession",
                        SessionScore = result,
                        ScoreDateString = DateTime.Now.ToString()
                    };
                    await App.MobileService.GetTable<SessionEval>().InsertAsync(item);
                }
                catch (Exception ex)
                {
                   // hapinessRatio.Text = ex.Message;
                }
                finally
                {
                    actionButton.Content = "RESET";
                    _isCaptureMode = false;
                }
            }
            else
            {
                previewImage.Visibility = Visibility.Collapsed;
                actionButton.Content = "EVALUATE";
                hapinessRatio.Text = "";
                _isCaptureMode = true;
            }
        }


        private async Task<bool> ImageCaptureAndDisplay()
        {
            ImageEncodingProperties imageFormat = ImageEncodingProperties.CreateJpeg();

            // create storage file in local app storage 
            _file = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                 $"myPhoto_{Guid.NewGuid()}.jpg",
                 CreationCollisionOption.GenerateUniqueName);

            // capture to file 
            await _captureManager.CapturePhotoToStorageFileAsync(imageFormat, _file);

            _bmpImage = new BitmapImage(new Uri(_file.Path));
            previewImage.Source = _bmpImage;

            return true;
        }

    }
}
