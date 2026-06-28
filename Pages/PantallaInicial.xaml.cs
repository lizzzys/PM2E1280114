using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
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

    // Método para tomar foto
    private async void OnTomarFoto(object sender, EventArgs e)
    {
        try
        {
            // Verificar permisos de cámara
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Error", "Se necesita permiso para usar la cámara", "OK");
                    return;
                }
            }

            var resultado = await MediaPicker.Default.CapturePhotoAsync();
            if (resultado == null) return;

            string ruta = Path.Combine(FileSystem.AppDataDirectory, resultado.FileName);
            using var origen = await resultado.OpenReadAsync();
            using var destino = File.OpenWrite(ruta);
            await origen.CopyToAsync(destino);

            _rutaImagen = ruta;
            imgSitio.Source = ImageSource.FromFile(ruta);

            await DisplayAlert("Éxito", "Foto tomada correctamente", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
        }
    }

    // Método para obtener ubicación
    private async void OnObtenerUbicacion(object sender, EventArgs e)
    {
        try
        {
            // Verificar permisos de ubicación
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Error", "Se necesita permiso para acceder a la ubicación", "OK");
                    return;
                }
            }

            // Verificar si el GPS está activo
            var lastLocation = await Geolocation.Default.GetLastKnownLocationAsync();
            if (lastLocation == null)
            {
                // Intentar obtener ubicación activa
                var solicitud = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                var ubicacion = await Geolocation.Default.GetLocationAsync(solicitud);

                if (ubicacion != null)
                {
                    entLatitud.Text = ubicacion.Latitude.ToString("F6");
                    entLongitud.Text = ubicacion.Longitude.ToString("F6");
                    await DisplayAlert("Éxito", "Ubicación obtenida correctamente", "OK");
                }
                else
                {
                    await DisplayAlert("GPS inactivo",
                        "No se pudo obtener la ubicación. Por favor active el GPS.", "Aceptar");
                }
            }
            else
            {
                entLatitud.Text = lastLocation.Latitude.ToString("F6");
                entLongitud.Text = lastLocation.Longitude.ToString("F6");
                await DisplayAlert("Éxito", "Ubicación obtenida correctamente", "OK");
            }
        }
        catch (FeatureNotEnabledException)
        {
            await DisplayAlert("GPS inactivo",
                "Por favor active el GPS para registrar el sitio.", "Aceptar");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "No se tienen permisos de ubicación", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al obtener ubicación: {ex.Message}", "Aceptar");
        }
    }

    // Método para guardar el sitio
    private async void OnGuardar(object sender, EventArgs e)
    {
        try
        {
            // Validaciones
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

            // Crear objeto Sitio
            var sitio = new Sitio
            {
                Descripcion = entDescripcion.Text.Trim(),
                Latitud = double.Parse(entLatitud.Text),
                Longitud = double.Parse(entLongitud.Text),
                RutaImagen = _rutaImagen
            };

            // Guardar en base de datos
            await _db.GuardarSitio(sitio);
            await DisplayAlert("Listo", "Sitio guardado correctamente.", "Aceptar");

            // Limpiar campos
            entDescripcion.Text = string.Empty;
            entLatitud.Text = string.Empty;
            entLongitud.Text = string.Empty;
            _rutaImagen = string.Empty;
            imgSitio.Source = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "Aceptar");
        }
    }

    // Método para ver lista de sitios
    private async void OnVerLista(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListaSitios(_db));
    }
}