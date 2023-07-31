using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class GrinderMk52Activated : ModBuff
	{
		public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("All Around Helper");
			// Description.SetDefault("Cleaning tools active");
            //BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override bool RightClick(int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(Main.LocalPlayer).GrinderMk2Battery = 0;
            return base.RightClick(buffIndex);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.40f;
            player.dashType = -1;

            LobotomyModPlayer modPlayer = LobotomyModPlayer.ModPlayer(player);
            modPlayer.GrinderMk2Active = true;

            if (player.buffTime[buffIndex] > 0)
                player.buffTime[buffIndex] = modPlayer.GrinderMk2Battery/4;
        }
    }
}