using Microsoft.EntityFrameworkCore;
using PatientManagementAPI.Data;
using PatientManagementAPI.Services;
using PatientManagementAPI.DTOs;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// 1. Register SQL Server DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Register Patient Service
builder.Services.AddScoped<IPatientService, PatientService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Register services

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// 5. Get all patients
app.MapGet("/api/patients",
    async (
        IPatientService patientService,
        PatientQueryParameters parameters,
        CancellationToken cancellationToken) =>
    {
        var patients = await patientService.GetPatientsAsync(cancellationToken);
        

        return Results.Ok(patients);
    });

// 6. Get patient by ID
app.MapGet("/api/patients/{id:int}",
    async (
        int id,
        IPatientService patientService,
        CancellationToken cancellationToken) =>
    {
        var patient = await patientService.GetPatientByIdAsync(
            id, cancellationToken);

        return patient is not null
            ? Results.Ok(patient)
            : Results.NotFound(
                new { Message = "Patient not found" });
    });


// POST: Create patient
app.MapPost("/api/patients",
    async (CreatePatientRequest request,
           IPatientService service,
           CancellationToken cancellationToken) =>
    {
        var errors = ValidateModel(request);

        if (errors.Count > 0)
            return Results.ValidationProblem(errors);

        var patient = await service.CreatePatientAsync(
            request, cancellationToken);

        return Results.CreatedAtRoute(
            "GetPatientById",
            new { id = patient.PatientID },
            patient);
    });

    // DELETE: Delete patient
app.MapDelete("/api/patients/{id:int}",
    async (int id,
           IPatientService service,
           CancellationToken cancellationToken) =>
    {
        var deleted = await service.DeletePatientAsync(
            id, cancellationToken);

        return deleted
            ? Results.NoContent()
            : Results.NotFound(new { Message = "Patient not found" });
    });


app.Run();

// Data Annotation validation helper
static Dictionary<string, string[]> ValidateModel<T>(T model)
{
    var context = new ValidationContext(model!);
    var results = new List<ValidationResult>();

    Validator.TryValidateObject(
        model!, context, results, validateAllProperties: true);

    return results
        .SelectMany(result =>
            (result.MemberNames.Any()
                ? result.MemberNames
                : new[] { string.Empty })
            .Select(member => new
            {
                Member = member,
                Error = result.ErrorMessage ?? "Invalid value"
            }))
        .GroupBy(x => x.Member)
        .ToDictionary(
            group => group.Key,
            group => group.Select(x => x.Error).ToArray());
}
