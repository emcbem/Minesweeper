using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
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
        Expert2,
        Expert3,
        Expert4,
        Expert5,
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
            {Difficulty.Easy, (10, 10, 20)},
            {Difficulty.Medium, (20, 20, 80)},
            {Difficulty.Hard, (30, 30, 300)},
            {Difficulty.Expert, (50, 50, 1500)},
            {Difficulty.Expert2, (50, 50, 1500)},
            {Difficulty.Expert3, (50, 50, 1500)},
            {Difficulty.Expert4, (50, 50, 1500)},
            {Difficulty.Expert5, (50, 50, 1500)},
            {Difficulty.SECRET, (100, 100, 9999)}
        };

        private GameState gameState;
        private int rows = 100, cols = 100;
        private int mines = 20;
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
            if(difficulty != Difficulty.SECRET)
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
                if(e.RightButton == MouseButtonState.Pressed)
                {
                    
                    var im = e.OriginalSource as Image;
                    if (im != null)
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
                }
                var ima = e.OriginalSource as Image;
                if(ima != null)
                {
                    if (ima.Source == Images.Flag) return;
                    if (ima.Source == Images.Clicked) return;
                    string name = ima.Tag.ToString();
                    string[] locations = name.Split(',');
                    int x = int.Parse(locations[0]);
                    int y = int.Parse(locations[1]);
                    if(gameState.board.checkIfBomb(x, y))
                    {
                        gameState.BLOWN();
                        ima.Source = Images.BlownMine;
                    }
                    else
                    {
                        int num = gameState.board.getNumAdj(x, y);
                        ima.Source = TypeToImage[num];

                        emptyTileSpread(x, y);
                        Draw();
                    }
                    return;
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
		
		

        private void Draw()
        {
            DrawGrid();
        }

        //MAKE CHECK SHOWN
        private void DrawGrid()
        {
            for(int i = 0; i<rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    if(gameState.board.checkIfShown(i, j))
                    {
                        int val = gameState.board.getNumAdj(i, j);
                        gridImages[i, j].Source = TypeToImage[val];
                    }
                    else
                    {
                        if(gameState.board.checkIfFlagged(i, j))
                        {
                            gridImages[i, j].Source = Images.Flag;
                        }
                        else
                        {
                            gridImages[i, j].Source = Images.Empty;
                        }
                    }
                }
            }
        }
    }
}
