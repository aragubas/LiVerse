#include "Startup.h"
#include "Scene.h"
using namespace LiVerse::Scenes;


Startup::Startup(UIRoot* uiRoot) : 
    Scene::Scene(uiRoot), m_PrimaryButton(uiRoot->Context, "Click Me"),
    m_CenterContainer(uiRoot->Context)
{
	m_CenterContainer.Type = ContainerType::Center;
	
    // Create UI Layer with m_CenterContainer
    m_PrimaryUILayer = uiRoot->CreateLayer(&m_CenterContainer);

	m_CenterContainer.AddControl(&m_PrimaryButton);
}

Startup::~Startup()
{

}

void Startup::Update(double deltaTime) 
{

}

void Startup::OnShutdown() 
{

}