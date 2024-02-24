// VideoPage

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using VideoLibrary; //using MyToolkit.Multimedia;


namespace WinBeta_Videos
{
    
    public sealed partial class VideoPage : Page
    {
        MainPage.Video ytvideo;
        //YouTubeQuality selectedQuality = YouTubeQuality.QualityHigh;//
        //YouTubeUri mainVideo;
        DataTransferManager dataTransferManager;

        /// <summary>
        /// Youtube "entities"
        /// </summary>
        public string link { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string Length { get; set; }
        public string AudioBitrate { get; set; }
        public string AudioFormats { get; set; }
        public string VideoFormat { get; set; }
        public string VideoRes { get; set; }
        public string FPS { get; set; }
        public string VideoID { get; set; }
        public string ChosenQuality { get; set; }
        public int ChosenQualityInt { get; set; }
        public bool IsHDQuality { get; set; }
        public YouTubeVideo video { get; set; }
        public YouTubeVideo maxVideo { get; set; }
        public YouTubeVideo maxBitrate { get; set; }
        public string ThrownEncodingError { get; set; }
        public IEnumerable<YouTubeVideo> videoInfos { get; set; }


        public VideoPage()
        {
            this.InitializeComponent();

            Windows.UI.Core.SystemNavigationManager
                .GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };

            dataTransferManager = DataTransferManager.GetForCurrentView();

            ChosenQuality = "144p";
            ChosenQualityInt = 144;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dataPackage = args.Request.Data;
            dataPackage.Properties.Title = "WinBeta Videos";
            dataPackage.Properties.Description = "Sharing Video Link";

            //RnD
            dataPackage.SetWebLink(new Uri("http://youtube.com/watch?v=" + ytvideo.Id));
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ytvideo = (MainPage.Video)e.Parameter;

            videosTitle.Text = ytvideo.Title;

            try
            {
                await LoadPage();
            }
            catch (Exception ex) // AggregateException
            {
                MessageDialog m = new MessageDialog("Could play video: " + ex.Message, 
                    "WinBeta Videos Error");
                await m.ShowAsync();
            }
        }

     
        private async Task LoadPage()
        {
            progressRing.IsActive = true;

            link = "http://youtube.com/watch?v=" + ytvideo.Id;

            /*
            /// Need to fix
            try
            {
                if (link.Contains("http:"))
                {
                    string fixedlink = link.Replace("http:", "https:");
                    link = fixedlink;
                }
                if (link.Contains("m."))
                {
                    string fixedlink = link.Replace("m.", "www.");
                    link = fixedlink;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] link.Replace error: " + ex.Message);
            }
            */

            try
            {
                //RnD
                //var mainVideo =
                //    await MyToolkit.Multimedia.YouTube.GetVideoUriAsync(
                //        ytvideo.Id/*"SuBDt7GCbIc"*/, default);// selectedQuality);

                //********************************************************************
                // starting point for YouTube actions
                YouTube youTube = YouTube.Default;

                // gets a Video object with info about the video
                YouTubeVideo Justvideo = youTube.GetVideo(link);
                videoInfos = youTube.GetAllVideosAsync(link).GetAwaiter().GetResult();

                //ProgressText.Text = "";

                //video = videoInfos.First(i => i.IsAdaptive);
                video = videoInfos.First(i => i.Resolution == ChosenQualityInt);

                Debug.WriteLine(video.Uri);
                //Debug.WriteLine("[i]  ChosenQualityInt=", ChosenQualityInt);
                //********************************************************************

                Uri result;
                if (!Uri.TryCreate(video.Uri, UriKind.Absolute, out result))
                {
                    Debug.WriteLine("[ex] Wrong uri!");    
                }

                mediaPlayer.Source = result;//ParseUri(video.Uri);//mainVideo.Uri;



                mediaPlayer.Play();
                progressRing.IsActive = false;
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    Debug.WriteLine("[ex] Quality Not Supported, try something else");
                }
                
                MessageDialog m = new MessageDialog(/*"Quality Not Supported, try something else"*/
                "Could play video: " + ex.Message, "WinBeta Videos Error");

                await m.ShowAsync();
                                            
                progressRing.IsActive = false;
            }     
        }

        private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as MenuFlyoutItem;

            switch (s.Text)
            {
                case "144p":
                    //selectedQuality = YouTubeQuality.Quality144P;
                    ChosenQuality = "144p";
                    ChosenQualityInt = 144;
                    break;
                case "240p":
                    //selectedQuality = YouTubeQuality.Quality240P;
                    ChosenQuality = "240p";
                    ChosenQualityInt = 240;
                    break;
                case "270p":
                    //selectedQuality = YouTubeQuality.Quality270P;
                    ChosenQuality = "270p";
                    ChosenQualityInt = 270;
                    break;
                case "360p":
                    //selectedQuality = YouTubeQuality.Quality360P;
                    ChosenQuality = "360p";
                    ChosenQualityInt = 360;
                    break;
                case "480p":
                    //selectedQuality = YouTubeQuality.Quality480P;
                    ChosenQuality = "480p";
                    ChosenQualityInt = 480;
                    break;
                case "520p":
                    //selectedQuality = YouTubeQuality.Quality520P;
                    ChosenQuality = "520p";
                    ChosenQualityInt = 520;
                    break;
                case "720p":
                    //selectedQuality = YouTubeQuality.Quality720P;
                    ChosenQuality = "720p";
                    ChosenQualityInt = 720;
                    break;
                case "1080p":
                    //selectedQuality = YouTubeQuality.Quality1080P;
                    ChosenQuality = "1080p";
                    ChosenQualityInt = 1080;
                    break;
                case "4k":
                    //selectedQuality = YouTubeQuality.Quality2160P;
                    ChosenQuality = "2160p";
                    ChosenQualityInt = 2160;
                    break;
                default:
                    MessageDialog m = new MessageDialog("Ubnknown Quality", "WinBeta Videos Error");
                    await m.ShowAsync();
                    break;
            }

            try
            {
                await LoadPage();
            }
            catch (AggregateException ex)
            {
                MessageDialog m = new MessageDialog("Could play video: " + ex.Message, "WinBeta Videos Error");
                await m.ShowAsync();
            }

        }

        private void shareButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            dataTransferManager.DataRequested -= OnDataRequested;
            dataTransferManager.DataRequested += OnDataRequested;

            DataTransferManager.ShowShareUI();
        }
    }
}
