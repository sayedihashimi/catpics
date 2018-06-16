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

            _currentIndex = indexOfImage;
            BuildUi();
        }

        protected ICatImageListView ListView { get; set; }

        private List<CatImage> _catImages = new List<CatImage>();

        private int _currentIndex = 0;
        private Image _mainImage { get; set; }

        private async Task InitCatImages(){
            // this is not needed and can be removed, but it may be useful to keep
            await ListView.FetchAndAddImagesToView();
            _catImages = ListView.GetCatImages();
        }

        private async void BuildUi(){
            await InitCatImages();

            _mainImage = new Image();
            _mainImage.Source = _catImages[_currentIndex].Url;
            _mainImage.Aspect = Aspect.AspectFit;

            _mainImage.HeightRequest = 600;
            _mainImage.WidthRequest = 600;

            var swipeGesture = new SwipeGestureRecognizer(_mainImage, new SwipeCallBack {
                OnRightSwipeFunc = (View) => {
                    MoveCurrentImageToPrevious();
                },
                OnTopSwipeFunc = (view) => {
                    MoveCurrentImageToPrevious();
                },

                OnLeftSwipeFunc = (view) => {
                    MoveCurrentImageToNext();
                },
                OnBottomSwipeFunc = (view) => {
                    MoveCurrentImageToNext();
                }
            });

            var layout = new RelativeLayout();
            layout.BackgroundColor = Color.Gray;
            layout.Children.Add(_mainImage, 
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
            _currentIndex = newIndex;
            System.Diagnostics.Debug.WriteLine($"Updating currentIndex to: {newIndex}");
            this._mainImage.Source = _catImages.ElementAt(_currentIndex).Url;

            if(Math.Abs(_catImages.Count - _currentIndex) < 3){
                System.Diagnostics.Debug.WriteLine($"Fetching new cat images");
                ListView.FetchAndAddImagesToView();
            }
        }
        private void MoveCurrentImageToNext(){
            int nextIndex = _currentIndex + 1;

            if(nextIndex >= _catImages.Count){
                nextIndex = 0;
            }
            if(nextIndex < 0){
                nextIndex = _catImages.Count -1;
            }

            UpdateCurrentIndex(nextIndex);
        }

        private void MoveCurrentImageToPrevious() {
            int nextIndex = _currentIndex - 1;

            if (nextIndex >= _catImages.Count) {
                nextIndex = 0;
            }
            if (nextIndex < 0) {
                nextIndex = _catImages.Count - 1;
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
