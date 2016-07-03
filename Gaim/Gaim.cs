using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

enum GameState
{
    Splashscreen = 0,
    MainMenu = 1,
    HomeMenu = 2,
    UpgradeMenu = 3,
    UpgradeShip = 4,
    UpgradeMainWeapon = 5,
    UpgradeAltWeapon = 6,
    UpgradeShield = 7,
    UpgradeEngine = 8,
    UpgradeLeftSidekick = 9,
    UpgradeRightSidekick = 10,
    Game = 11, 
    Pause = 12,
}

namespace Gaim
{
    public class Gaim : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int counter = 100;
        List<Overlay> inGameUI;
        Overlay splashScreen;
        VerticalBar shield;
        VerticalBar structure;
        VerticalBar engine;
        SpriteFont font;
        GameState gameState;
        #region Menus
        MainMenu mainMenu;
        HomeMenu homeMenu;
        FrontWeaponMenu frontWeaponMenu;
        UpgradeMainMenu upgradeMainMenu;
        RearWeaponMenu rearWeaponMenu;
        LeftSidekickMenu leftSidekickMenu;
        RightSidekickMenu rightSidekickMenu;
        ShipTypeMenu shipTypeMenu;
        ShieldMenu shieldMenu;
        EngineMenu engineMenu;
        #endregion
        Cursor cursor; bool isCursorVisible = false;
        bool newGame = false;
        int levelNumber = 0;

        public Gaim()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            IsMouseVisible = false;
            isCursorVisible = true;
            inGameUI = new List<Overlay>();
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            gameState = GameState.Splashscreen;
            Global.Game = this;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            splashScreen = new Overlay(Content.Load<Texture2D>("Overlays/splash"), Vector2.Zero);
            this.font = Content.Load<SpriteFont>("Testfont");
            mainMenu = new MainMenu();
            homeMenu = new HomeMenu();
            frontWeaponMenu = new FrontWeaponMenu();
            upgradeMainMenu = new UpgradeMainMenu();
            rearWeaponMenu = new RearWeaponMenu();
            leftSidekickMenu = new LeftSidekickMenu();
            rightSidekickMenu = new RightSidekickMenu();
            shipTypeMenu = new ShipTypeMenu();
            shieldMenu = new ShieldMenu();
            engineMenu = new EngineMenu();
            cursor = new Cursor(Content.Load<Texture2D>("Cursor"));
        }

        public void CreateLevel(bool newGame, int levelNumber)
        {
            Global.Level = new Level(newGame, levelNumber.ToString());
            inGameUI.Add(new Overlay(Content.Load<Texture2D>("Overlays/Sidebar"), new Vector2(600, 0)));
            inGameUI.Add(new Overlay(Content.Load<Texture2D>("Overlays/Announcementbar"), new Vector2(0, 750)));
            shield = new VerticalBar(Content.Load<Texture2D>("Overlays/Shieldbar"), new Vector2(682, 780), Global.Player.Shield);
            structure = new VerticalBar(Content.Load<Texture2D>("Overlays/Structurebar"), new Vector2(784, 780), Global.Player.Structure);
            engine = new VerticalBar(Content.Load<Texture2D>("Overlays/Enginebar"), new Vector2(667, 382), Global.Player.Engine);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            switch (gameState)
            {
                #region Splash Screen
                case GameState.Splashscreen:
                    isCursorVisible = true;
                    if (Input.KeyboardButtonPressed(Keys.Enter))
                        gameState = GameState.MainMenu;
                    break;
                #endregion

                #region Main Menu
                case GameState.MainMenu :
                    isCursorVisible = true;
                    mainMenu.Update();
                    if (mainMenu.Buttons[0].On)
                    {
                        mainMenu.Buttons[0].On = false;
                        this.newGame = true;
                        mainMenu.ResetButtons();
                        gameState = GameState.HomeMenu;
                    }
                    if (mainMenu.Buttons[1].On)
                    {
                        mainMenu.Buttons[1].On = false;
                        this.newGame = false;
                        mainMenu.ResetButtons();
                        gameState = GameState.HomeMenu;
                    }
                    if (mainMenu.Buttons[2].On)
                    {
                        Exit();
                    }
                    break;
                #endregion

                #region Home Menu
                case GameState.HomeMenu :
                    isCursorVisible = true;
                    homeMenu.Update();
                    if (homeMenu.Buttons[0].On)
                    {
                        CreateLevel(newGame, levelNumber);
                        gameState = GameState.Game;
                        homeMenu.Buttons[0].On = false;
                        homeMenu.ResetButtons();
                    }
                    if (homeMenu.Buttons[1].On)
                    {
                        gameState = GameState.UpgradeMenu;
                        homeMenu.Buttons[1].On = false;
                        homeMenu.ResetButtons();
                    }
                    if (homeMenu.Buttons[2].On)
                    {
                        newGame = false;
                        homeMenu.Buttons[2].On = false;
                        homeMenu.ResetButtons();
                    }
                    if (homeMenu.Buttons[3].On)
                    {
                        gameState = GameState.MainMenu;
                        homeMenu.Buttons[3].On = false;
                        homeMenu.ResetButtons();
                    }
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        gameState = GameState.MainMenu;
                        homeMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region UpgradeMenu
                case GameState.UpgradeMenu :
                    isCursorVisible = true;
                    upgradeMainMenu.Update();
                    if (upgradeMainMenu.Buttons[0].On)
                    {
                        gameState = GameState.UpgradeShip;
                        shipTypeMenu.LoadSave("Content/Save/Player.txt");
                        upgradeMainMenu.Buttons[0].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[1].On)
                    {
                        gameState = GameState.UpgradeMainWeapon;
                        frontWeaponMenu.LoadSave("Content/Save/Weapons.txt");
                        upgradeMainMenu.Buttons[1].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[2].On)
                    {
                        gameState = GameState.UpgradeAltWeapon;
                        rearWeaponMenu.LoadSave(Path.Combine("Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[2].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[3].On)
                    {
                        gameState = GameState.UpgradeShield;
                        shieldMenu.LoadSave(Path.Combine("Content/Save/Player.txt"));
                        upgradeMainMenu.Buttons[3].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[4].On)
                    {
                        gameState = GameState.UpgradeEngine;
                        engineMenu.LoadSave(Path.Combine("Content/Save/Player.txt"));
                        upgradeMainMenu.Buttons[4].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[5].On)
                    {
                        gameState = GameState.UpgradeLeftSidekick;
                        leftSidekickMenu.LoadSave(Path.Combine("Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[5].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[6].On)
                    {
                        gameState = GameState.UpgradeRightSidekick;
                        rightSidekickMenu.LoadSave(Path.Combine("Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[6].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[7].On)
                    {
                        gameState = GameState.HomeMenu;
                        upgradeMainMenu.Buttons[7].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        gameState = GameState.HomeMenu;
                        upgradeMainMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Ship Type Upgrade
                case GameState.UpgradeShip:
                    isCursorVisible = true;
                    shipTypeMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        shipTypeMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        shipTypeMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Main Weapon Upgrade
                case GameState.UpgradeMainWeapon :
                    isCursorVisible = true;
                    frontWeaponMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        frontWeaponMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        frontWeaponMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Rear Weapon Upgrade
                case GameState.UpgradeAltWeapon:
                    isCursorVisible = true;
                    rearWeaponMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        rearWeaponMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        rearWeaponMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Shield Upgrade
                case GameState.UpgradeShield:
                    isCursorVisible = true;
                    shieldMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        shieldMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        shieldMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Engine Upgrade
                case GameState.UpgradeEngine:
                    isCursorVisible = true;
                    engineMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        engineMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        engineMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Left Sidekick Upgrade
                case GameState.UpgradeLeftSidekick:
                    isCursorVisible = true;
                    leftSidekickMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        leftSidekickMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        leftSidekickMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Right Sidekick Upgrade
                case GameState.UpgradeRightSidekick:
                    isCursorVisible = true;
                    rightSidekickMenu.Update(gameTime);
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        rightSidekickMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        rightSidekickMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Game
                case GameState.Game:
                    Global.Level.Update(gameTime);
                    shield.Update(Global.Player.Shield);
                    structure.Update(Global.Player.Structure);
                    engine.Update(Global.Player.Engine);
                    counter--;
                    Rectangle gameBox = new Rectangle(0, 0, 600, 750);
                    if (gameBox.Contains(Input.MousePoint))
                        isCursorVisible = false;
                    else isCursorVisible = true;
                    if (Input.KeyboardButtonPressed(Keys.Escape))
                    {
                        gameState = GameState.HomeMenu;
                        MediaPlayer.Stop();
                    }
                    if (Input.KeyboardButtonPressed(Keys.P))
                    {
                        gameState = GameState.Pause;
                        MediaPlayer.Pause();
                    }
                    if (Global.Level.win)
                    {
                        gameState = GameState.HomeMenu;
                        MediaPlayer.Stop();
                    }
                    break;
                case GameState.Pause :
                    isCursorVisible = true;
                    if (Input.KeyboardButtonPressed(Keys.U))
                    {
                        gameState = GameState.Game;
                        MediaPlayer.Resume();
                    }
                    break;
                #endregion
            }
            cursor.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            switch (gameState)
            {
                case GameState.Splashscreen:
                    spriteBatch.Begin();
                    splashScreen.Draw(spriteBatch);
                    break;
                case GameState.MainMenu:
                    spriteBatch.Begin();
                    mainMenu.Draw(spriteBatch);
                    break;
                case GameState.HomeMenu:
                    spriteBatch.Begin();
                    homeMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeMenu:
                    spriteBatch.Begin();
                    upgradeMainMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeShip:
                    spriteBatch.Begin();
                    shipTypeMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeMainWeapon:
                    spriteBatch.Begin();
                    frontWeaponMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeAltWeapon:
                    spriteBatch.Begin();
                    rearWeaponMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeShield:
                    spriteBatch.Begin();
                    shieldMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeEngine:
                    spriteBatch.Begin();
                    engineMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeLeftSidekick:
                    spriteBatch.Begin();
                    leftSidekickMenu.Draw(spriteBatch);
                    break;
                case GameState.UpgradeRightSidekick:
                    spriteBatch.Begin();
                    rightSidekickMenu.Draw(spriteBatch);
                    break;
                case GameState.Game:
                    spriteBatch.Begin();
                    Global.Level.Draw(spriteBatch);
                    foreach (Overlay overlay in inGameUI)
                        overlay.Draw(spriteBatch);
                    shield.Draw(spriteBatch);
                    structure.Draw(spriteBatch);
                    engine.Draw(spriteBatch);
                    spriteBatch.DrawString(font, new Vector2(Mouse.GetState().X, Mouse.GetState().Y).ToString(), new Vector2(620, 420), Color.White);
                    spriteBatch.DrawString(font, Global.Level.Score.ToString(), new Vector2(620, 450), Color.White);
                    break;
                case GameState.Pause:
                    spriteBatch.Begin();
                    Global.Level.Draw(spriteBatch);
                    foreach (Overlay overlay in inGameUI)
                        overlay.Draw(spriteBatch);
                    shield.Draw(spriteBatch);
                    structure.Draw(spriteBatch);
                    engine.Draw(spriteBatch);
                    spriteBatch.DrawString(font, new Vector2(Mouse.GetState().X, Mouse.GetState().Y).ToString(), new Vector2(620, 420), Color.White);
                    spriteBatch.DrawString(font, Global.Level.Score.ToString(), new Vector2(620, 450), Color.White);
                    break;
            }

            if (isCursorVisible)
                cursor.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
