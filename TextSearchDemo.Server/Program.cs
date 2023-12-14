using TextSearchDemo.Configuration;
using TextSearchDemo.Trie.Interfaces;
using TextSearchDemo.Trie.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IReadService, ReadService>();
builder.Services.AddScoped<ITrieService, TrieService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.Configure<Settings>(builder.Configuration.GetSection(nameof(Settings)));
builder.Services
    .AddCors(x => x.AddPolicy("Any", builder => 
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowAnyHeader()
                .Build();
        }));

var app = builder.Build();

app.UseCors("Any");

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
