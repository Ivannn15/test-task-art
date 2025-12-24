namespace Test;

internal sealed class Item
{
    public Item(string name, int weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Item name must be provided", nameof(name));
        }

        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Item weight must be positive");
        }

        Name = name;
        Weight = weight;
    }

    public string Name { get; }
    public int Weight { get; }
}
