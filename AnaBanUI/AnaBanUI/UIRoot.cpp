#include "UIRoot.h"


UIRoot::UIRoot()
{
	
}


UIRoot::~UIRoot()
{
}


void UIRoot::Update(double deltaTime)
{

}

void UIRoot::draw(sf::RenderTarget& target, sf::RenderStates states) const
{
	sf::RectangleShape shape(sf::Vector2f(50, 50));
	shape.setPosition(50, 50);
	shape.setFillColor(sf::Color::Red);
	target.draw(shape);
}
