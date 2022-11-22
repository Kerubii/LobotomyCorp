using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Cocoon : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cocoon");
			Description.SetDefault("Speed reduced, periodically targetted by Spiderlings");
			Main.debuff[Type] = true;
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.GetGlobalNPC<LobotomyGlobalNPC>().RedEyesCocoon = true;
            base.Update(npc, ref buffIndex);
        }
    }
}