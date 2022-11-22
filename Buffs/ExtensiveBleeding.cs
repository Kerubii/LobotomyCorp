using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class ExtensiveBleeding : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Extensive Bleeding");
			Description.SetDefault("Using a weapon hurts you");
		}
		
		public override void Update(NPC npc, ref int BuffIndex)
		{ 
			LobotomyGlobalNPC.LNPC(npc).SanguineDesireExtensiveBleeding = true;
			float bleed = LobotomyGlobalNPC.LNPC(npc).SanguineDesireExtensiveBleedAmount;

			if (npc.buffTime[BuffIndex] <= 2 && bleed > 1f)
			{
				npc.buffTime[BuffIndex] = 120;
				LobotomyGlobalNPC.LNPC(npc).SanguineDesireExtensiveBleedAmount = bleed/3f;
				if (bleed < 1f)
				{
					npc.DelBuff(BuffIndex);
					BuffIndex--;
				}
			}
		}

        public override void Update(Player player, ref int buffIndex)
        {
			if (player.itemAnimation == player.itemAnimationMax)
            {

            }
        }
    }
}