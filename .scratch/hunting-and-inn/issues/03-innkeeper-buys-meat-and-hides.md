# 03: The Innkeeper buys Meat and Hides

**What to build:** A new Innkeeper module (no console I/O) lets the player sell all their Meat at 2 gold each and all their Hides at 4 gold each, returning *Sold* (with gold earned) or *NothingToSell*. The Inn's Innkeeper option shows a menu that repeats after each action until the player leaves: `1` Sell Meat, `2` Sell Hides, `6` Leave (options 3–5 arrive in ticket 04; numbers are fixed). This is the first way to earn **Gold**, so a player can now hunt, sell, and afford both Merchant items.

See `.scratch/hunting-and-inn/spec.md`.

**Blocked by:** 01, 02

**Status:** done

- [x] Selling 3 Meat earns 6 gold and leaves 0 Meat; selling 2 Hides earns 8 gold and leaves 0 Hides
- [x] Selling with none returns *NothingToSell* and changes nothing
- [x] Innkeeper menu repeats after each action and shows current gold; Leave returns to the Inn menu
- [x] New message keys exist in all four languages (automated test)
- [x] All existing and new tests pass

## Comments

Implemented in `Innkeeper.cs` and `Game.HandleInnkeeper`. Selling reports the gold earned through an `out` parameter. Manually played: sold 4 Meat for 8 gold, then bought Magical Armor with hunting money.
