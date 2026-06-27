using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Media;
using PM2E1280114.Data;
using PM2E1280114.Models;

namespace PM2E1280114.Pages;

public partial class PantallaInicial : ContentPage
{
    private readonly BaseDatos _db;
    private string _rutaImagen = string.Empty;

    public PantallaInicial(BaseDatos db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnTomarFoto(object sender, EventArgs e)
    {
        var resultado = await MediaPicker.Default.CapturePhotoAsync();
        if (resultado == null) return;

        string ruta = Path.Combine(FileSystem.AppDataDirectory, resultado.FileName);
        using var origen = await resultado.OpenReadAsync();
        using var destino = File.OpenWrite(ruta);
        await origen.CopyToAsync(destino);

        _rutaImagen = ruta;
        imgSitio.Source = ImageSource.FromFile(ruta);
    }

    private async void OnObtenerUbicacion(object sender, EventArgs e)
    {
        try
        {
            var solicitud = new GeolocationRequest(GeolocationAccuracy.Medium);
            var ubicacion = await Geolocation.Default.GetLocationAsync(solicitud);

            if (ubicacion != null)
            {
                entLatitud.Text = ubicacion.Latitude.ToString();
                entLongitud.Text = ubicacion.Longitude.ToString();
            }
        }
        catch (FeatureNotEnabledException)
        {
            await DisplayAlert("GPS inactivo",
                "Por favor active el GPS para registrar el sitio.", "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }

    private async void OnGuardar(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(entDescripcion.Text))
        {
            await DisplayAlert("Campo vacio", "Ingrese una descripcion.", "Aceptar");
            return;
        }
        if (string.IsNullOrWhiteSpace(entLatitud.Text) ||
            string.IsNullOrWhiteSpace(entLongitud.Text))
        {
            await DisplayAlert("Sin ubicacion", "Primero obtenga la ubicacion GPS.", "Aceptar");
            return;
        }
        if (string.IsNullOrEmpty(_rutaImagen))
        {
            await DisplayAlert("Sin imagen", "Primero tome una foto del sitio.", "Aceptar");
            return;
        }

        var sitio = new Sitio
        {
            Descripcion = entDescripcion.Text,
            Latitud = double.Parse(entLatitud.Text),
            Longitud = double.Parse(entLongitud.Text),
            RutaImagen = _rutaImagen
        };

        await _db.GuardarSitio(sitio);
        await DisplayAlert("Listo", "Sitio guardado correctamente.", "Aceptar");

        entDescripcion.Text = string.Empty;
        entLatitud.Text = string.Empty;
        entLongitud.Text = string.Empty;
        _rutaImagen = string.Empty;
        imgSitio.Source = null;
    }
}