
using Chest.API.Filters;
using Chest.Application.Interface;
using Chest.Application.Services;
using Chest.Application.Validators;
using Chest.Domain.Interfaces;
using Chest.Infrastructure.Data;
using Chest.Infrastructure.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore; // OBRIGATÓRIO para os validadores funcionarem

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DE SERVIÇOS DO SISTEMA ---
builder.Services.AddControllers(options =>
{
    // Descomente esta linha assim que criar o arquivo ChestExceptionFilter na API
    options.Filters.Add<ChestExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); 
});

// --- 2. BANCO DE DADOS (SQLite) ---
builder.Services.AddDbContext<ChestDbContext>(options =>
    options.UseSqlite("Data Source=ChestGame.db"));

// --- 3. INJEÇÃO DE DEPENDÊNCIA ---

// Repositórios
builder.Services.AddScoped<IChestRepository, ChestRepository>();

// Serviços de Aplicação (Segregados)
builder.Services.AddScoped<IChestCreationService, ChestCreationService>();
builder.Services.AddScoped<IChestHuntService, ChestHuntService>();
builder.Services.AddScoped<IChestManagementService, ChestManagementService>();
builder.Services.AddScoped<IChestSearchService, ChestSearchService>();
builder.Services.AddScoped<IChestDeletionService, ChestDeletionService>();

// Registro dos Validadores
// Se o erro persistir, verifique se instalou o pacote: FluentValidation.DependencyInjectionExtensions
builder.Services.AddValidatorsFromAssemblyContaining<CreateChestValidator>();
builder.Services.AddScoped<IChestHuntService, ChestHuntService>();
// --- 4. PIPELINE DA APLICAÇÃO ---
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();