using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Rpgtutorial
{
     /// <summary>
     /// Controls transitions and the type of menu animation
     /// </summary>
     public class MenuControl
     {
          private Menu m_menu;
          private Boolean isTransitioning;

          public MenuControl()
          {
               m_menu = new Menu();
               m_menu.OnMenuChange += this.OnMenuChange;
          }

          public void LoadContent(String menuPath)
          {
               if (menuPath != String.Empty)
               {
                    m_menu.ID = menuPath;
               }
          }

          public void UnloadContent()
          {
               m_menu.UnloadContent();
          }

          public void Update(GameTime gameTime)
          {
               if (isTransitioning == false)
               {
                    m_menu.Update(gameTime);
               }

               if (Input.Instance.KeyPressed(Keys.Enter) && isTransitioning == false)
               {
                    if (m_menu.Items[m_menu.ItemNumber].LinkType == "Screen")
                    {
                         Screen.Instance.ChangeScreens(m_menu.Items[m_menu.ItemNumber].LinkID);
                    }
                    else
                    {
                         isTransitioning = true;
                         m_menu.Transition(1.0f);

                         foreach (MenuItem item in m_menu.Items)
                         {
                              item.Image.StoreEffects();
                              item.Image.ActivateEffect("FadeEffect");
                         }
                    }
               }
               Transition(gameTime);
          }

          public void Draw(SpriteBatch spriteBatch)
          {
               m_menu.Draw(spriteBatch);
          }

          private void OnMenuChange(object sender, EventArgs e)
          {
               Serializer<Menu> serializer = new Serializer<Menu>();
               m_menu.UnloadContent();

               m_menu = serializer.Load(m_menu.ID);
               m_menu.LoadContent();
               m_menu.OnMenuChange += this.OnMenuChange;
               m_menu.Transition(0.0f);

               foreach (MenuItem item in m_menu.Items)
               {
                    item.Image.StoreEffects();
                    item.Image.ActivateEffect("FadeEffect");
               }
          }

          private void Transition(GameTime gameTime)
          {
               if (isTransitioning)
               {
                    for (int i = 0; i < m_menu.Items.Count; i++)
                    {
                         m_menu.Items[i].Image.Update(gameTime);
                         float first = m_menu.Items[0].Image.Alpha;
                         float last = m_menu.Items[m_menu.Items.Count - 1].Image.Alpha;

                         if (first == 0.0f && last == 0.0f)
                         {
                              m_menu.ID = m_menu.Items[m_menu.ItemNumber].LinkID;
                         }
                         else if (first == 1.0f && last == 1.0f)
                         {
                              isTransitioning = false;
                              foreach (MenuItem item in m_menu.Items)
                              {
                                   item.Image.RestoreEffects();
                              }
                         }
                    }
               }
          }
     }
}