#pragma once
#include "../Control.h"

class Button : public Control
{
    SDL_Rect backgroundRect;

public:
    Button();
    
    void Update(double deltaTime) override;
    void Draw(SDL_Renderer* renderer, double deltaTime) override;
};