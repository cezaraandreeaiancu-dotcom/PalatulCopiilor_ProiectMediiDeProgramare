using Microsoft.Extensions.Logging;
using PalatulCopiilor3.Mobile.Services;


namespace PalatulCopiilor3.Mobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // =====================
            // HttpClient + ApiClient
            // =====================
            builder.Services.AddSingleton(sp =>
            {
                var handler = new HttpClientHandler();

            #if ANDROID
                handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            #endif

                return new HttpClient(handler)
                {
            #if ANDROID
                    BaseAddress = new Uri("https://10.0.2.2:7127")
            #else
                    BaseAddress = new Uri("https://localhost:7127")
            #endif
                };
            });


            builder.Services.AddSingleton<ApiClient>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ReservationsPage>();
            builder.Services.AddTransient<CalendarPage>();


            return builder.Build();
        }
    }
}
