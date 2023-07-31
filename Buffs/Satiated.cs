using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Satiated : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Satiated");
			// Description.SetDefault("I CRAVE THIS MELODY");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).HarmonyConnected = true;
        }
    }
}