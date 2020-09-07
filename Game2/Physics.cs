using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game2 {
    class Physics {

        private Vector2 size;

        private Vector2 pos;
        private Vector2 vel;
        private Vector2 force;

        private const float ACCEL = .05F;
        private const float MAX_WALK_VEL = 5;
        private const float MAX_GRAVITY_VEL = 10;
        private const float JUMP_FORCE = 1f;
        private const float FRICTION = .05F;
        private const float GRAVITY = .05F;
        private const int SIZE = 30;

    }
}
