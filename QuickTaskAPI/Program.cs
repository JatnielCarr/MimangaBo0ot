using QuickTaskAPI.Services.Features.Mangas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "MangaBot API",
        Description = "Una API para gestionar una increíble colección de mangas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tu Nombre/Equipo",
            Url = new Uri("https://github.com/JatnielCarr")
        }
    });
});

// Registrar nuestro servicio como Singleton para mantener los datos en memoria
builder.Services.AddSingleton<MangaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MangaBot API V1");
        options.RoutePrefix = string.Empty; // Para que Swagger UI esté en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
