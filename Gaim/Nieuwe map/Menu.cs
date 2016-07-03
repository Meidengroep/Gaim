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
        public Menu(ContentManager cm)
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
        }

        public virtual void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !prevKey)
            {
                prevKey = true;
                if (selectedButton < buttons.Count - 1)
                    selectedButton++;
                else selectedButton = 0;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) & !prevKey)
            {
                prevKey = true;
                if (selectedButton > 0)
                    selectedButton--;
                else selectedButton = buttons.Count - 1;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down))
                prevKey = false;

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].IsSelected())
                    selectedButton = i;
            }

            if (buttons.Count > 0)
                buttons[selectedButton].IsTurnedOn();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !prevEnter)
                buttons[selectedButton].On = true;

            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
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
        public MainMenu(ContentManager cm)
            : base(cm)
        {
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/newoff"), cm.Load<Texture2D>("Buttons/newon"), new Vector2(300, 300)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/loadoff"), cm.Load<Texture2D>("Buttons/loadon"), new Vector2(300, 400)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/exitoff"), cm.Load<Texture2D>("Buttons/exiton"), new Vector2(300, 500)));
            this.backgroundSprite = cm.Load<Texture2D>("Overlays/mainmenu");
        }
    }

    class HomeMenu : Menu
    {
        public HomeMenu(ContentManager cm)
            : base(cm)
        {
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/nextleveloff"), cm.Load<Texture2D>("Buttons/nextlevelon"), new Vector2(300, 200)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/upgradeshipoff"), cm.Load<Texture2D>("Buttons/upgradeshipon"), new Vector2(250, 340)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/saveoff"), cm.Load<Texture2D>("Buttons/saveon"), new Vector2(300, 480)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/mainmenuoff"), cm.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(300, 550)));
            this.backgroundSprite = cm.Load<Texture2D>("Overlays/home");
        }
    }

    class UpgradeMainMenu : Menu
    {
        public UpgradeMainMenu(ContentManager cm)
            : base(cm)
        {

            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/nextleveloff"), cm.Load<Texture2D>("Buttons/nextlevelon"), new Vector2(300, 200)));
            this.buttons.Add(new Button(cm.Load<Texture2D>("Buttons/mainmenuoff"), cm.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(300, 550)));
            this.backgroundSprite = cm.Load<Texture2D>("Overlays/home");
        }
    }
}
