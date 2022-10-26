using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Mandelbrot;

internal class Program
{
	static void Main()
	{   
		const uint ScreenWidth = 1080;
		const uint ScreenHeight = 1080;

		float zoom = 2.8f;
		Vector2f res = new(ScreenWidth, ScreenHeight);

		RenderWindow window = new(new VideoMode((uint)res.X, (uint)res.Y), "Mandelbrot", Styles.Titlebar | Styles.Close);
		window.SetFramerateLimit(60);

		window.Closed += (_, _) => window.Close();
		window.MouseWheelScrolled += (object? _, MouseWheelScrollEventArgs e) =>
		{
			zoom -= e.Delta / 5.0f;
			Console.SetCursorPosition(0, 0);
			Console.WriteLine($"Current zoom: {zoom}");
		};

		RectangleShape frameBuffer = new(res);

		Shader shader = new(null, null, "./shaders/mandel.frag");
		RenderStates states = new(shader);
		shader.SetUniform("resolution", res);

		while (window.IsOpen)
		{
			window.DispatchEvents();

			shader.SetUniform("zoom", zoom);
			window.Draw(frameBuffer, states);
			window.Display();	
		}
	}
}