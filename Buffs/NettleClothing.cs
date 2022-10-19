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

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            if (modPlayer.BlackSwanNettleClothing < 6)
            {
                modPlayer.BlackSwanNettleAdd(0.00333f);
            }
            else
                modPlayer.BlackSwanNettleClothing = 6;

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