#include "CharacterView.h"
#include "View.h"
#include <iostream>
using namespace LiVerse::Views;


CharacterView::CharacterView(std::shared_ptr<UIRoot> uiRoot) : 
    Views::View(uiRoot), m_PrimaryButton(uiRoot->Context, "Click Me"),
    m_CenterContainer(uiRoot->Context)
{
	m_CenterContainer.Type = ContainerType::Center;
	
    // Create UI Layer with m_CenterContainer
    m_PrimaryUILayer = uiRoot->CreateLayer(&m_CenterContainer);

	m_CenterContainer.AddControl(&m_PrimaryButton);

    m_PrimaryButton.OnClick = []() { std::cout << "Pingas" << std::endl; };
}

CharacterView::~CharacterView()
{

}

void CharacterView::Update(double deltaTime) 
{

}

void CharacterView::OnShutdown() 
{

}