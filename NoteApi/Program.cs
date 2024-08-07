using Microsoft.EntityFrameworkCore;
using NotesApi;
using NotesApi.Middleware;
using NotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicatonDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreConnecton")));

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddScoped<IAccountService, AccountServices>();

var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();

// Применение миграций при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicatonDbContext>();
    dbContext.Database.Migrate(); // Применяет все миграции
}

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