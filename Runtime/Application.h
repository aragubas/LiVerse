#pragma once
#include <SFML/Graphics.hpp>
#include <fmt/printf.h>
#include <string>
#include <AnaBanUI/UIRoot.h>
#ifndef NDEBUG
#include <filesystem>
#endif

class Application
{
	std::string m_InitialWindowTitle;
	sf::RenderWindow *m_Window;
	UIRoot m_UIRoot;
	
	int Initialize();
	int Run();
	inline void ProcessEvents();
	void Update(double deltaTime);
	void Draw(double deltaTime);
	void OnShutdown();

public:
	/// @brief Main AnaBanUI Application class
	/// @param windowTitle Initial Window title
	Application(const char* windowTitle);

	/// @brief Set Window title
	/// @param windowTitle Window title
	void SetWindowTitle(const char* windowTitle);
	
	/// @brief Set UIRoot, disabling and deleting the current one
	/// @param uiRoot new UIRoot
	void SetUIRoot(UIRoot& uiRoot);

	/// @brief Bootstaps the application, blocking the calling thread
	/// @return non-zero if something goes wrong
	int Start();

};