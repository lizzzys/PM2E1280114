using SQLite;
using PM2E1280114.Models;

namespace PM2E1280114.Data;

public class BaseDatos
{
    private SQLiteAsyncConnection _conexion;

    public BaseDatos()
    {
        string ruta = Path.Combine(FileSystem.AppDataDirectory, "sitios.db3");
        _conexion = new SQLiteAsyncConnection(ruta);
        _conexion.CreateTableAsync<Sitio>().Wait();
    }

    public Task<List<Sitio>> ObtenerSitios()
    {
        return _conexion.Table<Sitio>().ToListAsync();
    }

    public Task<int> GuardarSitio(Sitio sitio)
    {
        return _conexion.InsertAsync(sitio);
    }

    public Task<int> EliminarSitio(Sitio sitio)
    {
        return _conexion.DeleteAsync(sitio);
    }
}