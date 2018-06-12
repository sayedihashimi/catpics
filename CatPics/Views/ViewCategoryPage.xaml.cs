using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatPics.Shared;
using CatPics.Shared.Api;
using CatPics.ViewModels;
using Xamarin.Forms;

namespace CatPics.Views {
    public partial class ViewCategoryPage : ContentPage {
        public ViewCategoryPage(string category) {
            InitializeComponent();
            BindingContext = new ViewCategoryPageViewModel(category);
            Category = category;
            Title = category;
            BuildUi();
        }

        protected StackLayout MainLayout { get; set; }
        protected string Category { get; set; }

        public double ImageWidth { get; set; } = 500.0;
        public double ImageHeight { get; set; } = 400.0;

        public int RefreshDelayMs { get; set; } = 100;
        public int NumImagesToFetchInitially { get; set; } = 10;
        public int NumImageToFetchAfterInitial { get; set; } = 10;

        public async void BuildUi() {
            MainLayout = new StackLayout {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            ScrollView scrollView = null;
            Content = scrollView = new ScrollView {
                Content = MainLayout,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var apiHelper = new WebApiHelper();
            var catImages = await apiHelper.GetCats(NumImagesToFetchInitially, Category, Shared.Api.ImageSize.Med);

            foreach(var cImage in catImages){
                MainLayout.Children.Add(GetImageFrom(cImage, ImageWidth,ImageHeight));
            }

            scrollView.Scrolled += (sender, e) => {
                ScrollView sv = sender as ScrollView;
                if(sv != null){
                    double maxScrollY = sv.ContentSize.Height - sv.Height;
                    if(maxScrollY - sv.ScrollY < 500){
                        // LoadMoreImages will handle ignoring repeat calls
                        LoadMoreImages();
                    }
                }
            };
        }

        private bool IsLoadingMoreImages { get; set; } = false;
        private async void LoadMoreImages(){
            if(!IsLoadingMoreImages){

                IsLoadingMoreImages = true;
                System.Diagnostics.Debug.WriteLine("Loading more images now");
                try{
                    var apiHelper = new WebApiHelper();
                    var catImages = await apiHelper.GetCats(NumImageToFetchAfterInitial, Category, Shared.Api.ImageSize.Med);
                    foreach (var cImage in catImages) {
                        MainLayout.Children.Add(GetImageFrom(cImage, ImageWidth, ImageHeight));
                    }
                }
                finally{
                    System.Diagnostics.Debug.WriteLine("1:***");
                    await Task.Delay(RefreshDelayMs);
                    IsLoadingMoreImages = false;
                    System.Diagnostics.Debug.WriteLine("2:***");
                }
            }
            else{
                System.Diagnostics.Debug.Write(".");
            }
        }

        private Image GetImageFrom(CatImage cImage, double imageWidth, double imageHeight){
            var img = new Image {
                Source = cImage.Url,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            if(imageWidth > 0){
                img.WidthRequest = imageWidth;
            }
            if(imageHeight > 0){
                img.HeightRequest = imageHeight;
            }

            return img;
        }
    }
}
