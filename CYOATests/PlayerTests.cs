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
    public void NewPlayer_StartsWithTenGoldAndNoArmor()
    {
        Player player = new Player();

        Assert.AreEqual(10, player.Gold);
        Assert.IsFalse(player.HasArmor);
    }
}
