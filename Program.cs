using SFML.Graphics;
using SFML.Window;

namespace Mandelbrot;

internal class Program
{
	private const uint ScreenWidth = 1080;
	private const uint ScreenHeight = 1080;

	static private double zoom = 2.3;

	static void Main()
	{
		var window = new RenderWindow(new VideoMode(ScreenWidth, ScreenHeight), "Mandelbrot", Styles.Default);
		window.Closed += (s, e) => window.Close();
		window.MouseWheelScrolled += MouseWheelMoved;

		var pixels = new Image(ScreenWidth, ScreenHeight);
		var texture = new Texture(ScreenWidth, ScreenHeight);
		var frameBuffer = new Sprite(texture);

		while (window.IsOpen)
		{
			window.DispatchEvents();

			Parallel.For(0, ScreenHeight, y =>
			{
				for (int x = 0; x < ScreenWidth; x++)
				{
					var pixel = (x: (((double)x / ScreenWidth) - 0.75) * zoom, y: (((double)y / ScreenHeight) - 0.5) * zoom);
					var color = Mandelbrot(pixel);
					pixels.SetPixel((uint)x, (uint)y, color);
				}
			});

			texture.Update(pixels);
			window.Draw(frameBuffer);
			window.Display(); 
		}
	}

	private static void MouseWheelMoved(object? _, MouseWheelScrollEventArgs e)
	{
		zoom -= e.Delta / 10.0;
		Console.SetCursorPosition(0, 0);
		Console.WriteLine($"Current zoom: {zoom}");
	}

	static Color Mandelbrot(in (double x, double y) c)
	{
		var z = (x: 0.0, y: 0.0);
		var z2 = (x: 0.0, y: 0.0);

		for (int i = 0; i < 128; i++)
		{
			z = (z2.x - z2.y + c.x, z.y * (z.x + z.x) + c.y);
			z2 = (z.x * z.x, z.y * z.y);

			if (z2.x + z2.y > 4.0)
			{
				return new Color((byte)(i * 2), (byte)(i * 4), (byte)(i * 8));
			}
		}

		return new Color(0, 0, 0, 255);
	}
}