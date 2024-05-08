using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiCrud.Data;
using System.Collections.ObjectModel;
using MauiCrud.Models;

namespace MauiCrud.ViewModels
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
               // throw;
            }
            finally
            {
                IsBusy = false;
                BusyText = "Processing...";
            }
        }
    }
}
