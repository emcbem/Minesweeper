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
using Syncfusion.SfSkinManager;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        
        private GameState gameState;
		#region Fields
        private string currentVisualStyle;
		private string currentSizeMode;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current visual style.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentVisualStyle
        {
            get
            {
                return currentVisualStyle;
            }
            set
            {
                currentVisualStyle = value;
                OnVisualStyleChanged();
            }
        }
		
		/// <summary>
        /// Gets or sets the current Size mode.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        public string CurrentSizeMode
        {
            get
            {
                return currentSizeMode;
            }
            set
            {
                currentSizeMode = value;
                OnSizeModeChanged();
            }
        }

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
			this.Loaded += OnLoaded;
        }
		/// <summary>
        /// Called when [loaded].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CurrentVisualStyle = "Windows11Dark";
	        CurrentSizeMode = "Default";
        }
		/// <summary>
        /// On Visual Style Changed.
        /// </summary>
        /// <remarks></remarks>
        private void OnVisualStyleChanged()
        {
            VisualStyles visualStyle = VisualStyles.Default;
            Enum.TryParse(CurrentVisualStyle, out visualStyle);            
            if (visualStyle != VisualStyles.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetVisualStyle(this, visualStyle);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

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

        private void Mouse_Down(object sender, MouseEventArgs e)
        {
            //if(!gameState.isBlown)
            //{
                if(e.RightButton == MouseButtonState.Pressed)
                {
                    
                    var im = e.OriginalSource as Image;
                    if (im != null)
                    {
                        if (im.Source == Images.Empty) im.Source = Images.Flag;
                        else if (im.Source == Images.Flag) im.Source = Images.Empty;
                        return;
                    }
                }
                var ima = e.OriginalSource as Image;
                if(ima != null)
                {
                    if (ima.Source == Images.Flag) return;
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
                        ima.Source = TypeToImage[gameState.board.getNumAdj(x, y)];
                    }
                }


            //}
        }
		
		/// <summary>
        /// On Size Mode Changed event.
        /// </summary>
        /// <remarks></remarks>
        private void OnSizeModeChanged()
        {
            SizeMode sizeMode = SizeMode.Default;
            Enum.TryParse(CurrentSizeMode, out sizeMode);
            if (sizeMode != SizeMode.Default)
            {
                SfSkinManager.ApplyStylesOnApplication = true;
                SfSkinManager.SetSizeMode(this, sizeMode);
                SfSkinManager.ApplyStylesOnApplication = false;
            }
        }

        private void Draw()
        {
            DrawGrid();
        }

    

        private void DrawGrid()
        {
            for(int i = 0; i<rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    int val = gameState.board.getNumAdj(i, j);
                    gridImages[i, j].Source = TypeToImage[val];
                }
            }
        }
    }
}
