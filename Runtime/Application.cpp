#include "Application.h"


Application::Application(const char *title) :
	m_Window(nullptr), m_InitialWindowTitle(title), m_UIRoot(UIRoot())
{
	
}


void Application::SetWindowTitle(const char* windowTitle)
{
	if (m_Window == nullptr) return;
	m_Window->setTitle(windowTitle);
}


int Application::Initialize()
{
#ifndef NDEBUG
	fmt::printf("[Debug] Current working directory: %s\n", std::filesystem::current_path().string());
#endif

	m_Window = new sf::RenderWindow(sf::VideoMode(800, 600), m_InitialWindowTitle);
	m_Window->setVerticalSyncEnabled(true);
	
	m_UIRoot = UIRoot();

	return 0;
}


int Application::Run()
{
	sf::Clock deltaClock;
	
	while (m_Window->isOpen())
	{
		double deltaTime = deltaClock.restart().asSeconds();
		
		ProcessEvents();
		Update(deltaTime);
		Draw(deltaTime);
	}
	
	return 0;
}


inline void Application::ProcessEvents()
{
	sf::Event event;
	
	while (m_Window->pollEvent(event))
	{
		if (event.type == sf::Event::Closed)
		{
			OnShutdown();
			return;
		}

		if (event.type == sf::Event::Resized)
		{
			unsigned int width = event.size.width;
			unsigned int height = event.size.height;
			bool needsResize = false;

			if (width < 800) 
			{
				width = 800;
				needsResize = true;	
			}
			if (height < 600) {
				height = 600;
				needsResize = true;
			}
			

			m_Window->setView(sf::View(sf::FloatRect(0, 0, (float)width, (float)height)));
			
			// FIXME: Crashes Wayland when resizing the window
			//if (needsResize) m_Window->setSize(sf::Vector2u(width, height));
		}

	}
}


void Application::OnShutdown()
{
	m_Window->close();
}


void Application::Update(double deltaTime)
{
	m_UIRoot.Update(deltaTime);
}


void Application::Draw(double deltaTime)
{
	m_Window->clear(sf::Color::Transparent);

	m_Window->draw(m_UIRoot);

	m_Window->display();
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
