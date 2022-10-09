using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LobotomyCorp.Items.Ruina.Natural
{
	public class InTheNameOfLoveAndHateR : SEgoItem
	{
        public int ArcanaManaCost = 500;

		public override void SetStaticDefaults() {
			Tooltip.SetDefault("In the name of Love and Justice~ Here comes Magical Girl!");
            
            EgoColor = LobotomyCorp.WawRarity;
            //Item.staff[Item.type] = true;
        }

        public override void SetDefaults() {
            PassiveText = "Arcana Slave - Summon a laser of love, costs " + ArcanaManaCost + " mana\n" +
                          "Love - Hitting an enemy or getting hit reduces Arcana Slave cost\n" +
                          "Villain - Hitting an enemy with Arcana Beats marks them, increasing Justice and Hate gained\n" +
                          "Justice - Increases defense and lowers damage when hitting enemies\n" +
                          "|Hate - Getting hit reduces defense and increases damage\n" +
                          "This Item is incomplete and unobtainable";

            Item.damage = 30; // Sets the Item's damage. Note that projectiles shot by this weapon will use its and the used ammunition's damage damageed together.
			Item.DamageType = DamageClass.Magic;; // sets the damage type to ranged
			Item.width = 40; // hitbox width of the Item
			Item.height = 20; // hitbox height of the Item
			Item.useTime = 20; // The Item's use time in ticks (60 ticks == 1 second.)
			Item.useAnimation = 20; // The length of the Item's use animation in ticks (60 ticks == 1 second.)
			Item.useStyle = ItemUseStyleID.Shoot; // how you use the Item (swinging, holding out, etc)
			Item.noMelee = true; //so the Item's animation doesn't do damage
			Item.knockBack = 4; // Sets the Item's knockback. Note that projectiles shot by this weapon will use its and the used ammunition's knockback damageed together.
			Item.value = 10000; // how much the Item sells for (measured in copper)
			Item.rare = ItemRarityID.Green; // the color that the Item's name will be in-game
			Item.UseSound = SoundID.Item11; // The sound that this Item plays when used.
			Item.autoReuse = true; // if you can hold click to automatically use it again
			Item.shoot = ModContent.ProjectileType<Projectiles.QueenLaser.ArcanaBeats>(); //idk why but all the guns in the vanilla source have this
			Item.shootSpeed = 1f; // the speed of the projectile (measured in pixels per frame)
            Item.channel = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Item.shoot = ModContent.ProjectileType<Projectiles.QueenLaser.Circle1>();
            }
            else
                Item.shoot = ModContent.ProjectileType<Projectiles.QueenLaser.ArcanaBeats>();

            position.X -= 42 * player.direction;
            position.Y -= 30;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-16, 0);
        }
    }
}
