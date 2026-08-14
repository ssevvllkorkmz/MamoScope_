using MamoScope_API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        }));

builder.Services.AddScoped<IMotorDriversRepository, MotorDriversRepository>();

builder.Services.AddOpenApi();

var app = builder.Build();



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await db.Database.OpenConnectionAsync();

    await using var command = db.Database.GetDbConnection().CreateCommand();

    command.CommandText = """
    SELECT
        current_database(),
        current_schema(),
        current_user,
        to_regclass('public."MotorDrivers"');
    """;

    await using var reader = await command.ExecuteReaderAsync();

    if (await reader.ReadAsync())
    {
        Console.WriteLine("========== DATABASE TEST ==========");
        Console.WriteLine($"Database: {reader.GetString(0)}");
        Console.WriteLine($"Schema: {reader.GetString(1)}");
        Console.WriteLine($"User: {reader.GetString(2)}");
        Console.WriteLine($"MotorDrivers: {(reader.IsDBNull(3) ? "YOK" : reader.GetString(3))}");
        Console.WriteLine("===================================");
    }

    await db.Database.CloseConnectionAsync();
}



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();