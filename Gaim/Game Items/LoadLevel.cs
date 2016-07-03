using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Gaim
{
    partial class Level
    {
        protected Texture2D tardSprite, bossSprite;
        protected Texture2D enemyBullet, ballBulletSprite;
        protected string levelNumber, levelName;
        protected List<string> spawnEnemyTypes;
        protected List<int> spawnEnemyLocationsX;
        protected List<int> spawnEnemyLocationsY;
        protected List<int> spawnEnemyTimes;

        protected void LoadLevel(string levelNumber, bool newGame)
        {
            spawnEnemyTypes = new List<string>();
            spawnEnemyLocationsX = new List<int>();
            spawnEnemyLocationsY = new List<int>();
            spawnEnemyTimes = new List<int>(); ;
            this.levelNumber = levelNumber;
            this.player = new Player(new Vector2(200, 200), newGame, false);
            this.bgManager = new Background(levelNumber, 4);
            this.tardSprite = Global.Game.Content.Load<Texture2D>("Enemies/" + levelNumber + "/" + "Tard");
            this.bossSprite = Global.Game.Content.Load<Texture2D>("Enemies/" + levelNumber + "/" + "Boss");
            this.enemyBullet = Global.Game.Content.Load<Texture2D>("Bullets/lol2");
            this.ballBulletSprite = Global.Game.Content.Load<Texture2D>("Bullets/ballBullet");
            ReadLevel();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Global.Game.Content.Load<Song>("Music/LowerAfterlifeBgm"));
            /*this.enemies.Add(new FiringTard(tardSprite, enemyBullet, new Vector2(100, 0), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(200, -50), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(300, -100), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(400, 0), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(150, -50), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(250, -100), g));
            this.enemies.Add(new TardEnemy(tardSprite, new Vector2(350, 0), g));
            this.enemies.Add(new TargetPracticeEnemy(targetSprite, new Vector2(100, 50), g));*/
        }

        protected void ReadLevel()
        {
            StreamReader fileReader = new StreamReader("Content/Levels/" + levelNumber + ".txt.");
            string regel = fileReader.ReadLine();
            int teller = 0;
            while (regel != null)
            {
                switch (teller)
                {
                    case 0:
                        levelName = regel;
                        break;
                    case 1:
                        spawnEnemyTypes.Add(regel);
                        break;
                    case 2:
                        spawnEnemyTimes.Add(int.Parse(regel));
                        break;
                    case 3:
                        spawnEnemyLocationsX.Add(int.Parse(regel));
                        break;
                    case 4:
                        spawnEnemyLocationsY.Add(int.Parse(regel));
                        break;
                    default:
                        break;
                }
                regel = fileReader.ReadLine();
                teller++;
                if (teller == 5)
                    teller = 1;
            }
            fileReader.Close();
        }

        protected void SpawnEnemy(string type, int posX, int posY)
        {
            switch (type)
            {
                case "TardEnemy":
                    this.enemies.Add(new TardEnemy(tardSprite, new Vector2(posX, posY)));
                    break;
                case "ShootingTard":
                    this.enemies.Add(new ShootingTard(tardSprite, enemyBullet, new Vector2(posX, posY)));
                    break;
                case "FromLeftAimingEnemy":
                    this.enemies.Add(new FromLeftAimingEnemy(tardSprite, new Vector2(posX, posY)));
                    break;
                case "FromLeftAimingEnemyLine":
                    this.enemies.Add(new FromLeftAimingEnemy(tardSprite, new Vector2(posX, posY)));
                    this.enemies.Add(new FromLeftAimingEnemy(tardSprite, new Vector2(posX - 60, posY)));
                    this.enemies.Add(new FromLeftAimingEnemy(tardSprite, new Vector2(posX - 120, posY)));
                    this.enemies.Add(new FromLeftAimingEnemy(tardSprite, new Vector2(posX - 180, posY)));
                    break;
                case "FromRightAimingEnemy":
                    this.enemies.Add(new FromRightAimingEnemy(tardSprite, new Vector2(posX, posY)));
                    break;
                case "FromRightAimingEnemyLine":
                    this.enemies.Add(new FromRightAimingEnemy(tardSprite, new Vector2(posX, posY)));
                    this.enemies.Add(new FromRightAimingEnemy(tardSprite, new Vector2(posX + 60, posY)));
                    this.enemies.Add(new FromRightAimingEnemy(tardSprite, new Vector2(posX + 120, posY)));
                    this.enemies.Add(new FromRightAimingEnemy(tardSprite, new Vector2(posX + 180, posY)));
                    break;
                case "TardBoss":
                    this.enemies.Add(new TardBoss(bossSprite, new Vector2(posX, posY), enemyBullet, ballBulletSprite));
                    break;
            }
        }
    }
}
