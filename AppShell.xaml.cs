namespace PM2E1280114;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("MapaSitio", typeof(Pages.MapaSitio));
    }
}