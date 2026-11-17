[TestClass]
public class PlayerTests
{
    [TestMethod]
    public void NewPlayer_ShouldHaveDefaultValues()
    {
        Player player = new Player();

        Assert.AreEqual("", player.Name);
        Assert.AreEqual("", player.Race);
        Assert.AreEqual("", player.Occupation);

        Assert.AreEqual(0, player.Strength);
        Assert.AreEqual(0, player.Agility);
        Assert.AreEqual(0, player.HealthPoints);

        Assert.IsNull(player.Weapon);
    }

    [TestMethod]
    public void EquipmentStats_ShowEnchantedWeaponNameAndBoostedDamage()
    {
        Messages messages = new Messages();
        messages.ReadDictionary();
        Player player = new Player { Weapon = new Weapon("dagger", 6, "-)==>") };
        new Merchant().Purchase(player, MerchantItem.Enchantment);

        var lines = player.GetEquipmentStatLines(messages);

        CollectionAssert.Contains(lines, "Weapon: Enchanted dagger");
        CollectionAssert.Contains(lines, "Weapon's Maximum Damage Points: 10");
    }

    [TestMethod]
    public void NewWeapon_IsNotEnchanted()
    {
        Assert.IsFalse(new Weapon("dagger", 6, "-)==>").IsEnchanted);
    }

    [TestMethod]
    public void NewPlayer_StartsWithTenGoldAndNoArmor()
    {
        Player player = new Player();

        Assert.AreEqual(10, player.Gold);
        Assert.IsFalse(player.HasArmor);
    }
}
