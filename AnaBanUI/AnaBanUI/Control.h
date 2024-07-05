#pragma once

class Control
{
public:
    virtual ~Control() = 0;

    virtual void Update(double deltaTime) = 0;
};