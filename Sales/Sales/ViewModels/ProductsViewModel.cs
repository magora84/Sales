﻿using GalaSoft.MvvmLight.Command;
using Sales.Common.Models;
using Sales.Helpers;
using Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        #region atributtos
        private ApiService apiService;
        private bool isRefreshing; 
        private ObservableCollection<ProductItemViewModel> products;
        private string filter;

        #endregion


        #region propiedades
        public string Filter {
            get { return this.filter; }
            set {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Product> MyProducts { get; set; }

        public ObservableCollection<ProductItemViewModel> Products { 
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region constructores
        public ProductsViewModel() {
            //parte de singleton colocar la instancia en el constructor
            instance = this;
            this.apiService = new ApiService();
            this.LoadProducts();
        }
        #endregion

        #region singleton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance() {
            if (instance== null) {
                return new ProductsViewModel();
            }
            return instance;
        }
        #endregion

        #region methods
        private async void LoadProducts() {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess) {
                this.IsRefreshing = false;
                await App.Navigator.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Product>(url, prefix, controller,Settings.TokenType,Settings.AccessToken);

            if (!response.IsSuccess) {
                this.IsRefreshing = false;
                await App.Navigator.DisplayAlert(Languages.Error, response.Message, "Accept");
                return;
            }
           this.MyProducts = (List<Product>)response.Result;
            this.RefreshList();
            
            this.IsRefreshing = false;
        }

        public void RefreshList() {
            if (string.IsNullOrEmpty(this.Filter)) {
                //expresion lamda para asignar elemetnos de  un tipo de clase a otra 
                //es mas eficiente que el foreach para asignar
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                });


                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description)
                    );
            }
            else {
                var myListProductItemViewModel = this.MyProducts.Select(p => new ProductItemViewModel {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();


                this.Products = new ObservableCollection<ProductItemViewModel>(
                    myListProductItemViewModel.OrderBy(p => p.Description)
                    );
            }
      
          
        }
        #endregion
        public ICommand RefreshCommand {
            get {
                return new RelayCommand(LoadProducts);
            }
        }
        public ICommand SearchCommand { get {
                return new RelayCommand(RefreshList);
            } }
    }
    

}
