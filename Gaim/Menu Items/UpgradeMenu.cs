using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Gaim
{
    #region Super Menu
    abstract class UpgradeMenu : Menu
    {
        public UpgradeMenu()
        {
            this.items = new List<Item>();
            this.save = new List<string>();
            this.previewBg = new Background("0", 4);
            this.previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
            //Global.Game.LoadLevel(false, -1);
        }

        protected List<Item> items;
        protected List<string> save;
        protected int selectedItem = 0;
        protected Background previewBg;
        protected PreviewPlayer previewPlayer;
        protected string fileName;
        protected int xBegin = 610, yBegin = 100;
        
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

            UpdateToLoadedSave();
        }

        public abstract void Save(string file);

        public abstract void UpdateToLoadedSave();

        public void Update(GameTime gt)
        {

            if (Input.KeyboardButtonPressed(Keys.Down))
            {
                if (selectedItem < items.Count - 1)
                    selectedItem++;
                else selectedItem = 0;
                Save("Content\\Save\\" + fileName + ".txt");
                LoadSave("Content\\Save\\" + fileName + ".txt");
                previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
            }

            if (Input.KeyboardButtonPressed(Keys.Up))
            {
                if (selectedItem > 0)
                    selectedItem--;
                else selectedItem = items.Count - 1;
                Save("Content\\Save\\" + fileName + ".txt");
                LoadSave("Content\\Save\\" + fileName + ".txt");
                previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
            }

            if (items[selectedItem].level >= 0)
            {
                if (Input.KeyboardButtonPressed(Keys.Left) && items[selectedItem].level > 1)
                {
                    items[selectedItem].level--;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
                }
                else if (Input.KeyboardButtonPressed(Keys.Right) && items[selectedItem].level < 5)
                {
                    items[selectedItem].level++;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].box.Contains(Input.MousePoint) && Input.MouseLeftClick)
                {
                    selectedItem = i;
                    items[i].isSelected = true;
                    for (int j = 0; j < items.Count; j++)
                        if (j != i)
                            items[j].isSelected = false;
                    Save("Content\\Save\\" + fileName + ".txt");
                    LoadSave("Content\\Save\\" + fileName + ".txt");
                    previewPlayer = new PreviewPlayer(new Vector2(275, 700), false, true);
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
        public FrontWeaponMenu()
        {
            this.items.Add(new PulseCannonItem(Global.Game.Content, new Vector2(xBegin, yBegin), "1"));
            this.items.Add(new LaserItem(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "1"));
            this.items.Add(new MinigunItem(Global.Game.Content, new Vector2(xBegin, yBegin + 200), "1"));
            this.items.Add(new EmptyItem(Global.Game.Content, new Vector2(xBegin, yBegin + 300), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[0] == items[i].type)
                {
                    selectedItem = i;
                    items[i].level = int.Parse(save[1]);
                }
                else items[i].level = 1;
            }
        }
    }

    class RearWeaponMenu : UpgradeMenu
    {
        public RearWeaponMenu()
        {
            this.items.Add(new FlameThrowerItem(Global.Game.Content, new Vector2(xBegin, yBegin), "1"));
            this.items.Add(new RearMissileItem(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "1"));
            this.items.Add(new EmptyItem(Global.Game.Content, new Vector2(xBegin, yBegin + 200), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[2] == items[i].type)
                {
                    selectedItem = i;
                    items[i].level = int.Parse(save[3]);
                }
                else items[i].level = 1;
            }
        }
    }

    class LeftSidekickMenu : UpgradeMenu
    {
        public LeftSidekickMenu()
        {
            this.items.Add(new AbombSidekickItem(Global.Game.Content, new Vector2(xBegin, yBegin), "-1"));
            this.items.Add(new EmptyItem(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[4] == items[i].type)
                    selectedItem = i;
            }
        }
    }

    class RightSidekickMenu : UpgradeMenu
    {
        public RightSidekickMenu()
        {
            this.items.Add(new AbombSidekickItem(Global.Game.Content, new Vector2(xBegin, yBegin), "-1"));
            this.items.Add(new EmptyItem(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[5] == items[i].type)
                    selectedItem = i;
            }
        }
    }
    #endregion

    #region Utility Menus
    class ShipTypeMenu : UpgradeMenu
    {
        public ShipTypeMenu()
        {
            this.items.Add(new PhoenixItem(Global.Game.Content, new Vector2(xBegin, yBegin), "-1"));
            this.items.Add(new Test2Item(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[0] == items[i].type)
                    selectedItem = i;
            }
        }
    }

    class ShieldMenu : UpgradeMenu
    {
        public ShieldMenu()
        {
            this.items.Add(new CBarrierLow(Global.Game.Content, new Vector2(xBegin, yBegin), "-1"));
            this.items.Add(new CBarrierMedium(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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

        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[1] == items[i].type)
                    selectedItem = i;
            }
        }
    }

    class EngineMenu : UpgradeMenu
    {
        public EngineMenu()
        {
            this.items.Add(new StandardMicrofusion1(Global.Game.Content, new Vector2(xBegin, yBegin), "-1"));
            this.items.Add(new StandardMicrofusion2(Global.Game.Content, new Vector2(xBegin, yBegin + 100), "-1"));
            this.backgroundSprite = Global.Game.Content.Load<Texture2D>("Overlays/UpgradeMenu");
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
        public override void UpdateToLoadedSave()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (save[2] == items[i].type)
                    selectedItem = i;
            }
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
        public Rectangle box;
        public bool isSelected = false;

        public void Update()
        {
            if (level >= 0)
            {
                if (isSelected)
                    if (Input.KeyboardButtonPressed(Keys.Left) && level > 1)
                    {
                        level--;
                    }
                    else if (Input.KeyboardButtonPressed(Keys.Right) && level < 5)
                    {
                        level++;
                    }
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

    #region Frontgun Items
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
