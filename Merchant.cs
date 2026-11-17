/// <summary>The magical items the Merchant sells.</summary>
public enum MerchantItem
{
    MagicalArmor
}

/// <summary>The outcome of trying to buy an item from the Merchant.</summary>
public enum PurchaseResult
{
    Purchased,
    NotEnoughGold,
    AlreadyOwned
}

/// <summary>
/// The trade rules for the merchant met on the south path: prices, ownership checks,
/// paying with gold and applying each item's effect. No console input or output here,
/// so the rules can be tested directly; Game handles the menu.
/// </summary>
public class Merchant
{
    public const int ItemPrice = 10;
    public const int MagicalArmorHealthBonus = 8;

    public PurchaseResult Purchase(Player player, MerchantItem item)
    {
        // Ownership is checked first, so an owner with no gold hears "already owned".
        if (Owns(player, item))
            return PurchaseResult.AlreadyOwned;

        if (player.Gold < ItemPrice)
            return PurchaseResult.NotEnoughGold;

        ApplyEffect(player, item);
        player.Gold -= ItemPrice;
        return PurchaseResult.Purchased;
    }

    /// <summary>The items still for sale to this player (anything they own is left out).</summary>
    public IEnumerable<MerchantItem> GetAvailableItems(Player player)
    {
        return Enum.GetValues<MerchantItem>().Where(item => !Owns(player, item));
    }

    private static bool Owns(Player player, MerchantItem item)
    {
        // No silent default: a new item must say how ownership is tracked.
        return item switch
        {
            MerchantItem.MagicalArmor => player.HasArmor,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, "Unknown merchant item.")
        };
    }

    private static void ApplyEffect(Player player, MerchantItem item)
    {
        switch (item)
        {
            case MerchantItem.MagicalArmor:
                player.HasArmor = true;
                player.HealthPoints += MagicalArmorHealthBonus;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(item), item, "Unknown merchant item.");
        }
    }
}
