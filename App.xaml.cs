using PM2E1280114.Data;

namespace PM2E1280114;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Crear la base de datos
        var db = new BaseDatos();

        // Pasar la base de datos al AppShell
        MainPage = new AppShell(db);   
    }
}