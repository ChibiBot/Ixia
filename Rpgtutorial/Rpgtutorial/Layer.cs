using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// Gameplay layer such as terrain, sprites, overlays... etc.
    /// </summary>
    public class Layer
    {
        /// <summary>
        /// Tile map consisting of images that link together to create
        /// a single flat image.  Renders as one image on the screen
        /// </summary>
        public class TileMap
        {
            [XmlElement("Row")]
            public List<String> Row = new List<String>();
        }

        [XmlElement("TileMap")]
        public TileMap Tile;

        public Image Image = new Image();
        public String SolidTiles = String.Empty;
        public String OverlayTiles = String.Empty;

        private SpriteStates m_state;
        private List<Tile> m_underLayTiles = new List<Tile>();
        private List<Tile> m_overlayTiles = new List<Tile>();

        /// <summary>
        /// Loads the tile dimensions with the pipeline
        /// </summary>
        /// <param name="tileDimensions"></param>
        public void LoadContent(Vector2 tileDimensions)
        {
            Image.LoadContent();
            Vector2 position = -tileDimensions;

            // Parse tile file by splitting it at every end bracket
            foreach (String row in this.Tile.Row)
            {
                String[] split = row.Split(']');
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;
                foreach (String s in split)
                {
                    if (s != String.Empty)
                    {
                        position.X += tileDimensions.X;
                        if (s.Contains("x") == false)
                        {
                            m_state = SpriteStates.Passive;
                            Tile tile = new Rpgtutorial.Tile();

                            String str = s.Replace("[", String.Empty);
                            int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

                            if (SolidTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                            {
                                m_state = SpriteStates.Solid;
                            }

                            tile.LoadContent(position, new Rectangle(
                                value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), m_state);

                            if (OverlayTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                            {
                                m_overlayTiles.Add(tile);
                            }
                            else
                            {
                                m_underLayTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unloads the layer content from the pipeline
        /// </summary>
        public void UnloadContent()
        {
            Image.UnloadContent();
        }

        /// <summary>
        ///
        /// Optimize this
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public void Update(GameTime gameTime, ref Player player)
        {
            foreach (Tile tile in m_underLayTiles)
            {
                tile.Update(gameTime, ref player);
            }

            foreach (Tile tile in m_overlayTiles)
            {
                tile.Update(gameTime, ref player);
            }
        }

        /// <summary>
        /// Draws the layer using a spritebatch and a specific draw type.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="drawType"></param>
        public void Draw(SpriteBatch spriteBatch, DrawType drawType)
        {
            List<Tile> tiles;
            if (drawType == DrawType.Underlay)
            {
                tiles = m_underLayTiles;
            }
            else if (drawType == DrawType.Overlay)
            {
                tiles = m_overlayTiles;
            }
            else { throw new ArgumentException("Unsupported DrawType"); }

            foreach (Tile tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }
    }
}