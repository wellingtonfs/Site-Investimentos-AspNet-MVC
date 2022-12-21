using Microsoft.EntityFrameworkCore;
using Sistemas_Distribuidos.Data;
using Sistemas_Distribuidos.Repositorio;
using Sistemas_Distribuidos.Services;
using Sistemas_Distribuidos.utils;

// Carregar variáveis de ambiente
DotEnv.Load();

string ConnDb = $"Server={Environment.GetEnvironmentVariable("CONNECTION_DB_SERVER")};";
ConnDb += $"Database={Environment.GetEnvironmentVariable("CONNECTION_DB_NAME")};";
ConnDb += $"User Id={Environment.GetEnvironmentVariable("CONNECTION_DB_USERID")};";
ConnDb += $"Password={Environment.GetEnvironmentVariable("CONNECTION_DB_PASSWORD")}";

// Iniciar criação da página
var builder = WebApplication.CreateBuilder(args);

// Add serviços para a página
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddEntityFrameworkSqlServer().AddDbContext<BancoContext>(o => o.UseSqlServer(ConnDb));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUserRepositorio, UserRepositorio>();
builder.Services.AddScoped<IHgRepositorio, HgRepositorio>();
builder.Services.AddScoped<ISessions, Sessions>();

builder.Services.AddHostedService<UpdateTask>();

// Configuração de Sessões
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly= true;
    o.Cookie.IsEssential= true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Inicia a página
app.Run();
