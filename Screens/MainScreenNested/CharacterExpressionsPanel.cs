﻿using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.Stores;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.Screens.MainScreenNested; 
public class CharacterExpressionsPanel : ControlBase {
  public event Action? OnNewExpressionButtonPressed;
  ScrollableList expressionsList;
  DockFillContainer optionsDockFill;


  public CharacterExpressionsPanel(ControlBase? parent) : base(parent) {
    optionsDockFill = new(this) { DockDirection = DockDirection.Left, Gap = 8 };
    expressionsList = new(this) { StretchElements = false, ListDirection = ScrollableListDirection.Horizontal, Gap = 8f };

    optionsDockFill.FillElement = expressionsList;

    Button newButton = new Button(optionsDockFill, "+");
    newButton.Click += new Action(() => OnNewExpressionButtonPressed?.Invoke() );
    optionsDockFill.DockElement = newButton;

    updateExpressionsList();
    CharacterStore.OnCurrentCharacterChanged += CharacterStore_OnCurrentCharacterChanged;
  }

  private void CharacterStore_OnCurrentCharacterChanged(CharacterRenderer.Character? obj) {
    if (obj == null) {
      expressionsList.Elements.Clear(); 
      return;
    }

    updateExpressionsList();
  }

  void updateExpressionsList() {
    expressionsList.Elements.Clear();
    if (CharacterStore.CurrentCharacter == null) { return; }

    foreach(var expression in CharacterStore.CurrentCharacter.LoadedSpriteCollections.Values) {
      Button expressionButton = new(expressionsList, expression.Name == "default" ? "Default" : expression.Name) { };

      expressionButton.Click += new Action(() => setExpression(expression.Name));

      expressionsList.Elements.Add(expressionButton);
    }

  }

  void setExpression(string expressionName) {
    if (CharacterStore.CurrentCharacter == null) { return; }
    CharacterStore.CurrentCharacter.SetExpression(expressionName);
  }

  public override void UpdateUI(double deltaTime) {
    FillControl(optionsDockFill);
  }

  public override void DrawControl(SpriteBatch spriteBatch, double deltaTime) {
    optionsDockFill.Draw(spriteBatch, deltaTime);
  }

  public override bool InputUpdate(PointerEvent pointerEvent) {
    return optionsDockFill.InputUpdate(pointerEvent);
  }

  public override bool InputUpdate(KeyboardEvent keyboardEvent) {
    return optionsDockFill.InputUpdate(keyboardEvent);
  }

  public override void Update(double deltaTime) {
    optionsDockFill.Update(deltaTime);
  }

}
