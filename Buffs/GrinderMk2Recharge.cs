using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class GrinderMk2Recharge : ModBuff
	{
		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Recharging");
			Description.SetDefault("Power recharging");
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //LobotomyModPlayer.ModPlayer(player).GrinderMk2Recharging = true;
            if (player.buffTime[buffIndex] == 1)
            {
                SoundEngine.PlaySound(new SoundStyle("LobotomyCorp/Sounds/Item/Helper_FullCharge"), player.Center);
            }
        }
    }
}