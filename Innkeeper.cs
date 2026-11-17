/// <summary>The outcome of one deal with the Innkeeper.</summary>
public enum InnkeeperResult
{
    Sold,
    NothingToSell,
    Eaten,
    NoMeat,
    Drank,
    NotEnoughGold
}

/// <summary>
/// The Innkeeper's trade rules: buying the player's Meat and Hides for Gold, cooking
/// the player's own Meat, and selling meals and drinks. No console input or output here;
/// Game handles the Innkeeper menu.
/// </summary>
public class Innkeeper
{
    public const int MeatPrice = 2;
    public const int HidePrice = 4;
    public const int MealPrice = 3;
    public const int DrinkPrice = 1;

    /// <summary>Buys all the player's Meat. goldEarned is 0 when there was nothing to sell.</summary>
    public InnkeeperResult SellMeat(Player player, out int goldEarned)
    {
        goldEarned = player.Meat * MeatPrice;
        if (player.Meat == 0)
            return InnkeeperResult.NothingToSell;

        player.Gold += goldEarned;
        player.Meat = 0;
        return InnkeeperResult.Sold;
    }

    /// <summary>Buys all the player's Hides. goldEarned is 0 when there was nothing to sell.</summary>
    public InnkeeperResult SellHides(Player player, out int goldEarned)
    {
        goldEarned = player.Hides * HidePrice;
        if (player.Hides == 0)
            return InnkeeperResult.NothingToSell;

        player.Gold += goldEarned;
        player.Hides = 0;
        return InnkeeperResult.Sold;
    }

    /// <summary>Cooks one of the player's Meat for free; eating it clears Hunger.</summary>
    public InnkeeperResult CookMeat(Player player)
    {
        if (player.Meat == 0)
            return InnkeeperResult.NoMeat;

        player.Meat--;
        player.Hunger = 0;
        return InnkeeperResult.Eaten;
    }

    public InnkeeperResult BuyMeal(Player player)
    {
        if (player.Gold < MealPrice)
            return InnkeeperResult.NotEnoughGold;

        player.Gold -= MealPrice;
        player.Hunger = 0;
        return InnkeeperResult.Eaten;
    }

    public InnkeeperResult BuyDrink(Player player)
    {
        if (player.Gold < DrinkPrice)
            return InnkeeperResult.NotEnoughGold;

        player.Gold -= DrinkPrice;
        player.Thirst = 0;
        return InnkeeperResult.Drank;
    }
}
