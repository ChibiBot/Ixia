using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rpgtutorial
{
    /// <summary>
    /// Image class containing graphics and text with render information
    /// </summary>
    public class Image
    {
        // Get the name of this class's name space for parsing
        private static readonly string NAMESPACE_PREFIX = typeof(Image).Namespace;

        //Public variables
        [XmlIgnore]
        public Texture2D Texture;

        public float Alpha;
        public String Text;
        public String FontName;
        public String Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public String Effects;
        public bool IsActive;
        public FadeEffect FadeEffect;
        public SpriteSheetEffect SpriteSheetEffect;

        //Member variables
        private Vector2 m_origin;
        private ContentManager m_content;
        private RenderTarget2D m_renderTarget;
        private SpriteFont m_font;
        private Dictionary<String, ImageEffect> m_effectList;

        public Image()
        {
            Path = String.Empty;
            Text = String.Empty;
            Effects = String.Empty;
            FontName = "Fonts/Arial";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            SourceRect = Rectangle.Empty;
            m_effectList = new Dictionary<String, ImageEffect>();
        }

        /// <summary>
        /// Stores an effect into the respective effect key in
        /// order for it to be loaded at a later time.
        /// </summary>
        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in m_effectList)
            {
                if (effect.Value.IsActive)
                {
                    Effects += effect.Key + ":";
                }
            }

            if (Effects != String.Empty)
            {
                Effects.Remove(Effects.Length - 1);
            }
        }

        /// <summary>
        /// Loads an effect from its respective effect key.
        /// </summary>
        public void RestoreEffects()
        {
            foreach (var effect in m_effectList)
            {
                DeactivateEffect(effect.Key);
            }

            String[] split = Effects.Split(':');

            foreach (String s in split)
            {
                ActivateEffect(s);
            }
        }

        /// <summary>
        /// Loads the Content
        /// </summary>
        public void LoadContent()
        {
            m_content = new ContentManager(
                Screen.Instance.Content.ServiceProvider, "Content");

            // If the path is present load the texture from the path.
            if (Path != String.Empty)
            {
                Texture = m_content.Load<Texture2D>(Path);
            }
            // Throw an exception since this should never happen.
            else
            {
                throw new ArgumentException("No path for texture to be loaded.");
            }

            m_font = m_content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            // If the texture is present set the X dimensions to the width of the texture or
            // text whichever is larger.
            if (Texture != null)
            {
                dimensions.X += Math.Max(Texture.Width, m_font.MeasureString(Text).X);
            }
            // Else set the dimensions to the width of the text.
            else
            {
                dimensions.X += m_font.MeasureString(Text).X;
            }

            // If hte texture is present set the Y dimensions to the height of the texture or
            // text whichever is larger
            if (Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, m_font.MeasureString(Text).Y);
            }
            // Set the dimensions to the height of the text
            else
            {
                dimensions.Y = m_font.MeasureString(Text).Y;
            }

            if (SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            m_renderTarget = new RenderTarget2D(Screen.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);

            Screen.Instance.GraphicsDevice.SetRenderTarget(m_renderTarget);
            Screen.Instance.GraphicsDevice.Clear(Color.Transparent);
            Screen.Instance.SpriteBatch.Begin();

            if (Texture != null)
            {
                Screen.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }
            Screen.Instance.SpriteBatch.DrawString(m_font, Text, Vector2.Zero, Color.White);
            Screen.Instance.SpriteBatch.End();

            Texture = m_renderTarget;

            Screen.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

            if (Effects != String.Empty)
            {
                String[] effectSplit = Effects.Split(':');
                foreach (String effect in effectSplit)
                {
                    ActivateEffect(effect);
                }
            }
        }

        /// <summary>
        /// Unloads the content from memory and deactivates the effect
        /// </summary>
        public void UnloadContent()
        {
            m_content.Unload();
            foreach (var effect in m_effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        /// <summary>
        /// Updates the current effect in respect to the gametime.
        /// </summary>
        /// <param name="gameTime">The current game cycle.</param>
        public void Update(GameTime gameTime)
        {
            foreach (var effect in m_effectList)
            {
                if (effect.Value.IsActive)
                {
                    effect.Value.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws the image using a spritebatch
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            m_origin = new Vector2(
                SourceRect.Width / 2,
                SourceRect.Height / 2);

            spriteBatch.Draw(
                Texture,
                Position,
                SourceRect,
                Color.White * Alpha,
                0.0f,
                m_origin,
                Scale,
                SpriteEffects.None,
                0.0f);
        }

        /// <summary>
        /// Starts an effect based on the string passed.
        ///
        /// </summary>
        /// <param name="effect"></param>
        public void ActivateEffect(String effect)
        {
            if (m_effectList.ContainsKey(effect))
            {
                m_effectList[effect].IsActive = true;
                var obj = this;
                m_effectList[effect].LoadContent(ref obj);
            }
            // Else throw an exception
            else
            {
                throw new ArgumentException("Effect " + effect + " is not in the effect list");
            }
        }

        /// <summary>
        /// Deactivates an effect with the name passed.  If the effect
        /// is already deactivated, then does nothing.
        /// </summary>
        /// <param name="effect"></param>
        public void DeactivateEffect(String effect)
        {
            if (m_effectList.ContainsKey(effect))
            {
                m_effectList[effect].IsActive = false;
                m_effectList[effect].UnloadContent();
            }
            // Else do nothing, this is acceptable
        }

        /// <summary>
        /// Sets the effect passed by loading it and adding it to the effect list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="effect"></param>
        private void SetEffect<T>(ref T effect)
        {
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            // Parse the effect and add it to effects list
            m_effectList.Add(effect.GetType().ToString().Replace(
                NAMESPACE_PREFIX + ".", ""), (effect as ImageEffect));
        }
    }
}