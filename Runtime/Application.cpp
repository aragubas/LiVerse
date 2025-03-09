#include "Application.h"
#include "Views/View.h"
#include "TaiyouUI/UIRoot.h"
#include <SDL3/SDL_blendmode.h>
#include <SDL3/SDL_render.h>
#include <TaiyouUI/Controls/Button.h>
#include <filesystem>
#include <iostream>
using namespace TaiyouUI;
using namespace LiVerse;

Application::Application() : 
	m_Window(std::unique_ptr<SDL_Window, decltype(&SDL_DestroyWindow)>(nullptr, &SDL_DestroyWindow)),
	m_Renderer(std::unique_ptr<SDL_Renderer, decltype(&SDL_DestroyRenderer)>(nullptr, &SDL_DestroyRenderer)), 
	m_UIRoot(std::unique_ptr<UIRoot>(nullptr)),
	m_CurrentScene(nullptr), 
	m_Running(true)
{
}

inline void Application::FatalError(const std::string& messageBody)
{
	fmt::printf("Fatal Error! %s\n", messageBody);

	int returnCode = SDL_ShowSimpleMessageBox(SDL_MESSAGEBOX_ERROR, "LiVerse - Fatal Error", messageBody.c_str(), NULL);
	if (returnCode != 0)
	{
		std::cout << "Could not display message box. " << SDL_GetError() << std::endl;
	}
}

inline void Application::SDLFatalError(const std::string& messageHead)
{
	std::string sdlError = std::string(SDL_GetError());
	std::string errorMessage = fmt::format("{:s} {:s}", messageHead, sdlError);

	FatalError(errorMessage);
}

void Application::ProcessEvents()
{
	SDL_Event event;

	while (SDL_PollEvent(&event))
	{
		// Event not processed by UIRoot
		if (event.type == SDL_EVENT_QUIT)
		{
			m_Running = false;
			return;
		}

		m_UIRoot->EventUpdate(event);
	}
}

void Application::OnShutdown()
{
	std::cout << "Application::OnShutdown(); Bye bye!" << std::endl;
}

void Application::Update(double deltaTime)
{
	// Set UIRoot size to window size
	int size_w, size_h = 0;
	SDL_GetRenderOutputSize(m_Renderer.get(), &size_w, &size_h);
	m_UIRoot->Size.x = size_w;
	m_UIRoot->Size.y = size_h;

	m_UIRoot->Update(deltaTime);

	if (m_CurrentScene != nullptr)
		m_CurrentScene->Update(deltaTime);
}

void Application::Draw(double deltaTime)
{
	SDL_SetRenderDrawBlendMode(m_Renderer.get(), SDL_BLENDMODE_BLEND);
	// Clear the screen
	SDL_SetRenderDrawColor(m_Renderer.get(), 0, 0, 0, 0);
	SDL_RenderClear(m_Renderer.get());

	m_UIRoot->Draw(m_Renderer.get(), deltaTime);

	// Update Window
	SDL_RenderPresent(m_Renderer.get());
}

//
//	Public Methods
//
int Application::Initialize()
{
#ifndef NDEBUG
	fmt::printf("[Debug] Current working directory: %s\n", std::filesystem::current_path().string());
#endif

	// Check if Application Data folder exists
	std::filesystem::path path = std::filesystem::path("Application Data");
	if (!std::filesystem::exists(path))
	{
		FatalError("Could not find Application Data directory.");
		return 1;
	}


	// Prefer wayland over X11 on Linux
#if __linux__
	SDL_SetHint(SDL_HINT_VIDEO_DRIVER, "wayland,x11");
#endif

	// Initialize SDL Video
	if (!SDL_Init(SDL_INIT_VIDEO))
	{
		SDLFatalError("Could not initialize SDL2.");
		return 1;
	}

	// Initialize SDL TTF
	if (!TTF_Init())
	{
		SDLFatalError("Could not initialize SDL2 TTF.");
		return 1;
	}

	m_Window.reset(SDL_CreateWindow("LiVerse alpha-2.0.0", 800, 600, SDL_WINDOW_TRANSPARENT | SDL_WINDOW_RESIZABLE), SDL_DestroyWindow);
	if (m_Window == NULL)
	{
		SDLFatalError("Could not create window.");
		return 1;
	}
	// Set window properties
	SDL_SetWindowMinimumSize(m_Window.get(), 640, 480);

	m_Renderer.reset(SDL_CreateRenderer(m_Window.get(), NULL), SDL_DestroyRenderer);
	if (m_Renderer == NULL)
	{
		SDLFatalError("Could not create renderer.");
		return 1;
	}
	// Enable VSync
	SDL_SetRenderVSync(m_Renderer.get(), 1);

	// TODO: Check if renderer supports Render Targets
	//if (!SDL_RenderTargetSupported(m_Renderer.get()))
	// {
	// 	SDLFatalError("Renderer does not support render targets,\nwhich is required by this application.\n\nThe application will close.");
	// 	return 1;
	// }

#ifndef NDEBUG
	fmt::printf("[Debug] Using video driver: %s\n", SDL_GetCurrentVideoDriver());
	fmt::printf("[Debug] Using audio driver: %s\n", SDL_GetCurrentAudioDriver());
#endif

	// Create UIRoot
	m_UIRoot = std::make_shared<UIRoot>(UIRoot(m_Renderer, m_Window));

#ifndef NDEBUG
	std::cout << "[Debug] Initialization Done" << std::endl;
#endif

	return 0;
}

std::shared_ptr<TaiyouUI::UIRoot> Application::GetUIRoot()
{
	return m_UIRoot;
}

void Application::AssignScene(Views::View* scene)
{
	// Un-instantiate current scene
	if (m_CurrentScene != nullptr)
	{				
		m_CurrentScene->OnShutdown();

		delete m_CurrentScene;

		m_UIRoot->ClearLayers();
	}	
	
	m_CurrentScene = scene;
	m_CurrentScene->ChangeSceneRequest = [this](Views::View* arg) { OnChangeSceneRequest(arg); };
}

void Application::OnChangeSceneRequest(Views::View* newScene)
{
	AssignScene(newScene);
}

void Application::SetWindowTitle(const char* windowTitle)
{
	if (!m_Window)
		return;
	SDL_SetWindowTitle(m_Window.get(), windowTitle);
}

int Application::Run()
{
	double currentTime = SDL_GetPerformanceCounter();
	double lastTime = 0;
	double deltaTime = 0.000000001;

	while (m_Window != NULL && m_Running)
	{
		lastTime = currentTime;
		currentTime = SDL_GetPerformanceCounter();

		ProcessEvents();
		Update(deltaTime);
		Draw(deltaTime);

		// Calculate delta time
		deltaTime = ((currentTime - lastTime) * 1000 / (double)SDL_GetPerformanceFrequency()) * 0.001;
	}

	// Application is closing
	OnShutdown();

	return 0;
}
