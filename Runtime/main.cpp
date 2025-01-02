#include "TaiyouUI/UIRoot.h"
#include "TaiyouUI/UIRootContext.h"
#define SDL_MAIN_HANDLED
#include <iostream>
#include <TaiyouUI/Controls/Button.h>
#include "Application.h"
using namespace TaiyouUI;


// "Main" river in southwestern Germany is the Rhine River.
// It flows through several key regions and cities in southwestern Germany,
// including Karlsruhe, Mannheim, and Mainz. The Rhine is one of the longest
// and most important rivers in Europe.
// https://en.wikipedia.org/wiki/Main_%28river%29
int main()
{
	std::cout << "LiVerse v2.0-alpha by Aragubas" << std::endl;

	Application app("LiVerse v2.0-alpha");

	// Return 1 if initialization fails
	if (app.Initialize() != 0)
	{
		std::cout << "App Initialization failed. Check log above for more information" << std::endl;
		return 1;
	}

	// Creates the test scene
	UIRootContext context = app.GetUIRoot()->Context;

	Container centerContainer = Container(context);
	centerContainer.Type = ContainerType::Center;
	Layer* layer = app.GetUIRoot()->CreateLayer(&centerContainer);

	Controls::Button button = Controls::Button(context, "Lorem Ipsum");
	centerContainer.AddControl(&button);

	// Run the Application
	return app.Run();
}
