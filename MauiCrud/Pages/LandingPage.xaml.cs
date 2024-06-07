using MauiCRUD.Data;
using MauiCRUD.ViewModels;

namespace MauiCRUD
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private async void OnGoToProductsClicked(object sender, EventArgs e)
        {
            var dbContext = new DatabaseContext();
            var productsViewModel = new ProductsViewModel(dbContext);
            await Navigation.PushAsync(new MainPage(productsViewModel));
        }
    }
}
