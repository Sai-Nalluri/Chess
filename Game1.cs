using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chess.UI;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private static readonly Color backgroundColor = new Color(51, 51, 51);

    BoardUI boardUI;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1366;
        _graphics.PreferredBackBufferHeight = 768;
        Window.IsBorderless = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        UIHelper.Initialize(GraphicsDevice);
        boardUI = new BoardUI();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);

        _spriteBatch.Begin();
        Vector2 centerPosition = new Vector2(
            (_graphics.PreferredBackBufferWidth - 640) / 2,  // 640 = 8 squares * 80 pixels
            (_graphics.PreferredBackBufferHeight - 640) / 2
        );
        boardUI.Draw(_spriteBatch, centerPosition);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
