#pragma once
#include "Container.h"

/// @brief A UI Layer, which represents a Layer with UI contents to be rendered by UIRoot 
class Layer
{
public:
    /// @brief Used internally by UIRoot, to store the index of this layer in the vector of layers
    unsigned int Index;

    /// @brief Pointer to a container
    Container* RootContainer;

    Layer(unsigned int index);
    ~Layer();
};