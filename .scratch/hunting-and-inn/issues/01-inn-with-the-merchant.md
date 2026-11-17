# 01: The Inn, west of the Crossroads, with the Merchant

**What to build:** The Crossroads gains a `w`/`west` choice leading to the **Inn**. The Inn shows a short introduction and a menu: `1` Innkeeper, `2` Merchant, `3` Leave. Choosing the Merchant runs the existing merchant encounter (rules unchanged) with a new Inn-appropriate introduction; leaving the Merchant returns to the Inn menu. The Innkeeper option exists but only says the Innkeeper has nothing to trade yet (filled in by ticket 03). Leaving the Inn returns to the Crossroads. South still leads to the Merchant until ticket 02 replaces it. Rewrite the Crossroads prompt and invalid-path text for the new choice.

See `.scratch/hunting-and-inn/spec.md` and the root `GLOSSARY.md`.

**Blocked by:** none

**Status:** done

- [x] Crossroads accepts `w`/`west` and reaches the Inn; prompt and invalid-path text list n/s/w/e
- [x] Inn menu offers Innkeeper / Merchant / Leave; invalid input shows "invalid" and redisplays
- [x] The Merchant at the Inn sells Magical Armor and Enchantments exactly as before, with a rewritten introduction
- [x] Leaving the Merchant returns to the Inn menu; leaving the Inn returns to the Crossroads
- [x] New and changed message keys exist in all four languages (automated test)
- [x] All existing and new tests pass

## Comments

Implemented together with tickets 02–04, so the interim "south still leads to the Merchant" step was skipped. `Game.HandleInn` shows the Inn menu; leaving the Merchant returns to it. The Merchant's intro and leave lines were rewritten for the Inn in all four languages. The prompt uses `w` for West in every language, matching how `n`/`s` already work.
