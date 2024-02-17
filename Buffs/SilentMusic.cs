using System;
using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class SilentMusic : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Qlipoth Disruption");
			// Description.SetDefault("Movement speed is reduced");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).DaCapoSilentMusic = true;
            int phase = LobotomyModPlayer.ModPlayer(player).DaCapoSilentMusicPhase;
            player.statDefense -= 10;
            if (phase % 4 > 0)
            {
                player.moveSpeed += .1f;
            }
            if (phase % 4 > 1)
            {
                player.GetAttackSpeed(DamageClass.Melee) += .12f;
                player.GetDamage(DamageClass.Generic) += .12f;
            }
            if (phase % 4 > 2)
            {
                player.endurance += .15f;
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<LobotomyGlobalNPC>().DaCapoSilentMusic = true;
            int phase = npc.GetGlobalNPC<LobotomyGlobalNPC>().DaCapoSilentMusicPhase;
        }
    }
}