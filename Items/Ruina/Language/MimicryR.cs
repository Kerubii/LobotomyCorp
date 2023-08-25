using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Language
{
    public class MimicryR : SEgoItem
	{
        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModContent.GetInstance<Configs.LobotomyServerConfig>().TestItemEnable;
        }

        public override void SetStaticDefaults() 
		{
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            // Tooltip.SetDefault("\"And the many shells cried out one word, \"Manager\".\"");
            
            EgoColor = LobotomyCorp.AlephRarity;
        }

        public override void SetDefaults() 
		{
            PassiveText = "heLLo? - Shoot a piercing projectile infront of you\n" +
                          "GoDdByE - deal 200% more damage when hitting enemies on your attack's sweetspot\n" +
                          "Mimic - Killing an enemy restores 20% of your health and grants A New Shell buff, disabling Shell" +
                          "|Wear Shell - Gain the debuff Craving for 1 minute when using this ewapon. When the debuff times out, the user dies\n" +
                          "This Item is incomplete and unobtainable";

            Item.damage = 265;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/NothingThere_Goodbye");
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.MimicryS>();
            Item.shootSpeed = 1;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
                damage = (int)(damage * 0.5f);
        }

        public override bool SafeCanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/NothingThere_Goodbye") with {Volume = 0.25f};
                Item.useTime = 42;
                Item.useAnimation = 42;
                Item.shoot = ModContent.ProjectileType<Projectiles.MimicryS>();
                Item.shootSpeed = 1;
            }
            else
            {
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/NothingThere_Hello") with {Volume = 0.18f};
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.shoot = ModContent.ProjectileType<Projectiles.MimicrySHello>();
                Item.shootSpeed = 8;
            }
            return base.SafeCanUseItem(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
		}
	}
}