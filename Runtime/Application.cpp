#include "Application.h"
#include <TaiyouUI/Controls/Button.h>

Application::Application(const char *title)
{
	m_Window = nullptr;
	m_InitialWindowTitle = title;
	m_UIRoot = nullptr;
	m_Running = true;
}

UIRoot *Application::GetUIRoot()
{
	return m_UIRoot;
}

void Application::SetWindowTitle(const char *windowTitle)
{
	if (!m_Window)
		return;
	SDL_SetWindowTitle(m_Window, windowTitle);
}

int Application::Initialize()
{
#ifndef NDEBUG
	fmt::printf("[Debug] Current working directory: %s\n", std::filesystem::current_path().string());
#endif

	// Prefer wayland over X11
	// SDL_SetHint(SDL_HINT_VIDEODRIVER, "wayland,x11");

	// Initialize SDL Video
	if (SDL_Init(SDL_INIT_VIDEO) < 0)
	{
		SDLFatalError("Could not initialize SDL2.");
		return 1;
	}

	// Initialize SDL TTF
	if (TTF_Init() == -1)
	{
		SDLFatalError("Could not initialize SDL2 TTF");
		return 1;
	}

	m_Window = SDL_CreateWindow("LiVerse", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, 800, 600, SDL_WINDOW_SHOWN | SDL_WINDOW_RESIZABLE);
	if (m_Window == NULL)
	{
		SDLFatalError("Could not create window.");
		return 1;
	}
	// Set window properties
	SDL_SetWindowMinimumSize(m_Window, 640, 480);

	m_Renderer = SDL_CreateRenderer(m_Window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
	if (m_Renderer == NULL)
	{
		SDLFatalError("Could not create renderer.");
		return 1;
	}
	// Enable VSync
	SDL_RenderSetVSync(m_Renderer, 1);

	// Check if renderer supports Render Targets
	if (!SDL_RenderTargetSupported(m_Renderer))
	{
		SDLFatalError("Renderer does not support render targets,\nwhich is required by this application.\n\nThe application will close");
		return 1;
	}

#ifndef NDEBUG
	fmt::printf("Using video driver: %s\n", SDL_GetCurrentVideoDriver());
	fmt::printf("Using audio driver: %s\n", SDL_GetCurrentAudioDriver());
#endif

	// Create UIRoot
	m_UIRoot = new UIRoot(m_Renderer, m_Window);

	// TODO: Raise an error message if callback is null

	OnUIRootInitialized(m_UIRoot);

#ifndef NDEBUG
	std::cout << "[Debug] Initialization Done" << std::endl;
#endif

	return 0;
}

inline void Application::SDLFatalError(const char *messageHead)
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
	double deltaTime = 0.000000001;

	while (m_Window != NULL && m_Running)
	{
		lastTime = currentTime;
		currentTime = SDL_GetPerformanceCounter();

		// Calculate delta time
		deltaTime = ((currentTime - lastTime) * 1000 / (double)SDL_GetPerformanceFrequency()) * 0.001;

		ProcessEvents();
		Update(deltaTime);
		Draw(deltaTime);
	}

	// Application is closing
	OnShutdown();

	return 0;
}

void Application::ProcessEvents()
{
	SDL_Event event;

	while (SDL_PollEvent(&event))
	{
		// Not processed by UIRoot
		if (event.type == SDL_QUIT)
		{
			m_Running = false;
			return;
		}

		m_UIRoot->EventUpdate(event);
	}
}

void Application::OnShutdown()
{
#ifndef NDEBUG
	std::cout << "Application::OnShutdown(); Bye bye!" << std::endl;
#endif
	SDL_DestroyRenderer(m_Renderer);
	SDL_DestroyWindow(m_Window);
}

void Application::Update(double deltaTime)
{
	// Set UIRoot size to window size
	int size_w, size_h = 0;
	SDL_GetRendererOutputSize(m_Renderer, &size_w, &size_h);
	m_UIRoot->Size.x = size_w;
	m_UIRoot->Size.y = size_h;

	m_UIRoot->Update(deltaTime);
}

void Application::Draw(double deltaTime)
{
	// Clear the screen
	SDL_SetRenderDrawColor(m_Renderer, 0, 0, 0, 0);
	SDL_RenderClear(m_Renderer);

	m_UIRoot->Draw(m_Renderer, deltaTime);

	// Update Window
	SDL_RenderPresent(m_Renderer);
}

//
//	Public Methods
//
int Application::Start()
{
	int initStatus = Initialize();
	if (initStatus != 0)
		return initStatus;

	return Run();
}