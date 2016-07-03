using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Gaim
{
    abstract class Menu
    {
        public Menu(Game g)
        {
            this.buttons = new List<Button>();
        }

        protected List<Button> buttons;
        protected int selectedButton = 0;
        protected Texture2D backgroundSprite;
        protected bool prevKey, prevEnter = true;

        public List<Button> Buttons
        {
            get { return this.buttons; }
        }

        public void ResetButtons()
        {
            foreach (Button button in buttons)
                button.prevMouse = true;
            selectedButton = 0;
            prevEnter = true;
            prevKey = true;
        }

        public virtual void Update()
        {
            KeyboardState keyBoard = Keyboard.GetState();
            if (keyBoard.IsKeyDown(Keys.Down) && !prevKey)
            {
                prevKey = true;
                if (selectedButton < buttons.Count - 1)
                    selectedButton++;
                else selectedButton = 0;
            }

            if (keyBoard.IsKeyDown(Keys.Up) & !prevKey)
            {
                prevKey = true;
                if (selectedButton > 0)
                    selectedButton--;
                else selectedButton = buttons.Count - 1;
            }

            if (keyBoard.IsKeyUp(Keys.Up) && keyBoard.IsKeyUp(Keys.Down))
                prevKey = false;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (i != selectedButton && buttons[i].IsSelected())
                    selectedButton = i;
            }

            if (buttons.Count > 0)
            {
                buttons[selectedButton].IsTurnedOn();
                if (keyBoard.IsKeyDown(Keys.Enter) && !prevEnter)
                    buttons[selectedButton].On = true;
            }

            if (keyBoard.IsKeyUp(Keys.Enter))
                prevEnter = false;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(backgroundSprite, Vector2.Zero, Color.White);
            foreach (Button button in buttons)
                button.Draw(sb);
        }
    }

    class MainMenu : Menu
    {
        public MainMenu(Game g)
            : base(g)
        {
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/newoff"), g.Content.Load<Texture2D>("Buttons/newon"), new Vector2(300, 300)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/loadoff"), g.Content.Load<Texture2D>("Buttons/loadon"), new Vector2(300, 400)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/exitoff"), g.Content.Load<Texture2D>("Buttons/exiton"), new Vector2(300, 500)));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/mainmenu");
        }
    }

    class HomeMenu : Menu
    {
        public HomeMenu(Game g)
            : base(g)
        {
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/nextleveloff"), g.Content.Load<Texture2D>("Buttons/nextlevelon"), new Vector2(300, 200)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/upgradeshipoff"), g.Content.Load<Texture2D>("Buttons/upgradeshipon"), new Vector2(250, 340)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/saveoff"), g.Content.Load<Texture2D>("Buttons/saveon"), new Vector2(300, 480)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/mainmenuoff"), g.Content.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(300, 550)));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
        }
    }

    class UpgradeMainMenu : Menu
    {
        public UpgradeMainMenu(Game g)
            : base(g)
        {
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/shiptypeoff"), g.Content.Load<Texture2D>("Buttons/shiptypeon"), new Vector2(400, 200)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/frontweaponoff"), g.Content.Load<Texture2D>("Buttons/frontweaponon"), new Vector2(420, 280)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/rearweaponoff"), g.Content.Load<Texture2D>("Buttons/rearweaponon"), new Vector2(420, 360)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/shieldoff"), g.Content.Load<Texture2D>("Buttons/shieldon"), new Vector2(400, 440)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/engineoff"), g.Content.Load<Texture2D>("Buttons/engineon"), new Vector2(400, 520)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/leftsidekickoff"), g.Content.Load<Texture2D>("Buttons/leftsidekickon"), new Vector2(420, 600)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/rightsidekickoff"), g.Content.Load<Texture2D>("Buttons/rightsidekickon"), new Vector2(420, 680)));
            this.buttons.Add(new Button(g.Content.Load<Texture2D>("Buttons/mainmenuoff"), g.Content.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(100, 680)));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
        }
    }
}
