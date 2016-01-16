using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rpgtutorial
{
    public class Layer
    {

        public class TileMap
        {
            [XmlElement("Row")]
            public List<String> Row;

            public TileMap()
            {
                Row = new List<String>();
            }
        }

        [XmlElement("TileMap")]
        public TileMap Tile;
        public Image Image;
        public String SolidTiles;
        public String OverlayTiles;

        private SpriteStates m_state;
        private List<Tile> m_underLayTiles;
        private List<Tile> m_overlayTiles;

            
    
        public Layer()
        {
            Image = new Image();
            m_underLayTiles = new List<Tile>();
            m_overlayTiles = new List<Tile>();
            SolidTiles = String.Empty;
            OverlayTiles = String.Empty;
        }

        public void LoadContent(Vector2 tileDimensions)
        {
            Image.LoadContent();
            Vector2 position = -tileDimensions;

            foreach(String row in this.Tile.Row)
            {
                String[] split = row.Split(']');
                position.X = -tileDimensions.X;
                position.Y += tileDimensions.Y;
                foreach(String s in split)
                {
                    
                    if(s != String.Empty)
                    {
                        position.X += tileDimensions.X;
                        if (s.Contains("x") == false)
                        {
                            m_state = SpriteStates.Passive;
                            Tile tile = new Rpgtutorial.Tile();

                            String str = s.Replace("[", String.Empty);
                            int value1 = int.Parse(str.Substring(0, str.IndexOf(':')));
                            int value2 = int.Parse(str.Substring(str.IndexOf(':') + 1));

                            if(SolidTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
                            {
                                m_state = SpriteStates.Solid;
                            }

                            tile.LoadContent(position, new Rectangle(
                                value1 * (int)tileDimensions.X, value2 * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), m_state);

                            if(OverlayTiles.Contains("[" + value1.ToString() + ":" + value2.ToString() + "]"))
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
            foreach(Tile tile in m_underLayTiles)
            {
                tile.Update(gameTime, ref player);
            }

            foreach(Tile tile in m_overlayTiles)
            {
                tile.Update(gameTime, ref player);
            }
        }

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
            
            foreach(Tile tile in tiles)
            {
                Image.Position = tile.Position;
                Image.SourceRect = tile.SourceRect;
                Image.Draw(spriteBatch);
            }
        }

    }
}
