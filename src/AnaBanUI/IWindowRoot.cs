using Microsoft.Xna.Framework.Graphics;

namespace LiVerse.AnaBanUI {
  public interface IWindowRoot {
    public ControlBase? RootElement { get; set; }

    public void Update(double deltaTime);
    public void Draw(SpriteBatch spriteBatch, double deltaTime);
  }
}