#include <iostream>
#include <TaiyouUI/Controls/Button.h>
#include "Application.h"

void CreateTestScene(UIRoot *uiRoot)
{
	std::cout << "Create test scene" << std::endl;

	// Creates the test scene
	Container *centerContainer = new Container();
	centerContainer->Type = ContainerType::Center;
	Layer *layer = uiRoot->CreateLayer(centerContainer);

	Button *button = new Button(uiRoot->Context);
	centerContainer->Controls.push_back(button);
}

// "Main" river in southwestern Germany is the Rhine River.
// It flows through several key regions and cities in southwestern Germany,
// including Karlsruhe, Mannheim, and Mainz. The Rhine is one of the longest
// and most important rivers in Europe.
// https://en.wikipedia.org/wiki/Main_%28river%29
int main()
{
	std::cout << "LiVerse v2.0-alpha" << std::endl;

	Application app("LiVerse v2.0-alpha");
	app.OnUIRootInitialized = &CreateTestScene;

	return app.Start();
}