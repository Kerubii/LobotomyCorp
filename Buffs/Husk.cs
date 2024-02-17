using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Husk : ModBuff
	{
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).MimicryHusk = true;
            player.buffTime[buffIndex] += 2;
        }
    }
}