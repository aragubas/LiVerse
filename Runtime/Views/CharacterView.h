#pragma once
#include <memory>
#include "View.h"
#include "TaiyouUI/Controls/Button.h"


namespace LiVerse::Views
{
    class CharacterView : public View 
    {
        uint m_PrimaryUILayer;
        Controls::Button m_PrimaryButton;
        Container m_CenterContainer;

    public:
        CharacterView(std::shared_ptr<UIRoot> uiRoot);
        virtual ~CharacterView();

        void Update(double deltaTime) override;
        void OnShutdown() override;
    };
}