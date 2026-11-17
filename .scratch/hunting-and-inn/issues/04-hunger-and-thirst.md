# 04: Hunger and Thirst

**What to build:** The player gains `Hunger` and `Thirst` (0–10, start 0), shown in stats as n/10, with "Starving"/"Parched" shown at 10. Every hunt, caught or not: first lose 2 health for each of Hunger and Thirst already at 10, then raise both by 2 (capped at 10). The Innkeeper menu gains `3` Cook my Meat (free, uses 1 Meat, Hunger → 0; *NoMeat* if none), `4` Buy a meal (3 gold, Hunger → 0), `5` Buy a drink (1 gold, Thirst → 0); meal and drink return *NotEnoughGold* when unaffordable and change nothing. Hunger damage can trigger the out-of-combat defeat from ticket 02.

See `.scratch/hunting-and-inn/spec.md`.

**Blocked by:** 02, 03

**Status:** done

- [x] New player has 0 Hunger and 0 Thirst; stats show both
- [x] A hunt raises Hunger and Thirst by 2, capped at 10
- [x] A hunt with Hunger at 10 costs 2 health; with both at 10 costs 4
- [x] Cooking Meat uses 1 Meat and sets Hunger to 0; with no Meat returns *NoMeat*
- [x] Meal (3 gold) and drink (1 gold) reset their stat and deduct gold; unaffordable returns *NotEnoughGold* and changes nothing
- [x] Starving/Parched messages show when a stat is at 10
- [x] New message keys exist in all four languages (automated test)
- [x] All existing and new tests pass

## Comments

The Hunger/Thirst cost is part of `Hunting.Hunt`. The damage is taken before the hunt raises the stats, so reaching 10 doesn't hurt, but hunting while already at 10 does. Stats and combat stats show "n/10", with "(Starving)"/"(Parched)" added at 10.
