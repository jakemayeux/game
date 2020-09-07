using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace Game2 {
    class Map {
        private Texture2D texture;

        private int[,] mapData = new int[,] {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };

        public Map(GraphicsDevice graphicsDevice) {
            texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.Blue });
        }

        public void Draw(SpriteBatch spriteBatch) {
            for (int y = 0; y < mapData.GetLength(0); y++) {
                for (int x = 0; x < mapData.GetLength(1); x++) {
                    if (mapData[y, x] == 1) {
                        spriteBatch.Draw(texture, new Rectangle(x * 30, y * 30, 30, 30), Color.White);
                    }
                }
            }
        }

        //up down left right
        public bool[] Collide(Vector2 pos) {
            var size = 30;
            var give = 10; // how much shorter to calculate bounging box lines
            var ret = new bool[4] { false, false, false, false };

            // up
            Vector2 utl = new Vector2(pos.X + give, pos.Y);
            Vector2 utr = new Vector2(pos.X + size - give, pos.Y);

            // down
            Vector2 dbl = new Vector2(pos.X + give, pos.Y + size);
            Vector2 dbr = new Vector2(pos.X + size - give, pos.Y + size);

            // left
            Vector2 ltl = new Vector2(pos.X, pos.Y + give);
            Vector2 lbl = new Vector2(pos.X, pos.Y + size - give);

            // right
            Vector2 rtl = new Vector2(pos.X + size, pos.Y + give);
            Vector2 rbl = new Vector2(pos.X + size, pos.Y + size - give);

            for (int y = 0; y < mapData.GetLength(0); y++) {
                for (int x = 0; x < mapData.GetLength(1); x++) {
                    if (mapData[y, x] == 1) {
                        if (pointCollision(utl, x * size, y * size, size, size)
                            || pointCollision(utr, x * size, y * size, size, size)) {
                            ret[0] = true;
                        }
                        if (pointCollision(dbl, x * size, y * size, size, size)
                            || pointCollision(dbr, x * size, y * size, size, size)) {
                            ret[1] = true;
                        }
                        if (pointCollision(ltl, x * size, y * size, size, size)
                            || pointCollision(lbl, x * size, y * size, size, size)) {
                            ret[2] = true;
                        }
                        if (pointCollision(rtl, x * size, y * size, size, size)
                            || pointCollision(rbl, x * size, y * size, size, size)) {
                            ret[3] = true;
                        }
                    }
                }
            }

            return ret;
        }

        private Boolean pointCollision(Vector2 point, int x, int y, int w, int h) {
            return point.X > x && point.X < x + w && point.Y > y && point.Y < y + h;
        }
    }
}
