#pragma once
#include <TaiyouUI/UIRoot.h>
#include <functional>
#include <memory>
using namespace TaiyouUI;

namespace LiVerse::Scenes
{
    class Scene
    {
    protected:
        std::shared_ptr<UIRoot> m_UIRoot;
    
    public:
        Scene(std::shared_ptr<UIRoot> uiRoot);
        virtual ~Scene() = default;

        virtual void Update(double deltaTime) = 0;
        virtual void OnShutdown() = 0;
        
        std::function<void(Scene*)> ChangeSceneRequest;
    };

}