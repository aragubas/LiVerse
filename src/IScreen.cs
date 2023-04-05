using LiVerse.AnaBanUI;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.src
{
    public interface IScreen
    {
        WindowRoot WindowRoot { get; }

        public void Deattach();

        public void Update(double deltaTime);
        public void Draw(SpriteBatch spriteBatch, double deltaTime);
    }
}
