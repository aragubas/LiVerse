#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>

class UIRoot : public sf::Drawable
{

public:
	UIRoot();
	~UIRoot();

	void Update(double deltaTime);
	void draw(sf::RenderTarget& target, sf::RenderStates states) const;
};
