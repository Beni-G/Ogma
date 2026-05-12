using FluentAssertions;
using Ogma.Domain.Orders.Entities;
using Ogma.Domain.SharedKernel.BaseTypes;
using Ogma.Domain.UnitTests.Orders.Helpers;

namespace Ogma.Domain.UnitTests.Orders.Tests;

public class OrderStatusTests
{
    [Fact]
    public void Create_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var name = "Pending";
        var description = "Order is pending.";
        // Act
        var orderStatus = OrderStatus.Create(name, description);
        // Assert
        orderStatus.Should().NotBeNull();
        orderStatus.Name.Should().Be(name);
        orderStatus.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var description = "Order is pending.";
        // Act
        Action act = () => OrderStatus.Create(invalidName, description);
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Reconstitute_ValidParameters_ShouldCreateInstance()
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var name = "Shipped";
        var description = "Order has been shipped.";
        // Act
        var orderStatus = OrderStatus.Reconstitute(id, name, description, OrdersTestData.GetMetadata());
        // Assert
        orderStatus.Should().NotBeNull();
        orderStatus.Id.Should().Be(id);
        orderStatus.Name.Should().Be(name);
        orderStatus.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Reconstitute_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var id = OrdersTestData.NextId();
        var description = "Order has been shipped.";
        // Act
        Action act = () => OrderStatus.Reconstitute(id, invalidName, description, OrdersTestData.GetMetadata());
        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateInstance()
    {
        // Arrange
        var orderStatus = OrderStatus.Create("Processing", "Order is being processed.");
        var newName = "Completed";
        var newDescription = "Order has been completed.";
        // Act
        orderStatus.Update(newName, newDescription);
        // Assert
        orderStatus.Name.Should().Be(newName);
        orderStatus.Description.Should().Be(newDescription);
    }

    [Fact]
    public void Update_ValidParameters_ShouldUpdateMetadata()
    {
        // Arrange
        var orderStatus = OrderStatus.Create("Processing", "Order is being processed.");
        var oldOrderStatusMetadata = new EntityMetadata(orderStatus.Metadata.CreatedAt, orderStatus.Metadata.UpdatedAt, orderStatus.Metadata.Version);
        var newName = "Completed";
        var newDescription = "Order has been completed.";
        // Act
        orderStatus.Update(newName, newDescription);
        // Assert
        orderStatus.Metadata.CreatedAt.Should().Be(oldOrderStatusMetadata.CreatedAt);
        orderStatus.Metadata.UpdatedAt.Should().BeAfter(oldOrderStatusMetadata.UpdatedAt);
        orderStatus.Metadata.Version.Should().Be(oldOrderStatusMetadata.Version + 1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Update_NullOrEmptyName_ShouldThrowArgumentException(string invalidName)
    {
        // Arrange
        var orderStatus = OrderStatus.Create("Processing", "Order is being processed.");
        var newDescription = "Order has been completed.";
        // Act
        Action act = () => orderStatus.Update(invalidName, newDescription);
        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
