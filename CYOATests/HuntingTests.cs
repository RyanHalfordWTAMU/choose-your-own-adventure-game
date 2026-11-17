/// <summary>A die that returns pre-set rolls in order, so hunt outcomes are predictable.</summary>
public class ScriptedDie : Die
{
    private readonly Queue<int> _rolls;

    public ScriptedDie(params int[] rolls)
    {
        _rolls = new Queue<int>(rolls);
    }

    public override int Roll(int sides)
    {
        int roll = _rolls.Dequeue();
        Assert.IsTrue(roll >= 1 && roll <= sides, $"Scripted roll {roll} is not possible on a d{sides}.");
        return roll;
    }
}

[TestClass]
public class HuntingTests
{
    private static Player NewHunter(int agility = 10, int health = 15, int hunger = 0, int thirst = 0)
    {
        return new Player { Agility = agility, HealthPoints = health, Hunger = hunger, Thirst = thirst };
    }

    [TestMethod]
    public void NewPlayer_HasNoGoodsHungerOrThirst()
    {
        Player player = new Player();
        Assert.AreEqual(0, player.Meat);
        Assert.AreEqual(0, player.Hides);
        Assert.AreEqual(0, player.Hunger);
        Assert.AreEqual(0, player.Thirst);
    }

    [TestMethod]
    public void TrackingRollAboveAgility_AnimalEscapes()
    {
        Player player = NewHunter(agility: 10);

        HuntResult result = new Hunting(new ScriptedDie(11)).Hunt(player);

        Assert.IsFalse(result.Caught);
        Assert.IsNull(result.Animal);
        Assert.AreEqual(0, player.Meat);
        Assert.AreEqual(0, player.Hides);
        Assert.AreEqual(15, player.HealthPoints);
    }

    [TestMethod]
    [DataRow(1, Animal.Rabbit, 1, 0)]
    [DataRow(3, Animal.Rabbit, 1, 0)]
    [DataRow(4, Animal.Deer, 2, 1)]
    [DataRow(5, Animal.Deer, 2, 1)]
    public void AnimalRoll_GivesTheRightAnimalAndGoods(int animalRoll, Animal expected, int meat, int hides)
    {
        Player player = NewHunter(agility: 10);

        // Tracking roll equal to Agility still catches.
        HuntResult result = new Hunting(new ScriptedDie(10, animalRoll)).Hunt(player);

        Assert.IsTrue(result.Caught);
        Assert.AreEqual(expected, result.Animal);
        Assert.AreEqual(meat, player.Meat);
        Assert.AreEqual(hides, player.Hides);
        Assert.AreEqual(15, player.HealthPoints);
    }

    [TestMethod]
    public void Boar_GivesMostGoods_ButGoresThePlayer()
    {
        Player player = NewHunter();

        HuntResult result = new Hunting(new ScriptedDie(1, 6, 3)).Hunt(player);

        Assert.AreEqual(Animal.Boar, result.Animal);
        Assert.AreEqual(3, player.Meat);
        Assert.AreEqual(1, player.Hides);
        Assert.AreEqual(3, result.GoreDamage);
        Assert.AreEqual(12, player.HealthPoints);
    }

    [TestMethod]
    public void Goods_AddUpOverSeveralHunts()
    {
        Player player = NewHunter();
        Hunting hunting = new Hunting(new ScriptedDie(1, 4, 1, 1));

        hunting.Hunt(player); // deer
        hunting.Hunt(player); // rabbit

        Assert.AreEqual(3, player.Meat);
        Assert.AreEqual(1, player.Hides);
    }

    [TestMethod]
    public void EveryHunt_RaisesHungerAndThirstByTwo_EvenWhenTheAnimalEscapes()
    {
        Player player = NewHunter(agility: 1);

        new Hunting(new ScriptedDie(20)).Hunt(player);

        Assert.AreEqual(2, player.Hunger);
        Assert.AreEqual(2, player.Thirst);
    }

    [TestMethod]
    public void HungerAndThirst_AreCappedAtTen()
    {
        Player player = NewHunter(agility: 1, hunger: 9, thirst: 9);

        new Hunting(new ScriptedDie(20)).Hunt(player);

        Assert.AreEqual(10, player.Hunger);
        Assert.AreEqual(10, player.Thirst);
        Assert.IsTrue(player.IsStarving);
        Assert.IsTrue(player.IsParched);
        Assert.AreEqual(15, player.HealthPoints, "Reaching 10 doesn't hurt; hunting while already at 10 does.");
    }

    [TestMethod]
    public void HuntingWhileStarving_CostsTwoHealth()
    {
        Player player = NewHunter(agility: 1, hunger: 10);

        HuntResult result = new Hunting(new ScriptedDie(20)).Hunt(player);

        Assert.AreEqual(2, result.HungerThirstDamage);
        Assert.AreEqual(13, player.HealthPoints);
    }

    [TestMethod]
    public void HuntingWhileStarvingAndParched_CostsFourHealth()
    {
        Player player = NewHunter(agility: 1, hunger: 10, thirst: 10);

        HuntResult result = new Hunting(new ScriptedDie(20)).Hunt(player);

        Assert.AreEqual(4, result.HungerThirstDamage);
        Assert.AreEqual(11, player.HealthPoints);
    }

    [TestMethod]
    public void HealthNeverDropsBelowZero()
    {
        Player player = NewHunter(health: 3, hunger: 10, thirst: 10);

        new Hunting(new ScriptedDie(1, 6, 4)).Hunt(player);

        Assert.AreEqual(0, player.HealthPoints);
    }
}
