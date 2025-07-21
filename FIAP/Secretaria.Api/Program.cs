using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Secretaria.Application.Dtos.Aluno;
using Secretaria.Application.Dtos.Turma;
using Secretaria.Application.Interfaces.Aluno.Commands;
using Secretaria.Application.Interfaces.Aluno.Queries;
using Secretaria.Application.Interfaces.Matricula.Commands;
using Secretaria.Application.Interfaces.Turma.Commands;
using Secretaria.Application.Interfaces.Turma.Queries;
using Secretaria.Application.Services;
using Secretaria.Application.UseCases.Aluno.Commands;
using Secretaria.Application.UseCases.Aluno.Queries;
using Secretaria.Application.UseCases.Matricula.Commands;
using Secretaria.Application.UseCases.Turma.Commands;
using Secretaria.Application.UseCases.Turma.Queries;
using Secretaria.Application.Validators;
using Secretaria.Domain.Interfaces;
using Secretaria.Infrastructure.Persistence;
using Secretaria.Infrastructure.Repositories;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SecretariaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
                .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                     options.JsonSerializerOptions.WriteIndented = true;
                 });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Secretaria API",
        Version = "v1"
    });
});


// UseCases
///Aluno
builder.Services.AddScoped<IAtualizarCadastroAlunoUseCase, AtualizarCadastroAlunoUseCase>();
builder.Services.AddScoped<IAtualizarSenhaUseCase, AtualizarSenhaUseCase>();
builder.Services.AddScoped<ICadastrarAlunoUseCase, CadastrarAlunoUseCase>();
builder.Services.AddScoped<IObterAlunoPorCpfUseCase, ObterAlunoPorCpfUseCase>();
builder.Services.AddScoped<IObterAlunoPorNomeUseCase, ObterAlunoPorNomeUseCase>();
///Matricula
builder.Services.AddScoped<IMatricularAlunoUseCase, MatricularAlunoUseCase>();
builder.Services.AddScoped<ITrancarMatriculaUseCase, TrancarMatriculaUseCase>();
///Turma
builder.Services.AddScoped<ICriarTurmaUseCase, CriarTurmaUseCase>();
builder.Services.AddScoped<IObterTurmaComListaAlunosUseCase, ObterTurmaComListaAlunosUseCase>();


//Validators
builder.Services.AddScoped<IValidator<AlunoRequestDto>, AlunoRequestDtoValidator>();
builder.Services.AddScoped<IValidator<TurmaRequestDto>, TurmaRequestDtoValidator>();
builder.Services.AddScoped<IValidator<string>, SenhaValidator>();



// Repositories
builder.Services.AddScoped<IAlunoRepository, AlunoRepository>();
builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<IMatriculaRepository, MatriculaRepository>();
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();


// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGeradorNumeroMatricula, GeradorNumeroMatricula>();



var app = builder.Build();

// HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Secretaria API V1");
    });

}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
