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

            double imgwidth = 500.0;
            double imgheight = 200.0;

            var apiHelper = new WebApiHelper();
            var catImages = await apiHelper.GetCats(10, Category, Shared.Api.ImageSize.Med);

            foreach(var cImage in catImages){
                MainLayout.Children.Add(GetImageFrom(cImage, imgwidth,imgheight));
            }

//            catImages.ElementAt(catImages.Count - 1)

            (MainLayout.Children.ElementAt(MainLayout.Children.Count-1)).Focused += (sender,e)=> {
                string foo = "bar";
                System.Diagnostics.Debug.WriteLine($"foo={foo}");
            };

            scrollView.Scrolled += (sender, e) => {
                // System.Diagnostics.Debug.WriteLine($"ScrollY: {((ScrollView)sender).ScrollY}");

                ScrollView sv = sender as ScrollView;
                if(sv != null){
                    double maxScrollY = sv.ContentSize.Height - sv.Height;
                    if(maxScrollY - sv.ScrollY < 1){
                        // load more images now
                        // System.Diagnostics.Debug.WriteLine($"load more images now");
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
                    double imgwidth = 500.0;
                    double imgheight = 200.0;
                    var apiHelper = new WebApiHelper();
                    var catImages = await apiHelper.GetCats(10, Category, Shared.Api.ImageSize.Med);
                    foreach (var cImage in catImages) {
                        MainLayout.Children.Add(GetImageFrom(cImage, imgwidth, imgheight));
                    }
                }
                finally{
                    System.Diagnostics.Debug.WriteLine("1:***");
                    await Task.Delay(100);
                    IsLoadingMoreImages = false;
                    System.Diagnostics.Debug.WriteLine("2:***");
                }
            }
            else{
                System.Diagnostics.Debug.Write(".");
            }
        }

        private Image GetImageFrom(CatImage cImage, double imageWidth, double imageHeight){
            return new Image {
                Source = cImage.Url,
                WidthRequest = imageWidth,
                HeightRequest = imageHeight,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
        }
    }
}
