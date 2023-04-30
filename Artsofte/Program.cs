using Microsoft.EntityFrameworkCore;
using Artsofte.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
// Add a singleton for the Data Access Layer to establish a connection to the database and provide CRUD (Create, Read, Update, Delete) operations.
builder.Services.AddSingleton<ModelsDataAccessLayer>(ModelsDataAccessLayer.Instance);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // This middleware helps to detect and diagnose errors with Entity Framework Core migrations.
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
