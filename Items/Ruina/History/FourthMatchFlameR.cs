using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.Localization.Language;

namespace LobotomyCorp.Items.Ruina.History
{
	public class FourthMatchFlameR : SEgoItem
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Penitence"); // By default, capitalization in classnames will damage spaces to the display name. You can customize the display name here by uncommenting this line.
            // DisplayName.SetDefault("Fourth Match Flame");

            // Tooltip.SetDefault(GetTooltip());
		}

		public override void SetDefaults() 
		{
            //Why doesnt this work on StaticDefaults now :V
            //PassiveText = GetPassiveList();
            EgoColor = LobotomyCorp.TethRarity;

            Item.damage = 41;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
            Item.shoot = ModContent.ProjectileType<Projectiles.FourthMatchFlameSlash>();
            Item.shootSpeed = 1f;
            Item.noUseGraphic = true;
            Item.noMelee = true;
			Item.autoReuse = true;            
		}

        public override bool SafeCanUseItem(Player player)
        {
            if (LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR < 3)
            {
                Item.useTime = 24;
                Item.useAnimation = 24;
                Item.shoot = 0;
                //Item.noUseGraphic = false;
                Item.noMelee = false;
                Item.autoReuse = true;
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/MatchGirl_NoBoom") with {Volume = 0.25f};
                LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR++;
            }
            else
            {
                if (player.altFunctionUse == 2)
                {
                    LobotomyModPlayer.ModPlayer(player).FourthMatchExplode(true);
                    LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR = 0;
                    return false;
                }

                Item.useTime = 30;
                Item.useAnimation = 30;
                Item.shoot = ModContent.ProjectileType<Projectiles.FourthMatchFlameGigaSlash>();
                //Item.noUseGraphic = true;
                Item.noMelee = true;
                Item.autoReuse = true;
                Item.UseSound = SoundID.Item1;
                Item.UseSound = new SoundStyle("LobotomyCorp/Sounds/Item/MatchGirl_Atk") with {Volume = 0.25f};
                LobotomyModPlayer.ModPlayer(player).FourthMatchExplode();
                LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR = 0;
                ModContent.GetInstance<LobSystem>().ScreenShake(15, 10f, 0.1f);
            }
            return true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage = (damage * 4);
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR > 0)
            {
                Item.noUseGraphic = false;
            }
            else
            {
                Item.noUseGraphic = true;
            }

            Vector2 offset = new Vector2(-82, 0).RotatedBy(player.itemRotation - MathHelper.ToRadians(45 * player.direction + (player.direction > 0 ? 180 : 0)));
            Vector2 ownerMountedCenter = player.RotatedRelativePoint(player.MountedCenter, true);
            Dust dust = Dust.NewDustPerfect(ownerMountedCenter + offset, 6, new Vector2(), 0, new Color(255, 255, 255), 2.8f * (float)(LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR / (float)3));
            dust.noGravity = true;
        }

        /*public override void ModifyWeaponDamage(Player player, ref float damage, ref float mult)
        {
            if (LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR > 3)
                damage += 5f;
        }*/

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            /*
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 120;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 120);*/

            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            /*
            if (target.HasBuff(ModContent.BuffType<Buffs.Matchstick>()))
                target.buffTime[target.FindBuffIndex(ModContent.BuffType<Buffs.Matchstick>())] += 120;
            else
                target.AddBuff(ModContent.BuffType<Buffs.Matchstick>(), 120);*/

            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff(BuffID.OnFire))
            {
                damage += 0.16f;
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return LobotomyModPlayer.ModPlayer(player).FourthMatchFlameR == 3;
        }

        public override void AddRecipes() 
		{
            CreateRecipe()
            .AddIngredient(ModContent.ItemType<FourthMatchFlame>())
            .AddIngredient(ItemID.CursedTorch, 99)
            .AddTile<Tiles.BlackBox3>()
            .Register();

            CreateRecipe()
            .AddIngredient(ModContent.ItemType<FourthMatchFlame>())
            .AddIngredient(ItemID.IchorTorch, 99)
            .AddTile<Tiles.BlackBox3>()
            .Register();
        }
	}
}