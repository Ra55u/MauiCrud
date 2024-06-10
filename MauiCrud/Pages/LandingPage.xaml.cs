using MauiCRUD.ViewModels;

namespace Pages
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
            BindingContext = new ProductsViewModel();
        }
    }
}
