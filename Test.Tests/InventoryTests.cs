using System;
using Xunit;
using Test;

namespace Test.Tests;

public class InventoryTests
{
    [Fact]
    public void AddItem_WhenDuplicateName_SumsWeight()
    {
        var inventory = new Inventory();

        inventory.AddItem(new Item("Sword", 10));
        inventory.AddItem(new Item("Sword", 5));

        var items = inventory.Items;
        Assert.Single(items);
        Assert.Equal(15, items[0].Weight);
    }

    [Fact]
    public void AddItem_WhenExceedsMaxWeight_Throws()
    {
        var inventory = new Inventory();
        inventory.AddItem(new Item("Armor", 90));

        var exception = Assert.Throws<InvalidOperationException>(
            () => inventory.AddItem(new Item("Shield", 15)));

        Assert.Contains("max weight", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FindItemsByName_ReturnsSubstringMatches()
    {
        var inventory = new Inventory();
        inventory.AddItem(new Item("Iron Sword", 10));
        inventory.AddItem(new Item("Wooden Shield", 12));

        var matches = inventory.FindItemsByName("sword");

        Assert.Single(matches);
        Assert.Equal("Iron Sword", matches[0].Name);
    }

    [Fact]
    public void RemoveItem_RemovesByName()
    {
        var inventory = new Inventory();
        inventory.AddItem(new Item("Potion", 5));

        var removed = inventory.RemoveItem(new Item("Potion", 1));

        Assert.True(removed);
        Assert.Empty(inventory.Items);
    }
}
