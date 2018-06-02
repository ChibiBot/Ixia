using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rpgtutorial.Util;
using System;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// Generic game screen.  Contains the xml path and loads the screen into the content manager.
    ///
    /// </summary>
    public class GameScreen
    {
        protected ContentManager content;
        public string XmlPath;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameScreen()
        {
            XmlPath = FileUtils.getLoadPath(this.GetType());
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(
                Screen.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            Input.Instance.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}