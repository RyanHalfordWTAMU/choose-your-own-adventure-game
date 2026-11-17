# 01: Merchant encounter with Magical Armor

**What to build:** When the player takes the south path, they meet the **Merchant** instead of the old "nothing happens, you go home bored" text. Every new player starts with 10 **Gold**. The merchant introduces themselves, shows the player's gold, and offers a numbered menu with **Magical Armor** (10 gold, +8 health points) and Leave. Buying Magical Armor deducts 10 gold, adds 8 to the player's current health points, gives the player armor, and ends the encounter with a thank-you. Choosing an item the player can't afford shows a "not enough gold" message and repeats the menu. Invalid input shows the existing invalid-choice message and repeats the menu. Leaving ends the encounter without buying. After the encounter the player is back at the path prompt and can go south again (Magical Armor is no longer offered once owned) or north to fight the dragon with the extra health. Character stats and combat stats show Gold and Armor ("None" when there's no armor). All new text exists in English, Spanish, French and Italian.

The purchase rules live in a new Merchant module with no console I/O. Given the player and an item, it returns *Purchased*, *NotEnoughGold* or *AlreadyOwned* (AlreadyOwned is checked first). Items are a closed set, modelled as an enumeration. This ticket only needs Magical Armor in it, but the shape must let ticket 02 add the Enchantment. See `.scratch/merchant-encounter/spec.md` and the root `GLOSSARY.md`.

Keep the menu numbering from the spec: `1` Magical Armor, `3` Leave. Option `2` (Enchant your weapon) arrives in ticket 02. Until then it may be absent from the list and treated as invalid input.

**Blocked by:** None (can start immediately)

**Status:** ready-for-agent

- [ ] A new player has 10 gold and no armor
- [ ] Taking the south path shows the merchant introduction, the player's gold, and the menu, and no longer shows the old south narrative
- [ ] Buying Magical Armor with 10 gold returns *Purchased*, sets gold to 0, adds 8 health points, and the player has armor
- [ ] Buying Magical Armor without enough gold returns *NotEnoughGold* and leaves gold, health and armor unchanged
- [ ] Buying Magical Armor when already owned returns *AlreadyOwned* and changes nothing
- [ ] After a purchase the encounter ends and the path prompt reappears. On a later trip south, Magical Armor is not listed
- [ ] Leave ends the encounter with nothing bought. Invalid input and not-enough-gold repeat the menu
- [ ] Character stats and pre-combat stats show Gold and Armor (or "None")
- [ ] Every new message key exists in all four languages (covered by an automated test)
- [ ] Merchant purchase rules are covered by MSTest tests that don't touch the console. All existing and new tests pass
