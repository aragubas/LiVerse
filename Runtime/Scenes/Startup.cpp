#include "Startup.h"
#include "Scene.h"
#include <iostream>
using namespace LiVerse::Scenes;


Startup::Startup(std::shared_ptr<UIRoot> uiRoot) : 
    Scene::Scene(uiRoot), m_PrimaryButton(uiRoot->Context, "Click Me"),
    m_CenterContainer(uiRoot->Context)
{
	m_CenterContainer.Type = ContainerType::Center;
	
    // Create UI Layer with m_CenterContainer
    m_PrimaryUILayer = uiRoot->CreateLayer(&m_CenterContainer);

	m_CenterContainer.AddControl(&m_PrimaryButton);

    m_PrimaryButton.OnClick = []() { std::cout << "Pingas" << std::endl; };
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