using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI.Containers {
  public enum SideBySideCointainerLayoutFlow {
    Left
  }
  
  public class SideBySideContainer : ContainerBase {
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

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      RecalculateUI();

      // Draw Elements
      for (int i = 0; i < Elements.Count; i++) {
        Elements[i].Draw(spriteBatch, deltaTime);
      }
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      // Nothing to Update
      if (Elements.Count == 0) { return false; }

      for (int i = 0; i < Elements.Count; i++) {
        if (Elements[i].InputUpdate(pointerEvent)) {
          return true;
        }
      }

      return false;
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
