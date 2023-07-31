using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Fairy : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fairy");
			// Description.SetDefault("Under the Fairy's care");
		}
		
		public override void Update(NPC npc, ref int BuffIndex)
		{
			LobotomyGlobalNPC.LNPC(npc).WingbeatFairyMeal = true;
            npc.defDefense -= 5;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).WingbeatFairyMeal = true;
            player.statDefense -= 5;
        }
    }
}