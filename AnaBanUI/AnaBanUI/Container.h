#pragma once
#include "Control.h"

class Container : public Control
{
public:

    
    virtual ~Container() = 0;

    virtual void Update(double deltaTime) = 0;
    virtual void draw(sf::RenderTarget& target, sf::RenderStates states) const = 0;
};