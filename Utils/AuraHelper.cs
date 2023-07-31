using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace LobotomyCorp.Utils
{
    public interface AuraBehavior
    {
        int intensity { get; }

        Texture2D GetTexture(Mod mod);
        Rectangle GetSourceRect(Texture2D texture, int index);
        Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle);

        void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle);
        void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle);
    }

    public class AuraParticle
    {
        public bool Active;
        public Vector2 position;
        public Vector2 velocity;
        public float rotation;
        public float scale;
        public int textureIndex;
        public int particleTime;
        AuraBehavior behavior;

        /*
        public AuraBehavior GetAuraBehavior { get { return behavior; } }
        public Vector2 Position { get { return position; } }
        public float Rotation { get { return rotation; } }
        public float Scale { get { return scale; } }
        */

        public AuraParticle(Player player, int dir, float gravDir, float time, AuraBehavior newBehavior)
        {
            behavior = newBehavior;
            Active = true;
            position = player.Center;
            velocity = Vector2.Zero;
            rotation = 0;
            scale = 1f;
            textureIndex = 0;
            particleTime = 0;
            behavior.SpawnParam(player, dir, gravDir, time, this);
        }
        
        public void Update(Player player, int dir, float gravDir, float time)
        {
            behavior.Behavior(player, dir, gravDir, time, this);
            particleTime++;
        }

        public DrawData Draw(ref PlayerDrawSet drawInfo, Mod mod)
        {
            Texture2D tex = behavior.GetTexture(mod);
            Rectangle frame = behavior.GetSourceRect(tex, textureIndex);

            DrawData data = new DrawData(
                tex,
                position - Main.screenPosition,
                frame,
                behavior.GetColor(drawInfo, this),
                rotation,
                frame.Size() / 2,
                scale,
                0,
                0);

            return data;
        }
    }

    public class TestAura : AuraBehavior
    {
        public int intensity => 3;

        public Texture2D GetTexture(Mod mod) { return mod.Assets.Request<Texture2D>("Misc/FlameParticlesL").Value; }

        public Rectangle GetSourceRect(Texture2D texture, int index)
        {
            return texture.Frame(4, 1, index);
        }

        public Color GetColor(PlayerDrawSet drawInfo, AuraParticle particle) { return Color.Blue * 0.9f * (1f - particle.particleTime / 13f); }

        public void SpawnParam(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.textureIndex = Main.rand.Next(4);
            particle.position.Y -= 12;
            particle.position.X += (8 * (float)Math.Sin(0.436332f * time));
            particle.velocity.Y -= 2f;
            particle.rotation = Main.rand.NextFloat(6.28f);
            particle.scale = 1f;
        }

        public void Behavior(Player player, int dir, float gravDir, float time, AuraParticle particle)
        {
            particle.position.Y += particle.velocity.Y;
            particle.scale -= 0.04f;

            if (particle.particleTime > 10)
            {
                particle.Active = false;
            }
        }
    }
}
