[TestClass]
public class MerchantTests
{
    private static Player NewThief(int gold = 10, int health = 15)
    {
        return new Player
        {
            Occupation = "Thief",
            HealthPoints = health,
            Gold = gold,
            Weapon = new Weapon("dagger", 6, "-)==>")
        };
    }

    [TestMethod]
    public void BuyMagicalArmor_WithEnoughGold_IsPurchased()
    {
        Player player = NewThief();
        Merchant merchant = new Merchant();

        PurchaseResult result = merchant.Purchase(player, MerchantItem.MagicalArmor);

        Assert.AreEqual(PurchaseResult.Purchased, result);
        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual(23, player.HealthPoints);
        Assert.IsTrue(player.HasArmor);
    }

    [TestMethod]
    public void BuyMagicalArmor_WithoutEnoughGold_ChangesNothing()
    {
        Player player = NewThief(gold: 9);
        Merchant merchant = new Merchant();

        PurchaseResult result = merchant.Purchase(player, MerchantItem.MagicalArmor);

        Assert.AreEqual(PurchaseResult.NotEnoughGold, result);
        Assert.AreEqual(9, player.Gold);
        Assert.AreEqual(15, player.HealthPoints);
        Assert.IsFalse(player.HasArmor);
    }

    [TestMethod]
    public void BuyMagicalArmor_WhenAlreadyOwned_ChangesNothing()
    {
        Player player = NewThief(gold: 20);
        Merchant merchant = new Merchant();
        merchant.Purchase(player, MerchantItem.MagicalArmor);

        PurchaseResult result = merchant.Purchase(player, MerchantItem.MagicalArmor);

        Assert.AreEqual(PurchaseResult.AlreadyOwned, result);
        Assert.AreEqual(10, player.Gold);
        Assert.AreEqual(23, player.HealthPoints);
    }

    [TestMethod]
    public void AlreadyOwned_IsCheckedBeforeNotEnoughGold()
    {
        Player player = NewThief();
        Merchant merchant = new Merchant();
        merchant.Purchase(player, MerchantItem.MagicalArmor);

        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual(PurchaseResult.AlreadyOwned, merchant.Purchase(player, MerchantItem.MagicalArmor));
    }

    [TestMethod]
    public void AvailableItems_IncludesMagicalArmor_UntilOwned()
    {
        Player player = NewThief();
        Merchant merchant = new Merchant();

        CollectionAssert.Contains(merchant.GetAvailableItems(player).ToList(), MerchantItem.MagicalArmor);

        merchant.Purchase(player, MerchantItem.MagicalArmor);

        CollectionAssert.DoesNotContain(merchant.GetAvailableItems(player).ToList(), MerchantItem.MagicalArmor);
    }
}
