#pragma once
#include <iostream>
#include <SFML/Graphics.hpp>
#include "Container.h"

/// @brief Controls input flow, manages rendering and activation/de-activation of the UI Layer Stack
class UIRoot : public sf::Drawable
{

public:
	Container* RootContainer;
	
	UIRoot();
	UIRoot(Container* rootControl);
	~UIRoot();

	void Update(double deltaTime);
	void draw(sf::RenderTarget& target, sf::RenderStates states) const;
};
