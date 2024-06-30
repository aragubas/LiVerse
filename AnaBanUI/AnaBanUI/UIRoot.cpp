#include "UIRoot.h"


UIRoot::UIRoot()
{
	
}


UIRoot::UIRoot(Container* rootControl) : RootContainer(rootControl)
{
    UIRoot();
}

UIRoot::~UIRoot()
{

}


void UIRoot::Update(double deltaTime)
{

}

void UIRoot::draw(sf::RenderTarget& target, sf::RenderStates states) const
{
	
}
