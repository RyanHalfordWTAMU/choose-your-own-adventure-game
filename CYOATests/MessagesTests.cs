// CYOATests/MessagesTests.cs
[TestClass]
public class MessagesTests
{
    [TestMethod]
    public void Constructor_DefaultLanguageIsEnglish()
    {
        Messages messages = new Messages();
        Assert.AreEqual("English", messages.CurrentLanguage);
    }

    [TestMethod]
    public void SetCurrentLanguage_SetsLanguageToSpanish()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Spanish");
        Assert.AreEqual("Spanish", messages.CurrentLanguage);
    }

    [TestMethod]
    public void SetCurrentLanguage_SetsLanguageToFrench()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("French");
        Assert.AreEqual("French", messages.CurrentLanguage);
    }

    [TestMethod]
    public void SetCurrentLanguage_SetsLanguageToEnglish()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Spanish");
        messages.SetCurrentLanguage("English");
        Assert.AreEqual("English", messages.CurrentLanguage);
    }

    [TestMethod]
    public void ReadDictionary_LoadsEnglishWelcomeMessage()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        string result = messages.GetMessage("welcome");
        Assert.AreEqual("Welcome to Choose Your Adventure Game", result);
    }

    [TestMethod]
    public void ReadDictionary_LoadsSpanishWelcomeMessage()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Spanish");
        messages.ReadDictionary();
        string result = messages.GetMessage("welcome");
        Assert.AreEqual("Bienvenido al Juego de Elige Tu Aventura", result);
    }

    [TestMethod]
    public void ReadDictionary_LoadsFrenchWelcomeMessage()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("French");
        messages.ReadDictionary();
        string result = messages.GetMessage("welcome");
        Assert.AreEqual("Bienvenue dans Choisissez Votre Aventure", result);
    }

    [TestMethod]
    public void GetMessage_ReturnsFallbackForUnknownKey()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        string result = messages.GetMessage("unknown_key");
        Assert.AreEqual("[unknown_key]", result);
    }

    [TestMethod]
    public void GetMessage_ReturnsDifferentMessageAfterLanguageSwitch()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        string english = messages.GetMessage("welcome");

        messages.SetCurrentLanguage("Spanish");
        messages.ReadDictionary();
        string spanish = messages.GetMessage("welcome");

        Assert.AreNotEqual(english, spanish);
    }

    [TestMethod]
    public void ReadDictionary_LoadsGoodbyeMessageInEachLanguage()
    {
        Messages messages = new Messages();

        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.AreEqual("Adventure ends. Bye.", messages.GetMessage("goodbye"));

        messages.SetCurrentLanguage("Spanish");
        messages.ReadDictionary();
        Assert.AreEqual("Adios!", messages.GetMessage("goodbye"));

        messages.SetCurrentLanguage("French");
        messages.ReadDictionary();
        Assert.AreEqual("Au revoir!", messages.GetMessage("goodbye"));
    }

    [TestMethod]
    public void ReadDictionary_AllRequiredKeysExistInEnglish()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();

        string[] requiredKeys = { "welcome", "menu",
                                   "choose_path", "path_north", "path_south", "attack_prompt",
                                   "victory", "defeat", "retreat", "invalid", "goodbye" };

        foreach (string key in requiredKeys)
        {
            string result = messages.GetMessage(key);
            Assert.AreNotEqual($"[{key}]", result, $"Key '{key}' is missing from English dictionary.");
        }
    }

    [TestMethod]
    public void ReadDictionary_AllRequiredKeysExistInSpanish()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Spanish");
        messages.ReadDictionary();

        string[] requiredKeys = { "welcome", "menu",
                                   "choose_path", "path_north", "path_south", "attack_prompt",
                                   "victory", "defeat", "retreat", "invalid", "goodbye" };

        foreach (string key in requiredKeys)
        {
            string result = messages.GetMessage(key);
            Assert.AreNotEqual($"[{key}]", result, $"Key '{key}' is missing from Spanish dictionary.");
        }
    }

    [TestMethod]
    public void ReadDictionary_AllRequiredKeysExistInFrench()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("French");
        messages.ReadDictionary();

        string[] requiredKeys = { "welcome", "menu",
                                   "choose_path", "path_north", "path_south", "attack_prompt",
                                   "victory", "defeat", "retreat", "invalid", "goodbye" };

        foreach (string key in requiredKeys)
        {
            string result = messages.GetMessage(key);
            Assert.AreNotEqual($"[{key}]", result, $"Key '{key}' is missing from French dictionary.");
        }
    }

    // --- IsValidRace ---

    [TestMethod]
    public void IsValidRace_ReturnsTrue_ForHuman()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsTrue(messages.IsValidRace("human"));
    }

    [TestMethod]
    public void IsValidRace_ReturnsTrue_CaseInsensitive()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsTrue(messages.IsValidRace("HUMAN"));
    }

    [TestMethod]
    public void IsValidRace_ReturnsFalse_ForInvalidRace()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsFalse(messages.IsValidRace("orc"));
    }

    [TestMethod]
    public void IsValidRace_ReturnsFalse_ForNull()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsFalse(messages.IsValidRace(null));
    }

    [TestMethod]
    public void NormalizeRace_ReturnsCanonical_ForHuman()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.AreEqual("Human", messages.NormalizeRace("human"));
    }

    [TestMethod]
    public void NormalizeRace_ReturnsCanonical_CaseInsensitive()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.AreEqual("Human", messages.NormalizeRace("HUMAN"));
    }

    // --- IsValidOccupation ---

    [TestMethod]
    public void IsValidOccupation_ReturnsTrue_ForFighter()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsTrue(messages.IsValidOccupation("fighter"));
    }

    [TestMethod]
    public void IsValidOccupation_ReturnsTrue_CaseInsensitive()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsTrue(messages.IsValidOccupation("FIGHTER"));
    }

    [TestMethod]
    public void IsValidOccupation_ReturnsFalse_ForInvalidOccupation()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsFalse(messages.IsValidOccupation("wizard"));
    }

    [TestMethod]
    public void IsValidOccupation_ReturnsFalse_ForNull()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.IsFalse(messages.IsValidOccupation(null));
    }

    [TestMethod]
    public void NormalizeOccupation_ReturnsCanonical_ForFighter()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.AreEqual("Fighter", messages.NormalizeOccupation("fighter"));
    }

    [TestMethod]
    public void NormalizeOccupation_ReturnsCanonical_CaseInsensitive()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("English");
        messages.ReadDictionary();
        Assert.AreEqual("Fighter", messages.NormalizeOccupation("FIGHTER"));
    }

    // --- Italian ---

    private static Messages LoadItalian()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Italian");
        messages.ReadDictionary();
        return messages;
    }

    [TestMethod]
    public void SetCurrentLanguage_SetsLanguageToItalian()
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage("Italian");
        Assert.AreEqual("Italian", messages.CurrentLanguage);
    }

    [TestMethod]
    public void ReadDictionary_LoadsItalianWelcomeMessage()
    {
        Messages messages = LoadItalian();
        Assert.AreEqual("Italian", messages.CurrentLanguage, "Italian should not fall back to English.");
        Assert.AreEqual("Benvenuto nel Gioco Scegli la Tua Avventura", messages.GetMessage("welcome"));
        Assert.AreEqual("Arrivederci!", messages.GetMessage("goodbye"));
    }

    [TestMethod]
    public void ReadDictionary_AllRequiredKeysExistInItalian()
    {
        Messages messages = LoadItalian();

        string[] requiredKeys = { "welcome", "menu",
                                   "choose_path", "path_north", "path_south", "attack_prompt",
                                   "victory", "defeat", "retreat", "invalid", "goodbye" };

        foreach (string key in requiredKeys)
        {
            string result = messages.GetMessage(key);
            Assert.AreNotEqual($"[{key}]", result, $"Key '{key}' is missing from Italian dictionary.");
        }
    }

    [TestMethod]
    public void Italian_RaceInput_IsValidAndNormalized()
    {
        Messages messages = LoadItalian();
        Assert.IsTrue(messages.IsValidRace("Mezzuomo"));
        Assert.AreEqual("Halfling", messages.NormalizeRace("mezzuomo"));
        Assert.AreEqual("Elf", messages.NormalizeRace("ELFO"));
        Assert.IsFalse(messages.IsValidRace("Halfling"), "English race names should not be accepted in Italian.");
    }

    [TestMethod]
    public void Italian_OccupationInput_IsValidAndNormalized()
    {
        Messages messages = LoadItalian();
        Assert.IsTrue(messages.IsValidOccupation("Arciere"));
        Assert.AreEqual("Thief", messages.NormalizeOccupation("ladro"));
    }

    [TestMethod]
    public void Italian_DisplayNames_AreTranslated()
    {
        Messages messages = LoadItalian();
        Assert.AreEqual("Mezzuomo", messages.TranslateRaceForDisplay("Halfling"));
        Assert.AreEqual("Guerriero", messages.TranslateOccupationForDisplay("Fighter"));
        Assert.AreEqual("spada lunga", messages.TranslateWeaponForDisplay("long sword"));
        Assert.AreEqual("artigli", messages.TranslateWeaponForDisplay("claws"));
    }

    // The completeness test below compares against English, so English itself must
    // have the merchant keys for that guard to cover them.
    [TestMethod]
    [DataRow("merchant_intro")]
    [DataRow("merchant_gold")]
    [DataRow("merchant_menu_header")]
    [DataRow("merchant_option_armor")]
    [DataRow("merchant_option_leave")]
    [DataRow("merchant_option_enchant")]
    [DataRow("enchanted_weapon_format")]
    [DataRow("merchant_thanks")]
    [DataRow("merchant_not_enough_gold")]
    [DataRow("merchant_already_owned")]
    [DataRow("merchant_leave")]
    [DataRow("item_magical_armor")]
    [DataRow("stats_gold")]
    [DataRow("stats_armor")]
    [DataRow("stats_armor_none")]
    public void English_HasMerchantMessageKey(string key)
    {
        Messages messages = new Messages();
        messages.ReadDictionary();
        Assert.AreNotEqual($"[{key}]", messages.GetMessage(key));
    }

    // A typo in a translated placeholder would crash the merchant menu at runtime,
    // so format every language's merchant strings with the arguments Game passes.
    [TestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void MerchantFormatStrings_FormatInEveryLanguage(string language)
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();

        StringAssert.Contains(string.Format(messages.GetMessage("merchant_gold"), 10), "10");
        StringAssert.StartsWith(string.Format(messages.GetMessage("merchant_option_armor"), "1", 8, 10), "1. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("merchant_option_leave"), "3"), "3. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("merchant_option_enchant"), "2", "x", 4, 10), "2. ");
        StringAssert.Contains(string.Format(messages.GetMessage("enchanted_weapon_format"), "WEAPON"), "WEAPON");
    }

    // The completeness test compares against English, so English must have the
    // Forest and Inn keys, and every language's format strings must accept Game's arguments.
    [TestMethod]
    [DataRow("English")]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void ForestAndInnStrings_ExistAndFormatInEveryLanguage(string language)
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();

        string[] keys = { "forest_intro", "forest_option_hunt", "forest_option_leave", "forest_leave",
                          "forest_defeat", "hunt_escaped", "hunt_starving", "hunt_parched",
                          "animal_rabbit", "animal_deer", "animal_boar", "inn_intro", "inn_menu_header",
                          "inn_option_innkeeper", "inn_option_merchant", "inn_option_leave", "inn_leave",
                          "innkeeper_intro", "innkeeper_nothing_to_sell", "innkeeper_ate", "innkeeper_no_meat",
                          "innkeeper_drank", "innkeeper_leave", "stats_meat", "stats_hides", "stats_hunger",
                          "stats_thirst", "stats_starving", "stats_parched" };
        foreach (string key in keys)
            Assert.AreNotEqual($"[{key}]", messages.GetMessage(key), $"{language} is missing '{key}'.");

        StringAssert.Contains(string.Format(messages.GetMessage("forest_status"), 11, 22, 33, 44, 55, 10), "55");
        StringAssert.Contains(string.Format(messages.GetMessage("hunt_hunger_damage"), 4), "4");
        StringAssert.Contains(string.Format(messages.GetMessage("hunt_caught"), "ANIMAL", 3, 1), "ANIMAL");
        StringAssert.Contains(string.Format(messages.GetMessage("hunt_caught_meat_only"), "ANIMAL", 1, 0), "ANIMAL");
        StringAssert.Contains(string.Format(messages.GetMessage("hunt_boar_gore"), 3), "3");
        StringAssert.Contains(string.Format(messages.GetMessage("innkeeper_goods"), 11, 22, 33, 44, 10), "44");
        StringAssert.StartsWith(string.Format(messages.GetMessage("innkeeper_option_sell_meat"), "1", 2), "1. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("innkeeper_option_sell_hides"), "2", 4), "2. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("innkeeper_option_cook"), "3"), "3. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("innkeeper_option_meal"), "4", 3), "4. ");
        StringAssert.StartsWith(string.Format(messages.GetMessage("innkeeper_option_drink"), "5", 1), "5. ");
        StringAssert.Contains(string.Format(messages.GetMessage("innkeeper_sold"), 14), "14");
        StringAssert.Contains(messages.GetMessage("path_prompt_full"), "'w'");
    }

    [TestMethod]
    [DataRow("English", "Enchanted long sword")]
    [DataRow("Spanish", "espada larga con encantamiento")]
    [DataRow("French", "epee longue avec enchantement")]
    [DataRow("Italian", "spada lunga con incantesimo")]
    public void WeaponDisplayName_UsesEnchantedFormat(string language, string expected)
    {
        Messages messages = new Messages();
        messages.SetCurrentLanguage(language);
        messages.ReadDictionary();
        Weapon weapon = new Weapon("long sword", 12, "-)=====>") { IsEnchanted = true };

        Assert.AreEqual(expected, messages.GetWeaponDisplayName(weapon));
    }

    [TestMethod]
    public void English_SouthPathNarrative_IsRemoved()
    {
        Messages messages = new Messages();
        messages.ReadDictionary();
        Assert.AreEqual("[south_path_narrative]", messages.GetMessage("south_path_narrative"));
    }

    // Guards every language (including Italian) against missing translations:
    // each language must have every message key that English has.
    [TestMethod]
    [DataRow("Spanish")]
    [DataRow("French")]
    [DataRow("Italian")]
    public void Language_HasEveryEnglishMessageKey(string language)
    {
        string jsonPath = Path.Combine(AppContext.BaseDirectory, "language_data.json");
        using System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(jsonPath));
        var root = doc.RootElement;

        Assert.IsTrue(root.TryGetProperty(language, out var section), $"{language} section is missing.");

        var englishKeys = root.GetProperty("English").GetProperty("dictionary")
            .EnumerateObject().Select(p => p.Name);
        var languageKeys = section.GetProperty("dictionary")
            .EnumerateObject().Select(p => p.Name).ToHashSet();

        var missing = englishKeys.Where(k => !languageKeys.Contains(k)).ToList();
        Assert.AreEqual(0, missing.Count, $"{language} is missing: {string.Join(", ", missing)}");
    }
}
