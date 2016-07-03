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
    #region Super Menu
    abstract class UpgradeMenu : Menu
    {
        public UpgradeMenu(Game g)
            : base(g)
        {
            this.items = new List<Item>();
            this.save = new List<string>();
            this.previewBg = new Background("0", 4, g);
            this.previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
            this.g = g;
        }

        protected List<Item> items;
        protected List<string> save;
        protected Point mousePosition;
        protected int selectedItem = 0;
        protected bool prevMouse = true;
        protected Background previewBg;
        protected PreviewPlayer previewPlayer;
        protected Game g;
        protected string fileName;
        
        public void LoadSave(string file)
        {
            save.Clear();
            StreamReader fileLezer;
            fileLezer = new StreamReader(file);
            string regel = fileLezer.ReadLine();
            while (regel != null)
            {
                save.Add(regel);
                regel = fileLezer.ReadLine();
            }
            fileLezer.Close();
        }

        public abstract void Save(string file);

        public void Update(GameTime gt)
        {
            MouseState mouse = Mouse.GetState();
            KeyboardState keyBoard = Keyboard.GetState();
            if (mouse.LeftButton == ButtonState.Released)
                prevMouse = false;
            mousePosition = new Point(mouse.X, mouse.Y);

            if (keyBoard.IsKeyDown(Keys.Down) && !prevKey)
            {
                prevKey = true;
                if (selectedItem < items.Count - 1)
                    selectedItem++;
                else selectedItem = 0;
                Save("Content\\Save\\" + fileName + ".txt");
                LoadSave("Content\\Save\\" + fileName + ".txt");
                previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
            }

            if (keyBoard.IsKeyDown(Keys.Up) && !prevKey)
            {
                prevKey = true;
                if (selectedItem > 0)
                    selectedItem--;
                else selectedItem = items.Count - 1;
                Save("Content\\Save\\" + fileName + ".txt");
                LoadSave("Content\\Save\\" + fileName + ".txt");
                previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
            }

            if (items[selectedItem].level >= 0)
            {
                if (keyBoard.IsKeyDown(Keys.Left) && items[selectedItem].level > 1 && !items[selectedItem].prevKey)
                {
                    items[selectedItem].level--;
                    items[selectedItem].prevKey = true;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
                }
                else if (keyBoard.IsKeyDown(Keys.Right) && items[selectedItem].level < 5 && !items[selectedItem].prevKey)
                {
                    items[selectedItem].level++;
                    items[selectedItem].prevKey = true;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
                }
                if (keyBoard.IsKeyUp(Keys.Left) && keyBoard.IsKeyUp(Keys.Right))
                    items[selectedItem].prevKey = false;
            }

            if (keyBoard.IsKeyUp(Keys.Up) && keyBoard.IsKeyUp(Keys.Down))
                prevKey = false;

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].box.Contains(mousePosition) && !prevMouse && mouse.LeftButton == ButtonState.Pressed)
                {
                    selectedItem = i;
                    items[i].isSelected = true;
                    for (int j = 0; j < items.Count; j++)
                        if (j != i)
                            items[j].isSelected = false;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(g, new Vector2(275, 700), false, true);
                }
                //items[i].Update();
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (i != selectedItem)
                    items[i].isSelected = false;
                else items[i].isSelected = true;
            }
            previewBg.Update(gt);
            previewPlayer.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            previewBg.Draw(sb);
            previewPlayer.Draw(sb);
            foreach (Item item in items)
                item.Draw(sb);
        }
    }
    #endregion

    #region Weapon Menus
    class FrontWeaponMenu : UpgradeMenu
    {
        public FrontWeaponMenu(Game g)
            : base(g)
        {
            this.items.Add(new PulseCannonItem(g.Content, new Vector2(200, 200), "1"));
            this.items.Add(new LaserItem(g.Content, new Vector2(200, 300), "1"));
            this.items.Add(new MinigunItem(g.Content, new Vector2(200, 400), "1"));
            this.items.Add(new EmptyItem(g.Content, new Vector2(200, 500), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "WeaponsTemp";
        }

        public override void Save(string file)
        {
            save[0] = items[selectedItem].type.ToString();
            save[1] = items[selectedItem].level.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }

    class RearWeaponMenu : UpgradeMenu
    {
        public RearWeaponMenu(Game g)
            : base(g)
        {
            this.items.Add(new FlameThrowerItem(g.Content, new Vector2(200, 200), "1"));
            this.items.Add(new RearMissileItem(g.Content, new Vector2(200, 350), "1"));
            this.items.Add(new EmptyItem(g.Content, new Vector2(200, 500), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "WeaponsTemp";
        }
        public override void Save(string file)
        {
            save[2] = items[selectedItem].type.ToString();
            save[3] = items[selectedItem].level.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }

    class LeftSidekickMenu : UpgradeMenu
    {
        public LeftSidekickMenu(Game g)
            : base(g)
        {
            this.items.Add(new AbombSidekickItem(g.Content, new Vector2(200, 200), "-1"));
            this.items.Add(new EmptyItem(g.Content, new Vector2(200, 300), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "WeaponsTemp";
        }
        public override void Save(string file)
        {
            save[4] = items[selectedItem].type.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }

    class RightSidekickMenu : UpgradeMenu
    {
        public RightSidekickMenu(Game g)
            : base(g)
        {
            this.items.Add(new AbombSidekickItem(g.Content, new Vector2(200, 200), "-1"));
            this.items.Add(new EmptyItem(g.Content, new Vector2(200, 300), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "WeaponsTemp";
        }
        public override void Save(string file)
        {
            save[5] = items[selectedItem].type.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }
    #endregion

    #region Player Items
    class ShipTypeMenu : UpgradeMenu
    {
        public ShipTypeMenu(Game g)
            : base(g)
        {
            this.items.Add(new PhoenixItem(g.Content, new Vector2(200, 200), "-1"));
            this.items.Add(new Test2Item(g.Content, new Vector2(200, 300), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "PlayerTemp";
        }

        public override void Save(string file)
        {
            save[0] = items[selectedItem].type.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }

    class ShieldMenu : UpgradeMenu
    {
        public ShieldMenu(Game g)
            : base(g)
        {
            this.items.Add(new CBarrierLow(g.Content, new Vector2(200, 200), "-1"));
            this.items.Add(new CBarrierMedium(g.Content, new Vector2(200, 300), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "PlayerTemp";
        }

        public override void Save(string file)
        {
            save[1] = items[selectedItem].type.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }

    class EngineMenu : UpgradeMenu
    {
        public EngineMenu(Game g)
            : base(g)
        {
            this.items.Add(new StandardMicrofusion1(g.Content, new Vector2(200, 200), "-1"));
            this.items.Add(new StandardMicrofusion2(g.Content, new Vector2(200, 300), "-1"));
            this.backgroundSprite = g.Content.Load<Texture2D>("Overlays/home");
            this.fileName = "PlayerTemp";
        }

        public override void Save(string file)
        {
            save[2] = items[selectedItem].type.ToString();

            FileStream fs = new FileStream(file, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < save.Count; i++)
                sw.WriteLine(save[i]);

            sw.Close();
            fs.Close();

            save.Clear();
        }
    }
    #endregion

    #region Super Item
    class Item
    {
        public Item(ContentManager cm, Vector2 position, string level)
        {
            this.position = position;
            this.level = int.Parse(level);
            this.font = cm.Load<SpriteFont>("Testfont");
        }

        protected Texture2D itemIcon;
        protected Vector2 position;
        public int level;
        public string type;
        protected SpriteFont font;
        protected Point mousePosition;
        public Rectangle box;
        public bool prevKey = true;
        public bool isSelected = false;

        public void Update()
        {
            if (level >= 0)
            {
                if (isSelected)
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) && level > 1 && !prevKey)
                    {
                        level--;
                        prevKey = true;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Right) && level < 5 && !prevKey)
                    {
                        level++;
                        prevKey = true;
                    }
                if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
                    prevKey = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(itemIcon, position, Color.White);
            if (level >= 0)
            {
                if (!isSelected)
                    sb.DrawString(font, level.ToString(), position + new Vector2(itemIcon.Width + 20, 0), Color.White);
                else sb.DrawString(font, level.ToString(), position + new Vector2(itemIcon.Width + 20, 0), Color.Red);
            }
            else
            {
                if (!isSelected)
                    sb.DrawString(font, type, position + new Vector2(itemIcon.Width + 20, 0), Color.White);
                else sb.DrawString(font, type, position + new Vector2(itemIcon.Width + 20, 0), Color.Red);
            }
        }
    }

    class EmptyItem : Item
    {
        public EmptyItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Icons/empty");
            this.type = "None";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Fontgun Items
    class PulseCannonItem : Item
    {
        public PulseCannonItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Bullets/lol");
            this.type = "Basic";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    class LaserItem : Item
    {
        public LaserItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Bullets/Laser");
            this.type = "Laser";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    class MinigunItem : Item
    {
        public MinigunItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Bullets/Minigun");
            this.type = "Minigun";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Reargun Items
    class FlameThrowerItem : Item
    {
        public FlameThrowerItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Particles/Fire/Fire1");
            this.type = "Flamethrower";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }

    class RearMissileItem : Item
    {
        public RearMissileItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Bullets/Rocket");
            this.type = "RearMissile";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Sidekick Items
    class AbombSidekickItem : Item
    {
        public AbombSidekickItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Bullets/Rocket");
            this.type = "ABomb";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Ship Type Items
    class PhoenixItem : Item
    {
        public PhoenixItem(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Ships/Phoenix/straight");
            this.type = "Phoenix";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }

    class Test2Item : Item
    {
        public Test2Item(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Ships/Test2/straight");
            this.type = "Test2";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Shield Items
    class CBarrierLow : Item
    {
        public CBarrierLow(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Icons/CBarrierLow");
            this.type = "CBarrierLow";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    class CBarrierMedium : Item
    {
        public CBarrierMedium(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Icons/CBarrierMedium");
            this.type = "CBarrierMedium";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion

    #region Engine Items
    class StandardMicrofusion1 : Item
    {
        public StandardMicrofusion1(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Icons/StandardMicrofusion1");
            this.type = "StandardMicrofusion1";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }

    class StandardMicrofusion2 : Item
    {
        public StandardMicrofusion2(ContentManager cm, Vector2 position, string level)
            : base(cm, position, level)
        {
            this.itemIcon = cm.Load<Texture2D>("Icons/StandardMicrofusion2");
            this.type = "StandardMicrofusion2";
            this.box = new Rectangle((int)position.X, (int)position.Y, itemIcon.Width * 10, itemIcon.Height);
        }
    }
    #endregion
}
