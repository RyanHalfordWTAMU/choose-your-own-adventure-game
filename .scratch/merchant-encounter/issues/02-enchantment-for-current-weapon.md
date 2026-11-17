# 02: Enchantment for the current weapon

**What to build:** The Merchant also offers an **Enchantment** as menu option `2` ("Enchant your {weapon}", +4 max damage, 10 gold). Buying it deducts 10 gold, raises the player's current weapon's maximum damage by 4, and marks the weapon enchanted, making it an **Enchanted Weapon**. The weapon keeps its type (a dagger stays a dagger) and its existing translated name, and is displayed through a per-language format such as "Enchanted {weapon}". Character stats and pre-combat stats show the Enchanted Weapon's name and boosted max damage. The boosted maximum flows through the existing damage roll, so the player hits the dragon harder. Once the player owns the Enchantment it is no longer listed on later trips south, and buying it again returns *AlreadyOwned*. Because each item costs the full 10 starting gold, a player who bought Magical Armor gets *NotEnoughGold* for the Enchantment, and the other way round.

Reuse the Merchant, purchase results, Gold and menu from ticket 01. See `.scratch/merchant-encounter/spec.md` and the root `GLOSSARY.md`.

**Blocked by:** 01 (Merchant encounter with Magical Armor)

**Status:** done

- [x] The merchant menu lists `1` Magical Armor, `2` Enchant your weapon, `3` Leave, omitting whatever the player already owns
- [x] Buying the Enchantment with 10 gold returns *Purchased*, sets gold to 0, raises weapon max damage by 4 (e.g. dagger 6 → 10), and marks the weapon enchanted
- [x] Buying the Enchantment without enough gold returns *NotEnoughGold* and leaves gold and weapon unchanged
- [x] Buying the Enchantment when already owned returns *AlreadyOwned* and changes nothing
- [x] Buying one item and then trying the other returns *NotEnoughGold*
- [x] An Enchanted Weapon's damage rolls stay between 1 and the new maximum and can exceed the old maximum (tested over many rolls)
- [x] Character stats and pre-combat stats show the enchanted weapon name and the boosted max damage
- [x] The enchanted-name format and enchantment menu and messages exist in all four languages (covered by an automated test)
- [x] All existing and new tests pass

## Comments

Implemented: `MerchantItem.Enchantment` (+4 max damage), `Weapon.IsEnchanted`, `Messages.GetWeaponDisplayName` (per-language `enchanted_weapon_format`), menu option 2 in `Game`. Spanish/French/Italian use gender-neutral phrasing ("{0} con encantamiento", "{0} avec enchantement", "{0} con incantesimo") because an adjective would have to agree with each weapon's gender. Manually played: buy enchantment -> armor shows not-enough-gold -> north, dealing 10 damage with the enchanted dagger.
