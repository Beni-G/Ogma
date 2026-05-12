using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.UnitTests.Orders.Helpers;

namespace Ogma.Domain.UnitTests.Orders.Tests;

public class OrderTypeTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateOrderType()
    {
        // Arrange
        var code = "OT001";
        var description = "Standard Order Type";
        // Act
        var orderType = OrderType.Create(code, description);
        // Assert
        orderType.Should().NotBeNull();
        orderType.Code.Should().Be(code);
        orderType.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_InvalidCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var description = "Standard Order Type";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => OrderType.Create(invalidCode, description));
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateOrderType()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var code = "OT003";
        var description = "Reconstituted Order Type";
        // Act
        var orderType = OrderType.Reconstitute(id, code, description, OrdersTestData.GetMetadata());
        // Assert
        orderType.Should().NotBeNull();
        orderType.Id.Should().Be(id);
        orderType.Code.Should().Be(code);
        orderType.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Reconstitute_InvalidId_ShouldThrowArgumentException(long invalidId)
    {
        // Arrange
        var code = "OT004";
        var description = "Invalid ID Order Type";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => OrderType.Reconstitute(invalidId, code, description, OrdersTestData.GetMetadata()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_InvalidCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var description = "Invalid Code Order Type";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => OrderType.Reconstitute(id, invalidCode, description, OrdersTestData.GetMetadata()));
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateOrderType()
    {
        // Arrange
        var orderType = OrderType.Reconstitute(1L, "custord", "Customer Orders", OrdersTestData.GetMetadata());
        var newCode = "sales_ord";
        var newDescription = "Sales order";
        // Act
        orderType.Update(newCode, newDescription);
        // Arrange
        orderType.Code.Should().Be(newCode);
        orderType.Description.Should().Be(newDescription);
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateMetadata()
    {
        // Arrange
        var orderType = OrderType.Reconstitute(1L, "custord", "Customer Orders", OrdersTestData.GetMetadata());
        var oldOrderTypeMetadata = new EntityMetadata(orderType.Metadata.CreatedAt, orderType.Metadata.UpdatedAt, orderType.Metadata.Version);
        var newCode = "sales_ord";
        var newDescription = "Sales order";
        // Act
        orderType.Update(newCode, newDescription);
        // Arrange
        orderType.Metadata.CreatedAt.Should().Be(oldOrderTypeMetadata.CreatedAt);
        orderType.Metadata.UpdatedAt.Should().BeAfter(oldOrderTypeMetadata.UpdatedAt);
        orderType.Metadata.Version.Should().Be(oldOrderTypeMetadata.Version + 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_InvalidCode_ShouldThrowArgumentException(string invalidCode)
    {
        // Arrange
        var orderType = OrderType.Reconstitute(1L, "custord", "Customer Orders", OrdersTestData.GetMetadata());
        var newDescription = "Sales order";
        // Act & Assert
        Assert.Throws<ArgumentException>(() => orderType.Update(invalidCode, newDescription));
    }
}
