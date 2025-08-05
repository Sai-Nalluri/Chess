using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess.UI;

public static class UIHelper
{
    private static Texture2D _pixel;

    public static void Initialize(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    // Draw filled rectangle
    public static void DrawSquare(SpriteBatch spriteBatch, Rectangle rect, Color fillColor)
    {
        spriteBatch.Draw(_pixel, rect, fillColor);
    }

    // Check if mouse is inside a rectangle
    public static bool IsMouseOver(Rectangle rect)
    {
        MouseState mouse = Mouse.GetState();
        return rect.Contains(mouse.Position);
    }

}