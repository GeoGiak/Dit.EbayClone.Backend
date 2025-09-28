using Dit.EbayClone.Backend.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.StartApplicationWithAdmin();

app.Run();
