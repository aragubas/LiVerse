#pragma once
#include <SFML/Graphics.hpp>
#include <fmt/printf.h>
#include <string>
#include <AnaBanUI/UIRoot.h>
#ifdef _DEBUG
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
	/// <summary>
	/// Main application class
	/// </summary>
	/// <param name="windowTitle"></param>
	Application(const char* windowTitle);

	/// <summary>
	/// Set window title
	/// </summary>
	/// <param name="windowTitle">Window title to set</param>
	void SetWindowTitle(const char* windowTitle);
	
	/// <summary>
	/// Set UIRoot 
	/// </summary>
	/// <param name="uiRoot"></param>
	void SetUIRoot(UIRoot& uiRoot);

	/// <summary>
	/// Called by main thread, bootstaps the application
	/// </summary>
	/// <returns></returns>
	int Start();

};