using SFML.Graphics;
using SFML.System;

namespace LiVerse.AnaBanUI.Containers;

public class FlexibleContainer : Control {
  public List<Control> Controls;
  public float Gap = 0f;
  public FlexibleContainerDirection direction = FlexibleContainerDirection.Rows;


  public FlexibleContainer(List<Control> controls = null) {
    if (controls != null)
      Controls = controls;
    else
      Controls = new();

  }

  void UpdateUI() {
    if (Controls.Count == 0) return;
    if (Controls.Count == 1) {
      Controls[0].RelativePosition = new Vector2f(0, 0);
      Controls[0].Size = ContentArea;
    }

    float lastX = 0;

    foreach (Control control in Controls) {
      control.RelativePosition = new Vector2f(lastX, control.RelativePosition.Y);
      control.Size = control.MinimumSize;

      lastX += control.Size.X + Gap;
    }
  }

  public override void Update(double deltaTime) {
    UpdateUI();

    foreach (Control control in Controls) {
      control.Update(deltaTime);
    }
  }

  protected override void DoDraw(RenderTarget target, RenderStates states) {
    foreach (Control control in Controls) {
      target.Draw(control, states);
    }
  }
}