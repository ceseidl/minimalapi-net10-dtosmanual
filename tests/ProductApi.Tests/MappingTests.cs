using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;
using ProductApi.Domain;
using ProductApi.Dtos;
using ProductApi.Mapping;


namespace ProductApi.Tests;

public class MappingTests
{
    [Fact]
    public void ToDto_Mapeia_Campos_Corretamente()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mouse",
            Price = 99.90m,
            Quantity = 10,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var dto = product.ToDto();

        Assert.Equal(product.Id, dto.Id);
        Assert.Equal(product.Name, dto.Name);
        Assert.Equal(product.Price, dto.Price);
        Assert.Equal(product.Quantity, dto.Quantity);
    }
}
