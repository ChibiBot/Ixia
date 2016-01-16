using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpgtutorial
{
    public enum DrawType
    {
        Overlay,
        Underlay
    };

    public class Tile
    {
        private Vector2 m_position;
        private Rectangle m_sourceRect;
        private SpriteStates m_state;

        public Rectangle SourceRect
        {
            get { return m_sourceRect; }
        }

        public Vector2 Position
        {
            get { return m_position; }
        }

        public void LoadContent(Vector2 position, Rectangle sourceRect, SpriteStates state)
        {
            this.m_position = position;
            this.m_sourceRect = sourceRect;
            this.m_state = state;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            if(m_state == SpriteStates.Solid)
            {
                Rectangle tileRect = new Rectangle((int)Position.X, (int)Position.Y,
                    m_sourceRect.Width, m_sourceRect.Height);

                Rectangle playerRect = new Rectangle((int)player.Image.Position.X,
                    (int)player.Image.Position.Y, player.Image.SourceRect.Width,
                    player.Image.SourceRect.Height);

                if (playerRect.Intersects(tileRect))
                {
                    if(player.Velocity.X < 0)
                    {
                        player.Image.Position.X = tileRect.Right;
                    }
                    else if (player.Velocity.X > 0)
                    {
                        player.Image.Position.X = tileRect.Left - player.Image.SourceRect.Width;
                    }
                    else if (player.Velocity.Y < 0)
                    {
                        player.Image.Position.Y = tileRect.Bottom;
                    }
                    else
                    {
                        player.Image.Position.Y = tileRect.Top - player.Image.SourceRect.Height;
                    }

                    player.Velocity = Vector2.Zero;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }


    }
}
