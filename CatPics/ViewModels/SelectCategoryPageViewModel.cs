using System;
using System.Windows.Input;
using CatPics.Views;
using Xamarin.Forms;

namespace CatPics.ViewModels {
    public class SelectCategoryPageViewModel : BaseViewModel {

        public SelectCategoryPageViewModel(INavigation navigation):base(){
            ShowCategory = new Command<string>(ExecuteShowCategory);
            Navigation = navigation;
        }

        private INavigation Navigation { get; set; }
        public ICommand ShowCategory { get; set; }
        private async void ExecuteShowCategory(string category){
            await Navigation.PushAsync(new NavigationPage(new ViewCategoryPage(category)));
        }
    }
}
