using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoomReservation_Item_I13L.Data;
using RoomReservation_Item_I13L.Data.Services;

namespace RoomReservation_Item_I13L
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
                });

            builder.Services.AddMauiBlazorWebView();

            var connectionString = "Data Source=DESKTOP-CUTKD51;Initial Catalog=DB_RoomReservation;Integrated Security=True;Trust Server Certificate=True";
            
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped<RoomService>();
            builder.Services.AddScoped<ReservationService>();
            builder.Services.AddScoped<UserService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
