using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeaBattle_CourseWork
{
    public partial class MainForm : Form
    {

        private Player _humanPlayer;
        private Player _computerPlayer;

        private Board _humanBoard;
        private Board _computerBoard;

        private GameConroller _controller;

        private ScoreBoard _scoreBoard;

        private Button _randomButton;
        private Button _startGameButton;
        private Button _replayGameButton;

        private Button _mainMenu_NewGameButton;
        private Button _mainMenu_StaticticButton;
        private Button _mainMenu_exitButton;

        private Button _classicModeButton;
        private Button _singleDeckModeButton;
        private Button _twoShipsModeButton;

        private Button _BactToMainMenuButton;

        private TextBox _PlayerNameTextBox;

        private RadioButton _difficultyEasyButton;
        private RadioButton _difficultyNormalButton;

        //Конструктор формы
        public MainForm()
        {
             SuspendLayout();

            _humanBoard = new Board();
            _computerBoard = new Board(false);

            _humanPlayer = new HumanPlayer("Player", _computerBoard);
            _computerPlayer = new ComputerPlayer("Computer");

            _randomButton = FormElementsCreator.CreateButtonForMainMenu("Random", Color.White, new Size(150, 50));
            _replayGameButton = FormElementsCreator.CreateButtonForMainMenu("Replay?", Color.White, new Size(150, 50));
            _startGameButton = FormElementsCreator.CreateButtonForMainMenu("Start Game", Color.White, new Size(150, 50));

            _BactToMainMenuButton = FormElementsCreator.CreateButtonForMainMenu("Back to Menu", Color.White, new Size(150, 50));

            _mainMenu_NewGameButton = FormElementsCreator.CreateButtonForMainMenu("New game", Color.White, new Size(150, 50));
            _mainMenu_StaticticButton = FormElementsCreator.CreateButtonForMainMenu("Statistics", Color.White, new Size(150, 50));
            _mainMenu_exitButton = FormElementsCreator.CreateButtonForMainMenu("Exit", Color.White, new Size(150, 50));

            _classicModeButton = FormElementsCreator.CreateButtonForMainMenu("Classic mode", Color.White, new Size(150, 50));
            _

            _scoreBoard = new ScoreBoard(_humanPlayer, _computerPlayer, 10);
            _controller = new GameConroller(_humanPlayer, _computerPlayer, _humanBoard, _computerBoard, _scoreBoard);

            _PlayerNameTextBox = FormElementsCreator.CreateTextBox("Player", Color.White);

            _difficultyEasyButton = FormElementsCreator.CreateRadioButton("Easy Level");
            _difficultyNormalButton = FormElementsCreator.CreateRadioButton("Normal Level");

            SetupWindow();
            LayoutControls();

            _scoreBoard.GameEnded += OnGameEnded;

            _randomButton.Click += OnRandomButtonClick;
            _replayGameButton.Click += OnReplayButtonClick;
            _startGameButton.Click += OnStartGameButtonClick;

            _mainMenu_exitButton.Click += OnMainMenuExitButtonClick;
            _mainMenu_NewGameButton.Click += OnMainMenuNewGameButtonClick;
            _PlayerNameTextBox.TextChanged += OnPlayerNameTextBoxChanged;

            _BactToMainMenuButton.Click += OnBackToMenuButtonClick;

            _difficultyEasyButton.Click += EasyButtonChecked;
            _difficultyNormalButton.Click += NormalButtonChecked;

            ResumeLayout();
            StartProgram();
        }

        //Инициализация программы и стратового меню
        private void StartProgram()
        {
            _mainMenu_NewGameButton.Visible = true;  
            _mainMenu_StaticticButton.Visible = true;
            _mainMenu_exitButton.Visible = true;
            _computerBoard.Visible = false;
            _humanBoard.Visible = false;
            _replayGameButton.Visible = false;
            _startGameButton.Visible = false;
            _randomButton.Visible = false;
            _scoreBoard.Visible = false;
            _PlayerNameTextBox.Visible = false;
            _difficultyEasyButton.Visible = false;
            _difficultyNormalButton.Visible = false;
            _BactToMainMenuButton.Visible = false;
        }
   
        //Инициализация параметром окна
        private void SetupWindow()
        {
            AutoScaleDimensions = new SizeF(8, 19);
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Calibri", 10, FontStyle.Regular, GraphicsUnit.Point, 186);
            Margin = Padding.Empty;
            Text = "Sea Battle"; 
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
        }   

        private static TextBox CreateTextButton()
        {
            return new TextBox();
        }

        //Инициализация контроллеров
        private void LayoutControls()
        {
            _mainMenu_NewGameButton.Location = new Point(225, 100);
            _mainMenu_StaticticButton.Location = new Point(225, 170);
            _mainMenu_exitButton.Location = new Point(225, 240);
            _humanBoard.Location = new Point(0, 0);
            _computerBoard.Location = new Point(_humanBoard.Right, 0);
            _scoreBoard.Location = new Point(25, _humanBoard.Bottom);
            _scoreBoard.Width = _computerBoard.Right - 25;
            _replayGameButton.Location = new Point(_computerBoard.Right - _replayGameButton.Width, _scoreBoard.Bottom);
            _startGameButton.Location = _replayGameButton.Location;
            _randomButton.Location = new Point(_replayGameButton.Location.X - _randomButton.Width - 25, _replayGameButton.Location.Y);
            _PlayerNameTextBox.Location = new Point(25, _humanBoard.Bottom);
            _difficultyEasyButton.Location = new Point(_humanBoard.Right + 50, 50);
            _difficultyNormalButton.Location = new Point(_humanBoard.Right + 50, 100);
            _BactToMainMenuButton.Location = new Point(_randomButton.Location.X - _BactToMainMenuButton.Width - 25, _randomButton.Location.Y);
            _difficultyEasyButton.Checked = true;

            Controls.AddRange(new Control[]
            {
                _humanBoard,
                _computerBoard,
                _scoreBoard,
                _replayGameButton,
                _randomButton,
                _startGameButton,
                _mainMenu_NewGameButton,
                _mainMenu_StaticticButton,
                _mainMenu_exitButton,
                _PlayerNameTextBox,
                _difficultyEasyButton,
                _difficultyNormalButton,
                _BactToMainMenuButton
            });

            ClientSize = new Size(_computerBoard.Right + 25, _startGameButton.Bottom + 25);
        }

        private void OnPlayerNameTextBoxChanged(object sender, EventArgs e)
        {
            _humanPlayer.Name = _PlayerNameTextBox.Text;
            _scoreBoard.ChangeFirstPlayerName(_humanPlayer.Name);
        }

        //События запуска новой игры
        private void OnMainMenuNewGameButtonClick(object sender, EventArgs e)
        {
            _mainMenu_NewGameButton.Visible = false;
            _mainMenu_StaticticButton.Visible = false;
            _mainMenu_exitButton.Visible = false;
            _humanBoard.Visible = true;
            _replayGameButton.Visible = true;
            _startGameButton.Visible = true;
            _randomButton.Visible = true;
            _PlayerNameTextBox.Visible = true;
            _difficultyEasyButton.Visible = true;
            _difficultyNormalButton.Visible = true;
            _BactToMainMenuButton.Visible = true;
            StartNewGame();
        }

        private void OnBackToMenuButtonClick(object sender, EventArgs e)
        {
            _scoreBoard.RefreshBoard();
            _PlayerNameTextBox.Text = "Player";
            StartProgram();
        }

        //Событие выхода из программы
        private void OnMainMenuExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnReplayButtonClick(object sender, EventArgs e)
        {
            _computerBoard.Visible = false;
            _scoreBoard.Visible = false;
            _difficultyEasyButton.Visible = true;
            _difficultyNormalButton.Visible = true;
            _PlayerNameTextBox.Visible = true;
            StartNewGame();
        }

        private void StartNewGame()
        {
            _randomButton.Visible = true;
            _startGameButton.Visible = true;
            _replayGameButton.Visible = false;
            _controller.NewGame();
        }

        private void OnStartGameButtonClick(object sender, EventArgs e)
        {
            _randomButton.Visible = false;
            _replayGameButton.Visible = false;
            _startGameButton.Visible = false;
            _scoreBoard.Visible = true;
            _computerBoard.Visible = true;
            _BactToMainMenuButton.Visible = false;
            _controller.StartGame();
        }

        private void OnRandomButtonClick(object sender, EventArgs e)
        {
            _humanBoard.AddRandomShips();
        }

        private void OnGameEnded(object sender, EventArgs e)
        {
            _randomButton.Visible = false;
            _startGameButton.Visible = false;
            _replayGameButton.Visible = true;
            _BactToMainMenuButton.Visible = true;
            _computerBoard.ShowShips();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainForm
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        private void EasyButtonChecked(object sender, EventArgs e)
        {
            _controller.SetEasyLevel();
        }
        private void NormalButtonChecked(object sender, EventArgs e)
        {
            _controller.SetNormLevel();
        }
    }
}


