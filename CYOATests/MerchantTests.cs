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
    public void BuyEnchantment_WithEnoughGold_BoostsCurrentWeapon()
    {
        Player player = NewThief();
        Merchant merchant = new Merchant();

        PurchaseResult result = merchant.Purchase(player, MerchantItem.Enchantment);

        Assert.AreEqual(PurchaseResult.Purchased, result);
        Assert.AreEqual(0, player.Gold);
        Assert.AreEqual("dagger", player.Weapon!.Type);
        Assert.AreEqual(10, player.Weapon.MaxDamage);
        Assert.IsTrue(player.Weapon.IsEnchanted);
        Assert.AreEqual(15, player.HealthPoints);
        Assert.IsFalse(player.HasArmor);
    }

    [TestMethod]
    public void BuyEnchantment_WithoutEnoughGold_ChangesNothing()
    {
        Player player = NewThief(gold: 5);
        Merchant merchant = new Merchant();

        PurchaseResult result = merchant.Purchase(player, MerchantItem.Enchantment);

        Assert.AreEqual(PurchaseResult.NotEnoughGold, result);
        Assert.AreEqual(5, player.Gold);
        Assert.AreEqual(6, player.Weapon!.MaxDamage);
        Assert.IsFalse(player.Weapon.IsEnchanted);
    }

    [TestMethod]
    public void BuyEnchantment_WhenAlreadyOwned_ChangesNothing()
    {
        Player player = NewThief(gold: 20);
        Merchant merchant = new Merchant();
        merchant.Purchase(player, MerchantItem.Enchantment);

        PurchaseResult result = merchant.Purchase(player, MerchantItem.Enchantment);

        Assert.AreEqual(PurchaseResult.AlreadyOwned, result);
        Assert.AreEqual(10, player.Gold);
        Assert.AreEqual(10, player.Weapon!.MaxDamage);
    }

    [TestMethod]
    public void BuyingOneItem_LeavesNotEnoughGoldForTheOther()
    {
        Player armorFirst = NewThief();
        Player enchantFirst = NewThief();
        Merchant merchant = new Merchant();

        merchant.Purchase(armorFirst, MerchantItem.MagicalArmor);
        merchant.Purchase(enchantFirst, MerchantItem.Enchantment);

        Assert.AreEqual(PurchaseResult.NotEnoughGold, merchant.Purchase(armorFirst, MerchantItem.Enchantment));
        Assert.AreEqual(PurchaseResult.NotEnoughGold, merchant.Purchase(enchantFirst, MerchantItem.MagicalArmor));
    }

    [TestMethod]
    public void EnchantedWeapon_DamageRolls_StayInRangeAndCanExceedOldMax()
    {
        Player player = NewThief();
        new Merchant().Purchase(player, MerchantItem.Enchantment);

        var rolls = Enumerable.Range(0, 2000).Select(_ => player.RollDamage()).ToList();

        Assert.IsTrue(rolls.All(r => r >= 1 && r <= 10), "A roll fell outside 1..10.");
        Assert.IsTrue(rolls.Any(r => r > 6), "No roll exceeded the dagger's old max of 6.");
    }

    [TestMethod]
    public void AvailableItems_LeavesOutEnchantment_OnceOwned()
    {
        Player player = NewThief(gold: 20);
        Merchant merchant = new Merchant();

        CollectionAssert.AreEqual(
            new[] { MerchantItem.MagicalArmor, MerchantItem.Enchantment },
            merchant.GetAvailableItems(player).ToList());

        merchant.Purchase(player, MerchantItem.Enchantment);

        CollectionAssert.AreEqual(new[] { MerchantItem.MagicalArmor }, merchant.GetAvailableItems(player).ToList());
    }

    [TestMethod]
    public void Enchantment_WithNoWeapon_IsNotAvailableAndChangesNothing()
    {
        Player player = new Player { HealthPoints = 15 };
        Merchant merchant = new Merchant();

        CollectionAssert.DoesNotContain(merchant.GetAvailableItems(player).ToList(), MerchantItem.Enchantment);
        Assert.AreEqual(PurchaseResult.NotAvailable, merchant.Purchase(player, MerchantItem.Enchantment));
        Assert.AreEqual(10, player.Gold);
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
