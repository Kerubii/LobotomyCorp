using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Matchstick : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Matchstick");
			Description.SetDefault("Reduce to ashes...");
            Main.debuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
		
		public override void Update(NPC npc, ref int BuffIndex)
		{
            LobotomyGlobalNPC.LNPC(npc).MatchstickBurnTime = (int)Math.Ceiling(npc.buffTime[BuffIndex] / 60f);
            if (npc.buffTime[BuffIndex] > 6000)
                npc.buffTime[BuffIndex] = 6000;

            LobotomyGlobalNPC.LNPC(npc).MatchstickBurn = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).MatchstickBurn = true;
            LobotomyModPlayer.ModPlayer(player).MatchstickBurnTime = (int)Math.Ceiling(player.buffTime[buffIndex] / 60f);
        }
    }
}