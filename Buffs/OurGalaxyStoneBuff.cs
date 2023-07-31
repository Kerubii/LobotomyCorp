using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class OurGalaxyStoneBuff : ModBuff
	{
		public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("Token of Friendship");
			// Description.SetDefault("Let's walk the galaxy together");
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).OurGalaxyStone = true;
            player.buffTime[buffIndex] = 5;

            if (LobotomyModPlayer.ModPlayer(player).OurGalaxyOwner < 0)
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