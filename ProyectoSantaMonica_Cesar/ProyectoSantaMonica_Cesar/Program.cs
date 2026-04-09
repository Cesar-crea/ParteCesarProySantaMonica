using ProyectoSantaMonica_Cesar.Data;
using Microsoft.EntityFrameworkCore;
using ProyectoSantaMonica_Cesar.Repository;

var builder = WebApplication.CreateBuilder(args);

//Agregar el repositorio
builder.Services.AddScoped<EspecialidadRepository>();

builder.Services.AddScoped<PacienteRepository>();

builder.Services.AddScoped<MedicoRepository>();

builder.Services.AddScoped<UsuarioRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession();

var app = builder.Build();

app.UseSession();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
