using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CatPics.Shared;
using CatPics.Shared.Api;
using CatPics.ViewModels;
using Xamarin.Forms;

namespace CatPics.Views {
    public partial class ViewCategoryPage : ContentPage, ICatImageListView {
        public ViewCategoryPage(string category) {
            InitializeComponent();
            BindingContext = new ViewCategoryPageViewModel(category);
            ApiHelper = new WebApiHelper();
            Category = category;
            Title = category;
            BuildUi();
        }

        protected StackLayout MainLayout { get; set; }
        protected string Category { get; set; }
        protected WebApiHelper ApiHelper { get; set; }

        protected List<CatImage> CatImages { get; set; } = new List<CatImage>();
        protected ObservableCollection<CatImage> NewCatImages { get; set; } = new ObservableCollection<CatImage>();

        public double ImageWidth { get; set; } = 500.0;
        public double ImageHeight { get; set; } = 400.0;

        public int RefreshDelayMs { get; set; } = 100;
        public int NumImagesToFetchInitially { get; set; } = 10;
        public int NumImagesToFetchAfterInitial { get; set; } = 10;

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

            await FetchAndAddImagesToView(NumImagesToFetchInitially);

            scrollView.Scrolled += async (sender, e) => {
                ScrollView sv = sender as ScrollView;
                if(sv != null){
                    double maxScrollY = sv.ContentSize.Height - sv.Height;
                    if(maxScrollY - sv.ScrollY < 500){
                        // LoadMoreImagesAfterScroll will handle ignoring repeat calls
                        await LoadMoreImagesAfterScroll();
                    }
                }
            };

            //var scrollTapped = new TapGestureRecognizer();
            //scrollTapped.Tapped += (s, e) => {
            //    var foo = new CatImageFullPageView(this);
            //    // Navigation.PushModalAsync(new NavigationPage(foo));
            //    Navigation.PushModalAsync(foo);
            //};

            //scrollView.GestureRecognizers.Add(scrollTapped);
        }

        public List<CatImage> GetCatImages(){
            return CatImages;
        }

        public virtual int GetIndexOfImageWithUrl(string url){
            int result = int.MinValue;

            if(_catSourceIndexMap.ContainsKey(url)){
                result = _catSourceIndexMap[url];
            }
            else{
                System.Diagnostics.Debug.WriteLine("temp");
            }

            return result;
        }

        public virtual async Task FetchAndAddImagesToView(int numToFetch = 10, ImageSize size = ImageSize.Med){
            var catImages = await ApiHelper.GetCats(numToFetch, Category, size);
            AddCatImagesToView(catImages);
        }

        public virtual void AddCatImagesToView(IEnumerable<CatImage>images){
            if(images != null && images.Count() > 0){
                foreach(CatImage img in images){
                    AddCatImageToView(img);
                }
            }
        }

        public virtual void AddCatImageToView(CatImage image){
            if(image != null){
                CatImages.Add(image);

                if(!_catSourceIndexMap.ContainsKey(image.Url)){
                    _catSourceIndexMap.Add(image.Url, CatImages.IndexOf(image));
                    MainLayout.Children.Add(GetImageFrom(image, ImageWidth, ImageHeight));
                }
                else{
                    System.Diagnostics.Debug.WriteLine($"Skipping image because it's already been added, image url=[{image.Url}");
                }
            }
        }
        private Dictionary<string, int> _catSourceIndexMap = new Dictionary<string, int>();
        private bool IsLoadingMoreImagesAfterScroll { get; set; } = false;
        private async Task LoadMoreImagesAfterScroll(){
            if(!IsLoadingMoreImagesAfterScroll){

                IsLoadingMoreImagesAfterScroll = true;

                try{
                    System.Diagnostics.Debug.WriteLine("Loading more images now");
                    await FetchAndAddImagesToView(NumImagesToFetchAfterInitial);

                    await Task.Delay(RefreshDelayMs);
                }
                finally{
                    IsLoadingMoreImagesAfterScroll = false;
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

            var imgTapped = new TapGestureRecognizer();
            imgTapped.Tapped += (sender, e) => {
                Image sourceImg = sender as Image;
                if(sourceImg == null){
                    return;
                }

                UriImageSource tappedImage = sourceImg.Source as UriImageSource;

                if(tappedImage != null){
                    int index = this.GetIndexOfImageWithUrl(tappedImage.Uri.AbsoluteUri);
                    var fullpageview = new CatImageFullPageView(this, index);
                    Navigation.PushModalAsync(fullpageview);
                }
            };

            img.GestureRecognizers.Add(imgTapped);
            return img;
        }
    }
}
