using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpgtutorial
{

    /// <summary>
    /// Main Menu screen for the game
    /// </summary>
    public class TitleScreen : GameScreen
    {
        private MenuControl m_menuControl;

        /// <summary>
        /// Default constructor, initalizes menu control
        /// </summary>
        public TitleScreen()
        {
            m_menuControl = new MenuControl();
        }

        /// <summary>
        /// Basic loader
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            m_menuControl.LoadContent("Content/Load/Menus/TitleMenu.xml");
        }

        /// <summary>
        /// Basic unloader
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
            m_menuControl.UnloadContent();
        }

        /// <summary>
        /// Updates the menu control
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_menuControl.Update(gameTime);
        }

        /// <summary>
        /// Draws the menu
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            m_menuControl.Draw(spriteBatch);
        }
    }
}
