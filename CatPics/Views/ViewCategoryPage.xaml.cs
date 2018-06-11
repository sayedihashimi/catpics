using System;
using System.Collections.Generic;
using CatPics.Shared;
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

        protected string Category { get; set; }

        public async void BuildUi() {
            var mainLayout = new StackLayout {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            Content = new ScrollView {
                Content = mainLayout,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            double imgwidth = 500.0;
            double imgheight = 200.0;

            var apiHelper = new WebApiHelper();
            var catImages = await apiHelper.GetCats(10, Category, Shared.Api.ImageSize.Med);

            foreach(var cImage in catImages){
                var img = new Image {
                    Source = cImage.Url,
                    WidthRequest = imgwidth,
                    HeightRequest = imgheight,
                    Aspect = Aspect.AspectFill,
                    HorizontalOptions = LayoutOptions.CenterAndExpand
                };
                mainLayout.Children.Add(img);
            }
        }
    }
}
