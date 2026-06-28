using PM2E1280114.Data;
using PM2E1280114.Pages;

namespace PM2E1280114;

public partial class AppShell : Shell
{
    // Constructor SIN parámetros (para XAML)
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("MapaSitio", typeof(MapaSitio));
    }

    // Constructor CON parámetros (para pasar la base de datos)
    public AppShell(BaseDatos db) : this()  // Llama al constructor sin parámetros
    {
        // Crear las páginas con la base de datos
        var pantallaInicio = new PantallaInicial(db);
        var listaSitios = new ListaSitios(db);

        // Limpiar y agregar los items
        Items.Clear();

        // Agregar tabs
        Items.Add(new ShellContent
        {
            Title = "Inicio",
            Content = pantallaInicio
        });

        Items.Add(new ShellContent
        {
            Title = "Mis Sitios",
            Content = listaSitios
        });
    }
}