/// <summary>The animals a Hunt in the Forest can catch.</summary>
public enum Animal
{
    Rabbit,
    Deer,
    Boar
}

/// <summary>What happened on one Hunt, so Game can describe it and tests can check it.</summary>
public class HuntResult
{
    public bool Caught { get; init; }
    public Animal? Animal { get; init; }
    public int MeatGained { get; init; }
    public int HidesGained { get; init; }

    // Health lost to the Boar's gore.
    public int GoreDamage { get; init; }

    // Health lost for hunting while already Starving and/or Parched.
    public int HungerThirstDamage { get; init; }
}

/// <summary>
/// The rules for hunting in the Forest: tracking against Agility, rolling the animal,
/// handing out Meat and Hides, and the Hunger/Thirst cost of every Hunt.
/// No console input or output here; Game handles the Forest menu.
/// </summary>
public class Hunting
{
    public const int HungerThirstPerHunt = 2;
    public const int StarvingParchedDamage = 2;
    public const int TrackingDieSides = 20;
    public const int AnimalDieSides = 6;
    public const int GoreDieSides = 4;

    private readonly Die _die;

    public Hunting(Die die)
    {
        _die = die;
    }

    public HuntResult Hunt(Player player)
    {
        // Hunting while already Starving or Parched hurts, before this hunt makes it worse.
        int hungerThirstDamage = (player.IsStarving ? StarvingParchedDamage : 0) +
                                 (player.IsParched ? StarvingParchedDamage : 0);
        LoseHealth(player, hungerThirstDamage);

        player.Hunger = Math.Min(Player.MaxHungerThirst, player.Hunger + HungerThirstPerHunt);
        player.Thirst = Math.Min(Player.MaxHungerThirst, player.Thirst + HungerThirstPerHunt);

        if (_die.Roll(TrackingDieSides) > player.Agility)
            return new HuntResult { Caught = false, HungerThirstDamage = hungerThirstDamage };

        Animal animal = _die.Roll(AnimalDieSides) switch
        {
            <= 3 => Animal.Rabbit,
            <= 5 => Animal.Deer,
            _ => Animal.Boar
        };

        var (meat, hides) = animal switch
        {
            Animal.Rabbit => (1, 0),
            Animal.Deer => (2, 1),
            _ => (3, 1)
        };
        player.Meat += meat;
        player.Hides += hides;

        int gore = animal == Animal.Boar ? _die.Roll(GoreDieSides) : 0;
        LoseHealth(player, gore);

        return new HuntResult
        {
            Caught = true,
            Animal = animal,
            MeatGained = meat,
            HidesGained = hides,
            GoreDamage = gore,
            HungerThirstDamage = hungerThirstDamage
        };
    }

    private static void LoseHealth(Player player, int amount)
    {
        player.HealthPoints = Math.Max(0, player.HealthPoints - amount);
    }
}
