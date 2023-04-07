using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src.AnaBanUI.Containers {
  public enum SideBySideCointainerLayoutFlow {
    Left
  }
  
  public class SideBySideContainer : ControlBase {
    public bool Lines { get; set; } = false;
    public float Margin { get; set; } = 0;
    public float Gap { get; set; } = 0;
    public List<ControlBase> Elements { get; set; } = new();
    public SideBySideCointainerLayoutFlow LayoutFlow { get; set; } = SideBySideCointainerLayoutFlow.Left;

    void RecalculateUI() {
      // Nothing to calculate
      if (Elements.Count == 0) { return; }

      if (LayoutFlow == SideBySideCointainerLayoutFlow.Left) {
        float lastX = 0;
        float weightedMinimumHeight = 0;
        
        foreach (ControlBase element in Elements) {
          element.Size = new Vector2(element.MinimumSize.X, Size.Y - (Margin * 2));
          element.RelativePosition = new Vector2(lastX + Margin, Margin);
          element.AbsolutePosition = new Vector2(AbsolutePosition.X + element.RelativePosition.X, AbsolutePosition.Y + element.RelativePosition.Y);

          lastX += element.Size.X + Gap;            

          if (element.MinimumSize.Y > weightedMinimumHeight) {
            weightedMinimumHeight = element.MinimumSize.Y;
          }
        }

        MinimumSize = new Vector2((lastX - Gap) + (Margin * 2), weightedMinimumHeight);

        if (Size.X > MinimumSize.X) {
          lastX = 0;
          weightedMinimumHeight = 0;

          foreach (ControlBase element in Elements) {            
            element.Size = new Vector2((Size.X - Gap) / Elements.Count - Margin, Size.Y - (Margin * 2));
            element.RelativePosition = new Vector2(lastX + Margin, Margin);
            element.AbsolutePosition = new Vector2(AbsolutePosition.X + element.RelativePosition.X, AbsolutePosition.Y + element.RelativePosition.Y);

            lastX += element.Size.X + Gap;

            if (element.MinimumSize.Y > weightedMinimumHeight) {
              weightedMinimumHeight = element.MinimumSize.Y;
            }
          }

        }


      }
      
    }

    void DrawElement(SpriteBatch spriteBatch, double deltaTime, ControlBase element) {

      if (!element.Visible) { return; }
      spriteBatch.End();
      spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(element.RelativePosition.X, element.RelativePosition.Y, 0));

      element.Draw(spriteBatch, deltaTime);
      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, element.Size.X, element.Size.Y), Color.Red);

      spriteBatch.End();
      spriteBatch.Begin();
    }


    public override void Draw(SpriteBatch spriteBatch, double deltaTime) {
      RecalculateUI();

      Viewport elementViewport = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, (int)Size.X, (int)Size.Y);
      Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;

      spriteBatch.End();
      spriteBatch.Begin();

      spriteBatch.GraphicsDevice.Viewport = elementViewport;

      //if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, MinimumSize.X, MinimumSize.Y), Color.Blue);

      // Draw Elements
      for (int i = 0; i < Elements.Count; i++) {
        DrawElement(spriteBatch, deltaTime, Elements[i]);
      }

      if (Lines) spriteBatch.DrawRectangle(new RectangleF(0, 0, Size.X, Size.Y), Color.Magenta);

      spriteBatch.End();

      //Restore SpriteBatch
      spriteBatch.GraphicsDevice.Viewport = oldViewport;

      spriteBatch.Begin();

    }

    public override void Update(double deltaTime) {
      // Nothing to Update
      if (Elements.Count == 0) { return; }

      for(int i = 0; i < Elements.Count; i++) {
        Elements[i].Update(deltaTime);
      }


    }
  }
}
