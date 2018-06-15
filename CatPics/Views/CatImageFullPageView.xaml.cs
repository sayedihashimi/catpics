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

            img.HeightRequest = 600;
            img.WidthRequest = 600;


            var layout = new RelativeLayout();
            layout.BackgroundColor = Color.Gray;
            layout.Children.Add(img, 
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

                                //yConstraint: Constraint.RelativeToView()

            Content = layout;
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
