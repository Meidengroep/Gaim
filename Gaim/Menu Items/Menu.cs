using System.Collections.Generic;
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
        public Menu()
        {
            this.buttons = new List<Button>();
        }

        protected List<Button> buttons;
        protected int selectedButton = 0;
        protected Texture2D backgroundSprite;

        public List<Button> Buttons
        {
            get { return this.buttons; }
        }

        public void ResetButtons()
        {
            selectedButton = 0;
        }

        public virtual void Update()
        {
            if (Input.KeyboardButtonPressed(Keys.Down))
            {
                if (selectedButton < buttons.Count - 1)
                    selectedButton++;
                else selectedButton = 0;
            }

            if (Input.KeyboardButtonPressed(Keys.Up))
            {
                if (selectedButton > 0)
                    selectedButton--;
                else selectedButton = buttons.Count - 1;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                if (i != selectedButton && buttons[i].IsSelected())
                    selectedButton = i;
            }

            if (buttons.Count > 0)
            {
                buttons[selectedButton].IsTurnedOn();
                if (Input.KeyboardButtonPressed(Keys.Enter))
                    buttons[selectedButton].On = true;
            }
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
        public MainMenu()
        {
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/newoff"), Global.Game.Content.Load<Texture2D>("Buttons/newon"), new Vector2(300, 300)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/loadoff"), Global.Game.Content.Load<Texture2D>("Buttons/loadon"), new Vector2(300, 400)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/exitoff"), Global.Game.Content.Load<Texture2D>("Buttons/exiton"), new Vector2(300, 500)));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/mainmenu");
        }
    }

    class HomeMenu : Menu
    {
        public HomeMenu()
        {
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/nextleveloff"), Global.Game.Content.Load<Texture2D>("Buttons/nextlevelon"), new Vector2(300, 200)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/upgradeshipoff"), Global.Game.Content.Load<Texture2D>("Buttons/upgradeshipon"), new Vector2(250, 340)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/saveoff"), Global.Game.Content.Load<Texture2D>("Buttons/saveon"), new Vector2(300, 480)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/mainmenuoff"), Global.Game.Content.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(300, 550)));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/home");
        }
    }

    class UpgradeMainMenu : Menu
    {
        public UpgradeMainMenu()
        {
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/shiptypeoff"), Global.Game.Content.Load<Texture2D>("Buttons/shiptypeon"), new Vector2(400, 200)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/frontweaponoff"), Global.Game.Content.Load<Texture2D>("Buttons/frontweaponon"), new Vector2(420, 280)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/rearweaponoff"), Global.Game.Content.Load<Texture2D>("Buttons/rearweaponon"), new Vector2(420, 360)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/shieldoff"), Global.Game.Content.Load<Texture2D>("Buttons/shieldon"), new Vector2(400, 440)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/engineoff"), Global.Game.Content.Load<Texture2D>("Buttons/engineon"), new Vector2(400, 520)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/leftsidekickoff"), Global.Game.Content.Load<Texture2D>("Buttons/leftsidekickon"), new Vector2(420, 600)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/rightsidekickoff"), Global.Game.Content.Load<Texture2D>("Buttons/rightsidekickon"), new Vector2(420, 680)));
            this.buttons.Add(new Button(Global.Game.Content.Load<Texture2D>("Buttons/mainmenuoff"), Global.Game.Content.Load<Texture2D>("Buttons/mainmenuon"), new Vector2(100, 680)));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/home");
        }
    }
}
