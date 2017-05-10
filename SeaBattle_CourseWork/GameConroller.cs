﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle_CourseWork
{
    public class GameConroller
    {
        private readonly Player _player1;
        private readonly Player _player2;
        private readonly Board _board1;
        private readonly Board _board2;
        private readonly ScoreBoard _scoreBoard;

        public GameConroller(Player player1, Player player2, Board board1, Board board2, ScoreBoard scoreBoard)
        {
            _player1 = player1;
            _player2 = player2;
            _board1 = board1;
            _board2 = board2;
            _scoreBoard = scoreBoard;

            _player1.Shooting += OnPlayerShooting;
            _player2.Shooting += OnPlayerShooting;

            _player1.Shot += OnPlayerShotShot;
            _player2.Shot += OnPlayerShotShot;
        }

        private void OnPlayerShotShot(object sender, ShootingEventArgs e)
        {
            if (_scoreBoard.GameHasEnded())
                return;

            var shooter = (Player)sender;
            var oponent = shooter == _player1 ? _player2 : _player1;

            if (e.Result != ShotResult.Missed)
            {
                shooter.Shoot();
            }
            else
            {
                oponent.Shoot();
            }
        }

        private void OnPlayerShooting(object sender, ShootingEventArgs e)
        {
            var shooter = (Player)sender;
            Board oponentBoard;
            Player oponent;
            if(shooter == _player1)
            {
                oponent = _player2;
                oponentBoard = _board2;
            }
            else
            {
                oponent = _player1;
                oponentBoard = _board1;
            }

            var shotResult = oponentBoard.OponentShotAt(e.X, e.Y);
            e.Result = shotResult;

            if (_scoreBoard.GameHasEnded())
                return;
        }

        public void NewGame()
        {
            _board1.Mode = BoardMode.Design;
            _board2.Mode = BoardMode.Design;
            _board1.AddRandomShips();
            _board2.AddRandomShips();
            _player1.Reset();
            _player2.Reset();
            _scoreBoard.NewGame();
        }

        public void StartGame()
        {
            var playerIndex = new Random(DateTime.Now.Millisecond).Next(1, 3);
            var player = playerIndex == 1 ? _player1 : _player2;

            _board1.Mode = BoardMode.Game;
            _board2.Mode = BoardMode.Game;

            _scoreBoard.NewGame();
            player.Shoot();
        }

        public void SetEasyLevel()
        {
            ((ComputerPlayer)_player2).SetEasyLevel();
        }

        public void SetNormLevel()
        {
            ((ComputerPlayer)_player2).SetNormLevel();
        }
    }
}
