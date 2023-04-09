using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiVerse.CharacterRenderer
{
    public interface IAnimator
    {
        /// <summary>
        /// If true, this animation object will be removed the next frame
        /// </summary>
        public bool AnimationEnded { get; set; }

        Vector2 Update(CharacterState state, double deltaTime);
    }
}
