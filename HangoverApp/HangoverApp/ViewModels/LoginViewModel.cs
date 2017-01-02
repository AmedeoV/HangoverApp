using HangoverApp.Models;
using HangoverApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HangoverApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        readonly ICPFeeds cpFeeds;


        public LoginViewModel(Page page)
            : base(page)
        {
            this.cpFeeds = DependencyService.Get<ICPFeeds>();
        }

        public const string LoginCommandPropertyName = "LoginCommand";
        Command loginCommand;
        public Command LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new Command(async () => await ExecuteLoginCommand()));
            }
        }

        private async Task ExecuteLoginCommand()
        {

            bool isLoginSuccess = false;
            if (IsBusy)
                return;

            IsBusy = true;
            loginCommand.ChangeCanExecute();

            try
            {
                isLoginSuccess = await cpFeeds.GetAccessToken(this.UserName, this.Password);
            }
            catch (Exception ex)
            {
                isLoginSuccess = false;
            }

            finally
            {
                IsBusy = false;
                loginCommand.ChangeCanExecute();
            }

            if (isLoginSuccess)
            {
                await page.Navigation.PushModalAsync(new RootPage());
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new RootPage();
            }
            else
            {
                await page.DisplayAlert("Login Error", "Login Error! please try Again", "Ok");
            }
        }


        string username = string.Empty;
        public const string UsernamePropertyName = "UserName";
        public string UserName
        {
            get { return username; }
            set { SetProperty(ref username, value, UsernamePropertyName); }
        }

        string password = string.Empty;
        public const string PasswordPropertyName = "Password";
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, PasswordPropertyName); }
        }
    }
}
