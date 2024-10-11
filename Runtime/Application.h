#pragma once
#include <SDL2/SDL.h>
#include <fmt/printf.h>
#include <string>
#include <functional>
#include <TaiyouUI/UIRoot.h>
#ifndef NDEBUG
#include <filesystem>
#endif

class Application
{
	const char *m_InitialWindowTitle;
	SDL_Window *m_Window;
	SDL_Renderer *m_Renderer;
	UIRoot *m_UIRoot;
	bool m_Running;

	void ProcessEvents();
	void Update(double deltaTime);
	void Draw(double deltaTime);
	void OnShutdown();

	////////////////////
	// Error handling //
	////////////////////

	/// @brief Displays a message box and logs SDL error to the console
	inline void SDLFatalError(const char *messageHead);

public:
	/// @brief Main TaiyouUI Application class
	/// @param windowTitle Initial Window title
	Application(const char *windowTitle);

	/// @brief Initializes SDL2, creates the Window and Renderer and creates the UIRoot
	/// @returns 0 if successful, 1 if otherwise
	int Initialize();

	/// @brief Returns a pointer to the UIRoot instance
	UIRoot *GetUIRoot();

	/// @brief Set Window title
	/// @param windowTitle Window title
	void SetWindowTitle(const char *windowTitle);

	/// @brief Starts the main loop, blocking the thread
	int Run();
};