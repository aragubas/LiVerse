#pragma once
#include "Scene.h"

namespace LiVerse::Scenes
{
    class Startup : public Scene
    {
    public:
        Startup(UIRoot* uiRoot);

        void Update(double deltaTime) override;
        void OnShutdown() override;
    };
}