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
        Level level;
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
        bool newGame = false, prevEsc = false;
        int levelNumber;

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
            mainMenu = new MainMenu(this);
            homeMenu = new HomeMenu(this);
            frontWeaponMenu = new FrontWeaponMenu(this);
            upgradeMainMenu = new UpgradeMainMenu(this);
            rearWeaponMenu = new RearWeaponMenu(this);
            leftSidekickMenu = new LeftSidekickMenu(this);
            rightSidekickMenu = new RightSidekickMenu(this);
            shipTypeMenu = new ShipTypeMenu(this);
            shieldMenu = new ShieldMenu(this);
            engineMenu = new EngineMenu(this);
            cursor = new Cursor(Content.Load<Texture2D>("Cursor"));
        }

        protected void LoadLevel(bool newGame, int levelNumber)
        {
            level = new Level(this, newGame, levelNumber.ToString());
            inGameUI.Add(new Overlay(Content.Load<Texture2D>("Overlays/Sidebar"), new Vector2(600, 0)));
            inGameUI.Add(new Overlay(Content.Load<Texture2D>("Overlays/Announcementbar"), new Vector2(0, 750)));
            shield = new VerticalBar(Content.Load<Texture2D>("Overlays/Shieldbar"), new Vector2(682, 780), level.Player.Shield);
            structure = new VerticalBar(Content.Load<Texture2D>("Overlays/Structurebar"), new Vector2(784, 780), level.Player.Structure);
            engine = new VerticalBar(Content.Load<Texture2D>("Overlays/Enginebar"), new Vector2(667, 382), level.Player.Engine);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            switch (gameState)
            {
                #region Splash Screen
                case GameState.Splashscreen:
                    isCursorVisible = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
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
                        LoadLevel(newGame, levelNumber);
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        gameState = GameState.MainMenu;
                        prevEsc = true;
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
                        shipTypeMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Player.txt"));
                        upgradeMainMenu.Buttons[0].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[1].On)
                    {
                        gameState = GameState.UpgradeMainWeapon;
                        frontWeaponMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[1].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[2].On)
                    {
                        gameState = GameState.UpgradeAltWeapon;
                        rearWeaponMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[2].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[3].On)
                    {
                        gameState = GameState.UpgradeShield;
                        shieldMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Player.txt"));
                        upgradeMainMenu.Buttons[3].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[4].On)
                    {
                        gameState = GameState.UpgradeEngine;
                        engineMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Player.txt"));
                        upgradeMainMenu.Buttons[4].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[5].On)
                    {
                        gameState = GameState.UpgradeLeftSidekick;
                        leftSidekickMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[5].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[6].On)
                    {
                        gameState = GameState.UpgradeRightSidekick;
                        rightSidekickMenu.LoadSave(Path.Combine(StorageContainer.TitleLocation, "Content/Save/Weapons.txt"));
                        upgradeMainMenu.Buttons[6].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (upgradeMainMenu.Buttons[7].On)
                    {
                        gameState = GameState.HomeMenu;
                        upgradeMainMenu.Buttons[7].On = false;
                        upgradeMainMenu.ResetButtons();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        gameState = GameState.HomeMenu;
                        prevEsc = true;
                        upgradeMainMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Ship Type Upgrade
                case GameState.UpgradeShip:
                    isCursorVisible = true;
                    shipTypeMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        shipTypeMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        shipTypeMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Main Weapon Upgrade
                case GameState.UpgradeMainWeapon :
                    isCursorVisible = true;
                    frontWeaponMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        frontWeaponMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        frontWeaponMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Rear Weapon Upgrade
                case GameState.UpgradeAltWeapon:
                    isCursorVisible = true;
                    rearWeaponMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        rearWeaponMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        rearWeaponMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Shield Upgrade
                case GameState.UpgradeShield:
                    isCursorVisible = true;
                    shieldMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        shieldMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        shieldMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Engine Upgrade
                case GameState.UpgradeEngine:
                    isCursorVisible = true;
                    engineMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        engineMenu.Save("Player");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        engineMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Left Sidekick Upgrade
                case GameState.UpgradeLeftSidekick:
                    isCursorVisible = true;
                    leftSidekickMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        leftSidekickMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        leftSidekickMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Right Sidekick Upgrade
                case GameState.UpgradeRightSidekick:
                    isCursorVisible = true;
                    rightSidekickMenu.Update(gameTime);
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !prevEsc)
                    {
                        rightSidekickMenu.Save("Content\\Save\\Weapons.txt");
                        gameState = GameState.UpgradeMenu;
                        prevEsc = true;
                        rightSidekickMenu.ResetButtons();
                    }
                    break;
                #endregion

                #region Game
                case GameState.Game:
                    level.Update(gameTime, this);
                    shield.Update(level.Player.Shield);
                    structure.Update(level.Player.Structure);
                    engine.Update(level.Player.Engine);
                    counter--;
                    Rectangle gameBox = new Rectangle(0, 0, 600, 750);
                    Point muisPointer = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                    if (gameBox.Contains(muisPointer))
                        isCursorVisible = false;
                    else isCursorVisible = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        gameState = GameState.HomeMenu;
                        prevEsc = true;
                        MediaPlayer.Stop();
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.P))
                    {
                        gameState = GameState.Pause;
                        MediaPlayer.Pause();
                    }
                    if (level.win)
                    {
                        gameState = GameState.HomeMenu;
                        MediaPlayer.Stop();
                    }
                    break;
                case GameState.Pause :
                    isCursorVisible = true;
                    if (Keyboard.GetState().IsKeyDown(Keys.U))
                    {
                        gameState = GameState.Game;
                        MediaPlayer.Resume();
                    }
                    break;
                #endregion
            }
            cursor.Update();
            if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                prevEsc = false;
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
                    level.Draw(spriteBatch);
                    foreach (Overlay overlay in inGameUI)
                        overlay.Draw(spriteBatch);
                    shield.Draw(spriteBatch);
                    structure.Draw(spriteBatch);
                    engine.Draw(spriteBatch);
                    spriteBatch.DrawString(font, new Vector2(Mouse.GetState().X, Mouse.GetState().Y).ToString(), new Vector2(620, 420), Color.White);
                    spriteBatch.DrawString(font, level.Score.ToString(), new Vector2(620, 450), Color.White);
                    break;
                case GameState.Pause:
                    spriteBatch.Begin();
                    level.Draw(spriteBatch);
                    foreach (Overlay overlay in inGameUI)
                        overlay.Draw(spriteBatch);
                    shield.Draw(spriteBatch);
                    structure.Draw(spriteBatch);
                    engine.Draw(spriteBatch);
                    spriteBatch.DrawString(font, new Vector2(Mouse.GetState().X, Mouse.GetState().Y).ToString(), new Vector2(620, 420), Color.White);
                    spriteBatch.DrawString(font, level.Score.ToString(), new Vector2(620, 450), Color.White);
                    break;
            }

            if (isCursorVisible)
                cursor.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
