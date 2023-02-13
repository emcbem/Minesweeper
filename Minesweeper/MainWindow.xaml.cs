using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
    enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Expert,
        SECRET
    }
    public partial class MainWindow : Window
    {
        private readonly Dictionary<int, ImageSource> TypeToImage = new()
        {
            {0, Images.Clicked },
            {1, Images.Tile1 },
            {2, Images.Tile2 },
            {3, Images.Tile3 },
            {4, Images.Tile4 },
            {5, Images.Tile5 },
            {6, Images.Tile6 },
            {7, Images.Tile7 },
            {8, Images.Tile8 }
        };

        private readonly Dictionary<Difficulty, (int rows, int columns, int mines)> DifficultyToType = new()
        {
            {Difficulty.Easy, (10, 10, 10)},
            {Difficulty.Medium, (20, 20, 80)},
            {Difficulty.Hard, (30, 30, 300)},
            {Difficulty.Expert, (50, 50, 2)}
        };

        private GameState gameState;
        private int rows = 100, cols = 100;
        private int mines = 8;
        private  Image[,] gridImages;
        private Difficulty difficulty = Difficulty.Easy;
      

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols, mines);

        }

		
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }
		
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Children.Clear();

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        Tag = $"{i},{j}"
                    };

                    images[i, j] = image;
                    GameGrid.Children.Add(image);
                }
            }

            return images;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
    
        }

        private void StartOver_Click(object sender, RoutedEventArgs e)
        {
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols, mines);
            Draw();
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if(difficulty != Difficulty.Easy)
            {
                difficulty--;
            }
            (int rows, int cols, int mines) result = DifficultyToType[difficulty];
            rows = result.rows; cols = result.cols;
            mines= result.mines;
            StartOver_Click(sender, e);
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if(difficulty != Difficulty.Expert)
            {
                difficulty++;
            }
            (int rows, int cols, int mines) result = DifficultyToType[difficulty];
            rows = result.rows; cols = result.cols;
            mines = result.mines;
            StartOver_Click(sender, e);
        }

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            if(!gameState.isBlown)
            {
                var im = e.OriginalSource as Image;
                if (im != null)
                {
                    if (e.RightButton == MouseButtonState.Pressed)
                    {
                        string name = im.Tag.ToString();
                        string[] locations = name.Split(',');
                        int x = int.Parse(locations[0]);
                        int y = int.Parse(locations[1]);
                        gameState.board.flag(x, y);
                        if (im.Source == Images.Empty) im.Source = Images.Flag;
                        else if (im.Source == Images.Flag) im.Source = Images.Empty;
                        return;
                    }
                    else if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        if (im.Source == Images.Flag) return;
                        if (im.Source == Images.Clicked) return;
                        string name = im.Tag.ToString();
                        string[] locations = name.Split(',');
                        int x = int.Parse(locations[0]);
                        int y = int.Parse(locations[1]);
                        if (gameState.board.checkIfBomb(x, y))
                        {
                            gameState.BLOWN();
                            im.Source = Images.BlownMine;
                        }
                        else if(gameState.board.checkIfShown(x,y))
                        {
                            ClickAround(x, y);
                        }
                        else
                        {
                            int num = gameState.board.getNumAdj(x, y);
                            im.Source = TypeToImage[num];

                            emptyTileSpread(x, y);
                        }
                        return;
                    }
                }
            }
        }

        private void emptyTileSpread(int x, int y)
        {
            if(gameState.board.checkIfShown(x, y))
            {
                return;
            }
            gameState.board.makeShown(x, y);
            DrawTile(x, y);
            if(gameState.board.getNumAdj(x, y) == 0)
            {
              emptyTileSpread(x + 1, y);
              emptyTileSpread(x + 1, y + 1);
              emptyTileSpread(x, y + 1);
              emptyTileSpread(x - 1, y + 1 );
              emptyTileSpread(x - 1, y);
              emptyTileSpread(x - 1, y - 1);
              emptyTileSpread(x, y - 1);
              emptyTileSpread(x + 1, y - 1);
            }
        }

        private void ClickAround(int x, int y)
        {
            if(gameState.board.getNumAdj(x,y) == gameState.board.getFlaggedAdj(x,y))
            {
                DrawTile(x + 1, y);
                DrawTile(x + 1, y + 1);
                DrawTile(x,     y + 1);
                DrawTile(x - 1, y + 1);
                DrawTile(x - 1, y);
                DrawTile(x - 1, y - 1);
                DrawTile(x,     y - 1);
                DrawTile(x + 1, y - 1);
            }
        }
		
		

        private void Draw()
        {
            DrawGrid();
        }

        private void DrawTile(int x, int y)
        {
            if (gameState.board.checkIfShown(x, y))
            {
                int val = gameState.board.getNumAdj(x, y);
                gridImages[x, y].Source = TypeToImage[val];
            }
            else
            {
                if (gameState.board.checkIfFlagged(x, y))
                {
                    gridImages[x, y].Source = Images.Flag;
                }
                else
                {
                    gridImages[x, y].Source = Images.Empty;
                }
            }
        }

        
        private void DrawGrid()
        {
            for(int i = 0; i<rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    DrawTile(i, j);
                }
            }
        }
    }
}
