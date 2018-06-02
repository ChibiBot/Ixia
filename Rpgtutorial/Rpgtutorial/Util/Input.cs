using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rpgtutorial
{
    /// <summary>
    /// Singleton class that translates user input into game commands
    /// </summary>
    public class Input
    {
        private KeyboardState m_currentKeyState;
        private KeyboardState m_previousKeyState;

        private static Input m_INSTANCE;

        /// <summary>
        /// Returns an instance of the Input class
        /// </summary>
        public static Input Instance
        {
            get
            {
                if (m_INSTANCE == null)
                {
                    m_INSTANCE = new Input();
                }

                return m_INSTANCE;
            }
        }

        /// <summary>
        /// Updates the keystate each cycle and records the
        /// previous keystate
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            m_previousKeyState = m_currentKeyState;
            if (Screen.Instance.IsTransitioning == false)
            {
                m_currentKeyState = Keyboard.GetState();
            }
        }

        /// <summary>
        /// Returns true if the key is pressed this cycle
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (m_currentKeyState.IsKeyDown(key) && m_previousKeyState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if a down key has been released this cycle
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (m_currentKeyState.IsKeyUp(key) && m_previousKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if a key is down
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (m_currentKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}