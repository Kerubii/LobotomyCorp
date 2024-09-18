using LobotomyCorp.Projectiles.Realized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LobotomyCorp.ModSystems
{
    class ModIntegrationSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            // Most often, mods require you to use the PostSetupContent hook to call their methods. This guarantees various data is initialized and set up properly

            // Boss Checklist shows comprehensive information about bosses in its own UI. We can customize it:
            // https://forums.terraria.org/index.php?threads/.50668/
            DoBossChecklistIntegration();

            // We can integrate with other mods here by following the same pattern. Some modders may prefer a ModSystem for each mod they integrate with, or some other design.
        }

        private void DoBossChecklistIntegration()
        {
            // The mods homepage links to its own wiki where the calls are explained: https://github.com/JavidPack/BossChecklist/wiki/%5B1.4.4%5D-Boss-Log-Entry-Mod-Call
            // If we navigate the wiki, we can find the "LogBoss" method, which we want in this case
            // A feature of the call is that it will create an entry in the localization file of the specified NPC type for its spawn info, so make sure to visit the localization file after your mod runs once to edit it

            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklistMod))
            {
                return;
            }

            // For some messages, mods might not have them at release, so we need to verify when the last iteration of the method variation was first added to the mod, in this case 1.6
            // Usually mods either provide that information themselves in some way, or it's found on the GitHub through commit history/blame
            if (bossChecklistMod.Version < new Version(1, 6))
            {
                return;
            }

            // The "LogBoss" method requires many parameters, defined separately below:

            // Your entry key can be used by other developers to submit mod-collaborative data to your entry. It should not be changed once defined
            string internalName = "RedMist";

            // Value inferred from boss progression, see the wiki for details
            float weight = 7.681f;

            // Used for tracking checklist progress
            Func<bool> downed = () => LobEventFlags.downedRedMist;

            // The NPC type of the boss
            int bossType = ModContent.NPCType<NPCs.RedMist.RedMist>();

            // The item used to summon the boss with (if available)
            int spawnItem = ModContent.ItemType<Items.ItemTiles.DisciplinaryShell>();

            // "collectibles" like relic, trophy, mask, pet
            /*List<int> collectibles = new List<int>()
            {
                ModContent.ItemType<Content.Items.Placeable.Furniture.MinionBossRelic>(),
                ModContent.ItemType<Content.Pets.MinionBossPet.MinionBossPetItem>(),
                ModContent.ItemType<Content.Items.Placeable.Furniture.MinionBossTrophy>(),
                ModContent.ItemType<Content.Items.Armor.Vanity.MinionBossMask>()
            };*/

            // By default, it draws the first frame of the boss, omit if you don't need custom drawing
            // But we want to draw the bestiary texture instead, so we create the code for that to draw centered on the intended location
            var customPortrait = (SpriteBatch sb, Rectangle rect, Color color) => {
                Texture2D texture = ModContent.Request<Texture2D>("LobotomyCorp/NPCs/RedMist/RedMist_Preview").Value;
                Vector2 centered = new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));

                Texture2D weapon = ModContent.Request<Texture2D>("LobotomyCorp/Items/Aleph/Twilight").Value;
                sb.Draw(weapon, centered + new Vector2(0, -texture.Height / 2), weapon.Frame(), color, -0.875f, new Vector2(0, weapon.Height), 1f, 0, 0);

                weapon = ModContent.Request<Texture2D>("LobotomyCorp/Items/Aleph/Justitia").Value;
                sb.Draw(weapon, centered + new Vector2(texture.Width / 2, 0), weapon.Frame(), color, 0, new Vector2(0, weapon.Height), 1f, 0, 0);

                weapon = ModContent.Request<Texture2D>("LobotomyCorp/Items/Aleph/Smile").Value;
                sb.Draw(weapon, centered + new Vector2(-texture.Width / 2, 0), weapon.Frame(), color, -1.57f, new Vector2(0, weapon.Height), 1f, 0, 0);

                weapon = ModContent.Request<Texture2D>("LobotomyCorp/Items/Zayin/Penitence").Value;
                sb.Draw(weapon, centered + new Vector2(texture.Width / 2, texture.Height / 2), weapon.Frame(), color, 1.57f, new Vector2(0, weapon.Height), 1f, 0, 0);

                weapon = ModContent.Request<Texture2D>("LobotomyCorp/Items/Teth/RedEyes").Value;
                sb.Draw(weapon, centered + new Vector2(-texture.Width / 2, texture.Height / 2), weapon.Frame(), color, 3.14f, new Vector2(0, weapon.Height), 1f, 0, 0);

                centered -= new Vector2(texture.Width / 2, texture.Height / 2);
                sb.Draw(texture, centered, color);
            };

            bossChecklistMod.Call(
                "LogBoss",
                Mod,
                internalName,
                weight,
                downed,
                bossType,
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.LobotomyCorp.BossChecklistIntegration.RedMist.EntryName"),
                    ["spawnInfo"] = Language.GetText("Mods.LobotomyCorp.BossChecklistIntegration.RedMist.SpawnInfo"),
                    ["spawnItems"] = spawnItem,
                    //["collectibles"] = collectibles,
                    ["customPortrait"] = customPortrait,
                    // Other optional arguments as needed are inferred from the wiki
                }
            );

            // Other bosses or additional Mod.Call can be made here.
        }
    }
}
