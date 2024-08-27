#include "DockFill.h"

DockFill::DockFill() : m_Controls(std::vector<Control*>())
{

}


void DockFill::BuildUI()
{

}


void DockFill::Update(double deltaTime)
{

}


void DockFill::Draw(SDL_Renderer *renderer, double deltaTime)
{
    for (Control* control : m_Controls)
    {
        control->Draw(renderer, deltaTime);
    }
}

uint DockFill::AddControl(Control *control)
{
    unsigned int index = m_Controls.size() + 1;

    m_Controls.push_back(control);

    return index;
}

void DockFill::RemoveControl()
{

}
