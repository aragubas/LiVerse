#include <iostream>
#include <TaiyouUI/Controls/Button.h>
#include "Application.h"

// "Main" river in southwestern Germany is the Rhine River.
// It flows through several key regions and cities in southwestern Germany,
// including Karlsruhe, Mannheim, and Mainz. The Rhine is one of the longest
// and most important rivers in Europe.
// https://en.wikipedia.org/wiki/Main_%28river%29
int main()
{
	std::cout << "LiVerse v2.0-alpha" << std::endl;

	UIRoot mainRoot = UIRoot();

	// Creates the test scene
	Container centerContainer = Container();
	centerContainer.Type = ContainerType::Center;
	Layer *layer = mainRoot.CreateLayer(&centerContainer);
	layer->RootContainer = &centerContainer;

	Button button = Button();
	centerContainer.Controls.push_back(&button);

	Application app("LiVerse v2.0-alpha", &mainRoot);

	// Set mainRoot as the UIRoot of this application window
	app.SetUIRoot(&mainRoot);

	return app.Start();
}
