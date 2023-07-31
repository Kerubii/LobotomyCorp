using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Cocoon : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cocoon");
			// Description.SetDefault("Reduced defense and 12% decreased damage");
			Main.debuff[Type] = true;
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.GetGlobalNPC<LobotomyGlobalNPC>().RedEyesCocoon = true;
			npc.defense -= 8;
            base.Update(npc, ref buffIndex);
        }
    }
}