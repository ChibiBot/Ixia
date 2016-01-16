using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// Generic game screen with transitions.
    /// 
    /// </summary>
    public class Screen
    {
        private const int m_WIDTH = 640;
        private const int m_HEIGHT = 480;
        private static Screen m_INSTANCE;

        [XmlIgnore]
        public Vector2 Dimensions {private set; get;}
        [XmlIgnore]
        public ContentManager Content { private set; get; }  
        [XmlIgnore]      
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public SpriteBatch SpriteBatch;
        public Image Image;
        [XmlIgnore]
        public bool IsTransitioning { get; private set; }

        private Serializer<GameScreen> m_xmlGameScreen;
        private GameScreen m_currentScreen;
        private GameScreen m_newScreen;

        /// <summary>
        /// Returns the singleton instance of 
        /// the screen.
        /// </summary>
        public static Screen Instance
        {
            get
            {
                if(m_INSTANCE == null)
                {
                    Serializer<Screen> xml = new Serializer<Screen>();
                    m_INSTANCE = xml.Load("Content/Load/Screen.xml");
                }

                return m_INSTANCE;
            }
        }

        /// <summary>
        /// Changes screen to the screenname passed in
        /// </summary>
        /// <param name="screenName"></param>
        public void ChangeScreens(String screenName)
        {
            m_newScreen = (GameScreen)Activator.CreateInstance(Type.GetType("Rpgtutorial." + screenName));
            Image.IsActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            IsTransitioning = true;
        }

        /// <summary>
        /// Processes the transition based on gamet
        /// </summary>
        /// <param name="gameTime"></param>
        private void Transistion(GameTime gameTime)
        {
            if(IsTransitioning)
            {
                Image.Update(gameTime);
                if(Image.Alpha == 1.0f)
                {
                    m_currentScreen.UnloadContent();
                    m_currentScreen = m_newScreen;
                    m_xmlGameScreen.Type = m_currentScreen.Type;
                    if(File.Exists(m_currentScreen.XmlPath))
                    {
                        m_currentScreen = m_xmlGameScreen.Load(m_currentScreen.XmlPath);
                    }
                    m_currentScreen.LoadContent();
                    
                }
                else if(Image.Alpha == 0.0f)
                {
                    Image.IsActive = false;
                    IsTransitioning = false;
                }
            }
        }

        /// <summary>
        /// Loads content from the content manager for
        /// the current screen
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            m_currentScreen.LoadContent();
            Image.LoadContent();
        }

        /// <summary>
        /// Unloads content
        /// </summary>
        public void UnloadContent()
        {
            m_currentScreen.UnloadContent();
            Image.UnloadContent();
        }

        /// <summary>
        /// Updates the transition based on gametime
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            m_currentScreen.Update(gameTime);
            this.Transistion(gameTime);
        }

        /// <summary>
        /// Draws the sprites and updates the transition image
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            m_currentScreen.Draw(spriteBatch);
            if(IsTransitioning)
            {
                Image.Draw(spriteBatch);
            }
        }

        private Screen()
        {
            //Singleton class hiding constructor
            Dimensions = new Vector2(m_WIDTH, m_HEIGHT);
            m_currentScreen = new GameplayScreen();
            m_xmlGameScreen = new Serializer<GameScreen>();
            m_xmlGameScreen.Type = m_currentScreen.Type;
           // m_currentScreen = m_xmlGameScreen.Load("Content/Load/SplashScreen.xml");
        }

    }
}
