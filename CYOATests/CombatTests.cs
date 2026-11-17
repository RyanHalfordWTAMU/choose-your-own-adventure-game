[TestClass]
public class CombatTests
{
    private Messages CreateEnglishMessages()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        return messages;
    }

    private Player CreateTestPlayer()
    {
        return new Player
        {
            Name = "TestHero",
            Race = "Human",
            Occupation = "Fighter",
            Strength = 15,
            Agility = 12,
            HealthPoints = 25,
            Weapon = new Weapon("long sword", 12, "-)=====>")
        };
    }

    private Dragon CreateTestDragon()
    {
        Dragon dragon = new Dragon(new Die())
        {
            Name = "Smolderfang",
            Strength = 18,
            Agility = 10,
            HealthPoints = 30,
            Weapon = new Weapon("claws", 12, "<<< claws >>>")
        };

        return dragon;
    }

    [TestMethod]
    public void Combat_Constructor_InitializesPlayerAndDragon()
    {
        Player player = new Player();
        Dragon dragon = new Dragon(new Die());
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        Assert.AreSame(player, combat.player);
        Assert.AreSame(dragon, combat.dragon);
    }

    [TestMethod]
    public void Combat_PlayerRetreated_DefaultsToFalse()
    {
        Player player = new Player();
        Dragon dragon = new Dragon(new Die());
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        Assert.IsFalse(combat.PlayerRetreated);
    }

    [TestMethod]
    public void GetCombatStatsDisplay_ReturnsNonEmptyString()
    {
        Player player = CreateTestPlayer();
        Dragon dragon = CreateTestDragon();
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        string result = combat.GetCombatStatsDisplay();

        Assert.IsFalse(string.IsNullOrWhiteSpace(result));
    }

    [TestMethod]
    public void GetCombatStatsDisplay_IncludesCombatHeaders()
    {
        Player player = CreateTestPlayer();
        Dragon dragon = CreateTestDragon();
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        string result = combat.GetCombatStatsDisplay();

        StringAssert.Contains(result, messages.GetMessage("combat_stats_header"));
        StringAssert.Contains(result, messages.GetMessage("combat_player_header"));
        StringAssert.Contains(result, messages.GetMessage("combat_dragon_header"));
    }

    [TestMethod]
    public void GetCombatStatsDisplay_IncludesPlayerCombatStats()
    {
        Player player = CreateTestPlayer();
        Dragon dragon = CreateTestDragon();
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        string result = combat.GetCombatStatsDisplay();

        StringAssert.Contains(result, player.Name);
        StringAssert.Contains(result, player.Strength.ToString());
        StringAssert.Contains(result, player.Agility.ToString());
        StringAssert.Contains(result, player.HealthPoints.ToString());
        StringAssert.Contains(result, player.Weapon!.Type);
        StringAssert.Contains(result, player.Weapon.MaxDamage.ToString());
    }

    [TestMethod]
    public void GetCombatStatsDisplay_IncludesDragonCombatStats()
    {
        Player player = CreateTestPlayer();
        Dragon dragon = CreateTestDragon();
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        string result = combat.GetCombatStatsDisplay();

        StringAssert.Contains(result, dragon.Name);
        StringAssert.Contains(result, dragon.Strength.ToString());
        StringAssert.Contains(result, dragon.Agility.ToString());
        StringAssert.Contains(result, dragon.HealthPoints.ToString());
        StringAssert.Contains(result, dragon.Weapon.Type);
        StringAssert.Contains(result, dragon.Weapon.MaxDamage.ToString());
    }

    [TestMethod]
    public void GetCombatStatsDisplay_UsesLocalizedStatLabels()
    {
        Player player = CreateTestPlayer();
        Dragon dragon = CreateTestDragon();
        Messages messages = CreateEnglishMessages();

        Combat combat = new Combat(player, dragon, messages);

        string result = combat.GetCombatStatsDisplay();

        StringAssert.Contains(result, messages.GetMessage("stats_name"));
        StringAssert.Contains(result, messages.GetMessage("stats_strength"));
        StringAssert.Contains(result, messages.GetMessage("stats_agility"));
        StringAssert.Contains(result, messages.GetMessage("stats_health"));
        StringAssert.Contains(result, messages.GetMessage("stats_weapon"));
        StringAssert.Contains(result, messages.GetMessage("stats_damage"));
    }

    // Runs combat with scripted keyboard input and returns everything it printed.
    private static string RunCombatWithInput(Combat combat, string input)
    {
        TextReader originalIn = Console.In;
        TextWriter originalOut = Console.Out;
        var output = new StringWriter();
        try
        {
            Console.SetIn(new StringReader(input));
            Console.SetOut(output);
            combat.StartCombat();
        }
        finally
        {
            Console.SetIn(originalIn);
            Console.SetOut(originalOut);
        }
        return output.ToString();
    }

    private static int CountOf(string text, string part) => text.Split(part).Length - 1;

    [TestMethod]
    [DoNotParallelize] // redirects the shared Console
    public void InvalidCombatInput_AsksAgain_InsteadOfAttacking()
    {
        Player player = CreateTestPlayer();
        player.HealthPoints = 1000;
        Dragon dragon = CreateTestDragon();
        dragon.HealthPoints = 1000;
        Combat combat = new Combat(player, dragon, CreateEnglishMessages());

        string output = RunCombatWithInput(combat, "x\nr\n");

        Assert.IsTrue(combat.PlayerRetreated);
        Assert.AreEqual(1, CountOf(output, "Invalid choice"));
        Assert.AreEqual(1, CountOf(output, "attack the dragon"), "Only the opening attack should happen.");
    }

    [TestMethod]
    [DoNotParallelize] // redirects the shared Console
    public void EndedCombatInput_CountsAsRetreat()
    {
        Player player = CreateTestPlayer();
        player.HealthPoints = 1000;
        Dragon dragon = CreateTestDragon();
        dragon.HealthPoints = 1000;
        Combat combat = new Combat(player, dragon, CreateEnglishMessages());

        string output = RunCombatWithInput(combat, "");

        Assert.IsTrue(combat.PlayerRetreated);
        Assert.AreEqual(1, CountOf(output, "attack the dragon"));
    }

    [TestMethod]
    public void DragonHealth_NeverShownBelowZero()
    {
        Player player = CreateTestPlayer();
        player.Strength = 20; // every attack roll hits
        Dragon dragon = CreateTestDragon();
        dragon.Agility = 0;   // every defense roll fails
        dragon.HealthPoints = 1;

        string output = player.Attack(dragon, CreateEnglishMessages());

        Assert.AreEqual(0, dragon.HealthPoints);
        StringAssert.Contains(output, "health points are now 0.");
    }

    [TestMethod]
    public void AttackIntro_ReadsNaturallyForEveryOccupation()
    {
        Player player = CreateTestPlayer();
        player.Occupation = "Archer";
        player.Strength = 0; // miss, so only the intro matters

        string output = player.Attack(CreateTestDragon(), CreateEnglishMessages());

        StringAssert.StartsWith(output, "Archer by trade, you draw your long sword and attack the dragon.");
    }
}
