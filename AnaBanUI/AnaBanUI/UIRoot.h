#pragma once
#include <iostream>
#include <vector>
#include "Layer.h"
#include "Container.h"

/// @brief Controls input flow, manages rendering and activation/de-activation of the UI Layer Stack
class UIRoot
{
	std::vector<Layer> m_Layers;

public:	
	UIRoot();
	
	/// @brief Adds the layer into the internal layer stack and returns the Layer index
	/// @param layer Layer reference
	/// @return 
	unsigned int CreateLayer(Container* container);
	
	/// @brief Reparents the container of a layer. Make sure to delete the old container, as that WILL cause a memory leak
	/// @param newContainer New container pointer to reparent
	void ReparentLayer(Container* newContainer);

	/// @brief Deletes the layer from the internal layer stack
	/// @param index Index to delete
	void RemoveLayer(unsigned int index);

	void Update(double deltaTime);
	void Draw(SDL_Renderer* renderer, double deltaTime);
};
