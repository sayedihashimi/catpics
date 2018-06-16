using System;
using Xamarin.Forms;

namespace CatPics.ViewModels {
    public class ViewCategoryPageViewModel :BaseViewModel {

        public ViewCategoryPageViewModel(string category):base(){
            Category = category;

        }

        public string Category { get; set; }
    }
}
