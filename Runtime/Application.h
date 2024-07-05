#pragma once
#include <SDL2/SDL.h>
#include <fmt/printf.h>
#include <string>
#include <AnaBanUI/UIRoot.h>
#ifndef NDEBUG
#include <filesystem>
#endif

class Application
{
	std::string m_InitialWindowTitle;
	SDL_Window* m_Window;
	SDL_Renderer* m_Renderer;
	UIRoot m_UIRoot;
	bool m_Running;
	float xPos = 0;

	int Initialize();
	int Run();
	void ProcessEvents();
	void Update(double deltaTime);
	void Draw(double deltaTime);
	void OnShutdown();

	// Error handling
	
	/// @brief Displays a message box and logs SDL error to the console
	inline void SDLFatalError(const char* messageHead);

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