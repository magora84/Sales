﻿using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Services;
using Sales.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels {
    public class LoginViewModel:BaseViewModel {
        #region Atributos
        private ApiService apiService;
        private bool isRunning;
        private bool isEnable;
        #endregion
        #region Propiedades
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
        public bool IsRunning {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnable {
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        #endregion
        #region Constructor
        public LoginViewModel() {
            this.apiService = new ApiService();
            this.IsEnable = true;
            this.IsRemembered = true;
        }

        #endregion
        #region Commands
        public ICommand RegisterCommand { get { return new RelayCommand(Register); } }
        private async void Register() {
            MainViewModel.GetInstance().Register = new RegisterViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        public ICommand LoginCommand { get { return new RelayCommand(Login); }   }

        private async void Login() {
            if (string.IsNullOrEmpty(this.Email)) {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.EmailValidation, 
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Password)) {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    Languages.PasswordValidation, 
                    Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.isEnable = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRunning = false;
                this.isEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var token = await this.apiService.GetToken(url, this.Email, this.Password);
            if (token== null || string.IsNullOrEmpty(token.AccessToken)) {
                this.IsRunning = false;
                this.isEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.SomethingWrong, Languages.Accept);
                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
            Settings.IsRemembered = this.IsRemembered;

            MainViewModel.GetInstance().Products = new ProductsViewModel();
            Application.Current.MainPage = new MasterPage();
            this.IsRunning = false;
            this.isEnable = true;
        }
        #endregion

    }
}
