using Terraria;
using Terraria.ModLoader;

namespace LobotomyCorp.Buffs
{
	public class TodaysLook : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Look for the Day");
			// Description.SetDefault("The Look for the Day");
			Main.buffNoTimeDisplay[Type] = true;
		}

        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
			int todaysLook = LobotomyModPlayer.ModPlayer(Main.LocalPlayer).TodaysExpressionFace;
			switch (todaysLook)
            {
				case 0://Happy
					tip = "Happy - Reduce damage taken by 20%, +30 defense and 75% decreased damage";
					break;
				case 1://Smile
					tip = "Smiling - +15 damage and 50% decreased damage";
					break;
				default://Neutral
					tip = "Neutral - No Effect";
					break;
				case 3://Sad
					tip = "Sad - 20% increased damage and -15 defense";
					break;
				case 4://Angry
					tip = "Angry - 50% increased damage and defense is set to 0";
					break;
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
			LobotomyModPlayer.ModPlayer(player).TodaysExpressionActive = true;
			player.buffTime[buffIndex] = 10;
			int todaysLook = LobotomyModPlayer.ModPlayer(player).TodaysExpressionFace;

			switch (todaysLook)
            {
				case 0://Happy
					player.endurance += 0.2f;
					player.statDefense += 30;
					break;
				case 1://Smile
					player.statDefense += 15;
					break;
				default://Neutral
					break;
				case 3://Sad
					player.statDefense -= 15;
					break;
				case 4://Angry
					player.statDefense -= 30;
					if (player.statDefense > 0)
						player.statDefense *= 0;
					break;
            }
        }
    }
}