using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using Sales.Views;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels {
    public class MainViewModel
    {
        #region propiedades
        public LoginViewModel Login { get; set; }
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }

        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        #endregion
        #region constructor
        public MainViewModel() {

            instance = this;
            this.loadMenu();

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

            await App.Navigator.PushAsync(new AddProductPage());
        }
        #endregion
        #region Metodos
        private void loadMenu() {
            this.Menu = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel {
                Icon = "ic_info",
                PageName = "AboutPage",
                Title = Languages.About,
            });

            this.Menu.Add(new MenuItemViewModel {
                Icon = "ic_phonelink_setup",
                PageName = "SetupPage",
                Title = Languages.Setup,
            });

            this.Menu.Add(new MenuItemViewModel {
                Icon = "ic_exit_to_app",
                PageName = "LoginPage",
                Title = Languages.Exit,
            });

        }
        #endregion
    }
}
