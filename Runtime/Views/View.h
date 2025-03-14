#pragma once
#include <TaiyouUI/UIRoot.h>
#include <functional>
#include <memory>
using namespace TaiyouUI;

namespace LiVerse::Views
{
    class View 
    {
    protected:
        std::shared_ptr<UIRoot> m_UIRoot;
    
    public:
        View(std::shared_ptr<UIRoot> uiRoot);
        virtual ~View() = default;

        virtual void Update(double deltaTime) = 0;
        virtual void OnShutdown() = 0;
        
        std::function<void(View*)> ChangeViewRequest;
    };

}