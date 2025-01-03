#pragma once
#include <TaiyouUI/UIRoot.h>
using namespace TaiyouUI;

namespace LiVerse::Scenes
{
    class Scene
    {
        UIRoot* m_UIRoot;
    public:
        Scene(UIRoot* uiRoot);

        virtual void Update(double deltaTime) = 0;
        virtual void OnShutdown() = 0;
        
        void (*ChangeSceneRequest)(Scene* newScene);
    };

}