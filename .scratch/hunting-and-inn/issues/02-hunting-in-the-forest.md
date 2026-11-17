# 02: Hunting in the Forest

**What to build:** South now leads to the **Forest** instead of the Merchant. The Forest menu is `1` Hunt, `2` Leave. A new Hunting module (no console I/O, takes a Die) resolves a hunt: d20 tracking roll ≤ Agility to catch; on a catch a d6 picks Rabbit (1–3: 1 Meat), Deer (4–5: 2 Meat, 1 Hide) or Boar (6: 3 Meat, 1 Hide, and a d4 health loss). The player gains `Meat` and `Hides` counts (start 0), shown in character stats. Make `Die.Roll` virtual so tests can script rolls. The old woods/merchant south narrative is removed. Hunger and Thirst are not part of this ticket.

See `.scratch/hunting-and-inn/spec.md`.

**Blocked by:** 01 (the Merchant must be reachable at the Inn before south stops leading to it)

**Status:** done

- [x] South reaches the Forest; the player can hunt repeatedly and then leave to the Crossroads
- [x] A tracking roll above Agility returns not-caught with no goods
- [x] Animal rolls 3, 5 and 6 yield Rabbit, Deer and Boar with the right Meat and Hides
- [x] A Boar removes the d4 roll from health; health never drops below 0
- [x] New player has 0 Meat and 0 Hides; stats show both
- [x] If a hunt leaves health at 0, a "collapsed in the forest" defeat message is shown and the game ends
- [x] New message keys exist in all four languages (automated test)
- [x] All existing and new tests pass

## Comments

Implemented in `Hunting.cs` (`Hunting`, `HuntResult`, `Animal`) and `Game.HandleForest`. `Die.Roll` is now virtual; tests use a `ScriptedDie`. A catch with no Hide (a Rabbit) uses its own message so it doesn't print "+0 Hide". Manually played: south → 6 hunts → Starving/Parched warnings → 4 health lost on the 6th hunt.
