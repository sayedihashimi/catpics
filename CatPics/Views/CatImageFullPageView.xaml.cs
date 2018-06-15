using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatPics.Shared.Api;
using Xamarin.Forms;

namespace CatPics.Views {
    public partial class CatImageFullPageView : ContentPage {
        public CatImageFullPageView(ICatImageListView listView) {
            InitializeComponent();
            ListView = listView;

            BuildUi();
        }

        protected ICatImageListView ListView { get; set; }

        private List<CatImage> CatImages { get; set; } = new List<CatImage>();

        private int currentIndex = 0;

        private (double Height, double Width) OriginalSize { get; set; }

        private async Task InitCatImages(){
            await ListView.FetchAndAddImagesToView();
            CatImages = ListView.GetCatImages();
        }

        private async void BuildUi(){
            await InitCatImages();
            Image img = new Image();
            img.Source = CatImages[currentIndex].Url;
            img.Aspect = Aspect.AspectFit;

            img.SizeChanged += (sender, e) => {
                if(OriginalSize.Height <= 0 && 
                   OriginalSize.Width <= 0 &&
                   img.Height > 0 &&
                   img.Width >0){

                    OriginalSize = (img.Width, img.Height);
                }

            };

            img.HeightRequest = 600;
            img.WidthRequest = 600;

            var layout = new StackLayout();
            layout.Children.Add(img);

            Content = img;
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height);
            Image img = Content as Image;

            if(img != null){
                System.Diagnostics.Debug.WriteLine($"height= {height} width={width}");

                var newsize = GetScreenSizeFor(OriginalSize.Width, OriginalSize.Height);
                img.WidthRequest = newsize.width;
                img.HeightRequest = newsize.height;
            }
        }

        private (double width, double height) GetScreenSizeFor(double width, double height){
            (double width, double height) retvalue = (0, 0);

            // see which is the long size
            if(width > height){
                retvalue.width = Math.Min(width, Width);
                // calculate height based on originalsize and new width
                retvalue.height = (int)Math.Floor(OriginalSize.Height / OriginalSize.Width * retvalue.width);
            }
            else{
                retvalue.height = Math.Min(height, Height);
                // calculate width based on originalsize and new width
                retvalue.width = (int)Math.Floor(OriginalSize.Width/OriginalSize.Height * retvalue.height);
            }

            return retvalue;
        }
    }
}
