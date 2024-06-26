#include <iostream>
#include "Application.h"

int main()
{	
	std::cout << "LiVerse v2.0-alpha" << std::endl;

	UIRoot mainRoot;

	Application app("LiVerse v2.0-alpha");

	app.SetUIRoot(mainRoot);

	return app.Start();
}
