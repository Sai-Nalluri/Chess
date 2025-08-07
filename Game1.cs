using System;
using Chess.GameCore;
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
    GameManager gameManager;

    private bool hasStarted = false;

    Texture2D texture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 1366;
        _graphics.PreferredBackBufferHeight = 768;
        Window.IsBorderless = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 120.0); // 120 FPS
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        texture = Content.Load<Texture2D>("chess pieces/white_pawn");

        UIHelper.Initialize(GraphicsDevice);
        boardUI = new BoardUI();
        gameManager = new GameManager();

        boardUI.Awake(_spriteBatch);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        if (!hasStarted)
        {
            gameManager.Start();
            hasStarted = true;
        }

        gameManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);

        _spriteBatch.Begin();

        _spriteBatch.Draw(texture, new Vector2(100, 100), Color.White);

        Vector2 centerPosition = new Vector2(
            (_graphics.PreferredBackBufferWidth - 640) / 2,
            (_graphics.PreferredBackBufferHeight - 640) / 2
        );
        boardUI.CreateBoardUI(_spriteBatch, centerPosition);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
