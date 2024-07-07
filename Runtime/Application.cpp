#include "Application.h"
#include <AnaBanUI/Controls/Button.h>
#include <AnaBanUI/Containers/DockFill.h>

Application::Application(const char *title) :
	m_Window(nullptr), m_InitialWindowTitle(title), m_UIRoot(UIRoot()),
	m_Running(true)
{
	
}


void Application::SetWindowTitle(const char* windowTitle)
{

}


int Application::Initialize()
{
#ifndef NDEBUG
	fmt::printf("[Debug] Current working directory: %s\n", std::filesystem::current_path().string());
#endif

	if (SDL_Init(SDL_INIT_VIDEO) < 0)
	{
		SDLFatalError("Could not initialize SDL2.");		
		return 1;
	}

	m_Window = SDL_CreateWindow("LiVerse", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 800, 600, SDL_WINDOW_SHOWN);
	if (m_Window == NULL)
	{
		SDLFatalError("Could not create window.");
	}

	m_Renderer = SDL_CreateRenderer(m_Window, -1, SDL_RENDERER_ACCELERATED);
	if (m_Renderer == NULL)
	{
		SDLFatalError("Could not create renderer.");
	}
	// Enable VSync
	SDL_RenderSetVSync(m_Renderer, 1);

	// Initialize AnaBanUI
	m_UIRoot = UIRoot();
	DockFill* dockFill = new DockFill();
	Button* button = new Button();

	dockFill->AddControl(button);

	uint layerIndex = m_UIRoot.CreateLayer(dockFill);

	return Run();
}


inline void Application::SDLFatalError(const char* messageHead)
{
	std::string sdlError = std::string(SDL_GetError());
	std::string errorMessage = fmt::format("{:s} {:s}", messageHead, sdlError);

	fmt::printf("Fatal Error! %s\n", errorMessage);
	
	SDL_ShowSimpleMessageBox(SDL_MESSAGEBOX_ERROR, "LiVerse - Fatal Error", errorMessage.c_str(), NULL);
}


int Application::Run()
{
	double currentTime = SDL_GetPerformanceCounter();
	double lastTime = 0;
	double deltaTime = 0.0001;

	while (m_Window != NULL && m_Running)
	{
		lastTime = currentTime;
		currentTime = SDL_GetPerformanceCounter();

		deltaTime = ((currentTime - lastTime) * 1000 / (double)SDL_GetPerformanceFrequency()) * 0.001;

		ProcessEvents();
		Update(deltaTime);
		Draw(deltaTime);
	}

	// Application is closing
	SDL_DestroyRenderer(m_Renderer);
	SDL_DestroyWindow(m_Window);

	return 0;
}


void Application::ProcessEvents()
{
	SDL_Event event;

	while (SDL_PollEvent(&event))
	{
		if (event.type == SDL_QUIT)
		{
			OnShutdown();
			return;
		}
	}
}


void Application::OnShutdown()
{
	m_Running = false;
}


void Application::Update(double deltaTime)
{
	m_UIRoot.Update(deltaTime);
}


void Application::Draw(double deltaTime)
{
	// Clear the screen
	SDL_SetRenderDrawColor(m_Renderer, 0, 0, 0, 0);
	SDL_RenderClear(m_Renderer);
	
	m_UIRoot.Draw(m_Renderer, deltaTime);

	// Update Window
	SDL_RenderPresent(m_Renderer);
}



void Application::SetUIRoot(UIRoot& uiRoot)
{
	m_UIRoot = uiRoot;
}


int Application::Start()
{
	int initStatus = Initialize();
	if (initStatus != 0) return initStatus;
		
	return Run();
}
