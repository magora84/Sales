

namespace Sales.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Services;
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductItemViewModel: Product
    {
        #region Atributes
        private ApiService apiService;
        #endregion

        #region Constructor
        public ProductItemViewModel() {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commandos
        public ICommand DeleteProductCommand { get {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct() {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm, 
                Languages.DeleteConfirmation, 
                Languages.Yes, 
                Languages.No);
            if (!answer) {
                return;
            }
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {           
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
                var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller,this.ProductId);

            if (!response.IsSuccess) {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, "Accept");
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();
            var deletedProduct = productsViewModel.Products.Where(p => p.ProductId == this.ProductId).FirstOrDefault();
            if (deletedProduct !=  null) {
                productsViewModel.Products.Remove(deletedProduct);
            }
        }
        #endregion
    }
}
