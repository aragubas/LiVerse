using LiVerse.AnaBanUI;
using LiVerse.AnaBanUI.Containers;
using LiVerse.AnaBanUI.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LiVerse.Screens.MainScreenNested {
  public class AudioSettingsScreen : ControlBase {

    ScrollableList scrollableList;
    DockFillContainer dockFill;

    public AudioSettingsScreen() {
      scrollableList = new();

      dockFill = new();
      Label audioInputDeviceToggleTitle = new("Input Device: ") { Color = Color.Black };
      Label e = new("todo") { Color = Color.Black };

      dockFill.DockType = DockFillContainerDockType.Left;
      dockFill.DockElement = audioInputDeviceToggleTitle;
      dockFill.FillElement = e;

      scrollableList.Elements.Add(dockFill);
    }

    public override void DrawElement(SpriteBatch spriteBatch, double deltaTime) {
      scrollableList.Draw(spriteBatch, deltaTime);
    }

    public override void Update(double deltaTime) {
      scrollableList.Size = ContentArea;
      scrollableList.RelativePosition = RelativePosition;
      scrollableList.AbsolutePosition = AbsolutePosition;

      MinimumSize = scrollableList.MinimumSize;
      
    }
  }
}
