using HangoverApp.Controls;
using HangoverApp.Style;
using HangoverApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HangoverApp.Helpers;
using HangoverApp.Models;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace HangoverApp.Views
{
    public class MainListPage : ContentPage
    {
        View _tabs;

        RelativeLayout relativeLayout;
        private TabbedPageViewModel _viewModel;
        CarouselLayout.IndicatorStyleEnum _indicatorStyle;
        LoginViewModel _loginViewModel;

        //public MainListPage()
        //{

        //    _indicatorStyle = CarouselLayout.IndicatorStyleEnum.Tabs;

        //    // viewModel = new SwitcherPageViewModel();
        //    _viewModel = new TabbedPageViewModel();
        //    BindingContext = _viewModel;
        //    // BindingContext = viewModel;
        //    //BackgroundColor = Color.Black;
        //    Title = "CodeProject";
        //    NavigationPage.SetHasNavigationBar(this, true);
        //    NavigationPage.SetHasBackButton(this, false);
        //    relativeLayout = new RelativeLayout
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand
        //    };

        //    var pagesCarousel = CreatePagesCarousel();
        //    //var dots = CreatePagerIndicatorContainer();
        //    _tabs = CreateTabs();

        //    switch (pagesCarousel.IndicatorStyle)
        //    {

        //        case CarouselLayout.IndicatorStyleEnum.Tabs:
        //            var tabsHeight = 50;
        //            relativeLayout.Children.Add(_tabs,
        //                Constraint.Constant(0),
        //                Constraint.RelativeToParent((parent) => { return parent.Height - tabsHeight; }),
        //                Constraint.RelativeToParent(parent => parent.Width),
        //                Constraint.Constant(tabsHeight)
        //            );

        //            relativeLayout.Children.Add(pagesCarousel,
        //                Constraint.RelativeToParent((parent) => { return parent.X; }),
        //                Constraint.RelativeToParent((parent) => { return parent.Y; }),
        //                Constraint.RelativeToParent((parent) => { return parent.Width; }),
        //                Constraint.RelativeToView(_tabs, (parent, sibling) => { return parent.Height - (sibling.Height); })
        //            );
        //            break;
        //    }

        //    Content = relativeLayout;
        //}

        public MainListPage()
        {
            BindingContext = _loginViewModel = new LoginViewModel(this);
            var activityIndicator = new ActivityIndicator
            {
                Color = Color.FromHex("#8BC34A"),
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            BackgroundImage = "LoginScreen.png";
            var layout = new StackLayout { Padding = 20 };
            layout.Children.Add(activityIndicator);
            var button = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Scale = 2,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Feed Me!",
                BackgroundColor = Color.FromHex("#8BC34A")

            };

            var textBox = new Entry()
            {
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Margin = 1,
                BackgroundColor = Color.Black,
                Placeholder = "Enter your postcode...",
                PlaceholderColor = Color.Gray
            };

            var backgroundImage = new Image()
            {
                Aspect = Aspect.Fill,
                Source = FileImageSource.FromFile("LoginScreen.png")
            };


            layout.Children.Add(textBox);
            layout.Children.Add(button);

            var relativelayout = new RelativeLayout();


            relativelayout.Children.Add(backgroundImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            relativelayout.Children.Add(layout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            button.Clicked += (sender, e) =>
            {
                WebOperations operation = new WebOperations();
                if (String.IsNullOrEmpty(textBox.Text))
                {
                    DisplayAlert("Error", "Please enter your postcode", "Re-try");
                }
                else
                {
                    var source = operation.SearchByPostcode(CrossSecureStorage.Current.GetValue("myCookie"), textBox.Text);
                    RestaurantsDeserializer deserialize = new RestaurantsDeserializer();
                    List<Restaurant> restaurants = deserialize.Restaurants(source);

                    Page page = new Page();
                    page.Navigation.PushModalAsync(new StoresPage(restaurants));
                    if (Device.OS == TargetPlatform.Android)
                        Application.Current.MainPage = new StoresPage(restaurants);
                    //show the view with the restaurants
                }
            };

            Content = new ScrollView { Content = relativelayout };

        }

        //CarouselLayout CreatePagesCarousel()
        //{
        //    var carousel = new CarouselLayout
        //    {
        //        HorizontalOptions = LayoutOptions.FillAndExpand,
        //        VerticalOptions = LayoutOptions.FillAndExpand,
        //        IndicatorStyle = _indicatorStyle,
        //        ItemTemplate = new DataTemplate(typeof(DynamicTemplateLayout))
        //    };
        //    carousel.SetBinding(CarouselLayout.ItemsSourceProperty, "Pages");
        //    carousel.SetBinding(CarouselLayout.SelectedItemProperty, "CurrentPage", BindingMode.TwoWay);

        //    return carousel;
        //}


        //View CreatePagerIndicatorContainer()
        //{
        //    return new StackLayout
        //    {
        //        Children = { CreatePagerIndicators() }
        //    };
        //}

        //View CreatePagerIndicators()
        //{
        //    var pagerIndicator = new PagerIndicatorDots() { DotSize = 5, DotColor = Color.Black };
        //    pagerIndicator.SetBinding(PagerIndicatorDots.ItemsSourceProperty, "Pages");
        //    pagerIndicator.SetBinding(PagerIndicatorDots.SelectedItemProperty, "CurrentPage");
        //    return pagerIndicator;
        //}

        //View CreateTabsContainer()
        //{
        //    return new StackLayout
        //    {
        //        Children = { CreateTabs() }
        //    };
        //}

        //View CreateTabs()
        //{
        //    var pagerIndicator = new PagerIndicatorTabs() { HorizontalOptions = LayoutOptions.CenterAndExpand };
        //    pagerIndicator.RowDefinitions.Add(new RowDefinition() { Height = 50 });
        //    pagerIndicator.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        //    //pagerIndicator.SetBinding(Grid.ColumnDefinitionsProperty, "Pages", BindingMode.Default, new SpacingConverter());
        //    pagerIndicator.SetBinding(PagerIndicatorTabs.ItemsSourceProperty, "Pages");
        //    pagerIndicator.SetBinding(PagerIndicatorTabs.SelectedItemProperty, "CurrentPage");

        //    return pagerIndicator;
        //}
    }

    //public class SpacingConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var items = value as IEnumerable<ICarouselViewModel>;

    //        var collection = new ColumnDefinitionCollection();
    //        foreach (var item in items)
    //        {
    //            collection.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
    //        }
    //        return collection;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}
}
