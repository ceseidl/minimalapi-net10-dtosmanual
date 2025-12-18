
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ProductApi.Domain;
using ProductApi.Dtos;
using ProductApi.Mapping;

namespace ProductApi.Endpoints
{
    public static class ProductEndpoints
    {
        // "Banco" em memória para simplificar (sem EF/Core)
        private static readonly List<Product> _db = new();

        // Método de extensão sobre RouteGroupBuilder
        public static RouteGroupBuilder MapProductsEndpoints(this RouteGroupBuilder group)
        {
            // GET /products
            group.MapGet("/", () =>
            {
                var response = _db.Select(p => p.ToDto());
                return Results.Ok(response);
            });

            // GET /products/{id}
            group.MapGet("/{id:guid}", (Guid id) =>
            {
                var p = _db.FirstOrDefault(x => x.Id == id);
                if (p is null)
                {
                    return Results.NotFound(new { message = "Produto não encontrado." });
                }

                return Results.Ok(p.ToDto());
            })
            .WithName("GetProductById");

            // POST /products
            group.MapPost("/", (CreateProductRequest req) =>
            {
                var entity = req.ToEntity();
                entity.Id = Guid.NewGuid();
                _db.Add(entity);

                return Results.CreatedAtRoute(
                    routeName: "GetProductById",
                    routeValues: new { id = entity.Id },
                    value: entity.ToDto()
                );
            });

            // PUT /products/{id}
            group.MapPut("/{id:guid}", (Guid id, UpdateProductRequest req) =>
            {
                if (id != req.Id)
                {
                    return Results.BadRequest(new { message = "O ID da rota e o ID do corpo devem ser iguais." });
                }

                var idx = _db.FindIndex(x => x.Id == id);
                if (idx < 0)
                {
                    return Results.NotFound(new { message = "Produto não encontrado." });
                }

                var updated = req.ToEntity();
                _db[idx] = updated;

                return Results.NoContent();
            });

            // DELETE /products/{id}
            group.MapDelete("/{id:guid}", (Guid id) =>
            {
                var p = _db.FirstOrDefault(x => x.Id == id);
                if (p is null)
                {
                    return Results.NotFound(new { message = "Produto não encontrado." });
                }

                _db.Remove(p);
                return Results.NoContent();
            });

            // Sempre retorne o group no final do método de extensão
            return group;
        }
    }
}
