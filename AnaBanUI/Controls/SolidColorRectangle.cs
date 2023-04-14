﻿using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.AnaBanUI.Controls {
  public class SolidColorRectangle : ControlBase {
    public ControlBase Element { get; set; }
    public Color BackgroundColor { get; set; } = Color.Transparent;


    public SolidColorRectangle(ControlBase element) {
      Element = element;
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      spriteBatch.FillRectangle(new RectangleF(Vector2.Zero, ContentArea), BackgroundColor);

      Element.DrawElement(spriteBatch, deltaTime);

      //spriteBatch.DrawRectangle(new RectangleF(Vector2.Zero, Element.Size), BorderColor, BorderThickness);
    }

    public override void Update(double deltaTime) {
      MinimumSize = Element.MinimumSize;
      Element.Size = ContentArea;
      Element.AbsolutePosition = AbsolutePosition;

      Element.Update(deltaTime);
    }

  }
}
