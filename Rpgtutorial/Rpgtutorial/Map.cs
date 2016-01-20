using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rpgtutorial
{
     public class Map
     {
          [XmlElement("Layer")]
          public List<Layer> Layer;

          public Vector2 TileDimensions;

          public Map()
          {
               Layer = new List<Layer>();
               TileDimensions = Vector2.Zero;
          }

          public void LoadContent()
          {
               foreach (Layer tile in Layer)
               {
                    tile.LoadContent(TileDimensions);
               }
          }

          public void UnloadContent()
          {
               foreach (Layer tile in Layer)
               {
                    tile.UnloadContent();
               }
          }

          public void Update(GameTime gameTime, ref Player player)
          {
               foreach (Layer tile in Layer)
               {
                    tile.Update(gameTime, ref player);
               }
          }

          public void Draw(SpriteBatch spriteBatch, DrawType drawType)
          {
               foreach (Layer tile in Layer)
               {
                    tile.Draw(spriteBatch, drawType);
               }
          }
     }
}