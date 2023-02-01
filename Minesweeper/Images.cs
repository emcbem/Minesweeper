using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Minesweeper
{
	public static class Images
	{
		public readonly static ImageSource Empty = LoadImage("BlankTile.png");
        public readonly static ImageSource Tile1 = LoadImage("1Tile.png");
        public readonly static ImageSource Tile2 = LoadImage("2Tile.png");
        public readonly static ImageSource Tile3 = LoadImage("3Tile.png");
        public readonly static ImageSource Tile4 = LoadImage("4Tile.png");
        public readonly static ImageSource Tile5 = LoadImage("5Tile.png");
        public readonly static ImageSource Tile6 = LoadImage("6Tile.png");
        public readonly static ImageSource Tile7 = LoadImage("7Tile.png");
        public readonly static ImageSource Tile8 = LoadImage("8Tile.png");
        public readonly static ImageSource Clicked =  LoadImage("ClickedTile.png");
        public readonly static ImageSource Flag = LoadImage("Flagged.png");
        public readonly static ImageSource BlownMine = LoadImage("BlownUpMine.png");
        public readonly static ImageSource ShownMine = LoadImage("ShownMine.png");

        private static ImageSource LoadImage(string fileName)
		{
			return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
		
		}
		
	}
}

