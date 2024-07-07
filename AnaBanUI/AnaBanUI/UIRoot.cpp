#include "UIRoot.h"


UIRoot::UIRoot() : m_Layers(std::vector<Layer>())
{

}

unsigned int UIRoot::CreateLayer(Container *container)
{
    unsigned int index = m_Layers.size() + 1;
    Layer layer(index);

    layer.RootContainer = container;

    m_Layers.push_back(layer);

    return index;
}


void UIRoot::RemoveLayer(unsigned int index)
{
    m_Layers.erase(m_Layers.begin() + index);
}


void UIRoot::Update(double deltaTime)
{
    for (Layer layer : m_Layers)
    {
        layer.RootContainer->Update(deltaTime);
    }    
}


void UIRoot::Draw(SDL_Renderer *renderer, double deltaTime)
{
    for (Layer layer : m_Layers)
    {
        layer.RootContainer->Draw(renderer, deltaTime);
    }    
}
