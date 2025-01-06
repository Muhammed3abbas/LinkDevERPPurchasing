using System;
using System.Linq;
using Xunit;
using Purchasing.Domain.Models;
using Purchasing.Domain.Enums;

namespace Purchasing.Tests
{
    //public class PurchaseOrderTests
    //{
    //    [Fact]
    //    public void Can_Approve_From_Created_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Act
    //        order.Approve();

    //        // Assert
    //        Assert.Equal(PurchaseOrderState.Approved, order.State);
    //    }

    //    [Fact]
    //    public void Cannot_Approve_From_NonCreated_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.Approve();

    //        // Act & Assert
    //        Assert.Throws<InvalidOperationException>(() => order.Approve());
    //    }

    //    [Fact]
    //    public void Can_Ship_From_Approved_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.Approve();

    //        // Act
    //        order.Ship();

    //        // Assert
    //        Assert.Equal(PurchaseOrderState.Shipped, order.State);
    //    }

    //    [Fact]
    //    public void Cannot_Ship_From_NonApproved_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Act & Assert
    //        Assert.Throws<InvalidOperationException>(() => order.Ship());
    //    }

    //    [Fact]
    //    public void Can_Close_From_Shipped_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.Approve();
    //        order.Ship();

    //        // Act
    //        order.Close();

    //        // Assert
    //        Assert.Equal(PurchaseOrderState.Closed, order.State);
    //    }

    //    [Fact]
    //    public void Cannot_Close_From_NonShipped_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Act & Assert
    //        Assert.Throws<InvalidOperationException>(() => order.Close());
    //    }

    //    [Fact]
    //    public void Can_Deactivate_Order()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Act
    //        order.Deactivate();

    //        // Assert
    //        Assert.Equal(PurchaseOrderState.Deactivated, order.State);
    //    }

    //    [Fact]
    //    public void Cannot_Deactivate_Closed_Order()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.Approve();
    //        order.Ship();
    //        order.Close();

    //        // Act & Assert
    //        Assert.Throws<InvalidOperationException>(() => order.Deactivate());
    //    }

    //    [Fact]
    //    public void Can_Add_Item_To_PurchaseOrder()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Act
    //        order.AddItem("G123", 200);

    //        // Assert
    //        Assert.Single(order.Items);
    //        Assert.Equal(1200, order.TotalPrice); // Updated to reflect total amount after adding item
    //    }

    //    [Fact]
    //    public void Cannot_Add_Duplicate_Item_To_PurchaseOrder()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.AddItem("G123", 200);

    //        // Act & Assert
    //        Assert.Throws<InvalidOperationException>(() => order.AddItem("G123", 200));
    //    }

    //    [Fact]
    //    public void TotalAmount_Should_Be_Calculated_Correctly()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);
    //        order.AddItem("G123", 200);
    //        order.AddItem("G124", 300);

    //        // Act
    //        var totalAmount = order.TotalPrice;

    //        // Assert
    //        Assert.Equal(1500, totalAmount);
    //    }

    //    [Fact]
    //    public void Order_Should_Initialize_With_Created_State()
    //    {
    //        // Arrange
    //        var order = new PurchaseOrder("PO123", DateTime.Now, 1000);

    //        // Assert
    //        Assert.Equal(PurchaseOrderState.Created, order.State);
    //    }
    //}
}
