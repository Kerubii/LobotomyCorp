using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Hopeless : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hopeless");
			Description.SetDefault("Cannot attack while moving, cannot move while attacking");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).Hopeless = true;
        }
    }
}