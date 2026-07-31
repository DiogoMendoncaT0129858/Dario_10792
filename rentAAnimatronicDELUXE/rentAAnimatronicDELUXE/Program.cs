using System.Net.Sockets;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

//Polly Cache Config 
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IAsyncCacheProvider>(single =>
{
    var Mprovider = single.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
    return new MemoryCacheProvider(Mprovider);
});

builder.Services.AddSingleton<AsyncCachePolicy>(policy =>
{
    var Cprovider = policy.GetRequiredService<IAsyncCacheProvider>();
    return Policy.CacheAsync(Cprovider, TimeSpan.FromMinutes(4));
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Linha que connecta a API e o Mountbank
builder.Services.AddHttpClient("InventoryCheck", client =>
{
    client.BaseAddress = new Uri("http://localhost:1987");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
