#pragma once
#include <TaiyouUI/UIRoot.h>
#include <functional>
using namespace TaiyouUI;

namespace LiVerse::Scenes
{
    class Scene
    {
    protected:
        UIRoot* m_UIRoot;
    
    public:
        Scene(UIRoot* uiRoot);
        virtual ~Scene() = default;

        virtual void Update(double deltaTime) = 0;
        virtual void OnShutdown() = 0;
        
        std::function<void(Scene*)> ChangeSceneRequest;
    };

}