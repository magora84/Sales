namespace Sales.ViewModels {
    using Common.Models;
    using Plugin.Media.Abstractions;
    using Sales.Services;
    using Xamarin.Forms;

    public class EditProductViewModel:BaseViewModel {
         #region Attributes
        private MediaFile file;
        private ApiService apiService;
        private ImageSource imageSource;
        private bool isRunning;
        private bool isEnable;
  
        private Product product;
        #endregion

        #region Propiedades
        public bool IsRunning {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnable {
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        public ImageSource ImageSource {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        public Product Product {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }
        #endregion
        #region constructores

        public EditProductViewModel(Product produc) {
            this.product = product;
            this.apiService = new ApiService();
            this.IsEnable = true;
            this.ImageSource = product.ImageFullPath;
        } 
        #endregion
    }
}
