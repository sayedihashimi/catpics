﻿using System;
using System.Collections.Generic;
using System.Linq;
using CatPics.Shared;
using CatPics.Shared.Api;
using CatPics.ViewModels;
using Xamarin.Forms;

namespace CatPics.Views {
    public partial class CategoryPage
        : ContentPage {
        public CategoryPage() {
            InitializeComponent();
            BindingContext = new CategoryPageViewModel();

            BuildUi();

            //try{
            //    var foo = new WebApiHelper();
            //    var result = foo.GetCategories();
            //}
            //catch(Exception ex){
            //    System.Diagnostics.Debug.WriteLine(ex.ToString());
            //}


        }


        protected async void BuildUi(){
            var apiHelper = new WebApiHelper();
            var categories = await apiHelper.GetCategories();
            var images = new List<CatImage>();

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

            foreach(var cat in categories){
                var imgresult = await apiHelper.GetCat(cat.Name);
                if(imgresult != null){
                    images.Add(imgresult);

                    var img = new Image {
                        Source = (await apiHelper.GetCat(cat.Name)).Url,
                        WidthRequest = imgwidth,
                        HeightRequest = imgheight,
                        Aspect=Aspect.AspectFill,
                        HorizontalOptions=LayoutOptions.CenterAndExpand
                    };
                    var label = new Label {
                        Text = cat.Name,
                        FontAttributes=FontAttributes.Bold,
                        TextColor = Color.WhiteSmoke
                                         ,
                        FontSize = 30.0,
                        HeightRequest=40,
                        BackgroundColor = Color.Gray
                    };

                    var rellayout = new RelativeLayout();
                    rellayout.Children.Add(img, xConstraint: null);

                    rellayout.Children.Add(label, 
                                           xConstraint:Constraint.RelativeToParent((parent)=>{
                                                return 0; 
                                            }), 
                                           yConstraint:Constraint.RelativeToParent((Parent)=>{
                                                return 60;
                                            }));

                    mainLayout.Children.Add(rellayout);
                    var tapped = new TapGestureRecognizer();
                    tapped.Tapped += (s, e) => {
                        try{
                            // if the sender is an image, get the sibling label for the category value
                            // if the sender is a label, get the text for the category value

                            Label catLabel = s as Label;
                            if(catLabel == null ){
                                // get it from the parent (Rel layout)
                                Image catImg = s as Image;
                                RelativeLayout relativeLayout = null;
                                if(img != null){
                                    relativeLayout = catImg.Parent as RelativeLayout;
                                }

                                if(relativeLayout != null){
                                    catLabel = relativeLayout.Children.ElementAt(1) as Label;
                                }
                            }

                            if(catLabel != null){
                                string category = catLabel.Text;

                            }

                        }
                        catch(Exception ex){
                            
                        }
                    };
                    img.GestureRecognizers.Add(tapped);
                    label.GestureRecognizers.Add(tapped);

                }
            }
        }


        protected override void OnAppearing() {
            base.OnAppearing();
        }
    }
}