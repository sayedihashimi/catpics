using System;
using Xamarin.Forms;

namespace CatPics.ViewModels {
    public class ViewCategoryPageViewModel :BaseViewModel {

        public ViewCategoryPageViewModel(string category):base(){
            Category = category;

        }

        public string Category { get; set; }



        /*
var mainLayout = new StackLayout {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand//,
                // Wrap=FlexWrap.Wrap,
                //Direction=FlexDirection.Row
            };
            Content = new ScrollView {
                Content = mainLayout,
                VerticalOptions=LayoutOptions.CenterAndExpand,
                HorizontalOptions=LayoutOptions.CenterAndExpand
            };

            // mainFlexLayout.HeightRequest = Content.Height;

            double imgwidth = 500.0;
            double imgheight = 200.0;         
         */

    }
}
