using MongoDB.Driver;
using PatientNotesApi.Database;
using PatientNotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var client = new MongoClient(settings!.ConnectionString);

    return client;
});

builder.Services.AddSingleton<PatientNotesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
