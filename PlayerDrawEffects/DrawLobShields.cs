using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ReLogic.Content;

namespace LobotomyCorp.PlayerDrawEffects
{
    public class DrawLobShields : PlayerDrawLayer
    {
        public static Asset<Texture2D> RedShield;
        public static Asset<Texture2D> WhiteShield;
        public static Asset<Texture2D> BlackShield;
        public static Asset<Texture2D> PaleShield;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                WhiteShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/WhiteShield");
                RedShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/RedShield");
                BlackShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/BlackShield");
                PaleShield = Mod.Assets.Request<Texture2D>("Misc/BulletShield/PaleShield");

                Main.QueueMainThreadAction(() =>
                {
                    LobotomyCorp.PremultiplyTexture(RedShield.Value);
                    LobotomyCorp.PremultiplyTexture(WhiteShield.Value);
                    LobotomyCorp.PremultiplyTexture(BlackShield.Value);
                    LobotomyCorp.PremultiplyTexture(PaleShield.Value);
                });
            }
        }

        public override void Unload()
        {
            WhiteShield = null;
            RedShield = null;
            BlackShield = null;
            PaleShield = null;
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(drawInfo.drawPlayer);
            return ModPlayer.ShieldActive;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.FrontAccFront);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player Player = drawInfo.drawPlayer;
            LobotomyModPlayer ModPlayer = LobotomyModPlayer.ModPlayer(Player);

            Texture2D shieldTex = RedShield.Value;
            if (ModPlayer.WhiteShield)
                shieldTex = WhiteShield.Value;
            if (ModPlayer.BlackShield)
                shieldTex = BlackShield.Value;
            if (ModPlayer.PaleShield)
                shieldTex = PaleShield.Value;

            //Current Shield state
            bool broken = ModPlayer.ShieldAnim <= 60;

            //Static positions etc
            Rectangle frame = new Rectangle(0, shieldTex.Height / 2 * broken.ToInt(), shieldTex.Width, shieldTex.Height / 2);
            Vector2 origin = frame.Size() / 2;
            Vector2 drawPos = new Vector2((drawInfo.Position.X + Player.width / 2f - Main.screenPosition.X), (int)(drawInfo.Position.Y + Player.height / 2f - Main.screenPosition.Y));

            //Shield%
            float shieldHealth = ((float)ModPlayer.ShieldHP / (float)ModPlayer.ShieldHPMax);
            //Color - Become less visible the lower the health
            float colorOpacity = 0.6f + 0.2f * shieldHealth;
            if (broken)
                colorOpacity = 0.4f + 0.2f * shieldHealth;
            Color color = Color.White * colorOpacity;
            color = Player.GetImmuneAlpha(color, drawInfo.shadow);
            color.A = (byte)(color.A * 0.7f);

            //Scale - slowly beating, shrinks a bit when damaged
            float progress = ((float)ModPlayer.ShieldAnim - (broken ? 0 : 60)) / 60f;
            float scale = 0.8f + 0.2f * shieldHealth + 0.05f * (float)Math.Sin(2f * (float)Math.PI * progress);

            DrawData data = new DrawData(
                shieldTex,
                drawPos,
                frame,
                color,
                0f,
                origin,
                scale,
                0,
                0);
            drawInfo.DrawDataCache.Add(data);
        }
    }
}
