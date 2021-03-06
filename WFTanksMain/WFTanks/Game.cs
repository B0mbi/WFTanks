﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WFTanks
{
    class Game
    {
        private static Form1 FormAccess;
        private List<PictureBox> Walls = new List<PictureBox>();
        public static GameStates CurrentGameState = GameStates.InGame;

        PictureBox EnemyTank = new PictureBox();
        PictureBox AllyTank = new PictureBox();

        public enum Move
        {
            Down,
            Left,
            Right,
            Up
        }

        public enum GameStates
        {
            Win,
            Lose,
            Draw,
            InGame
        }

        public uint _score;

        public uint Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public void SetForm(Form1 FormConstructor)
        {
            FormAccess = FormConstructor;
        }

        public Game()
        {
            try
            {
                Walls.Add((PictureBox)FormAccess.Controls.Find("EagleImage", true)[0]);
            }
            catch (IndexOutOfRangeException) { }
            catch (NullReferenceException) { goto First; }

            foreach (Control c in FormAccess.Controls)
            {
                if (c is PictureBox && ((string)c.Tag == "Wall"))
                { Walls.Add((PictureBox)c); }
            }

            try
            { EnemyTank = (PictureBox)FormAccess.Controls.Find("EnemyTanksDesign", true)[0]; }

            catch (IndexOutOfRangeException)
            { EnemyTank = null; }


            try
            { AllyTank = (PictureBox)FormAccess.Controls.Find("AllyTanksDesign", true)[0]; }

            catch (IndexOutOfRangeException)
            { AllyTank = null; }
            First:
            ;
        }

        public bool Collisions(Move Tank, PictureBox AllyTank)
        {
            if (!Walls.Contains(EnemyTank))
            { Walls.Add(EnemyTank); }

            int a = 1;
            System.Drawing.Rectangle thing = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < Walls.LongCount(); i++)
            {
                if (Walls[i] == null)
                    continue;

                thing = Walls[i].Bounds;

                if (Tank == Move.Left)
                {
                    thing.X += a;
                    if (AllyTank.Bounds.IntersectsWith(thing))
                        return true;

                }

                else if (Tank == Move.Up)
                {
                    thing.Y += a;
                    if (AllyTank.Bounds.IntersectsWith(thing))
                        return true;
                }

                else if (Tank == Move.Down)
                {
                    thing.Y -= a;
                    if (AllyTank.Bounds.IntersectsWith(thing))
                        return true;
                }

                else if (Tank == Move.Right)
                {
                    thing.X -= a;
                    if (AllyTank.Bounds.IntersectsWith(thing))
                        return true;
                }
            }
            return false;
        }

        public bool CollisionsForEnemies(Move Tank, PictureBox EnemyTank)
        {
            if (!Walls.Contains(AllyTank))
            { Walls.Add(AllyTank); }

            int a = 1;
            System.Drawing.Rectangle thing = new System.Drawing.Rectangle(0, 0, 0, 0);

            for (int i = 0; i < Walls.Count(); i++)
            {
                if (Walls[i] == null)
                    continue;

                thing = Walls[i].Bounds;

                if (Tank == Move.Left)
                {
                    thing.X += a;
                    if (EnemyTank.Bounds.IntersectsWith(thing))
                        return true;
                }

                else if (Tank == Move.Up)
                {
                    thing.Y += a;
                    if (EnemyTank.Bounds.IntersectsWith(thing))
                        return true;
                }

                else if (Tank == Move.Down)
                {
                    thing.Y -= a;
                    if (EnemyTank.Bounds.IntersectsWith(thing))
                        return true;
                }

                else if (Tank == Move.Right)
                {
                    thing.X -= a;
                    if (EnemyTank.Bounds.IntersectsWith(thing))
                        return true;
                }
            }
            return false;
        }

        public PictureBox CollisionsForBullets(Move BulletMove, bool isAllyTank, PictureBox Bullet)
        {
            if (isAllyTank && !Walls.Contains(EnemyTank))
            { Walls.Add(EnemyTank); }

            else if (!isAllyTank && !Walls.Contains(AllyTank))
            { Walls.Add(AllyTank); }

            int a = 1;
            System.Drawing.Rectangle thing = new System.Drawing.Rectangle(0, 0, 0, 0);

            var DisEagle = new Action(() => { FormAccess.Controls.Remove(FormAccess.EagleImage); });
            var DisTanks = new Action(() => { FormAccess.Controls.Remove(FormAccess.EnemyTanksDesign); });

            for (int i = 0; i < Walls.Count(); i++)
            {
                if (Walls[i] == null)
                    continue;

                thing = Walls[i].Bounds;

                if (BulletMove == Move.Left)
                {
                    thing.X += a;
                    if (Bullet.Bounds.IntersectsWith(thing))
                    {
                        if (Walls[i].Name == "EagleImage" || Walls[i].Name == "AllyTanksDesign")
                        {
                            FormAccess.EagleImage.Invoke(DisEagle);
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved;
                        }
                        if (Walls[i].Name == "EnemyTanksDesign")
                        {
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved1;
                        }

                        return Walls[i];
                    }

                }

                else if (BulletMove == Move.Up)
                {
                    thing.Y += a;
                    if (Bullet.Bounds.IntersectsWith(thing))
                    {
                        if (Walls[i].Name == "EagleImage" || Walls[i].Name == "AllyTanksDesign")
                        {
                            FormAccess.EagleImage.Invoke(DisEagle);
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved;
                        }
                        if (Walls[i].Name == "EnemyTanksDesign")
                        {
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved1;
                        }

                        return Walls[i];
                    }
                }

                else if (BulletMove == Move.Down)
                {
                    thing.Y -= a;
                    if (Bullet.Bounds.IntersectsWith(thing))
                    {
                        if (Walls[i].Name == "EagleImage" || Walls[i].Name == "AllyTanksDesign")
                        {
                            FormAccess.EagleImage.Invoke(DisEagle);
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved;
                        }
                        if (Walls[i].Name == "EnemyTanksDesign")
                        {
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved1;
                        }

                        return Walls[i];
                    }
                }

                else if (BulletMove == Move.Right)
                {
                    thing.X -= a;
                    if (Bullet.Bounds.IntersectsWith(thing))
                    {
                        if (Walls[i].Name == "EagleImage" || Walls[i].Name == "AllyTanksDesign")
                        {
                            FormAccess.EagleImage.Invoke(DisEagle);
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved;
                        }

                        if (Walls[i].Name == "EnemyTanksDesign")
                        {
                            FormAccess.ControlRemoved += FormAccess_ControlRemoved1;
                        }

                        return Walls[i];
                    }
                }
            }

            if (isAllyTank && Walls.Contains(EnemyTank))
            { Walls.Remove(EnemyTank); }

            else if (!isAllyTank && Walls.Contains(AllyTank))
            { Walls.Remove(AllyTank); }

            return null;
        }

        private void FormAccess_ControlRemoved1(object sender, ControlEventArgs e)
        {
            FormAccess.SetIsEnemyDead(true);
            var isOK = MessageBox.Show("Winner :3");
            Application.Exit();
        }

        private void FormAccess_ControlRemoved(object sender, ControlEventArgs e)
        {
            FormAccess.SetIsEnemyDead(true);
            var isOK = MessageBox.Show("Loser :3");
            Application.Exit();
        }
    }
}
