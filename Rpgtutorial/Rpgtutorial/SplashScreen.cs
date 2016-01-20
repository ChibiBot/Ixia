using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rpgtutorial
{
     /// <summary>
     /// Splash screen which displays the first image the player
     /// will see and the first impression of the game is derived
     /// from this screen.
     /// </summary>
     public class SplashScreen : GameScreen
     {
          public Image Image;

          /// <summary>
          /// Loads image
          /// </summary>
          public override void LoadContent()
          {
               base.LoadContent();
               Image.LoadContent();
          }

          /// <summary>
          /// Unloads Image
          /// </summary>
          public override void UnloadContent()
          {
               base.UnloadContent();
               Image.UnloadContent();
          }

          /// <summary>
          /// Updates Image
          /// </summary>
          /// <param name="gameTime"></param>
          public override void Update(GameTime gameTime)
          {
               base.Update(gameTime);
               Image.Update(gameTime);

               if (Input.Instance.KeyPressed(Keys.Enter, Keys.Z))
               {
                    Screen.Instance.ChangeScreens("TitleScreen");
               }
          }

          /// <summary>
          /// Draws Image
          /// </summary>
          /// <param name="spriteBatch"></param>
          public override void Draw(SpriteBatch spriteBatch)
          {
               Image.Draw(spriteBatch);
          }
     }
}