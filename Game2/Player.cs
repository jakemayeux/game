using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Game2
{

    class Player : PhysicsObject
    {

        private PhysicsObject physicsObject;

        private Vector2 pos;
        private Vector2 vel;
        private Vector2 force;
        private Texture2D texture;

        private const float ACCEL = .005f;
        private const float DECCEL = .005f;
        private const float MAX_WALK_VEL = 0.4f;
        private const float MAX_GRAVITY_VEL = 0.6f;
        private const float JUMP_FORCE = .05f;
        private const float FRICTION = .05f;
        private const float GRAVITY = .005f;
        private const int SIZE = 30;
        private const float EPSILON = DECCEL; //velocity to be considered zero


        private bool[] collisions;

        private Vector2[] trails;

        private bool isJumpPressed = false;


        public Player(GraphicsDevice graphicsDevice)
        {
            pos = new Vector2(0, 0);
            vel = new Vector2(0, 0);
            force = new Vector2(0, 0);

            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.Red });

            trails = new Vector2[30];

            for (int i = 0; i < 30; i++)
            {
                trails[i] = new Vector2(0, 0);
            }
        }


        public void Update(GamePadState gamePadState, KeyboardState keyboardState, double dt)
        {
            //m = 1 -> [force] = [accel] and force = accel
            force.X = 0;
            force.Y = 0;

            //y-component rules taken from jakes update function
            if (vel.Y < MAX_GRAVITY_VEL)
            {
                force.Y += GRAVITY;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && isGrounded()) {
                force.Y = -JUMP_FORCE;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            { //if the right key is down accelerate in +x direction
                if (vel.X < MAX_WALK_VEL)
                {
                    force.X = ACCEL;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Left)) //if the left key is down accelerate is -x direction
            {
                if (vel.X > -MAX_WALK_VEL)
                {
                    force.X = -ACCEL;
                }
            }

            //if neither key is held down we want it to stop moving
            if (keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right))
            {
                if (vel.X > EPSILON)
                { //if the velocity is sufficiently positive, DECCELL in units of velocity here
                    force.X = -DECCEL;
                }
                else if (vel.X < -EPSILON)
                { //if the velocity is sufficiently negative
                    force.X = DECCEL;
                }
                else 
                { //velocity is sufficiently close to zero
                    vel.X = 0.0f;
                }

            }


            //ensure velocity doesn't overshoot for sufficiently large dt
            bool overshotVel = vel.X > 0 && vel.X - DECCEL * (float)dt < EPSILON;
            overshotVel = overshotVel && (vel.X < 0 && vel.X + DECCEL * float(dt) > -EPSILON);
            if (overshotVel) {
               vel.X = 0.0.f;
            }
            //update velocity and position vectors
            vel += force * (float)dt;
            pos += vel * (float)dt;
        }

        public void Update3(GamePadState gamePadState, KeyboardState keyboardState, double dt)
        {

            force.X = 0;
            force.Y = 0;

            if (vel.Y < MAX_GRAVITY_VEL)
            {
                force.Y += GRAVITY;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if (vel.X < MAX_WALK_VEL)
                {
                    force.X += ACCEL;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (vel.X > -MAX_WALK_VEL)
                {
                    force.X -= ACCEL;
                }
            }

            // stop walking
            //if (keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.Right)) {
            //    if (vel.X > 0) {
            //        if (vel.X < DECCEL) {
            //            force.X -= vel.X;
            //        } else {
            //            force.X -= DECCEL;
            //        }
            //    } else if (vel.X < 0) {
            //        if (vel.X > DECCEL) {
            //            force.X -= vel.X;
            //        } else {
            //            force.X += DECCEL;
            //        }
            //    }
            //}

            vel += force * (float)dt;
            pos += vel * (float)dt;
        }

        public void Update2(GamePadState gamePadState, KeyboardState keyboardState, double dt)
        {
            var leftStick = gamePadState.ThumbSticks.Left;

            force.X = 0;
            force.Y = 0;
            if (vel.Y < MAX_GRAVITY_VEL)
            {
                force.Y += GRAVITY;
            }

            if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Space))
            {
                if (!isJumpPressed && isGrounded())
                {
                    force.Y += -JUMP_FORCE;
                }
                isJumpPressed = true;
            }
            else
            {
                isJumpPressed = false;
            }


            // apply running forces if we are under max velocity
            if (leftStick.X < 0 || keyboardState.IsKeyDown(Keys.Left))
            {
                if (vel.X > -MAX_WALK_VEL)
                {
                    force.X += -ACCEL;
                }
            }
            else if (leftStick.X > 0 || keyboardState.IsKeyDown(Keys.Right))
            {
                if (vel.X < MAX_WALK_VEL)
                {
                    force.X += ACCEL;
                }
            }

            var reduction = dt * FRICTION;
            var speedX = Math.Abs(vel.X);
            if (reduction >= speedX)
            {
                vel.X = 0;
            }
            else if (speedX > MAX_WALK_VEL || force.X == 0)
            {
                var slowdown = (speedX - ((float)dt * FRICTION)) / speedX;
                vel *= slowdown;
            }

            //if (leftStick.Y > 0) {
            //    if (vel.Y > -MAX_WALK_VEL) {
            //        force.Y = -ACCEL;
            //    }
            //} else if (leftStick.Y < 0) {
            //    if (vel.Y < MAX_WALK_VEL) {
            //        force.Y = ACCEL;
            //    }
            //}


            //Debug.WriteLine(pos);


            //var speed = vel.Length();
            //var speedX = Math.Abs(vel.X);
            //if (force.Length() == 0) {
            //    var reduction = dt * FRICTION;
            //    if (reduction >= speed) {
            //        vel.X = 0;
            //        vel.Y = 0;
            //    }

            //    if (Math.Abs(vel.X) > MAX_VEL || force.X == 0) {
            //        var slowdown = (speed - ((float)dt * FRICTION)) / speed;
            //        vel *= slowdown;
            //    }
            //}



            vel += force * (float)dt;


            //if (vel.Length() > MAX_VEL) {
            //    vel.Normalize();
            //    vel *= MAX_VEL;
            //}

            pos += vel;
        }

        public void Collide(bool[] collisions)
        {
            this.collisions = collisions;
            if (collisions[0])
            {
                if (vel.Y < 0)
                {
                    vel.Y = 0;
                }
                pos.Y = roundToMultiple(pos.Y, SIZE);
            }
            if (collisions[1])
            {
                if (vel.Y > 0)
                {
                    vel.Y = 0;
                }
                pos.Y = roundToMultiple(pos.Y, SIZE);
            }
            if (collisions[2])
            {
                if (vel.X > 0)
                {
                    vel.X = 0;
                }
                pos.X = roundToMultiple(pos.X, SIZE);
            }
            if (collisions[3])
            {
                if (vel.X > 0)
                {
                    vel.X = 0;
                }
                pos.X = roundToMultiple(pos.X, SIZE);
            }
        }

        private bool isGrounded()
        {
            return collisions[1];
        }

        private float roundToMultiple(float val, float multiple)
        {
            return (float)Math.Round(val / multiple) * multiple;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)pos.X, (int)pos.Y, SIZE, SIZE), Color.White);

            //asdf++;

            //if (asdf >= 30) {
            //    asdf = 0;
            //}
            //trails[asdf] = pos;
            //for (int i = 0; i < 30; i++) {
            //    spriteBatch.Draw(texture, new Rectangle((int)trails[i].X, (int)trails[i].Y, SIZE, SIZE), Color.Gray);
            //}
        }

        public Vector2 getPos()
        {
            return pos;
        }
    }
}
