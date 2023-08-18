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
					tip = "Happy - Reduce damage taken by 20%, +30 defense and 15% decreased final damage";
					break;
				case 1://Smile
					tip = "Smiling - +15 defense and 10% decreased final damage";
					break;
				default://Neutral
					tip = "Neutral - No Effect";
					break;
				case 3://Sad
					tip = "Sad - 10% increased final damage and -15 defense";
					break;
				case 4://Angry
					tip = "Angry - 20% increased final damage, increases damage taken by 30% and defense is set to 0";
					break;
            }
        }

		public const float TODAYDAMAGEHAPPY = 0.85f;
		public const float TODAYDAMAGESMILE = 0.9f;
		public const float TODAYDAMAGENEUTRAL = 1;
		public const float TODAYDAMAGESAD = 1.1f;
		public const float TODAYDAMAGEANGRY = 1.2f;

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
					player.endurance -= 0.3f;
					player.statDefense -= 30;
					if (player.statDefense > 0)
						player.statDefense *= 0;
					break;
            }
        }
    }
}