using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiCRUD.Data;
using MauiCRUD.Models;
using System.Collections.ObjectModel;

namespace MauiCRUD.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly DatabaseContext _context;

        public ProductsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Product> _products = new();

        [ObservableProperty]
        private Product _operatingProduct = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _busyText;

        public async Task LoadProductsAsync()
        {
            await ExecuteAsync(async () =>
            {
                var products = await _context.GetAllAsync<Product>();
                if (products is not null && products.Any())
                {
                    Products ??= new ObservableCollection<Product>();

                    foreach (var product in products)
                    {
                        Products.Add(product);
                    }
                }
            }, "Fetching products...");
        }

        private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
        {
            IsBusy = true;
            BusyText = busyText ?? "Processing...";
            try
            {
                await operation?.Invoke();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }

        [ICommand]
        private void SetOperatingProduct(Product? product) => OperatingProduct = product ?? new();

        [ICommand]
        private async Task SaveProductAsync()
        {
            if (OperatingProduct is null)
                return;

            var (isValid, errorMessage) = OperatingProduct.Validate();
            if (!isValid)
            {
                await Shell.Current.DisplayAlert("Validation Error", errorMessage, "Ok");
                return;
            }

            var busyText = OperatingProduct.Id == 0 ? "Creating product..." : "Updating product...";
            await ExecuteAsync(async () =>
            {
                if (OperatingProduct.Id == 0)
                {
                    await _context.AddItemAsync<Product>(OperatingProduct);
                    Products.Add(OperatingProduct);
                }
                else
                {
                    await _context.UpdateItemAsync<Product>(OperatingProduct);
                }
            }, busyText);

            OperatingProduct = new Product();
        }


        private async Task DeleteProductAsync(Product product)
        {
            var confirm = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this product?", "Yes", "No");
            if (!confirm)
                return;

            await ExecuteAsync(async () =>
            {
                if (product != null)
                {
                    await _context.DeleteItemAsync<Product>(product);
                    Products.Remove(product);
                }
            }, "Deleting product...");
        }
    }
}