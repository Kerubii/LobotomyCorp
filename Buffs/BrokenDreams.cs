using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class BrokenDreams : ModBuff
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Broken Dreams");
			Description.SetDefault("30% increased damage taken");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).BlackSwanBrokenDream = true;
            LobotomyModPlayer.ModPlayer(player).BlackSwanNettleClothing = 0;
            player.endurance -= 0.3f;
        }
    }
}