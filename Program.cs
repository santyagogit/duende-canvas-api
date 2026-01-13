using DuendeCanvasAPI.Application.UseCases;
using DuendeCanvasAPI.Domain.Interfaces;
using DuendeCanvasAPI.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<IProductoRepository>(new ProductoRepository(builder.Configuration));
builder.Services.AddTransient<GetProductosQuery>();

// Etiquetas services
builder.Services.AddScoped<IEtiquetaRepository, EtiquetaRepository>();
builder.Services.AddTransient<CreateEtiquetaCommand>();
builder.Services.AddTransient<GetEtiquetaByIdQuery>();
builder.Services.AddTransient<GetUltimasEtiquetasQuery>();
builder.Services.AddTransient<VerificarNombreEtiquetaQuery>();
builder.Services.AddTransient<UpdateEtiquetaCommand>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
