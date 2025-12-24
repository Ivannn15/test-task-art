namespace Test;

internal sealed class Inventory
{
    public const int MaxWeight = 100;

    private readonly Dictionary<string, Item> _items = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _sync = new();
    private int _totalWeight;

    public IReadOnlyList<Item> Items
    {
        get
        {
            lock (_sync)
            {
                return _items.Values.ToList();
            }
        }
    }

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_sync)
        {
            if (_items.TryGetValue(item.Name, out var existing))
            {
                var newWeight = existing.Weight + item.Weight;
                var newTotalWeight = _totalWeight - existing.Weight + newWeight;

                EnsureMaxWeight(newTotalWeight);

                _items[existing.Name] = new Item(existing.Name, newWeight);
                _totalWeight = newTotalWeight;
                return;
            }

            var totalWeight = _totalWeight + item.Weight;
            EnsureMaxWeight(totalWeight);

            _items.Add(item.Name, item);
            _totalWeight = totalWeight;
        }
    }

    public bool RemoveItem(Item item)
    {
        if (item is null)
        {
            return false;
        }

        lock (_sync)
        {
            if (!_items.Remove(item.Name, out var removed))
            {
                return false;
            }

            _totalWeight -= removed.Weight;
            return true;
        }
    }

    public IReadOnlyList<Item> FindItemsByName(string namePart)
    {
        ArgumentNullException.ThrowIfNull(namePart);

        lock (_sync)
        {
            return _items.Values
                .Where(item => item.Name.Contains(namePart, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    private static void EnsureMaxWeight(int totalWeight)
    {
        if (totalWeight > MaxWeight)
        {
            throw new InvalidOperationException($"Inventory max weight of {MaxWeight} exceeded");
        }
    }
}
