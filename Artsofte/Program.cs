using Microsoft.EntityFrameworkCore;
using Artsofte.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<ModelsDAL>(ModelsDAL.Instance);

/*//Add the DB context to the container and include the connection string from the appsettings.json file. If the connection string is not found, it will return an exception 
builder.Services.AddDbContext<ArtsofteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ArtsofteContext") ?? throw new InvalidOperationException("Connection string 'ArtsofteContext' not found.")));
*/
// This middleware helps to detect and diagnose errors with Entity Framework Core migrations.
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
//This code creates a new scope for the application services, gets the required ArtsofteContext service,
//ensures the database is created, and initializes the database using the DbInitializer class.
//The DbInitializer class contains code to populate the database with initial data.
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var context = services.GetRequiredService<ArtsofteContext>();
//    context.Database.EnsureCreated();
//    DbInitializer.Initialize(context);
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
