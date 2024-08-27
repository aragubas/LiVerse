#pragma once
#include "Control.h"

class Container : public Control
{
    /// @brief Called when the UI needs to be re-built. Usually after resizing/adding component
    virtual void BuildUI() = 0;

public:    
    virtual void Update(double deltaTime) = 0;
    virtual void Draw(SDL_Renderer* renderer, double deltaTime) = 0;

    virtual uint AddControl(Control* control) = 0;
    virtual void RemoveControl() = 0;
};