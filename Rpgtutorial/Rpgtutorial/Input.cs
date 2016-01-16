using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Rpgtutorial
{
    public class Input
    {
        private KeyboardState m_currentKeyState;
        private KeyboardState m_previousKeyState;

        private static Input m_INSTANCE;

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

        public void Update(GameTime gameTime)
        {
            m_previousKeyState = m_currentKeyState;
            if(Screen.Instance.IsTransitioning == false)
            {
                m_currentKeyState = Keyboard.GetState();
            }
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if(m_currentKeyState.IsKeyDown(key) && m_previousKeyState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach(Keys key in keys)
            {
                if (m_currentKeyState.IsKeyUp(key) && m_previousKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;            
        }

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
