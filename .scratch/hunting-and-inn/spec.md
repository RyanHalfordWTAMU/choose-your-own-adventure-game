# Spec: Hunting and the Inn

**Status:** ready-for-agent

## Problem Statement

The player starts with 10 gold and has no way to earn more, so they can buy exactly one item from the Merchant and that is the whole economy. Nothing in the world besides the Merchant and the Dragon gives the player something to do before the final fight, and the "woods" to the south exist only as a backdrop for the Merchant.

## Solution

The world gains two places reached from the **Crossroads**:

- **South → the Forest**, where the player can **Hunt** animals for **Meat** and **Hides**.
- **West → the Inn**, where the **Innkeeper** buys Meat and Hides for **Gold** and sells food and drink, and where the **Merchant** now trades (moved from the woods).

Hunting makes the player hungrier and thirstier. **Hunger** and **Thirst** run from 0 to 10; once either is at its maximum, further hunts cost health, and a player whose health reaches 0 outside combat is defeated. Eating (the player's own Meat, cooked free by the Innkeeper, or a bought meal) and drinking reset them.

North still leads to the Dragon, and the Dragon fight still ends the game. All new dialogue is available in English, Spanish, French and Italian.

## User Stories

1. As a player, I want the Crossroads to offer north, south, west and exit, so that I can reach every place in the world.
2. As a player, I want west to take me to the Inn, so that I have somewhere to trade.
3. As a player, I want to meet the Merchant inside the Inn, so that I can still buy Magical Armor and Enchantments.
4. As a player, I want the Inn to offer a menu of the Innkeeper, the Merchant, and leaving, so that I choose who to deal with.
5. As a player, I want south to take me to the Forest, so that I can hunt.
6. As a player, I want the Forest to offer Hunt or Leave, and to hunt as many times as I like before leaving.
7. As a player, I want a hunt to depend on my Agility, so that my character's stats matter outside the Dragon fight.
8. As a player, I want a successful hunt to roll which animal I caught, so that each hunt feels different.
9. As a player, I want a Rabbit to give 1 Meat, a Deer 2 Meat and 1 Hide, and a Boar 3 Meat and 1 Hide.
10. As a player, I want a Boar to gore me for 1–4 health, so that the best catch carries a risk.
11. As a player, I want a failed hunt to tell me the animal got away, so that I understand why I got nothing.
12. As a player, I want every hunt, successful or not, to raise my Hunger and Thirst by 2 (capped at 10).
13. As a player, I want a hunt to cost 2 health for each of Hunger or Thirst already at 10, so that hunting while starving is dangerous.
14. As a player, I want to be told when I am Starving or Parched, so that I know to visit the Inn.
15. As a player, I want to be defeated if my health reaches 0 outside a fight, with a message explaining why, so that ignoring Hunger and Thirst has a consequence.
16. As a player, I want to sell Meat for 2 gold each and Hides for 4 gold each to the Innkeeper.
17. As a player, I want selling to sell all my Meat or all my Hides at once, so that trading takes few steps.
18. As a player, I want the Innkeeper to cook one of my Meat for free and set my Hunger to 0, so that I choose between gold and food.
19. As a player, I want to buy a meal for 3 gold that sets my Hunger to 0, so that I can eat without Meat.
20. As a player, I want to buy a drink for 1 gold that sets my Thirst to 0.
21. As a player, I want to be told when I have nothing to sell, no Meat to cook, or not enough gold, and see the menu again.
22. As a player, I want my character stats to show Meat, Hides, Hunger and Thirst.
23. As a Spanish-, French- or Italian-speaking player, I want all new dialogue in my language.
24. As a player, I want the old woods/merchant narrative and the two-path crossroads text replaced, so that the story matches the new map.
25. As a developer, I want hunting and Innkeeper trade rules separated from console input and output, so that they can be tested automatically with scripted die rolls.

## Implementation Decisions

- **Crossroads.** The path prompt accepts `n`/`north` (Dragon), `s`/`south` (Forest), `w`/`west` (Inn), `e`/`exit`. The prompt text and invalid-path message are rewritten in all four languages.
- **Merchant moves to the Inn.** The Merchant module and its rules are unchanged; only where the encounter is reached changes, and its introduction is rewritten to fit the Inn. This supersedes the merchant spec's "no earning gold" and "merchant on the south path" points.
- **Inn menu:** `1` Innkeeper, `2` Merchant, `3` Leave. Leaving returns to the Crossroads.
- **New Hunting module** owns the hunt rules and does no console I/O. It takes a Die. A hunt returns a result describing: whether the animal was caught, which animal (Rabbit, Deer, Boar — a closed enumeration), Meat and Hides gained, and health lost (from the Boar and from Starving/Parched).
  - Tracking roll: d20; caught if the roll ≤ the player's Agility.
  - Animal roll (only on a catch): d6 — 1–3 Rabbit, 4–5 Deer, 6 Boar.
  - Boar: an additional d4 health lost.
  - Hunger/Thirst penalty: before raising them, each of Hunger and Thirst already at 10 costs 2 health. Then both rise by 2, capped at 10.
  - Health never goes below 0.
- **Testability of dice.** `Die.Roll` becomes `virtual` so tests can supply a scripted die (prior art: Dragon already takes a Die).
- **New Innkeeper module** owns the Inn's trade rules, no console I/O. Operations and results:
  - Sell all Meat (2 gold each) / sell all Hides (4 gold each) → *Sold* (with gold earned) or *NothingToSell*.
  - Cook Meat: costs 1 Meat, sets Hunger to 0 → *Eaten* or *NoMeat*.
  - Buy meal: 3 gold, sets Hunger to 0 → *Eaten* or *NotEnoughGold*.
  - Buy drink: 1 gold, sets Thirst to 0 → *Drank* or *NotEnoughGold*.
  - Innkeeper menu: `1` Sell Meat, `2` Sell Hides, `3` Cook my Meat, `4` Buy a meal, `5` Buy a drink, `6` Leave. Menu numbers are fixed. Failures show a message and redisplay the menu; successes show a message and redisplay the menu (unlike the Merchant, so the player can sell and then eat in one visit).
- **Player gains** `Meat`, `Hides`, `Hunger`, `Thirst` (all start at 0). No general inventory.
- **Starving/Parched** are display states when Hunger/Thirst is 10, not separate stats.
- **Defeat outside combat.** If a hunt leaves health at 0, the Forest shows a new "you collapse in the forest" defeat line and the game ends, the same way a Dragon defeat ends it.
- **Stats display.** Character stats and the shared equipment/stat lines gain Meat, Hides, Hunger (n/10) and Thirst (n/10).
- **Localization.** Every new player-facing string is a message key present in all four language sections.

## Testing Decisions

- **Good tests** assert on returned results and on the player's state afterwards (gold, health, Meat, Hides, Hunger, Thirst), not on internals or console output.
- **Seams:** Hunting (with a scripted Die), Innkeeper, Player starting state, Messages completeness.
- **Required cases:**
  - New player: 0 Meat, 0 Hides, 0 Hunger, 0 Thirst.
  - Tracking roll above Agility: not caught, no goods, Hunger and Thirst +2.
  - Each animal roll (3, 5, 6) yields the right animal and goods; Boar removes the d4 roll from health.
  - Hunger/Thirst cap at 10; a hunt with Hunger at 10 costs 2 health; both at 10 costs 4.
  - Health never drops below 0 from hunting.
  - Selling Meat/Hides earns the right gold and zeroes the count; selling with none returns *NothingToSell* and changes nothing.
  - Cook with Meat consumes 1 and zeroes Hunger; with none returns *NoMeat*.
  - Meal and drink deduct gold and reset the stat; without enough gold return *NotEnoughGold* and change nothing.
  - Every new message key exists in all four languages.
- **Not automated:** the Crossroads, Forest and Inn console menus. Verify manually: south → hunt several times → west → sell → buy from Merchant → north.

## Out of Scope

- Hunting fights using the Combat system, or animals with stats.
- Dragon loot or other gold sources.
- Selling to the Merchant or buying from the Innkeeper anything other than meals and drinks.
- Resting/renting rooms or healing.
- Hunger/Thirst rising from travel or affecting combat.
- Selling partial stacks.
- Saving the game.
