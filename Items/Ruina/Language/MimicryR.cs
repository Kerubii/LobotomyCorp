using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Language
{
    public class MimicryR : SEgoItem
	{
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
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			//Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/NothingThere_Goodbye");
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.MimicryS>();
            Item.shootSpeed = 1;
		}

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
                damage = (int)(damage * 0.7f);
        }

        public override bool SafeCanUseItem(Player player)
        {
            if (player.altFunctionUse != 2)
            {
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Nullthing_Attack1") with {Volume = 0.25f};
                Item.shoot = ModContent.ProjectileType<Projectiles.Realized.MimicryR>();
                Item.shootSpeed = 1;
                Item.channel = true;
            }
            else
            {
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/Nullthing_Skill1_Ching") with {Volume = 0.18f};
                Item.shoot = ModContent.ProjectileType<Projectiles.Realized.MimicryRHello>();
                Item.shootSpeed = 16;
                Item.channel = false;
            }
            return player.ownedProjectileCounts[Item.shoot] == 0;
        }

        public override float UseAnimationMultiplier(Player player)
        {
            if (player.altFunctionUse == 2 && !LobotomyModPlayer.ModPlayer(player).MimicryShell)
                return 1.5f;
            return base.UseAnimationMultiplier(player);
        }

        public override float UseTimeMultiplier(Player player)
        {
            if (player.altFunctionUse == 2 && !LobotomyModPlayer.ModPlayer(player).MimicryShell)
                return 1.5f;
            return base.UseTimeMultiplier(player);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<Aleph.Mimicry>())
            .AddIngredient(ItemID.FleshBlock, 10)
            .AddIngredient(ItemID.FragmentSolar)
            .AddTile<Tiles.BlackBox3>()
            .AddCondition(RedMistCond)
            .Register();
        }
	}
}