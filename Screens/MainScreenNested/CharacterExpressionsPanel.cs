using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Events;
using LiVerse.Stores;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.Screens.MainScreenNested {
  public class CharacterExpressionsPanel : ControlBase {

    ScrollableList expressionsList;

    public CharacterExpressionsPanel() {
      expressionsList = new() { StretchElements = false, ListDirection = ScrollableListDirection.Horizontal, Gap = 4f };

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
        Button expressionButton = new(expression.Name == "default" ? "Default" : expression.Name) { };

        expressionButton.Click += new Action(() => setExpression(expression.Name));

        expressionsList.Elements.Add(expressionButton);
      }

    }

    void setExpression(string expressionName) {
      if (CharacterStore.CurrentCharacter == null) { return; }
      CharacterStore.CurrentCharacter.SetExpression(expressionName);
    }

    public override void UpdateUI(double deltaTime) {
      FillElement(expressionsList);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      expressionsList.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return expressionsList.InputUpdate(pointerEvent);
    }

    public override bool InputUpdate(KeyboardEvent keyboardEvent) {
      return expressionsList.InputUpdate(keyboardEvent);
    }

    public override void Update(double deltaTime) {
      expressionsList.Update(deltaTime);
    }

  }
}
