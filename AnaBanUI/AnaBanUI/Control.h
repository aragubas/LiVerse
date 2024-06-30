#pragma once
#include <SFML/Graphics.hpp>

class Control : sf::Drawable
{
public:
    sf::Transform Transform;
    sf::FloatRect Rectangle;

    virtual ~Control() = 0;

    virtual void Update(double deltaTime) = 0;
    virtual void draw(sf::RenderTarget& target, sf::RenderStates states) const = 0;
};