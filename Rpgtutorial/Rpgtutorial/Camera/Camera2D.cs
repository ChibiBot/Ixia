#region Version History (1.0)

// From http://www.dreamincode.net/forums/topic/237979-2d-camera-in-xna/
// Quick note: if you want to use this, pass in GraphicsDevice.Viewport to the Camera constructor.

// Camera2D cam = new Camera2D(GraphicsDevice.Viewport);

// 03.07.11 ~ Created

#endregion Version History (1.0)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rpgtutorial
{
     /// <summary>
     /// Simple two dimensional camera.   Includes zooming, panning, and rotation.
     /// </summary>
     public class Camera2D
     {
          #region Fields

          protected float _zoom;
          protected Matrix _transform;
          protected Matrix _inverseTransform;
          protected Vector2 _pos;
          protected float _rotation;
          protected Viewport _viewport;
          protected MouseState _mState;
          protected KeyboardState _keyState;
          protected Int32 _scroll;

          #endregion Fields

          #region Properties

          public float Zoom
          {
               get { return _zoom; }
               set { _zoom = value; }
          }

          /// <summary>
          /// Camera View Matrix Property
          /// </summary>
          public Matrix Transform
          {
               get { return _transform; }
               set { _transform = value; }
          }

          /// <summary>
          /// Inverse of the view matrix, can be used to get objects screen coordinates
          /// from its object coordinates
          /// </summary>
          public Matrix InverseTransform
          {
               get { return _inverseTransform; }
          }

          /// <summary>
          ///
          /// </summary>
          public Vector2 Pos
          {
               get { return _pos; }
               set { _pos = value; }
          }

          public float Rotation
          {
               get { return _rotation; }
               set { _rotation = value; }
          }

          #endregion Properties

          #region Constructor

          public Camera2D(Viewport viewport)
          {
               _zoom = 1.0f;
               _scroll = 1;
               _rotation = 0.0f;
               _pos = Vector2.Zero;
               _viewport = viewport;
          }

          #endregion Constructor

          #region Methods

          /// <summary>
          /// Update the camera view
          /// </summary>
          public void Update()
          {
               //Call Camera Input
               Input();
               //Clamp zoom value
               _zoom = MathHelper.Clamp(_zoom, 0.0f, 10.0f);
               //Clamp rotation value
               _rotation = ClampAngle(_rotation);
               //Create view matrix
               _transform = Matrix.CreateRotationZ(_rotation) *
                               Matrix.CreateScale(new Vector3(_zoom, _zoom, 1)) *
                               Matrix.CreateTranslation(_pos.X, _pos.Y, 0);
               //Update inverse matrix
               _inverseTransform = Matrix.Invert(_transform);
          }

          public Matrix get_transformation(GraphicsDevice graphicsDevice)
          {
               _transform =       // Thanks to o KB o for this solution
                 Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                                            Matrix.CreateRotationZ(Rotation) *
                                            Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                            Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
               return _transform;
          }

          /// <summary>
          /// Example Input Method, rotates using cursor keys and zooms using mouse wheel
          /// </summary>
          protected virtual void Input()
          {
               _mState = Mouse.GetState();
               _keyState = Keyboard.GetState();
               //Check zoom
               if (_mState.ScrollWheelValue > _scroll)
               {
                    _zoom += 0.1f;
                    _scroll = _mState.ScrollWheelValue;
               }
               else if (_mState.ScrollWheelValue < _scroll)
               {
                    _zoom -= 0.1f;
                    _scroll = _mState.ScrollWheelValue;
               }
               //Check rotation
               if (_keyState.IsKeyDown(Keys.Left))
               {
                    _rotation -= 0.1f;
               }
               if (_keyState.IsKeyDown(Keys.Right))
               {
                    _rotation += 0.1f;
               }
               //Check Move
               if (_keyState.IsKeyDown(Keys.A))
               {
                    _pos.X += 0.5f;
               }
               if (_keyState.IsKeyDown(Keys.D))
               {
                    _pos.X -= 0.5f;
               }
               if (_keyState.IsKeyDown(Keys.W))
               {
                    _pos.Y += 0.5f;
               }
               if (_keyState.IsKeyDown(Keys.S))
               {
                    _pos.Y -= 0.5f;
               }
          }

          /// <summary>
          /// Clamps a radian value between -pi and pi
          /// </summary>
          /// <param name="radians">angle to be clamped</param>
          /// <returns>clamped angle</returns>
          protected float ClampAngle(float radians)
          {
               while (radians < -MathHelper.Pi)
               {
                    radians += MathHelper.TwoPi;
               }
               while (radians > MathHelper.Pi)
               {
                    radians -= MathHelper.TwoPi;
               }
               return radians;
          }

          #endregion Methods
     }
}