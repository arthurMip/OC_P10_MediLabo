var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var urlConfigs= builder.Configuration.GetSection("URL");


builder.Services.AddHttpClient("auth_api", client =>
{
    //client.BaseAddress = new Uri("https://localhost:7000/api/auth");
    Console.WriteLine($"AUTH API URL: {urlConfigs["AUTH_API"]!}");
    client.BaseAddress = new Uri(urlConfigs["AUTH_API"]!);

});

builder.Services.AddHttpClient("patients_api", client =>
{
    //Console.WriteLine($"PATIENT API URL: {urlConfigs["PATIENT_API"]!}");
    //client.BaseAddress = new Uri(urlConfigs["PATIENT_API"]!);
    client.BaseAddress = new Uri("http://localhost:5000/api");
});

builder.Services.AddHttpClient("notes_api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/api/notes");
});


builder.Services
    .AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
