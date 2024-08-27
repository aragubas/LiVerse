#include "Button.h"
#include <stdio.h>

Button::Button()
{
    backgroundRect = SDL_Rect();
    backgroundRect.x = 2;
    backgroundRect.y = 2;
    backgroundRect.w = 357;
    backgroundRect.h = 200;
}


void Button::Update(double deltaTime)
{

}


void Button::Draw(SDL_Renderer *renderer, double deltaTime)
{
    SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
    SDL_RenderFillRect(renderer, &backgroundRect);
}
