using Frontend.Core;
using Frontend.Entities;
using Frontend.VTRepository;
using Frontend.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<CognitiveServiceApi>(builder.Configuration.GetSection("CognitiveServiceApi"));
builder.Services.Configure<LanguajeDictionary>(builder.Configuration.GetSection("LanguajeDictionary"));
builder.Services.AddSingleton<IV2TServices, V2TServices>();
builder.Services.AddSingleton<IV2TRepository, V2TRepository>();
builder.Services.AddAutoMapper(typeof(V2TMapper));
var app = builder.Build();

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
    pattern: "{controller=V2T}/{action=Index}/{id?}");

app.Run();



