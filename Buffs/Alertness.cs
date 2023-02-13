using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class Alertness : ModBuff
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Alerted");
            Description.SetDefault("What careless fool stepped on my baby?");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            LobotomyModPlayer.ModPlayer(player).RedEyesAlerted = true;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}