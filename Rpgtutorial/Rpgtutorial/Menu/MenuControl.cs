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
          private String m_activeEffect;
          private Boolean m_isTransitioning;

          /// <summary>
          /// Default constructor, creates a new menu.
          /// </summary>
          public MenuControl()
          {
               m_menu = new Menu();
               m_menu.OnMenuChange += this.OnMenuChange;
               m_activeEffect = typeof(FadeEffect).Name;
               m_isTransitioning = false;
          }

          /// <summary>
          /// Loads the menu content from the menu path.
          /// </summary>
          /// <param name="menuPath"></param>
          public void LoadContent(String menuPath)
          {
               if (menuPath != String.Empty)
               {
                    m_menu.ID = menuPath;
               }
          }

          /// <summary>
          /// Unloads content from menu.
          /// </summary>
          public void UnloadContent()
          {
               m_menu.UnloadContent();
          }

          /// <summary>
          /// Updates menu position and transition effects
          /// </summary>
          /// <param name="gameTime"></param>
          public void Update(GameTime gameTime)
          {
               if (m_isTransitioning == false)
               {
                    m_menu.Update(gameTime);
               }

               if (Input.Instance.KeyPressed(Keys.Enter) && m_isTransitioning == false)
               {
                    if (m_menu.Items[m_menu.ItemNumber].LinkType.Equals(typeof(Screen).Name))
                    {
                         Screen.Instance.ChangeScreens(m_menu.Items[m_menu.ItemNumber].LinkID);
                    }
                    else
                    {
                         m_isTransitioning = true;
                         m_menu.Transition(1.0f);

                         foreach (MenuItem item in m_menu.Items)
                         {
                              item.Image.StoreEffects();
                              item.Image.ActivateEffect(m_activeEffect);
                         }
                    }
               }
               Transition(gameTime);
          }

          /// <summary>
          /// Draws the menu using a spritebatch
          /// </summary>
          /// <param name="spriteBatch"></param>
          public void Draw(SpriteBatch spriteBatch)
          {
               m_menu.Draw(spriteBatch);
          }

          /// <summary>
          /// When the menu changes, unloads the content and
          /// loads the content for the menu selection.
          /// </summary>
          /// <param name="sender"></param>
          /// <param name="e"></param>
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
                    item.Image.ActivateEffect(m_activeEffect);
               }
          }

          /// <summary>
          /// Sets the transition effect based on the items and the
          /// gametime.
          /// </summary>
          /// <param name="gameTime"></param>
          private void Transition(GameTime gameTime)
          {
               if (m_isTransitioning)
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
                              m_isTransitioning = false;
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