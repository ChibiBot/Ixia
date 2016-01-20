using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpgtutorial
{
     /// <summary>
     /// The player represention for the game
     /// Contains movespeed and velocity as well as
     /// the player image.
     /// </summary>
     public class Player
     {
          public Image Image;
          public Vector2 Velocity;
          public float MoveSpeed;
          public int MoveIncrement = 16;

          /// <summary>
          /// Constructor defaults the velocity to zero
          /// </summary>
          public Player()
          {
               Velocity = Vector2.Zero;
          }

          /// <summary>
          /// Loads the image content
          /// </summary>
          public void LoadContent()
          {
               Image.LoadContent();
          }

          /// <summary>
          /// Unloads the image content
          /// </summary>
          public void UnloadContent()
          {
               Image.UnloadContent();
          }

          /// <summary>
          /// Updates the player position based on keyboard
          /// input
          /// </summary>
          /// <param name="gameTime"></param>
          public void Update(GameTime gameTime)
          {
               // Vertical Movement
               Image.IsActive = true;
               if (Velocity.X == 0)
               {
                    if (Input.Instance.KeyDown(Keys.Down))
                    {
                         Velocity.Y = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                         Image.SpriteSheetEffect.CurrentFrame.Y = 0;
                    }
                    else if (Input.Instance.KeyDown(Keys.Up))
                    {
                         Velocity.Y = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                         Image.SpriteSheetEffect.CurrentFrame.Y = 3;
                    }
                    // Don't Move
                    else
                    {
                         Velocity.Y = 0;
                    }
               }

               // Horizontal Movement
               if (Velocity.Y == 0)
               {
                    if (Input.Instance.KeyDown(Keys.Right))
                    {
                         Velocity.X = MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                         Image.SpriteSheetEffect.CurrentFrame.Y = 2;
                    }
                    else if (Input.Instance.KeyDown(Keys.Left))
                    {
                         Velocity.X = -MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                         Image.SpriteSheetEffect.CurrentFrame.Y = 1;
                    }
                    // Don't Move
                    else
                    {
                         Velocity.X = 0;
                    }
               }

               if (Velocity.X == 0 && Velocity.Y == 0)
               {
                    Image.IsActive = false;
               }

               Image.Update(gameTime);

               // Move character
               Image.Position += Velocity;
          }

          /// <summary>
          /// Renders the player sprite
          /// </summary>
          /// <param name="spriteBatch"></param>
          public void Draw(SpriteBatch spriteBatch)
          {
               Image.Draw(spriteBatch);
          }
     }
}