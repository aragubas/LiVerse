#include <iostream>
#include "Application.h"

// Main river in southwestern Germany is the Rhine River.
// It flows through several key regions and cities in southwestern Germany,
// including Karlsruhe, Mannheim, and Mainz. The Rhine is one of the longest
// and most important rivers in Europe.
int main()
{
	std::cout << "LiVerse v2.0-alpha" << std::endl;

	UIRoot *mainRoot = new UIRoot();

	Application app("LiVerse v2.0-alpha", mainRoot);

	return app.Start();
}
