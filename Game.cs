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
                        HandleMerchantEncounter(player, messages);
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

            // null means the input stream ended, so there's nothing left to ask.
            if (input == null || input == "exit" || input == "e")
                return "exit";

            Console.WriteLine(messages.GetMessage("path_invalid"));
        }
    }

    // Menu numbers stay fixed so they don't shift when an owned item is left off the list.
    // They're formatted into the translated menu lines, so this is the only place they live.
    private static readonly Dictionary<string, MerchantItem> MerchantMenu = new()
    {
        ["1"] = MerchantItem.MagicalArmor,
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
            }
        }
    }

    private static string GetMerchantOptionText(string number, MerchantItem item, Player player, Messages messages)
    {
        return item switch
        {
            MerchantItem.MagicalArmor => string.Format(messages.GetMessage("merchant_option_armor"),
                number, Merchant.MagicalArmorHealthBonus, Merchant.ItemPrice),
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
