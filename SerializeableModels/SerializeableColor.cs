using Microsoft.Xna.Framework;

namespace LiVerse.SerializeableModels {
  public struct SerializeableColor {
    public int R;
    public int G;
    public int B;
    public int A;

    public SerializeableColor(Color color) {
      R = color.R;
      G = color.G;
      B = color.B;
      A = color.A;
    }

    public SerializeableColor(int r, int g, int b, int a) {
      R = r;
      G = g;
      B = b;
      A = a;
    }

    public Color ToColor() {
      return new Color() {R = (byte)R, G = (byte)G, B = (byte)B, A = (byte)A};
    }

    public override string ToString() {
      return $"SerializeableColor ({R}, {G}, {B}, {A})";
    }
  }
}
