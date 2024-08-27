#pragma once
#include "../Container.h"
#include "../Control.h"
#include <vector>

class DockFill : public Container
{
    std::vector<Control*> m_Controls;
    void BuildUI() override;

public:
    DockFill();

    void Update(double deltaTime) override;
    void Draw(SDL_Renderer* renderer, double deltaTime) override;

    uint AddControl(Control* control) override;
    void RemoveControl() override;
};