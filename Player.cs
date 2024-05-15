using Microsoft.Win32.SafeHandles;
using PixoEngine;
using PixoEngine.Graphics;
using PixoEngine.Input;
using PixoEngine.Objects;
using PixoEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsTest
{
    public class Player : Entity
    {
        private const int MovementSpeed = 20;
        private const int MaxMovementSpeed = 400;
        private Vector2 _velocity;
        private KeyboardState _keyboardState;
        private bool _standsOnFloor = true;
        private float _gravitationalForce = 0.0f;

        public Player(Vector2 position)
        {
            ePosition = position;
        }

        public override void Start()
        {
            DrawOrder = 3;
            _velocity = Vector2.Zero;
            Tag = "player";
            base.Start();
        }

        public override FRectangle GetHitbox()
        {
            return new FRectangle(ePosition.X - 30, ePosition.Y - 30, 60, 60);
        }
        public FRectangle GetNotFullHitbox()
        {
            return new FRectangle(ePosition.X - 29, ePosition.Y - 29, 58, 58);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game.Sprites["player"], ePosition - ScreenState.Camera.Position, null, new Vector2(30), new Vector2(1.0f), 0.0f);
            base.Draw(spriteBatch);
        }

        public override void Update()
        {
            _keyboardState = Keyboard.GetState();

            MovementUpdate();

            GroundBlocksCollision();
            MovableCubesCollision();

            ePosition.X += _velocity.X / 100;
            ePosition.Y += _velocity.Y;

            base.Update();
        }

        private void GroundBlocksCollision()
        {
            if (HasPotentialCollision(GetNotFullHitbox(), _velocity.X / 100, _velocity.Y, "grblock"))
            {
                _velocity.X /= 5;
                _velocity.X = -_velocity.X;
            }
        }

        private void MovableCubesCollision()
        {
            Entity[] movableCubes = ScreenState.EntityContainer.GetEntitiesWithTag("mvcube");
            FRectangle hitbox = GetHitbox();
            hitbox.X += _velocity.X / 100;
            foreach (MovableCube item in movableCubes)
            {
                FRectangle itemHitbox = item.GetHitbox();
                FRectangle itemHitboxCopy = itemHitbox;
                itemHitboxCopy.Height -= 15;
                if (hitbox.Intersects(itemHitbox) && hitbox.Bottom - 2 > itemHitbox.Top && hitbox.Top + 2 < itemHitbox.Bottom)
                {
                    if (HasPotentialCollision(itemHitboxCopy, (item.Velocity.X + (_velocity.X / 2)) / 100, item.Velocity.Y, "grblock") == false)
                    {
                        _velocity.X /= 2;
                        item.AddVelocity(_velocity);
                    }
                    else
                    {
                        _velocity.X = -(MovementSpeed * 2);
                    }
                }
            }
        }

        private void MovementUpdate()
        {
            FRectangle notFullHitbox = GetNotFullHitbox();
            bool justMoved = false;
            float futureVelX;
            if (_keyboardState.IsKeyDown(Keys.D))
            {
                futureVelX = _velocity.X + MovementSpeed;
                if (HasPotentialCollision(notFullHitbox, futureVelX / 100, _velocity.Y, "grblock") == false || HasPotentialCollision(notFullHitbox, futureVelX / 100, _velocity.Y, "mvcube") == false)
                {
                    if (futureVelX < MaxMovementSpeed && _standsOnFloor)
                    {
                        _velocity.X += MovementSpeed;
                        justMoved = true;
                    }
                    if (futureVelX < MaxMovementSpeed / 3)
                    {
                        _velocity.X += MovementSpeed / 3;
                        justMoved = true;
                    }
                }
            }
            if (_keyboardState.IsKeyDown(Keys.A))
            {
                futureVelX = _velocity.X - MovementSpeed;
                if (HasPotentialCollision(notFullHitbox, futureVelX / 100, _velocity.Y, "grblock") == false || HasPotentialCollision(notFullHitbox, futureVelX / 100, _velocity.Y, "mvcube") == false)
                {
                    if (futureVelX > -MaxMovementSpeed && _standsOnFloor)
                    {
                        _velocity.X -= MovementSpeed;
                        justMoved = true;
                    }
                    if (futureVelX > -(MaxMovementSpeed / 3))
                    {
                        _velocity.X -= MovementSpeed / 3;
                        justMoved = true;
                    }
                }
            }
            if (justMoved == false)
            {
                if (_standsOnFloor)
                {
                    ReduceSpeed(MovementSpeed);
                }
                else
                {
                    ReduceSpeed(2);
                }
            }
            if (_gravitationalForce < MaxGravitationalForce)
                _gravitationalForce += GravitationalStrength;
            FloorCollisionUpdate();
            JumpPlatformsCollisionUpdate();
            if (_keyboardState.IsKeyDown(Keys.Space) && _standsOnFloor)
            {
                _gravitationalForce = -5.5f;
                _velocity.Y = _gravitationalForce;
                _standsOnFloor = false;
            }
        }

        private void JumpPlatformsCollisionUpdate()
        {
            bool hasCollidedWithAnyJumpPlatform = HasPotentialCollision(0, 0, "jpblock");
            if (hasCollidedWithAnyJumpPlatform)
            {
                _gravitationalForce = -7;
                _velocity.Y = _gravitationalForce;
            }
        }

        private void FloorCollisionUpdate()
        {
            bool willCollideVertically = HasPotentialCollision(_velocity.X / 100, _velocity.Y, "grblock") || HasPotentialCollision(_velocity.X / 100, _velocity.Y, "mvcube");
            _velocity.Y = _gravitationalForce;
            _standsOnFloor = willCollideVertically && _velocity.Y > 0;
            if (willCollideVertically)
            {
                _gravitationalForce = 0;
                _velocity.Y = 0;
            }
        }

        private void ReduceSpeed(int speedReducingValue)
        {
            if (_velocity.X > 0)
            {
                if (_velocity.X - speedReducingValue < 0)
                    _velocity.X = 0;
                else
                    _velocity.X -= speedReducingValue;
            }
            if (_velocity.X < 0)
            {
                if (_velocity.X + speedReducingValue > 0)
                    _velocity.X = 0;
                else
                    _velocity.X += speedReducingValue;
            }
        }
    }
}
