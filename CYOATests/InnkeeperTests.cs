[TestClass]
public class InnkeeperTests
{
    private static Player NewTraveler(int gold = 10, int meat = 0, int hides = 0, int hunger = 0, int thirst = 0)
    {
        return new Player { Gold = gold, Meat = meat, Hides = hides, Hunger = hunger, Thirst = thirst };
    }

    [TestMethod]
    public void SellMeat_PaysTwoGoldEach_AndEmptiesTheMeat()
    {
        Player player = NewTraveler(gold: 0, meat: 3);

        InnkeeperResult result = new Innkeeper().SellMeat(player, out int earned);

        Assert.AreEqual(InnkeeperResult.Sold, result);
        Assert.AreEqual(6, earned);
        Assert.AreEqual(6, player.Gold);
        Assert.AreEqual(0, player.Meat);
    }

    [TestMethod]
    public void SellHides_PaysFourGoldEach_AndEmptiesTheHides()
    {
        Player player = NewTraveler(gold: 1, hides: 2);

        InnkeeperResult result = new Innkeeper().SellHides(player, out int earned);

        Assert.AreEqual(InnkeeperResult.Sold, result);
        Assert.AreEqual(8, earned);
        Assert.AreEqual(9, player.Gold);
        Assert.AreEqual(0, player.Hides);
    }

    [TestMethod]
    public void SellingWithNothingToSell_ChangesNothing()
    {
        Player player = NewTraveler(gold: 5);
        Innkeeper innkeeper = new Innkeeper();

        Assert.AreEqual(InnkeeperResult.NothingToSell, innkeeper.SellMeat(player, out int meatGold));
        Assert.AreEqual(InnkeeperResult.NothingToSell, innkeeper.SellHides(player, out int hideGold));
        Assert.AreEqual(0, meatGold + hideGold);
        Assert.AreEqual(5, player.Gold);
    }

    [TestMethod]
    public void CookMeat_UsesOneMeat_AndClearsHunger()
    {
        Player player = NewTraveler(gold: 4, meat: 2, hunger: 8);

        InnkeeperResult result = new Innkeeper().CookMeat(player);

        Assert.AreEqual(InnkeeperResult.Eaten, result);
        Assert.AreEqual(1, player.Meat);
        Assert.AreEqual(0, player.Hunger);
        Assert.AreEqual(4, player.Gold, "Cooking your own Meat is free.");
    }

    [TestMethod]
    public void CookMeat_WithNoMeat_ChangesNothing()
    {
        Player player = NewTraveler(hunger: 8);

        Assert.AreEqual(InnkeeperResult.NoMeat, new Innkeeper().CookMeat(player));
        Assert.AreEqual(8, player.Hunger);
    }

    [TestMethod]
    public void BuyMeal_CostsThreeGold_AndClearsHunger()
    {
        Player player = NewTraveler(gold: 5, hunger: 10);

        Assert.AreEqual(InnkeeperResult.Eaten, new Innkeeper().BuyMeal(player));
        Assert.AreEqual(2, player.Gold);
        Assert.AreEqual(0, player.Hunger);
    }

    [TestMethod]
    public void BuyMeal_WithoutEnoughGold_ChangesNothing()
    {
        Player player = NewTraveler(gold: 2, hunger: 10);

        Assert.AreEqual(InnkeeperResult.NotEnoughGold, new Innkeeper().BuyMeal(player));
        Assert.AreEqual(2, player.Gold);
        Assert.AreEqual(10, player.Hunger);
    }

    [TestMethod]
    public void BuyDrink_CostsOneGold_AndClearsThirst()
    {
        Player player = NewTraveler(gold: 1, thirst: 6);

        Assert.AreEqual(InnkeeperResult.Drank, new Innkeeper().BuyDrink(player));
        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual(0, player.Thirst);
    }

    [TestMethod]
    public void BuyDrink_WithoutGold_ChangesNothing()
    {
        Player player = NewTraveler(gold: 0, thirst: 6);

        Assert.AreEqual(InnkeeperResult.NotEnoughGold, new Innkeeper().BuyDrink(player));
        Assert.AreEqual(6, player.Thirst);
    }
}
