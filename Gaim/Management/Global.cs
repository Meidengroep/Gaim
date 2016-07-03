using Microsoft.Xna.Framework;

namespace Gaim
{
    static class Global
    {
        #region Members

        static Gaim game;
        static Level level;
        static Player player;

        #endregion

        public static Gaim Game
        {
            get { return game; }
            set { game = value; }
        }

        public static Level Level
        {
            get { return level; }
            set { level = value; }
        }

        public static Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public static int ViewPortWidth
        {
            get { return game.GraphicsDevice.Viewport.Width; }
        }

        public static int ViewPortHeight
        {
            get { return game.GraphicsDevice.Viewport.Height; }
        }
    }
}