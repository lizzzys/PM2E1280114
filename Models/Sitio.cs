using SQLite;

namespace PM2E1280114.Models;

public class Sitio
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Descripcion { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string RutaImagen { get; set; } = string.Empty;
}