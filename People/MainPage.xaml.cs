using People.Models;
using System.Collections.Generic;
using People.ViewModels;

namespace People;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // Configurar el ViewModel como BindingContext
        BindingContext = new MainPageViewModel(App.PersonRepo.DbPath); 
    }

    




}
