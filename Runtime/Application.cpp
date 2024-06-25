#include "Application.h"
#include <AnaBanUI/WindowRoot.h>

Application::Application() :
	m_Window(nullptr)
{
}


int Application::Initialize()
{
#ifdef _DEBUG
	fmt::printf("[Debug] Current working directory: %s\n", std::filesystem::current_path().string());
#endif

	m_Window = new sf::RenderWindow(sf::VideoMode(800, 600), "LiVerse v2.0-alpha");
	m_Window->setVerticalSyncEnabled(true);
	
	WindowRoot root;

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
			m_Window->close();
		}

		if (event.type == sf::Event::Resized)
		{
			unsigned int width = event.size.width;
			unsigned int height = event.size.height;

			if (width < 800) width = 800;
			if (height < 600) height = 600;

			m_Window->setView(sf::View(sf::FloatRect(0, 0, (float)width, (float)height)));
			m_Window->setSize(sf::Vector2u(width, height));
		}

	}
}

void Application::Update(double deltaTime)
{

}

void Application::Draw(double deltaTime)
{
	m_Window->clear(sf::Color::Transparent);

	m_Window->display();
}


/// <summary>
/// Called by main thread, bootstaps the application
/// </summary>
/// <returns></returns>
int Application::Start()
{
	int initStatus = Initialize();
	if (initStatus != 0) return initStatus;
	
	
	return Run();
}
