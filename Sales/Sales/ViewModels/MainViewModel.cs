using GalaSoft.MvvmLight.Command;
using Sales.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
   public class MainViewModel
    {
        #region propiedades
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        #endregion
        #region constructor
        public MainViewModel() {

            instance = this;
        }
        #endregion

        #region singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance() {
            if (instance == null) {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion
        #region comandos
        public ICommand AddProductCommand {
            get {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct() {

            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        } 
        #endregion
    }
}
