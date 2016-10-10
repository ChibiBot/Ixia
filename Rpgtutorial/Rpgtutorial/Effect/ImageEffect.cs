using Microsoft.Xna.Framework;

namespace Rpgtutorial
{
     /// <summary>
     /// Parent class for image effects.
     /// </summary>
     public class ImageEffect
     {
          public bool IsActive;

          protected Image Image;

          /// <summary>
          /// Default constructor, sets active to false
          /// </summary>
          public ImageEffect()
          {
               IsActive = false;
          }

          /// <summary>
          /// Loads the content from the pipeline
          /// </summary>
          /// <param name="image"></param>
          public virtual void LoadContent(ref Image image)
          {
               this.Image = image;
          }

          /// <summary>
          /// Unloads content from the pipeline.
          /// </summary>
          public virtual void UnloadContent()
          {
               // TODO: Unload content.
          }

          /// <summary>
          /// Updates the effect in regards to gametime.
          /// </summary>
          /// <param name="gameTime"></param>
          public virtual void Update(GameTime gameTime)
          {
               // TODO: Update effect.
          }
     }
}