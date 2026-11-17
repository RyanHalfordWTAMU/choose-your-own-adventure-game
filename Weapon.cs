public class Weapon
{
    public string Type { get; set; }
    public int MaxDamage { get; set; }
    public string AsciiArt { get; set; }

    // True once the Merchant's Enchantment has been applied (an Enchanted Weapon).
    // The weapon keeps its Type so the existing name translations still work.
    public bool IsEnchanted { get; set; }

    public Weapon(string type, int maxDamage, string asciiArt)
    {
        Type = type;
        MaxDamage = maxDamage;
        AsciiArt = asciiArt;
    }
}