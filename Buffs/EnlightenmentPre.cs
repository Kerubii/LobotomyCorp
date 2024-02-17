using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Audio;

namespace LobotomyCorp.Buffs
{
	public class EnlightenmentPre : ModBuff
	{
		public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("Gooey Waste");
			// Description.SetDefault("Reduced movement speed and Defense");
            Main.debuff[Type] = true;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] == 1)
            {
                LobotomyGlobalNPC.ApplyEnlightenment(npc);
            }
        }
    }
}