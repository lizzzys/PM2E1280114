

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

        lblDescripcion.Text = Uri.UnescapeDataString(Desc);
        lblLatitud.Text = Lat;
        lblLongitud.Text = Lon;

        string rutaImg = Uri.UnescapeDataString(Img);
        if (File.Exists(rutaImg))
            imgMapa.Source = ImageSource.FromFile(rutaImg);
    }

    private async void OnVerMapa(object sender, EventArgs e)
    {
        try
        {
            double lat = double.Parse(Lat);
            double lon = double.Parse(Lon);
            string desc = Uri.UnescapeDataString(Desc);

            var location = new Location(lat, lon);
            var options = new MapLaunchOptions { Name = desc };
            await Map.Default.OpenAsync(location, options);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
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