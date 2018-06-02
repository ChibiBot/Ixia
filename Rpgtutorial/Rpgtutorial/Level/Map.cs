using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// This class represents the level or play area for the
    /// game.
    /// </summary>
    public class Map
    {
        [XmlElement("Layer")]
        public List<Layer> Layer;

        public Vector2 TileDimensions;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Map()
        {
            Layer = new List<Layer>();
            TileDimensions = Vector2.Zero;
        }

        /// <summary>
        /// Load each tile layer
        /// </summary>
        public void LoadContent()
        {
            foreach (Layer tile in Layer)
            {
                tile.LoadContent(TileDimensions);
            }
        }

        /// <summary>
        /// Calls the tile unloadcontent method.
        /// </summary>
        public void UnloadContent()
        {
            foreach (Layer tile in Layer)
            {
                tile.UnloadContent();
            }
        }

        /// <summary>
        /// Updates the tiles in reference to the player and
        /// gametime.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (Layer tile in Layer)
            {
                tile.Update(gameTime, ref player);
            }
        }

        /// <summary>
        /// Draws the layers using a spritebatch and drawtype.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="drawType"></param>
        public void Draw(SpriteBatch spriteBatch, DrawType drawType)
        {
            foreach (Layer tile in Layer)
            {
                tile.Draw(spriteBatch, drawType);
            }
        }
    }
}