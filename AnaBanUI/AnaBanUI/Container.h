#pragma once
#include "Control.h"

class Container : public Control
{
public:    
    virtual ~Container() = 0;

    virtual void Update(double deltaTime) = 0;
};