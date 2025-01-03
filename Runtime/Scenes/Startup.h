#pragma once
#include "Scene.h"
#include "TaiyouUI/Controls/Button.h"


namespace LiVerse::Scenes
{
    class Startup : public Scene
    {
        uint m_PrimaryUILayer;
        Controls::Button m_PrimaryButton;
        Container m_CenterContainer;

    public:
        Startup(UIRoot* uiRoot);
        virtual ~Startup();

        void Update(double deltaTime) override;
        void OnShutdown() override;
    };
}