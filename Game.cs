using PixoEngine;
using PixoEngine.Graphics;
using PixoEngine.Services;
using PixoEngine.ToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTest
{
    public class Game : Window
    {
        private SpriteBatch _spriteBatch;
        public static int Ticks = Environment.TickCount;
        public static Dictionary<string, Sprite> Sprites;

        protected override void Initialize()
        {
            Sprites = new Dictionary<string, Sprite>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadSprite("assets\\player.png", "player");
            LoadSprite("assets\\movableblock.png", "movableblock");
            LoadSprite("assets\\jumpPlatformGlow.png", "jpBlockGlow");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Pixo.Initialize(this, null, _spriteBatch, new GamePlayState());
            base.LoadContent();
        }

        public void LoadSprite(string assetName, string exportName)
        {
            Sprites.Add(exportName, new Sprite(GraphicsDevice, assetName));
        }

        protected override void UnloadContent()
        {
            foreach (Sprite sprite in Sprites.Values)
                sprite.Dispose();
            Sprites.Clear();
            base.UnloadContent();
        }

        protected override void Update(double delta)
        {
            Pixo.Update(this, null, _spriteBatch);
            base.Update(delta);
        }

        protected override void Draw(double delta)
        {
            Pixo.Draw(this, null, _spriteBatch);
            base.Draw(delta);
        }
    }
}
