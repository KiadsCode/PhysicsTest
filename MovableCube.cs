using PixoEngine;
using PixoEngine.Objects;
using PixoEngine.Services;

namespace PhysicsTest
{
    public class MovableCube : Entity
    {
        private float _gravitationalForce = 0.0f;
        private Vector2 _velocity = Vector2.Zero;
        private bool _standsOnFloor;

        public Vector2 Velocity
        {
            get
            {
                return _velocity;
            }
        }

        public bool StandsOnFloor
        {
            get
            {
                return _standsOnFloor;
            }
        }

        public override FRectangle GetHitbox()
        {
            return new FRectangle(ePosition.X - 30, ePosition.Y - 30, 60, 60);
        }

        public MovableCube(Vector2 position)
        {
            ePosition = position;
        }

        public MovableCube()
        {
            ePosition = Vector2.Zero;
        }

        public override void Start()
        {
            Tag = "mvcube";
            Name = $"MvCube{Game.Ticks++}";
            DrawOrder = 1;
            base.Start();
        }

        public void AddVelocity(Vector2 velocity)
        {
            _velocity += velocity;
        }

        public override void Update()
        {
            if (_gravitationalForce < MaxGravitationalForce)
                _gravitationalForce += GravitationalStrength;
            _velocity.Y = _gravitationalForce;

            bool willVerticallyCollide = CollisionWithOtherCubes(new Vector2(0, _velocity.Y)) || HasPotentialCollision(0, _velocity.Y, "grblock");
            _standsOnFloor = willVerticallyCollide && _velocity.Y > 0;
            if (willVerticallyCollide)
            {
                _velocity.Y = 0;
                _gravitationalForce = 0;
            }

            XCollision();
            JumpPlatformsCollisionUpdate();
            MoveCubesPhysically();

            if (_standsOnFloor)
            {
                ReduceSpeed(20);
            }
            else
            {
                ReduceSpeed(2);
            }

            ePosition += new Vector2(_velocity.X / 100, _velocity.Y);
            base.Update();
        }

        private void XCollision()
        {
            FRectangle hitbox = GetHitbox();
            hitbox.Height -= 15;
            if (HasPotentialCollision(hitbox, _velocity.X / 100, 0, "grblock"))
                _velocity.X = 0;
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

        private void MoveCubesPhysically()
        {
            Entity[] movableCubes = ScreenState.EntityContainer.GetEntitiesWithTag("mvcube");
            FRectangle hitbox = GetHitbox();
            hitbox.X += _velocity.X / 100;
            foreach (MovableCube item in movableCubes)
            {
                if (hitbox.Intersects(item.GetHitbox()) && Name != item.Name)
                {
                    _velocity.X /= 2;
                    item.AddVelocity(_velocity);
                }
            }
        }

        private bool CollisionWithOtherCubes(Vector2 velocity)
        {
            Entity[] movableCubes = ScreenState.EntityContainer.GetEntitiesWithTag("mvcube");
            FRectangle hitbox = GetHitbox();
            hitbox.X += _velocity.X / 100;
            hitbox.Y += _velocity.Y;
            foreach (MovableCube item in movableCubes)
            {
                FRectangle itemHitbox = item.GetHitbox();
                if (hitbox.Intersects(itemHitbox) && Name != item.Name)
                {
                    return true;
                }
            }
            return false;
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game.Sprites["movableblock"], ePosition - ScreenState.Camera.Position, null, new Vector2(30), 1.0f, 0.0f);
            base.Draw(spriteBatch);
        }
    }
}
