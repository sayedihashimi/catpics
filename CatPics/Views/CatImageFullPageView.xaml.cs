using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatPics.Shared.Api;
using System.Linq;
using Xamarin.Forms;

namespace CatPics.Views {
    // from: https://stackoverflow.com/a/45454234/105999
    public partial class CatImageFullPageView : ContentPage {
        public CatImageFullPageView(ICatImageListView listView, int indexOfImage) {
            InitializeComponent();
            ListView = listView;

            currentIndex = indexOfImage;
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
        private Image MainImage { get; set; }
        private async void BuildUi(){
            await InitCatImages();

            MainImage = new Image();
            MainImage.Source = CatImages[currentIndex].Url;
            MainImage.Aspect = Aspect.AspectFit;

            MainImage.HeightRequest = 600;
            MainImage.WidthRequest = 600;

            var swipeGesture = new SwipeGestureRecognizer(MainImage, new SwipeCallBack {
                OnRightSwipeFunc = (View) => {
                    MoveCurrentImageToPrevious();
                },
                OnLeftSwipeFunc = (view) => {
                    MoveCurrentImageToNext();
                }
            });

            var layout = new RelativeLayout();
            layout.BackgroundColor = Color.Gray;
            layout.Children.Add(MainImage, 
                                widthConstraint: Constraint.RelativeToParent((parent)=>{
                                    return parent.Width;
                                }),
                                heightConstraint: Constraint.RelativeToParent( (parent)=>{
                                    return parent.Height;
                                }) );


            var closeButton = new Button();
            closeButton.Image = "close.png";
            layout.Children.Add(closeButton,
                                xConstraint: Constraint.RelativeToParent((parent) => {
                                    return 20;
                                }),
                                yConstraint: Constraint.RelativeToParent((parent) => {
                                    return 20;
                                }));
            closeButton.Clicked += (sender, e) => {
                Navigation.PopModalAsync();
            };

            Content = layout;
        }
        private void UpdateCurrentIndex(int newIndex){
            currentIndex = newIndex;
            System.Diagnostics.Debug.WriteLine($"Updating currentIndex to: {newIndex}");
            this.MainImage.Source = CatImages.ElementAt(currentIndex).Url;
        }
        private void MoveCurrentImageToNext(){
            int nextIndex = currentIndex + 1;

            if(nextIndex >= CatImages.Count){
                nextIndex = 0;
            }
            if(nextIndex < 0){
                nextIndex = CatImages.Count -1;
            }

            UpdateCurrentIndex(nextIndex);
        }

        private void MoveCurrentImageToPrevious() {
            int nextIndex = currentIndex - 1;

            if (nextIndex >= CatImages.Count) {
                nextIndex = 0;
            }
            if (nextIndex < 0) {
                nextIndex = CatImages.Count - 1;
            }

            UpdateCurrentIndex(nextIndex);
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height);
            Image img = Content as Image;

            if(img != null){
                System.Diagnostics.Debug.WriteLine($"height= {height} width={width}");
                img.HeightRequest = height;
                img.WidthRequest = width;
            }

            RelativeLayout layout = Content as RelativeLayout;
            if(layout != null){
                layout.WidthRequest = width;
                layout.HeightRequest = height;
            }
        }
    }
}
