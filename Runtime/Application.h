#pragma once
#include <SDL2/SDL.h>
#include <fmt/printf.h>
#include <string>
#include <TaiyouUI/UIRoot.h>
#include <TaiyouUI/Turk/Turk.h>
#include "Scenes/Scene.h"
#ifndef NDEBUG
#endif

namespace LiVerse
{
	class Application
	{
		const char* m_InitialWindowTitle;
		SDL_Window* m_Window;
		SDL_Renderer* m_Renderer;
		TaiyouUI::UIRoot* m_UIRoot;
		Scenes::Scene* m_CurrentScene;
		bool m_Running;

		void ProcessEvents();
		void Update(double deltaTime);
		void Draw(double deltaTime);
		void OnShutdown();

		////////////////////
		// Error handling //
		////////////////////

		/// @brief Displays a message box for SDL2 errors and logs the error to stdout
		inline void FatalError(const std::string &messageBody);

		/// @brief Wrapper for FatalError for showing SDL2 errors
		inline void SDLFatalError(const std::string &messageHead);

	public:
		/// @brief Main TaiyouUI Application class
		/// @param windowTitle Initial Window title
		Application(const char *windowTitle);

		/// @brief Initializes SDL2, creates the Window and Renderer and creates the UIRoot
		/// @returns 0 if successful, 1 if otherwise
		int Initialize();

		/// @brief Returns a pointer to the UIRoot instance
		TaiyouUI::UIRoot *GetUIRoot();

		/// @brief Set Window title
		/// @param windowTitle Window title
		void SetWindowTitle(const char *windowTitle);

		/// @brief Called from scene when it wants to change the scene
		void OnChangeSceneRequest(Scenes::Scene* newScene);

		/// @brief Discards one scene (if any) and assigns new scene
		/// @param windowTitle Window title
		void AssignScene(Scenes::Scene* scene);

		/// @brief Starts the application loop, blocking the calling thread
		int Run();
	};
}