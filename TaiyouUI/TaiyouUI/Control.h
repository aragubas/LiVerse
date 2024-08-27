#pragma once
#include <SDL2/SDL.h>

class Control
{
    SDL_FPoint RelativePosition;
    SDL_FPoint AbsolutePosition;
    
public:
    virtual void Update(double deltaTime) = 0;
    virtual void Draw(SDL_Renderer* renderer, double deltaTime) = 0;
};