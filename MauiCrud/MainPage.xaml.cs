using MauiCRUD.ViewModels;

namespace MauiCRUD
{
    public partial class MainPage : ContentPage
    {
        public MainPage(ProductsViewModel productsViewModel)
        {
            InitializeComponent();
            BindingContext = productsViewModel;
        }
    }
}