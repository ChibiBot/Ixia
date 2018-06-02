using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpgtutorial
{
    /// <summary>
    /// Enum for overlays and undarlays.
    /// </summary>
    public enum DrawType
    {
        Overlay,
        Underlay
    };

    /// <summary>
    /// Tiles consist of graphics and dimensions with
    /// sprite states.  Multiple tiles can be placed together
    /// to form tilesheets in layers.
    /// </summary>
    public class Tile
    {
        private Vector2 m_position;
        private Rectangle m_sourceRect;
        private SpriteStates m_state;

        /// <summary>
        /// Gets a rectangle containing the dimensions of
        /// the tile.
        /// </summary>
        public Rectangle SourceRect
        {
            get { return m_sourceRect; }
        }

        /// <summary>
        /// Gets the position vector
        /// </summary>
        public Vector2 Position
        {
            get { return m_position; }
        }

        /// <summary>
        /// Loads the content from the pipeline
        /// </summary>
        /// <param name="position"></param>
        /// <param name="sourceRect"></param>
        /// <param name="state"></param>
        public void LoadContent(Vector2 position, Rectangle sourceRect, SpriteStates state)
        {
            this.m_position = position;
            this.m_sourceRect = sourceRect;
            this.m_state = state;
        }

        /// <summary>
        /// Unloads content from the pipeline
        /// </summary>
        public void UnloadContent()
        {
            //!TODO: implement this between areas.
        }

        /// <summary>
        /// Updates the how the tile is rendered in regards to
        /// the player.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public void Update(GameTime gameTime, ref Player player)
        {
            // Only check this when the tile is solid.
            if (m_state == SpriteStates.Solid)
            {
                Rectangle tileRect = new Rectangle((int)Position.X, (int)Position.Y,
                    m_sourceRect.Width, m_sourceRect.Height);

                Rectangle playerRect = new Rectangle((int)player.Image.Position.X,
                    (int)player.Image.Position.Y, player.Image.SourceRect.Width,
                    player.Image.SourceRect.Height);

                // Set the players position back to the normalized
                // edge of the tile.
                if (playerRect.Intersects(tileRect))
                {
                    if (player.Velocity.X < 0)
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

                    // Stop the player.
                    player.Velocity = Vector2.Zero;
                }
            }
        }

        /// <summary>
        /// Draws the individual tile
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // TODO: Implement this
        }
    }
}