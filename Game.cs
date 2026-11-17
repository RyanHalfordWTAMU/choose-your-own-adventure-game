public class Game
{
    public void StartGame()
    {
        // Title screen
        Console.WriteLine("=====================================");
        Console.WriteLine("Welcome to Choose Your Adventure Game");
        Console.WriteLine("=====================================\n");

        // Instantiate Die first (per sequence diagram)
        Die die = new Die();

        Dragon dragon = new Dragon(die);

        // Language selection shown before Messages dictionary is loaded
        Console.WriteLine("Select language / Selecciona idioma / Choisissez la langue / Seleziona la lingua:");
        Console.WriteLine("1. English  2. Espanol  3. Francais  4. Italiano");
        Console.Write("Enter choice: ");
        string? langInput = Console.ReadLine()?.Trim();
        string selectedLanguage = GetLanguageFromChoice(langInput);

        // Initialize Messages with selected language (per sequence diagram)

        Messages messages = new Messages();
        messages.SetCurrentLanguage(selectedLanguage);
        messages.ReadDictionary();
        // Display welcome message
        Console.WriteLine();
        Console.WriteLine(messages.GetMessage("welcome"));
        Console.WriteLine();

        // Main menu loop
        bool running = true;
        while (running)
        {
            Console.WriteLine(messages.GetMessage("menu"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                Player player = new Player();
                player.CreateCharacter(messages);

                bool inAdventureMenu = true;
                while (inAdventureMenu)
                {
                    string selectedPath = PromptForPath(messages);

                    if (selectedPath == "exit")
                    {
                        Console.WriteLine(messages.GetMessage("adventure_ends"));
                        running = false;
                        inAdventureMenu = false;
                    }
                    else if (selectedPath == "south")
                    {
                        bool playerDied = HandleForest(player, die, messages);
                        if (playerDied)
                        {
                            running = false;
                            inAdventureMenu = false;
                        }
                    }
                    else if (selectedPath == "west")
                    {
                        HandleInn(player, messages);
                    }
                    else if (selectedPath == "north")
                    {
                        HandleDragonEncounter(player, dragon, messages);

                        running = false;
                        inAdventureMenu = false;
                    }
                }
            }
            else if (choice == "2")
            {
                Console.WriteLine(messages.GetMessage("goodbye"));
                running = false;
            }
            else
            {
                Console.WriteLine(messages.GetMessage("invalid"));
            }
        }
    }

    public void EndGame(bool playerWon, bool playerRetreated, Messages messages)
    {
        if (playerRetreated)
            Console.WriteLine(messages.GetMessage("retreat"));
        else if (playerWon)
            Console.WriteLine(messages.GetMessage("victory"));
        else
            Console.WriteLine(messages.GetMessage("defeat"));
    }

    // Maps the language menu number to the language name used in language_data.json.
    // Anything unrecognized falls back to English.
    public string GetLanguageFromChoice(string? input)
    {
        return input?.Trim() switch
        {
            "2" => "Spanish",
            "3" => "French",
            "4" => "Italian",
            _ => "English"
        };
    }

    public bool IsValidMenuChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim();
        return trimmed == "1" || trimmed == "2";
    }

    public bool IsValidPathChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim().ToLower();
        return trimmed == "n" || trimmed == "north" ||
               trimmed == "s" || trimmed == "south" ||
               trimmed == "w" || trimmed == "west" ||
               trimmed == "e" || trimmed == "exit";
    }

    public bool IsValidCombatChoice(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        string trimmed = input.Trim().ToLower();
        return trimmed == "a" || trimmed == "attack" ||
               trimmed == "r" || trimmed == "retreat";
    }

    private string PromptForPath(Messages messages)
    {
        while (true)
        {
            Console.WriteLine(messages.GetMessage("path_prompt_full"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "north" || input == "n")
                return "north";

            if (input == "south" || input == "s")
                return "south";

            if (input == "west" || input == "w")
                return "west";

            // null means the input stream ended, so there's nothing left to ask.
            if (input == null || input == "exit" || input == "e")
                return "exit";

            Console.WriteLine(messages.GetMessage("path_invalid"));
        }
    }

    // ---------- Forest ----------

    // Returns true if the player collapsed (health reached 0), which ends the game.
    private bool HandleForest(Player player, Die die, Messages messages)
    {
        Hunting hunting = new Hunting(die);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(messages.GetMessage("forest_intro"));
        Console.ResetColor();

        while (true)
        {
            Console.WriteLine(string.Format(messages.GetMessage("forest_status"),
                player.HealthPoints, player.Meat, player.Hides, player.Hunger, player.Thirst, Player.MaxHungerThirst));
            Console.WriteLine(messages.GetMessage("forest_option_hunt"));
            Console.WriteLine(messages.GetMessage("forest_option_leave"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim();

            // null means the input stream ended; treat it as leaving instead of looping forever.
            if (input == null || input == "2")
            {
                Console.WriteLine(messages.GetMessage("forest_leave"));
                return false;
            }

            if (input != "1")
            {
                Console.WriteLine(messages.GetMessage("invalid"));
                continue;
            }

            HuntResult result = hunting.Hunt(player);
            DescribeHunt(result, player, messages);

            if (player.HealthPoints <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(messages.GetMessage("forest_defeat"));
                Console.ResetColor();
                return true;
            }
        }
    }

    private static void DescribeHunt(HuntResult result, Player player, Messages messages)
    {
        if (result.HungerThirstDamage > 0)
            Console.WriteLine(string.Format(messages.GetMessage("hunt_hunger_damage"), result.HungerThirstDamage));

        if (!result.Caught)
        {
            Console.WriteLine(messages.GetMessage("hunt_escaped"));
        }
        else
        {
            string animalName = messages.GetMessage(result.Animal switch
            {
                Animal.Rabbit => "animal_rabbit",
                Animal.Deer => "animal_deer",
                _ => "animal_boar"
            });
            string caughtKey = result.HidesGained > 0 ? "hunt_caught" : "hunt_caught_meat_only";
            Console.WriteLine(string.Format(messages.GetMessage(caughtKey), animalName, result.MeatGained, result.HidesGained));

            if (result.GoreDamage > 0)
                Console.WriteLine(string.Format(messages.GetMessage("hunt_boar_gore"), result.GoreDamage));
        }

        if (player.IsStarving)
            Console.WriteLine(messages.GetMessage("hunt_starving"));
        if (player.IsParched)
            Console.WriteLine(messages.GetMessage("hunt_parched"));
    }

    // ---------- Inn ----------

    private void HandleInn(Player player, Messages messages)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(messages.GetMessage("inn_intro"));
        Console.ResetColor();

        while (true)
        {
            Console.WriteLine(messages.GetMessage("inn_menu_header"));
            Console.WriteLine(messages.GetMessage("inn_option_innkeeper"));
            Console.WriteLine(messages.GetMessage("inn_option_merchant"));
            Console.WriteLine(messages.GetMessage("inn_option_leave"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim();

            if (input == null || input == "3")
            {
                Console.WriteLine(messages.GetMessage("inn_leave"));
                return;
            }

            if (input == "1")
                HandleInnkeeper(player, messages);
            else if (input == "2")
                HandleMerchantEncounter(player, messages);
            else
                Console.WriteLine(messages.GetMessage("invalid"));
        }
    }

    private void HandleInnkeeper(Player player, Messages messages)
    {
        Innkeeper innkeeper = new Innkeeper();
        Console.WriteLine(messages.GetMessage("innkeeper_intro"));

        while (true)
        {
            Console.WriteLine(string.Format(messages.GetMessage("merchant_gold"), player.Gold));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_goods"),
                player.Meat, player.Hides, player.Hunger, player.Thirst, Player.MaxHungerThirst));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_option_sell_meat"), "1", Innkeeper.MeatPrice));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_option_sell_hides"), "2", Innkeeper.HidePrice));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_option_cook"), "3"));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_option_meal"), "4", Innkeeper.MealPrice));
            Console.WriteLine(string.Format(messages.GetMessage("innkeeper_option_drink"), "5", Innkeeper.DrinkPrice));
            Console.WriteLine(string.Format(messages.GetMessage("merchant_option_leave"), "6"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim();

            if (input == null || input == "6")
            {
                Console.WriteLine(messages.GetMessage("innkeeper_leave"));
                return;
            }

            int goldEarned = 0;
            InnkeeperResult? result = input switch
            {
                "1" => innkeeper.SellMeat(player, out goldEarned),
                "2" => innkeeper.SellHides(player, out goldEarned),
                "3" => innkeeper.CookMeat(player),
                "4" => innkeeper.BuyMeal(player),
                "5" => innkeeper.BuyDrink(player),
                _ => null
            };

            string message = result switch
            {
                InnkeeperResult.Sold => string.Format(messages.GetMessage("innkeeper_sold"), goldEarned),
                InnkeeperResult.NothingToSell => messages.GetMessage("innkeeper_nothing_to_sell"),
                InnkeeperResult.Eaten => messages.GetMessage("innkeeper_ate"),
                InnkeeperResult.NoMeat => messages.GetMessage("innkeeper_no_meat"),
                InnkeeperResult.Drank => messages.GetMessage("innkeeper_drank"),
                InnkeeperResult.NotEnoughGold => messages.GetMessage("merchant_not_enough_gold"),
                _ => messages.GetMessage("invalid")
            };
            Console.WriteLine(message);
        }
    }

    // ---------- Merchant ----------

    // Menu numbers stay fixed so they don't shift when an owned item is left off the list.
    // They're formatted into the translated menu lines, so this is the only place they live.
    private static readonly Dictionary<string, MerchantItem> MerchantMenu = new()
    {
        ["1"] = MerchantItem.MagicalArmor,
        ["2"] = MerchantItem.Enchantment,
    };
    private const string MerchantLeaveChoice = "3";

    private void HandleMerchantEncounter(Player player, Messages messages)
    {
        Merchant merchant = new Merchant();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(messages.GetMessage("merchant_intro"));
        Console.ResetColor();

        while (true)
        {
            var available = merchant.GetAvailableItems(player).ToList();

            Console.WriteLine(string.Format(messages.GetMessage("merchant_gold"), player.Gold));
            Console.WriteLine(messages.GetMessage("merchant_menu_header"));

            foreach (var (number, menuItem) in MerchantMenu)
            {
                if (available.Contains(menuItem))
                    Console.WriteLine(GetMerchantOptionText(number, menuItem, player, messages));
            }

            Console.WriteLine(string.Format(messages.GetMessage("merchant_option_leave"), MerchantLeaveChoice));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim();

            // null means the input stream ended; treat it as leaving instead of looping forever.
            if (input == null || input == MerchantLeaveChoice)
            {
                Console.WriteLine(messages.GetMessage("merchant_leave"));
                return;
            }

            if (!MerchantMenu.TryGetValue(input, out MerchantItem item))
            {
                Console.WriteLine(messages.GetMessage("invalid"));
                continue;
            }

            switch (merchant.Purchase(player, item))
            {
                case PurchaseResult.Purchased:
                    Console.WriteLine(messages.GetMessage("merchant_thanks"));
                    return;
                case PurchaseResult.NotEnoughGold:
                    Console.WriteLine(messages.GetMessage("merchant_not_enough_gold"));
                    break;
                case PurchaseResult.AlreadyOwned:
                    Console.WriteLine(messages.GetMessage("merchant_already_owned"));
                    break;
                case PurchaseResult.NotAvailable:
                    Console.WriteLine(messages.GetMessage("invalid"));
                    break;
            }
        }
    }

    private static string GetMerchantOptionText(string number, MerchantItem item, Player player, Messages messages)
    {
        return item switch
        {
            MerchantItem.MagicalArmor => string.Format(messages.GetMessage("merchant_option_armor"),
                number, Merchant.MagicalArmorHealthBonus, Merchant.ItemPrice),
            MerchantItem.Enchantment when player.Weapon is { } weapon => string.Format(messages.GetMessage("merchant_option_enchant"),
                number, messages.GetWeaponDisplayName(weapon), Merchant.EnchantmentDamageBonus, Merchant.ItemPrice),
            _ => throw new ArgumentOutOfRangeException(nameof(item), item, "Unknown merchant item.")
        };
    }

    private void HandleDragonEncounter(Player player, Dragon dragon, Messages messages)
    {
        Console.WriteLine(messages.GetMessage("north_path_narrative"));

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(string.Format(messages.GetMessage("dragon_intro"), dragon.Name));
        Console.ResetColor();

        Console.WriteLine(messages.GetMessage("dragon_stats_intro"));
        dragon.DisplayStats(messages);
        while (true)
        {
            Console.WriteLine(messages.GetMessage("dragon_encounter_prompt"));
            Console.Write(messages.GetMessage("enter_choice"));
            string? input = Console.ReadLine()?.Trim().ToLower();

            if (input == "a" || input == "attack")
            {
                Combat combat = new Combat(player, dragon, messages);
                bool playerWon = combat.StartCombat();

                if (combat.PlayerRetreated)
                {
                    return; //combat already handled retreat message
                }
                else
                {
                   EndGame(playerWon, false, messages); 
                }
                
                return;
            }

        if (input == "r" || input == "retreat")
        {
            Console.WriteLine(messages.GetMessage("retreat"));
            return;
        }

        Console.WriteLine(messages.GetMessage("invalid"));
    }
}
}
