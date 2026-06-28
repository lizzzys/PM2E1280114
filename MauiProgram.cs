using Microsoft.Extensions.Logging;
using PM2E1280114.Data;

namespace PM2E1280114;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();
        // .UseMauiMaps();   

        builder.Services.AddSingleton<BaseDatos>();
        builder.Services.AddSingleton<Pages.PantallaInicial>();
        builder.Services.AddSingleton<Pages.ListaSitios>();
        builder.Services.AddTransient<Pages.MapaSitio>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}