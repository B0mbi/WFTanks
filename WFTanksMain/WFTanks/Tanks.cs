﻿namespace WFTanks
{
    abstract class Tanks
    {
        protected enum Levels
        {
            Small,
            Medium,
            Big
        }

        protected enum Sides
        {
            AI,
            Buddy
        }

        protected Sides Side;

        private uint _currentPositionX;

        public uint CurrentPositionX
        {
            get { return _currentPositionX; }
            set { _currentPositionX = value; }
        }

        private uint _currentPositionY;

        public uint CurrentPositionY
        {
            get { return _currentPositionY; }
            set { _currentPositionY = value; }
        }

        private int _healthPoints;

        public int HealthPoints
        {
            get { return _healthPoints; }
            set { _healthPoints = value; }
        }

        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set { _playerName = value; }
        }


        protected Levels CurrentLevel;
        // TODO Skin!!!!!
        public virtual void Shot(Game.Move TankDirection, Form1 FormAccess)
        {
            // TODO Tanks : Shot()
        }

        public void Movement()
        {
            // TODO Tanks : Movement()
        }
    }
}
