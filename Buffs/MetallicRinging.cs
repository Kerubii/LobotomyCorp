using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class MetallicRinging : ModBuff
	{
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Metalic Ringing");
			// Description.SetDefault("My head... turning into metal...");
            Main.debuff[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.ichor = true;
            npc.GetGlobalNPC<LobotomyGlobalNPC>().RegretMetallicRinging = true;
        }
    }
}