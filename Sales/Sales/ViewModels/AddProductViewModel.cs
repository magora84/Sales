using GalaSoft.MvvmLight.Command;
using Sales.Helpers;
using System.Windows.Input;
using Xamarin.Forms;
using Sales.Services;
using Sales.Common.Models;

namespace Sales.ViewModels {
    public class AddProductViewModel:BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isRunning;
        private bool isEnable;
        #endregion
        #region Properties 

        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public bool IsRunning {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnable {
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        #endregion

        #region Constructors
        public AddProductViewModel() {
            this.apiService = new ApiService();
            this.IsEnable = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand {
            get {
                return new RelayCommand(Save);
            }
        }

        private async void Save() {
            if (string.IsNullOrEmpty(this.Description)) {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError
                    , Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Price)) {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError
                    , Languages.Accept);
                return;
            }
            var price = decimal.Parse(this.Price);

            if (price < 0) {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError
                    , Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.IsEnable = false;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.isRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error, 
                    connection.Message, 
                    Languages.Accept);
                return;
            }
            var product = new Product
            {
                //la fecha sera capturada en el servidor 
                Description= this.Description,
                Price= price,
                Remarks=this.Remarks,
            };
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.Post(url, prefix, controller,product);

            if (!response.IsSuccess) {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }
            var newProduct = (Product)response.Result;
            // llamamos la clase cargada en memoria de ProductsViewModel
            var viewModel = ProductsViewModel.GetInstance();
            viewModel.Products.Add(newProduct);
            

            this.IsRunning = false;
            this.IsEnable = true;
            await Application.Current.MainPage.Navigation.PopAsync();

        }

        #endregion
    }
}
