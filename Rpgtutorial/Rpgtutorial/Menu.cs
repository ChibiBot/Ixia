using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Rpgtutorial
{
     public class Menu
     {
          public event EventHandler OnMenuChange;

          public String Axis;
          public String Effects;

          [XmlElement("Item")]
          public List<MenuItem> Items;

          private int m_itemNumber;
          private String m_id;

          public int ItemNumber
          {
               get { return m_itemNumber; }
          }

          public String ID
          {
               get { return m_id; }
               set
               {
                    m_id = value;
                    OnMenuChange(this, null);
               }
          }

          public Menu()
          {
               m_id = String.Empty;
               m_itemNumber = 0;
               Effects = String.Empty;
               Axis = "Y";
               Items = new List<MenuItem>();
          }

          public void Transition(float alpha)
          {
               foreach (MenuItem item in Items)
               {
                    item.Image.IsActive = true;
                    item.Image.Alpha = alpha;
                    if (alpha == 0.0f)
                    {
                         item.Image.FadeEffect.Increase = true;
                    }
                    else
                    {
                         item.Image.FadeEffect.Increase = false;
                    }
               }
          }

          public void LoadContent()
          {
               String[] split = Effects.Split(':');
               foreach (MenuItem item in Items)
               {
                    item.Image.LoadContent();
                    foreach (String s in split)
                    {
                         item.Image.ActivateEffect(s);
                    }
                    this.AlignMenuItems();
               }
          }

          public void UnloadContent()
          {
               foreach (MenuItem item in Items)
               {
                    item.Image.UnloadContent();
               }
          }

          public void Update(GameTime gameTime)
          {
               if (Axis == "X")
               {
                    if (Input.Instance.KeyPressed(Keys.Right))
                    {
                         m_itemNumber++;
                    }
                    else if (Input.Instance.KeyPressed(Keys.Left))
                    {
                         m_itemNumber--;
                    }
               }
               else if (Axis == "Y")
               {
                    if (Input.Instance.KeyPressed(Keys.Down))
                    {
                         m_itemNumber++;
                    }
                    else if (Input.Instance.KeyPressed(Keys.Up))
                    {
                         m_itemNumber--;
                    }
               }

               if (m_itemNumber < 0)
               {
                    m_itemNumber = Items.Count - 1;
               }
               else if (m_itemNumber > Items.Count - 1)
               {
                    m_itemNumber = 0;
               }

               for (int i = 0; i < Items.Count; i++)
               {
                    if (i == m_itemNumber)
                    {
                         Items[i].Image.IsActive = true;
                    }
                    else
                    {
                         Items[i].Image.IsActive = false;
                    }

                    Items[i].Image.Update(gameTime);
               }
          }

          public void Draw(SpriteBatch spriteBatch)
          {
               foreach (MenuItem item in Items)
               {
                    item.Image.Draw(spriteBatch);
               }
          }

          private void AlignMenuItems()
          {
               Vector2 dimensions = Vector2.Zero;
               foreach (MenuItem item in Items)
               {
                    dimensions += new Vector2(item.Image.SourceRect.Width,
                        item.Image.SourceRect.Height);
               }

               dimensions = new Vector2((Screen.Instance.Dimensions.X -
                   dimensions.X) / 2, (Screen.Instance.Dimensions.Y - dimensions.Y) / 2);

               foreach (MenuItem item in Items)
               {
                    if (Axis == "X")
                    {
                         item.Image.Position = new Vector2(dimensions.X,
                             (Screen.Instance.Dimensions.Y - item.Image.SourceRect.Height) / 2);
                    }
                    else if (Axis == "Y")
                    {
                         item.Image.Position = new Vector2((Screen.Instance.Dimensions.X -
                             item.Image.SourceRect.Width) / 2 + (Screen.Instance.Dimensions.X * 0.45f), dimensions.Y + (Screen.Instance.Dimensions.Y / 3));
                    }

                    dimensions += new Vector2(item.Image.SourceRect.Width,
                        item.Image.SourceRect.Height);
               }
          }
     }
}