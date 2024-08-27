#pragma once
#include <SDL2/SDL.h>
#include <fmt/printf.h>
#include <string>
#include <TaiyouUI/UIRoot.h>
#ifndef NDEBUG
#include <filesystem>
#endif

class Application
{
	std::string m_InitialWindowTitle;
	SDL_Window *m_Window;
	SDL_Renderer *m_Renderer;
	UIRoot *m_UIRoot;
	bool m_Running;

	int Initialize();
	int Run();
	void ProcessEvents();
	void Update(double deltaTime);
	void Draw(double deltaTime);
	void OnShutdown();

	//
	// Error handling
	//

	/// @brief Displays a message box and logs SDL error to the console
	inline void SDLFatalError(const char *messageHead);

public:
	/// @brief Main TaiyouUI Application class
	/// @param windowTitle Initial Window title
	Application(const char *windowTitle, UIRoot *uiRoot = nullptr);

	/// @brief Set Window title
	/// @param windowTitle Window title
	void SetWindowTitle(const char *windowTitle);

	/// @brief Set UIRoot, disabling and deleting the current one
	/// @param uiRoot new UIRoot
	void SetUIRoot(UIRoot *uiRoot);

	/// @brief Bootstraps the application, blocking the calling thread
	/// @return non-zero if something goes wrong
	int Start();
};