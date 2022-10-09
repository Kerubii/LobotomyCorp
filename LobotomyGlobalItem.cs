using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp
{
	public class LobotomyGlobalItem : GlobalItem
    {
        public static LobotomyGlobalItem LobItem(Item item)
        {
            return item.GetGlobalItem<LobotomyGlobalItem>();
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            return base.Clone(item, itemClone);
        }

        public bool CustomDraw = false;
        public Texture2D CustomTexture = null;

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {
                if (arg == ItemID.QueenBeeBossBag && Main.rand.Next(2) == 0)
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ModContent.ItemType<Items.Hornet>());
                else if (arg == ItemID.WallOfFleshBossBag && Main.rand.Next(4) == 0)
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ModContent.ItemType < Items.Censored>());
            }
            else if (context == "present")
            {
                if (Main.rand.Next(100) == 0)
                    player.QuickSpawnItem(player.GetSource_OpenItem(arg), ModContent.ItemType < Items.Christmas>());
            }
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            if (modPlayer.TodaysExpressionActive)
                damage = (int)(damage * modPlayer.TodaysExpressionDamage());
        }

        public override void ModifyHitPvp(Item item, Player player, Player target, ref int damage, ref bool crit)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            if (modPlayer.TodaysExpressionActive)
                damage = (int)(damage * modPlayer.TodaysExpressionDamage());
        }
    }
}