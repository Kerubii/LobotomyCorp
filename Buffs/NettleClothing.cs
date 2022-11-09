using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class NettleClothing : ModBuff
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Clothing of Nettle");
			Description.SetDefault("Nettle regenerating, 75% reduced damage and Attackers also take damage");
            Main.buffNoTimeDisplay[Type] = true;
        }

        /*
        Holding BlackSwan gains following effects
        1 - 15% increased attack speed
        2 - 20% increased movement speed
        3 - 20% increased reflection damage
        4 - 100% increased true melee damage
        5 - Attacks and reflected projectiles inflict Ichor and Gooey Waste
        6 - Reduces 100% damage taken and extended invulnerability
        */
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            float NettleCount = LobotomyModPlayer.ModPlayer(Main.LocalPlayer).BlackSwanNettleClothing;
            if (NettleCount < 1)
                tip += "\nAfter gaining a nettle, Black Swan gains various effects";
            else
                tip += "\nBlack Swan gains the following effects:\n15% increased attack speed";
            if (NettleCount >= 2)
                tip += "\n20% increased movement speed";
            if (NettleCount >= 3)
                tip += "\n20% increased reflection damage";
            if (NettleCount >= 4)
                tip += "\n100% increased true melee damage and enhances melee range";
            if (NettleCount >= 5)
                tip += "\nAttacks and reflected projectiles inflict Ichor and Gooey Waste";
            if (NettleCount >= 6)
                tip += "\nNext attack is nullified and gain temporary invincibility";

            base.ModifyBuffTip(ref tip, ref rare);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            if (modPlayer.BlackSwanNettleClothing < 6)
            {
                modPlayer.BlackSwanNettleAdd(0.00333f);
            }
            else
                modPlayer.BlackSwanNettleClothing = 6;

            if ((int)Math.Floor(modPlayer.BlackSwanNettleClothing) >= 2 && player.HeldItem.type == ModContent.ItemType<Items.Ruina.Literature.BlackSwanR>())
                player.moveSpeed += 0.2f;

            player.buffTime[buffIndex] = 10;
            if (modPlayer.BlackSwanBrokenDream)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}