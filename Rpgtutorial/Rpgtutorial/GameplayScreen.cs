﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rpgtutorial
{
    /// <summary>
    /// Gameplay screen.  Loads player data and map data.
    /// When this is loaded the gameplay starts.
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        private Player m_player;
        private Map m_map;

        public override void LoadContent()
        {
            base.LoadContent();

            Serializer<Player> playerLoader = new Serializer<Player>();
            Serializer<Map> mapLoader = new Serializer<Map>();
            //TODO: DEBUG data, change later
            m_player = playerLoader.Load("Content/Load/Gameplay/Player.xml");
            m_map = mapLoader.Load("Content/Load/Gameplay/Maps/Map1.xml");
            m_player.LoadContent();
            m_map.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            m_player.UnloadContent();
            m_map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_player.Update(gameTime);
            m_map.Update(gameTime, ref m_player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            m_map.Draw(spriteBatch, DrawType.Underlay);
            m_player.Draw(spriteBatch);
            m_map.Draw(spriteBatch, DrawType.Overlay);
        }
    }
}