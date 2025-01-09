#include "Scenes/Startup.h"
#include <TaiyouUI/UIRoot.h>
#include <TaiyouUI/UIRootContext.h>
#include <SDL_version.h>
#include <string>
#define SDL_MAIN_HANDLED
#include <iostream>
#include <TaiyouUI/Controls/Button.h>
#include "Application.h"
using namespace TaiyouUI;
using namespace LiVerse;


// "Main" river in southwestern Germany is the Rhine River.
// It flows through several key regions and cities in southwestern Germany,
// including Karlsruhe, Mannheim, and Mainz. The Rhine is one of the longest
// and most important rivers in Europe.
// https://en.wikipedia.org/wiki/Main_%28river%29
int main()
{
	std::cout << "LiVerse v2.0-alpha by Aragubas" << std::endl;
	
	SDL_version version;
	SDL_GetVersion(&version);
	std::string versionString;
	versionString += std::to_string(version.major);
	versionString += ".";
	versionString += std::to_string(version.minor);
	versionString += ".";
	versionString += std::to_string(version.patch);
	std::cout << "using SDL2 version: " << versionString << std::endl;

	LiVerse::Application app("LiVerse v2.0-alpha");

	// Return 1 if initialization fails
	if (app.Initialize() != 0)
	{
		std::cout << "App Initialization failed. Check log above for more information" << std::endl;
		return 1;
	}

	// Creates the startup scene
	Scenes::Startup* startupScene = new Scenes::Startup(app.GetUIRoot());

	app.AssignScene(startupScene);

	// Run the Application
	return app.Run();
}
