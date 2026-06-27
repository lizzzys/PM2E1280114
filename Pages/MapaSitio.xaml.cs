using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace PM2E1280114.Pages;

[QueryProperty("Lat", "lat")]
[QueryProperty("Lon", "lon")]
[QueryProperty("Desc", "desc")]
[QueryProperty("Img", "img")]
public partial class MapaSitio : ContentPage
{
    public string Lat { get; set; } = "0";
    public string Lon { get; set; } = "0";
    public string Desc { get; set; } = "";
    public string Img { get; set; } = "";

    public MapaSitio()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarMapa();
    }

    private void CargarMapa()
    {
        try
        {
            double latitud = double.Parse(Lat);
            double longitud = double.Parse(Lon);

            lblDescripcion.Text = Uri.UnescapeDataString(Desc);

            var coordenada = new Location(latitud, longitud);

            var pin = new Pin
            {
                Label = Uri.UnescapeDataString(Desc),
                Location = coordenada,
                Type = PinType.Place
            };

            miMapa.Pins.Clear();
            miMapa.Pins.Add(pin);

            miMapa.MoveToRegion(
                MapSpan.FromCenterAndRadius(coordenada, Distance.FromKilometers(1)));
        }
        catch
        {
            DisplayAlert("Error", "No se pudo cargar la ubicacion.", "Aceptar");
        }
    }

    private async void OnCompartir(object sender, EventArgs e)
    {
        string rutaImagen = Uri.UnescapeDataString(Img);

        if (!File.Exists(rutaImagen))
        {
            await DisplayAlert("Sin imagen",
                "No hay imagen disponible para compartir.", "Aceptar");
            return;
        }

        await Share.Default.RequestAsync(new ShareFileRequest
        {
            Title = "Compartir sitio",
            File = new ShareFile(rutaImagen)
        });
    }
}