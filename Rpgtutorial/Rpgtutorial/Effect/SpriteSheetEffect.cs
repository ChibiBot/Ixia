using Microsoft.Xna.Framework;

namespace Rpgtutorial
{
     /// <summary>
     /// Combines multiple areas of a sprite sheet to create an
     /// effect and animates it based on the frames within.
     /// </summary>
     public class SpriteSheetEffect : ImageEffect
     {
          public int FrameCounter;
          public int SwitchFrame;
          public Vector2 CurrentFrame;
          public Vector2 AmountOfFrames;

          public int FrameWidth
          {
               get
               {
                    if (Image.Texture != null)
                    {
                         return Image.Texture.Width / (int)AmountOfFrames.X;
                    }
                    return 0;
               }
          }

          public int FrameHeight
          {
               get
               {
                    if (Image.Texture != null)
                    {
                         return Image.Texture.Height / (int)AmountOfFrames.Y;
                    }
                    return 0;
               }
          }

          /// <summary>
          /// Default constructor for a sprite sheet effect
          /// </summary>
          public SpriteSheetEffect()
          {
               AmountOfFrames = new Vector2(3, 4);
               CurrentFrame = new Vector2(1, 0);
               SwitchFrame = 100;
               FrameCounter = 0;
          }

          /// <summary>
          /// Loads the image
          /// </summary>
          /// <param name="image"></param>
          public override void LoadContent(ref Image image)
          {
               base.LoadContent(ref image);
          }

          /// <summary>
          /// Unloads the image
          /// </summary>
          public override void UnloadContent()
          {
               base.UnloadContent();
          }

          /// <summary>
          /// Updates the animation based on gametime
          /// </summary>
          /// <param name="gameTime"></param>
          public override void Update(GameTime gameTime)
          {
               base.Update(gameTime);
               if (Image.IsActive)
               {
                    FrameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (FrameCounter >= SwitchFrame)
                    {
                         FrameCounter = 0;
                         CurrentFrame.X++;

                         if (CurrentFrame.X * FrameWidth >= Image.Texture.Width)
                         {
                              CurrentFrame.X = 0;
                         }
                    }
               }
               else
               {
                    CurrentFrame.X = 1;
               }

               Image.SourceRect = new Rectangle((int)CurrentFrame.X * FrameWidth,
                   (int)CurrentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
          }
     }
}