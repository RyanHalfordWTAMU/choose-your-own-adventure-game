# Spec: Merchant Encounter

**Status:** ready-for-agent

## Problem Statement

The south path is a dead end. When the player chooses south, the game prints a single line saying nothing happens and that the player returns home "poor and still bored", then drops them back at the path prompt. The choice has no consequence, the narrative contradicts what the game actually does (the player doesn't go home), and nothing the player does before facing the dragon can improve their odds. The player has no gold and no way to acquire better equipment.

## Solution

When the player travels south, they meet a **Merchant** who offers two magical items for **Gold**:

- **Magical Armor**, which grants bonus health points.
- An **Enchantment** for the player's current weapon, which raises its maximum damage, turning it into an **Enchanted Weapon**.

Every player starts with 10 gold and each item costs 10 gold, so the player must choose one. The player can also leave without buying. After the encounter the player returns to the path prompt and may go north to fight the dragon with whatever they bought. The merchant appears on every trip south, offering only the items the player doesn't already own. All new dialogue is available in English, Spanish, French and Italian.

## User Stories

1. As a player, I want to meet a merchant when I take the south path, so that the south path is meaningful instead of a dead end.
2. As a player, I want the merchant to be introduced with a short piece of narrative, so that the encounter feels like part of the story.
3. As a player, I want to start my adventure with 10 gold, so that I have something to spend at the merchant.
4. As a player, I want to be told how much gold I have when the merchant's menu appears, so that I know what I can afford.
5. As a player, I want the merchant to list each item with its effect and price, so that I can make an informed choice.
6. As a player, I want to buy Magical Armor for 10 gold, so that I have more health points when I fight the dragon.
7. As a player, I want Magical Armor to add 8 health points as soon as I buy it, so that the benefit is immediate and visible.
8. As a player, I want to buy an Enchantment for 10 gold, so that my weapon deals more damage to the dragon.
9. As a player, I want the Enchantment to raise my current weapon's maximum damage by 4, so that my occupation's weapon stays mine but becomes stronger.
10. As a player of any occupation, I want the Enchantment to give the same +4 bonus, so that the upgrade is fair whether I carry a dagger or a long sword.
11. As a player, I want my weapon to be shown as an "Enchanted" weapon after the Enchantment, so that I can see the upgrade took effect.
12. As a player, I want to be able to leave the merchant without buying anything, so that I am never forced to spend my gold.
13. As a player, I want to be told when I don't have enough gold for an item, so that I understand why the purchase didn't happen.
14. As a player, I want the merchant's menu to reappear after a "not enough gold" message, so that I can choose something else or leave.
15. As a player, I want my gold, health and weapon to stay unchanged when a purchase fails, so that a failed attempt costs me nothing.
16. As a player, I want the encounter to end after I buy something, so that I return to the path prompt without extra steps.
17. As a player, I want to be able to travel south again after visiting the merchant, so that I can return to the merchant.
18. As a player returning to the merchant, I want items I already own to be left off the menu, so that I can't accidentally buy the same thing twice.
19. As a player, I want to be prevented from buying the same item twice even if I enter its option, so that I never waste gold on a duplicate.
20. As a player, I want to choose merchant options by typing `1`, `2` or `3`, so that the input works like the game's other numbered menus.
21. As a player, I want an invalid menu entry to show an error and redisplay the menu, so that a typo doesn't end the encounter.
22. As a player, I want my character stats to show my gold, so that I can track my money.
23. As a player, I want my character stats to show my armor (or "None"), so that I can see whether I'm protected.
24. As a player, I want the combat stats shown before the dragon fight to include my gold, armor and Enchanted Weapon, so that I can see how my purchase affects the fight.
25. As a player, I want my Enchanted Weapon's higher maximum damage to actually apply when I hit the dragon, so that the purchase genuinely helps in combat.
26. As a player, I want Magical Armor's bonus health to carry into the dragon fight, so that I can survive more hits.
27. As a player, I want to be able to go north to the dragon after visiting the merchant, so that I can use what I bought.
28. As a Spanish-, French- or Italian-speaking player, I want all merchant dialogue, item names and messages in my chosen language, so that the experience is consistent.
29. As a player, I want the old "nothing happens, you go home bored" text to no longer appear, so that the narrative matches what actually happens.
30. As a developer, I want the purchase rules separated from console input and output, so that they can be tested automatically.
31. As a developer, I want each purchase attempt to report a clear outcome, so that the game can show the right message and tests can check the result.

## Implementation Decisions

- **New Merchant module.** A Merchant owns the trade rules: the price of each item (10 gold each), whether the player can afford it, whether the player already owns it, deducting gold and applying the item's effect. Its main operation takes the player and the chosen item and returns a purchase result: *Purchased*, *NotEnoughGold* or *AlreadyOwned*. It does no console input or output.
- **Items are a closed set of two:** Magical Armor and Enchantment. Represent them as an enumeration (or equivalent), not as free-form strings.
- **Order of checks:** *AlreadyOwned* is checked before *NotEnoughGold*, so a player who owns an item and has no gold is told they already own it.
- **Player gains Gold**, starting at 10 for every new character. Gold never goes below 0.
- **Player gains an optional Armor.** A player has no armor until they buy Magical Armor. Buying it adds 8 to the player's current health points at the moment of purchase. There is no separate maximum-health concept, and Armor does not reduce damage per hit.
- **Weapon gains an enchanted flag.** An Enchantment raises the existing weapon's maximum damage by 4 and marks it enchanted. It keeps its type (long sword stays a long sword) so occupation flavour and the existing weapon-name translations still work. The display name is a per-language format such as "Enchanted {weapon}".
- **No inventory system.** Gold, the optional Armor and the weapon's enchanted state are the only new player state.
- **Game wiring.** Choosing south runs the merchant encounter instead of printing the old south narrative. The encounter prints the merchant introduction, the player's gold and a numbered menu: `1` Magical Armor, `2` Enchant your weapon, `3` Leave. Items the player already owns are omitted from the menu. On *Purchased*, the merchant thanks the player and the encounter ends. On *NotEnoughGold* the message is shown and the menu repeats. Leaving ends the encounter. Invalid input shows the existing "invalid choice" message and the menu repeats. When the encounter ends, control returns to the path prompt.
- **Stats display.** Character stats and combat stats both gain Gold and Armor lines. The weapon line shows the enchanted display name when applicable, and the max-damage line shows the boosted value.
- **Combat changes are implicit.** The higher weapon maximum damage flows through the existing damage roll. The armor's health bonus is already part of health points. The combat algorithm itself does not change.
- **Localization.** Every new piece of player-facing text is a message key present in all four language sections of the language data: merchant introduction, gold display, menu lines, item names, purchase confirmation, not-enough-gold, leave, the stats labels for gold and armor, "None", and the enchanted-weapon name format. The old south narrative key is replaced or removed.
- **Merchant introduction (English reference text):** "Deep in the woods, a hooded merchant beside a creaking cart calls out: 'Traveler! I have wonders for those with gold to spare.'" The other languages get equivalent translations.

## Testing Decisions

- **What makes a good test:** tests assert on externally observable behaviour (the purchase result returned, and the player's gold, health, armor and weapon afterwards), not on how the Merchant is implemented internally. Tests must not depend on console input or output.
- **Seams under test:**
  1. **Merchant** (primary seam): purchase outcomes and their effects on the player.
  2. **Player:** starting state, and the Enchanted Weapon's damage roll.
  3. **Messages / language data:** completeness of new keys across all four languages.
- **Required test cases:**
  - A new player has 10 gold, no armor, and an un-enchanted weapon once one is assigned.
  - Buying Magical Armor with 10 gold: result *Purchased*, gold becomes 0, health increases by 8, player has armor.
  - Buying an Enchantment with 10 gold: result *Purchased*, gold becomes 0, weapon max damage increases by 4 (e.g. dagger 6 → 10), weapon is marked enchanted.
  - Buying either item with insufficient gold: result *NotEnoughGold*, and gold, health, armor and weapon are all unchanged.
  - Buying an item already owned: result *AlreadyOwned*, and nothing changes.
  - An Enchanted Weapon's damage rolls stay within 1 and the new maximum, and can exceed the old maximum (over many rolls).
  - Every new message key exists in English, Spanish, French and Italian.
- **Prior art:** the existing MSTest suite in the test project. Player tests build a Player directly and assert on its properties. Combat and Dragon tests exercise rules without the console. Messages tests check language loading and lookups.
- **Not automated:** the console-driven merchant menu loop in Game, consistent with the existing untested path and dragon prompts. Verify it manually by playing south → buy → north.

## Out of Scope

- A general inventory system or carrying multiple items of a kind.
- Damage-reducing armor, armor slots, or armor affecting the Agility dodge roll.
- Replacing the player's weapon with a different weapon type.
- Selling items back, haggling, discounts or variable prices.
- Earning more gold (e.g. as loot from the dragon) or rolling starting gold.
- Restoring or capping health, healing items, or a maximum-health stat.
- Multiple merchants, or merchants on other paths.
- Saving or persisting purchases between games.
- Automated tests of the console menu flow.

## Further Notes

- Glossary terms used here (**Merchant**, **Gold**, **Magical Armor**, **Enchantment**, **Enchanted Weapon**) are defined in the root glossary. Keep using them in code, messages and tickets.
- Because the prices equal the starting gold, a player can buy exactly one item per game. The numbers are deliberately simple and can be rebalanced later without changing the design.
- Today the dragon fight ends the game, so purchases only matter for that single fight.
