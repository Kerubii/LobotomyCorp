using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Language
{
    public class SmileR : SEgoItem
	{
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<Configs.LobotomyServerConfig>().TestItemEnable;
        }

        public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("\"And the many shells cried out one word, \"Manager\".\"");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            EgoColor = LobotomyCorp.AlephRarity;
        }

        public override void SetDefaults() 
		{
            Item.damage = 265;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			//Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/NothingThere_Goodbye");
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Realized.SmileRSword>();
            Item.shootSpeed = 1;
		}
	}
}