#pragma once
#include <SFML/Graphics.hpp>
#include <fmt/printf.h>
#ifdef _DEBUG
#include <filesystem>
#endif

class Application
{
	sf::RenderWindow* m_Window;
	
	int Initialize();
	int Run();
	inline void ProcessEvents();
	void Update(double deltaTime);
	void Draw(double deltaTime);

public:
	Application();
	
	int Start();

};