using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Game2 {

    class Player : PhysicsObject {

        private PhysicsObject physicsObject;

        //private Vector2 pos;
        //private Vector2 vel;
        //private Vector2 force;
        private Texture2D texture;


        private bool[] collisions;

        private Vector2[] trails;
        private int asdf = 0;

        private bool isJumpPressed = false;


        public Player(GraphicsDevice graphicsDevice) {
            pos = new Vector2(0, 0);
            vel = new Vector2(0, 0);
            force = new Vector2(0, 0);

            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.Red });

            trails = new Vector2[30];

            for (int i = 0; i < 30; i++) {
                trails[i] = new Vector2(0, 0);
            }
        }

        public void Update(GamePadState gamePadState, double dt) {
            var leftStick = gamePadState.ThumbSticks.Left;

            force.X = 0;
            if (vel.Y < MAX_GRAVITY_VEL) {
                force.Y = GRAVITY;
            } else {
                force.Y = 0;
            }

            if (gamePadState.Buttons.A == ButtonState.Pressed) {
                if (!isJumpPressed && isGrounded()) {
                    force.Y = -JUMP_FORCE;
                    Debug.WriteLine("JUMPING " + dt);
                }
                isJumpPressed = true;
            } else {
                isJumpPressed = false;
            }


            // apply running forces if we are under max velocity
            if (leftStick.X < 0) {
                if (vel.X > -MAX_WALK_VEL) {
                    force.X = -ACCEL;
                }
            } else if (leftStick.X > 0) {
                if (vel.X < MAX_WALK_VEL) {
                    force.X = ACCEL;
                }
            }

            var reduction = dt * FRICTION;
            var speedX = Math.Abs(vel.X);
            if (reduction >= speedX) {
                vel.X = 0;
            } else if (speedX > MAX_WALK_VEL || force.X == 0) {
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

        public void Collide(bool[] collisions) {
            this.collisions = collisions;
            if (collisions[0]) {
                if (vel.Y < 0) {
                    vel.Y = 0;
                }
                pos.Y = roundToMultiple(pos.Y, SIZE);
            }
            if (collisions[1]) {
                if (vel.Y > 0) {
                    vel.Y = 0;
                }
                pos.Y = roundToMultiple(pos.Y, SIZE);
            }
            if (collisions[2]) {
                if (vel.X > 0) {
                    vel.X = 0;
                }
                pos.X = roundToMultiple(pos.X, SIZE);
            }
            if (collisions[3]) {
                if (vel.X > 0) {
                    vel.X = 0;
                }
                pos.X = roundToMultiple(pos.X, SIZE);
            }
        }

        private bool isGrounded() {
            return collisions[1];
        }

        private float roundToMultiple(float val, float multiple) {
            return (float)Math.Round(val / multiple) * multiple;
        }

        public void Draw(SpriteBatch spriteBatch) {
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

        public Vector2 getPos() {
            return pos;
        }
    }
}












