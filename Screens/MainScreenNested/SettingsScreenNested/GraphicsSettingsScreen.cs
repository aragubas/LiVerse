using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using LiVerse.AnaBanUI.Controls.ComboBox;
using LiVerse.AnaBanUI.Events;
using LiVerse.Screens.MainScreenNested.SettingsScreenNested.GraphicsSettingsScreenNested;
using LiVerse.Stores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.Screens.MainScreenNested.SettingsScreenNested {
  public class GraphicsSettingsScreen : ControlBase {
    ScrollableList ScrollableList { get; }

    public GraphicsSettingsScreen() {
      ScrollableList = new() { ParentControl = this, Gap = 6 };

      ScrollableList.Elements.Add(new WindowTransparencyColorSettings());
    }

    public override void UpdateUI(double deltaTime) {
      FillElement(ScrollableList);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      ScrollableList.Draw(spriteBatch, deltaTime);
    }

    public override bool InputUpdate(PointerEvent pointerEvent) {
      return ScrollableList.InputUpdate(pointerEvent);
    }

    public override void Update(double deltaTime) {
      ScrollableList.Update(deltaTime);
    }
  }
}
