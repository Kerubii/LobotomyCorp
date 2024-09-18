using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Content;

namespace LobotomyCorp.PlayerDrawEffects
{
    public class DrawFrontMiscellaneous : PlayerDrawLayer
    {
        private static Asset<Texture2D> AlriunePetal;
        private static Asset<Texture2D> MusicalAddiction;
        private static Asset<Texture2D> TodaysLook;
        private static Asset<Texture2D> BlackSwan;
        private static Asset<Texture2D> OurGalaxy;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(drawInfo.drawPlayer);
            return !drawInfo.drawPlayer.dead && (
                ModPlayer.FaintAromaPetal > 0 ||
                ModPlayer.HarmonyAddiction ||
                ModPlayer.TodaysExpressionActive ||
                ModPlayer.BlackSwanNettleClothing > 0 ||
                ModPlayer.OurGalaxyStone);
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

        public override void Load()
        {
            AlriunePetal = Mod.Assets.Request<Texture2D>("Misc/AlriunePetal");
            MusicalAddiction = Mod.Assets.Request<Texture2D>("Misc/MusicalAddiction");
            TodaysLook = Mod.Assets.Request<Texture2D>("Misc/TodaysExpression");
            BlackSwan = Mod.Assets.Request<Texture2D>("Misc/Nettle");
            OurGalaxy = Mod.Assets.Request<Texture2D>("Misc/OurGalaxyStone");
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            /*
            if (AlriunePetal == null)
            {
                AlriunePetal = Mod.Assets.Request<Texture2D>("Misc/AlriunePetal");
            }
            if ()*/

            Player Player = drawInfo.drawPlayer;
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(Player);
            if (ModPlayer.FaintAromaPetal > 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (ModPlayer.FaintAromaPetal <= ModPlayer.FaintAromaPetalMax * i)
                        continue;

                    Texture2D texture = AlriunePetal.Value;
                    int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                    int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y);

                    float rot = 0f;
                    if (i == 1)
                        rot += 90;
                    else if (i == 2)
                        rot += 45;
                    Vector2 Offset = new Vector2(-24 * Player.direction, 0).RotatedBy(MathHelper.ToRadians(rot * Player.direction));
                    Offset.Y -= 14;

                    rot = MathHelper.ToRadians(rot * Player.direction + (Player.direction == 1 ? 0 : 180)) - MathHelper.ToRadians(135);

                    float alpha = Terraria.Utils.GetLerpValue(0, 1f, ((ModPlayer.FaintAromaPetal - ModPlayer.FaintAromaPetalMax * i) / ModPlayer.FaintAromaPetalMax));
                    Color color = Lighting.GetColor((int)(Player.position.X / 16), (int)(Player.position.Y / 16)) * alpha;

                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY) + Offset, null, color, rot, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }

            if (ModPlayer.HarmonyAddiction)
            {
                Texture2D texture = MusicalAddiction.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X) + Main.rand.Next(2);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y) - 32;

                Color color = Color.White;

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                drawInfo.DrawDataCache.Add(data);
            }

            if (ModPlayer.TodaysExpressionActive)
            {
                Texture2D texture = TodaysLook.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y) - 56;

                Color color = Color.White;

                float scale = 0.75f;
                if (ModPlayer.TodaysExpressionTimer < 60)
                {
                    float prog = ModPlayer.TodaysExpressionTimer / 60f;
                    float amount = 0.1f * (float)Math.Sin((3f * Math.PI) * prog);
                    if (amount < 0)
                        amount *= -1;

                    scale += amount;
                }
                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), texture.Frame(5, 1, ModPlayer.TodaysExpressionFace), color, 0, new Vector2(texture.Width / 10f, texture.Height / 2f), scale, 0, 0);
                drawInfo.DrawDataCache.Add(data);

                int timer = ModPlayer.TodaysExpressionTimerMax - 30;
                if (ModPlayer.TodaysExpressionTimer > timer)
                {
                    float prog = (ModPlayer.TodaysExpressionTimer - timer) / 30f;

                    scale += 0.25f * (float)Math.Sin(1.57f * (1f - prog));
                    float opacity = prog;
                    data = new DrawData(texture, new Vector2(drawX, drawY), texture.Frame(5, 1, ModPlayer.TodaysExpressionFace), color * opacity, 0, new Vector2(texture.Width / 10f, texture.Height / 2f), scale, 0, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }

            if (ModPlayer.BlackSwanNettleClothing > 0)
            {
                float clothing = ModPlayer.BlackSwanNettleClothing;
                int currentActive = (int)Math.Ceiling(clothing);
                Texture2D texture = BlackSwan.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y);
                for (int i = 0; i < currentActive; i++)
                {
                    Vector2 pos = new Vector2(drawX, drawY) + new Vector2(30 + 3 * (float)Math.Sin(MathHelper.ToRadians(5 * (float)Main.timeForVisualEffects)), 0).RotatedBy(MathHelper.ToRadians(120 + 60 * i));

                    float rotation = (float)Math.Atan2(pos.Y - drawY, pos.X - drawX) + 2.35619f;

                    Color color = Color.White;
                    if (clothing - i < 1f)
                    {
                        float opacity = clothing - i / 1f;
                        color *= 0.5f * opacity;
                    }

                    DrawData data = new DrawData(texture, pos, null, color, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                    drawInfo.DrawDataCache.Add(data);
                }
            }

            if (ModPlayer.OurGalaxyStone)
            {
                Texture2D texture = OurGalaxy.Value;
                int drawX = (int)(drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X);
                int drawY = (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y) - 56;

                Color color = Color.White;

                DrawData data = new DrawData(texture, new Vector2(drawX, drawY), texture.Frame(), color, 0, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, 0, 0);
                drawInfo.DrawDataCache.Add(data);
            }
        }
    }
}
