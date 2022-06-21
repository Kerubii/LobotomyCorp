using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class BeeSpore : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
			Description.SetDefault("Your neck feels itchy");
			Main.debuff[Type] = true;
		}
		
		public override void Update(NPC npc, ref int BuffIndex)
		{
			LobotomyGlobalNPC.LNPC(npc).QueenBeeSpore = true;
		}

		/* Inert
        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).WingbeatFairyMeal = true;
            player.statDefense -= 5;
        }*/
    }
}