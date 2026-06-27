using PM2E1280114.Data;
using PM2E1280114.Models;

namespace PM2E1280114.Pages;

public partial class ListaSitios : ContentPage
{
    private readonly BaseDatos _db;

    public ListaSitios(BaseDatos db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listaSitios.ItemsSource = await _db.ObtenerSitios();
    }

    private async void OnEliminar(object sender, EventArgs e)
    {
        var sitio = (Sitio)((Button)sender).CommandParameter;

        bool confirmar = await DisplayAlert("Eliminar",
            $"Desea eliminar '{sitio.Descripcion}'?", "Si", "No");

        if (!confirmar) return;

        await _db.EliminarSitio(sitio);
        listaSitios.ItemsSource = await _db.ObtenerSitios();
    }

    private async void OnDobleClick(object sender, TappedEventArgs e)
    {
       
        var sitio = (Sitio)((Border)sender).BindingContext;

        await Shell.Current.GoToAsync(
            $"MapaSitio?lat={sitio.Latitud}&lon={sitio.Longitud}" +
            $"&desc={Uri.EscapeDataString(sitio.Descripcion)}" +
            $"&img={Uri.EscapeDataString(sitio.RutaImagen)}");
    }
}