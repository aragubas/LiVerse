﻿using LiVerse.AnabanUI.Controls;
using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Drawables;
using Microsoft.Xna.Framework;

namespace LiVerse.Screens.MainScreenNested {
  public class NewCharacterExpressionScreen {
    UILayer UIRootLayer;

    public NewCharacterExpressionScreen() {
      UIRootLayer = new() { BackgroundRectDrawable = new() { Color = Color.FromNonPremultiplied(0, 0, 0, 127) } };

      DockFillContainer mainDockFillContainer = new() { Margin = new Vector2(40), BackgroundRectDrawable = new() { Color = Color.White } };
      DockFillContainer titleDockFill = new() {
        DockType = DockFillContainerDockType.Right
      };

      Label titleLabel = new("New Expression", 28, "Ubuntu") { Color = Color.Black };
      Button closeButton = new("X");
      closeButton.Click += ToggleUILayer;

      titleDockFill.DockElement = closeButton;
      titleDockFill.FillElement = titleLabel;

      LineEdit lineEdit = new("Each word in this sentance is different so it's easier to track whatever the textbox is advancing the text properly and stuff bwah");

      mainDockFillContainer.DockElement = titleDockFill;
      mainDockFillContainer.FillElement = lineEdit;

      UIRootLayer.RootElement = mainDockFillContainer;
    }

    public void ToggleUILayer() {
      if (UIRoot.UILayers.Contains(UIRootLayer)) {
        UIRoot.UILayers.Remove(UIRootLayer);
        return;
      }

      UIRoot.UILayers.Add(UIRootLayer);
    }

  }
}
