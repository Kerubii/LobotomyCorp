using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Festival : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Festival");
			// Description.SetDefault("Everything will be peaceful");
		}

        public override void Update(Player player, ref int buffIndex)
        {
			LobotomyModPlayer.ModPlayer(player).WingbeatFairyMeal = true;
        }
    }
}