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

    /// <summary>
    /// Image class containing graphics and text with render information
    /// </summary>
    public class Image
    {
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

        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in m_effectList)
            {
                if(effect.Value.IsActive)
                {
                    Effects += effect.Key + ":";
                }
            }

            if (Effects != String.Empty)
            {
                Effects.Remove(Effects.Length - 1);
            }
        }

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

            if(Path != String.Empty)
            {
                Texture = m_content.Load<Texture2D>(Path);
            }

            m_font = m_content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if(Texture != null)
            {
                dimensions.X += Math.Max(Texture.Width, m_font.MeasureString(Text).X);                
            }
            else
            {
                dimensions.X += m_font.MeasureString(Text).X;
            }           

            if(Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, m_font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = m_font.MeasureString(Text).Y;
            }

            if(SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            m_renderTarget = new RenderTarget2D(Screen.Instance.GraphicsDevice,
                (int)dimensions.X, (int)dimensions.Y);

            Screen.Instance.GraphicsDevice.SetRenderTarget(m_renderTarget);
            Screen.Instance.GraphicsDevice.Clear(Color.Transparent);
            Screen.Instance.SpriteBatch.Begin();
            if(Texture != null)
            {
                Screen.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
            }            
            Screen.Instance.SpriteBatch.DrawString(m_font, Text, Vector2.Zero, Color.White);
            Screen.Instance.SpriteBatch.End();

            Texture = m_renderTarget;

            Screen.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);
            SetEffect<SpriteSheetEffect>(ref SpriteSheetEffect);

            if(Effects != String.Empty)
            {
                String[] effectSplit = Effects.Split(':');
                foreach(String effect in effectSplit)
                {
                    ActivateEffect(effect);
                }
            }

        }

        public void UnloadContent()
        {
            m_content.Unload();
            foreach(var effect in m_effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

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

        public void ActivateEffect(String effect)
        {
            if(m_effectList.ContainsKey(effect))
            {
                m_effectList[effect].IsActive = true;
                var obj = this;
                m_effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(String effect)
        {
            if(m_effectList.ContainsKey(effect))
            {
                m_effectList[effect].IsActive = false;                
                m_effectList[effect].UnloadContent();
            }
        }

        private void SetEffect<T>(ref T effect)
        {
            if(effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                (effect as ImageEffect).IsActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            m_effectList.Add(effect.GetType().ToString().Replace("Rpgtutorial.", ""), (effect as ImageEffect));
        }
    }
}
