using System;
using System.Collections.Generic;

public class Table<R, C, V>
{
    private readonly Dictionary<R, Dictionary<C, V>> worktable = new();

    public void AddOrUpdate(R rowKey, C columnKey, V value)
    {
        if (!worktable.ContainsKey(rowKey))
        {
            worktable[rowKey] = new Dictionary<C, V>();
        }

        worktable[rowKey][columnKey] = value;
    }

   
    public V GetValue(R rowKey, C columnKey)
    {
        if (worktable.TryGetValue(rowKey, out var columns) && columns.TryGetValue(columnKey, out var value))
        {
            return value;
        }

        throw new KeyNotFoundException($"No value found for row key '{rowKey}' and column key '{columnKey}'.");
    }

   
    public bool Contains(R rowKey, C columnKey)
    {
        return worktable.ContainsKey(rowKey) && worktable[rowKey].ContainsKey(columnKey);
    }

  
    public bool Remove(R rowKey, C columnKey)
    {
        if (worktable.ContainsKey(rowKey) && worktable[rowKey].Remove(columnKey))
        {
            if (worktable[rowKey].Count == 0)
            {
                worktable.Remove(rowKey);
            }
            return true;
        }

        return false;
    }
}

public class FootballTeam : IEquatable<FootballTeam>
{
    public string Title { get; }
    public string City { get; }
    public int FoundationYear { get; }

    public FootballTeam(string title, string city, int foundationYear)
    {
        Title = title;
        City = city;
        FoundationYear = foundationYear;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as FootballTeam);
    }

    public bool Equals(FootballTeam other)
    {
        return other != null && Title == other.Title && City == other.City && FoundationYear == other.FoundationYear;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, City, FoundationYear);
    }

    public override string ToString()
    {
        return $"{Title} ({City}, {FoundationYear})";
    }
}

public class Tournament : IEquatable<Tournament>
{
    public string Title { get; }
    public bool IsInternational { get; }
    public int FoundationYear { get; }

    public Tournament(string title, bool isInternational, int foundationYear)
    {
        Title = title;
        IsInternational = isInternational;
        FoundationYear = foundationYear;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Tournament);
    }

    public bool Equals(Tournament other)
    {
        return other != null && Title == other.Title && IsInternational == other.IsInternational && FoundationYear == other.FoundationYear;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, IsInternational, FoundationYear);
    }

    public override string ToString()
    {
        return $"{Title} (International: {IsInternational}, {FoundationYear})";
    }
}

public class Creature : IEquatable<Creature>
{
    public string Name { get; }
    public string Type { get; } 

    public Creature(string name, string type)
    {
        Name = name;
        Type = type;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Creature);
    }

    public bool Equals(Creature other)
    {
        return other != null && Name == other.Name && Type == other.Type;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Type);
    }

    public override string ToString()
    {
        return $"{Name} ({Type})";
    }
}

public class Element : IEquatable<Element>
{
    public string Name { get; }

    public Element(string name)
    {
        Name = name;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Element);
    }

    public bool Equals(Element other)
    {
        return other != null && Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }
}

public class Spell
{
    public string Name { get; }
    public int PowerLevel { get; }

    public Spell(string name, int powerLevel)
    {
        Name = name;
        PowerLevel = powerLevel;
    }

    public override string ToString()
    {
        return $"{Name} (Power: {PowerLevel})";
    }
}

class Program
{
    static void Main()
    {
   
        var team1 = new FootballTeam("Dynamo", "Kyiv", 1927);
        var team2 = new FootballTeam("Shakhtar", "Donetsk", 1936);

        var tournament1 = new Tournament("Ukrainian Premier League", false, 1992);
        var tournament2 = new Tournament("UEFA Champions League", true, 1955);

    
        var table = new Table<FootballTeam, Tournament, HashSet<int>>();

        table.AddOrUpdate(team1, tournament1, new HashSet<int> { 2000, 2001, 2003 });
        table.AddOrUpdate(team2, tournament1, new HashSet<int> { 2002, 2005, 2008 });
        table.AddOrUpdate(team1, tournament2, new HashSet<int> { 1999 });

       
        Console.WriteLine($"Dynamo wins in Ukrainian Premier League: {string.Join(", ", table.GetValue(team1, tournament1))}");
        Console.WriteLine($"Shakhtar wins in Ukrainian Premier League: {string.Join(", ", table.GetValue(team2, tournament1))}");

      
        Console.WriteLine($"Does Dynamo have results in UEFA Champions League? {table.Contains(team1, tournament2)}");

     
        table.Remove(team1, tournament2);
        Console.WriteLine($"After removal: Does Dynamo have results in UEFA Champions League? {table.Contains(team1, tournament2)}");

    
        var dragon = new Creature("Ancient Dragon", "Dragon");
        var elf = new Creature("Forest Elf", "Elf");
        var troll = new Creature("Mountain Troll", "Troll");

        var fire = new Element("Fire");
        var water = new Element("Water");
        var air = new Element("Air");
        var earth = new Element("Earth");

     
        var magicTable = new Table<Creature, Element, HashSet<Spell>>();

       
        magicTable.AddOrUpdate(dragon, fire, new HashSet<Spell>
        {
            new Spell("Inferno Breath", 90),
            new Spell("Firestorm", 85)
        });

        magicTable.AddOrUpdate(elf, water, new HashSet<Spell>
        {
            new Spell("Healing Rain", 70),
            new Spell("Aqua Shield", 65)
        });

        magicTable.AddOrUpdate(troll, earth, new HashSet<Spell>
        {
            new Spell("Stone Skin", 80),
            new Spell("Quake Strike", 75)
        });

        magicTable.AddOrUpdate(elf, air, new HashSet<Spell>
        {
            new Spell("Wind Walk", 60),
            new Spell("Gale Force", 55)
        });

        Console.WriteLine($"Spells for {dragon} in {fire}:");
        foreach (var spell in magicTable.GetValue(dragon, fire))
        {
            Console.WriteLine($" - {spell}");
        }

        Console.WriteLine($"\nSpells for {elf} in {water}:");
        foreach (var spell in magicTable.GetValue(elf, water))
        {
            Console.WriteLine($" - {spell}");
        }

        Console.WriteLine($"\nDoes {troll} have spells in {air}? {magicTable.Contains(troll, air)}");

      
        magicTable.Remove(elf, air);
        Console.WriteLine($"\nAfter removal: Does {elf} have spells in {air}? {magicTable.Contains(elf, air)}");
    }
}
