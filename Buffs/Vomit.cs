using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Vomit : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vomit");
			Description.SetDefault("Decreased life regeneration");
            Main.debuff[Type] = true;
            //Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).SmileDebuff = true;
        }
    }
}