using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Gluttony : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Predation");
			// Description.SetDefault("I need something fresh...");
			Main.debuff[Type] = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).WingbeatGluttony = true;
        }
    }
}