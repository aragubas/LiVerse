#include "UIRoot.h"


UIRoot::UIRoot() : m_Layers(std::vector<Layer>())
{
	// Creates Layer 0

}


UIRoot::~UIRoot()
{

}

unsigned int UIRoot::CreateLayer(Container *container)
{
    unsigned int index = m_Layers.size() + 1;
    Layer layer(index);

    layer.RootContainer = container;

    return index;
}


void UIRoot::RemoveLayer(unsigned int index)
{
    m_Layers.erase(m_Layers.begin() + index);
}


void UIRoot::Update(double deltaTime)
{

}
