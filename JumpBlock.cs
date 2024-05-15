using PixoEngine;
using PixoEngine.Graphics;
using PixoEngine.Objects;
using PixoEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTest
{
    public class JumpBlock : Entity
    {
        public JumpBlock(Vector2 position)
        {
            ePosition = position;
        }

        public JumpBlock()
        {
            ePosition = Vector2.Zero;
        }

        public override void Start()
        {
            Tag = "jpblock";
            DrawOrder = 2;
            base.Start();
        }

        public override FRectangle GetHitbox()
        {
            return new FRectangle(ePosition.X, ePosition.Y, 60, 60);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            FRectangle rect = GetHitbox();
            rect.X -= ScreenState.Camera.Position.X;
            rect.Y -= ScreenState.Camera.Position.Y;
            spriteBatch.DrawRectangle(rect, new Color(0, 255, 0, 255));
            spriteBatch.Draw(Game.Sprites["jpBlockGlow"], (ePosition - new Vector2(11.5f, 11.5f)) - ScreenState.Camera.Position);
            base.Draw(spriteBatch);
        }
    }
}
