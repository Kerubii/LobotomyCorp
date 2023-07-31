using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Glitter : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Glitter");
			// Description.SetDefault("Red Shoes");
		}
		
		public override void Update(NPC npc, ref int BuffIndex)
		{
			LobotomyGlobalNPC.LNPC(npc).SanguineDesireGlitter = true;
		}
    }
}