using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ProductApi.Endpoints;   // garante acesso ao método de extensão MapProductsEndpoints
using ProductApi.Domain;     // se você referenciar Product aqui
using ProductApi.Dtos;       // se você referenciar DTOs aqui
using ProductApi.Mapping;    // se você referenciar mapeamentos aqui

var builder = WebApplication.CreateBuilder(args);

// Serviços básicos
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

var app = builder.Build();

// Swagger apenas em Dev (pode usar em Prod se desejar)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Health checks
app.MapHealthChecks("/health");

// Endpoints de produtos (extension method definido em ProductEndpoints.cs)
app.MapGroup("/products").MapProductsEndpoints();

app.Run();

//// Necessário para testes de integração com WebApplicationFactory
