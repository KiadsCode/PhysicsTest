using PixoEngine;
using PixoEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTest
{
    public class GamePlayState : ScreenState
    {
        private KeyboardState _oldKeyboard = Keyboard.GetState();
        public static Vector2 CameraPosition = Vector2.Zero;

        public override void OnStart()
        {
            base.OnStart();
            ReloadMap();
        }

        public override void OnUpdate()
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.R) && _oldKeyboard.IsKeyUp(Keys.R))
                ReloadMap();
            if (keyboard.IsKeyDown(Keys.Right))
                Camera.TranslatePosition(4, 0);
            if (keyboard.IsKeyDown(Keys.Left))
                Camera.TranslatePosition(-4, 0);

            if (keyboard.IsKeyDown(Keys.Down))
                Camera.TranslatePosition(0, 4);
            if (keyboard.IsKeyDown(Keys.Up))
                Camera.TranslatePosition(0, -4);

            _oldKeyboard = keyboard;
            base.OnUpdate();
        }

        private void ReloadMap()
        {
            EntityContainer.Clear();
            LoadMap("assets\\map.pmf");
        }

        protected override void OnObjectNode(string tag, Vector2 position)
        {
            Vector2 offsetToWorld = new Vector2(400, 200);
            switch (tag)
            {
                case "player":
                    EntityContainer.Add(new Player(position + offsetToWorld));
                    break;
                case "clblock":
                    EntityContainer.Add(new GroundBlock(position + offsetToWorld));
                    break;
                case "jpblock":
                    EntityContainer.Add(new JumpBlock(position + offsetToWorld));
                    break;
                case "weiblock":
                    EntityContainer.Add(new MovableCube(position + offsetToWorld));
                    break;
            }
            base.OnObjectNode(tag, position);
        }
    }
}
