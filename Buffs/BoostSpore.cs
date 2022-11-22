using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class BoostSpore : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Increase Aggression");
			Description.SetDefault("They live for the only one queen");
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 14;
        }

		public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}