﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFTanks
{
    class Bullet
    {

        private Game.Move TankDirection;
        private Control.ControlCollection FormAccessControl;

        private delegate PictureBox DelegateToGetAllyTanks();
        private DelegateToGetAllyTanks GetAllyTankDesign;

        private delegate List<PictureBox> DelegateListAlmostDestroyed();
        private DelegateListAlmostDestroyed AlmostDestroyed;

        private delegate void DelegateSetIsEnemyDead(bool a);
        private DelegateSetIsEnemyDead SetIsEnemyDead;

        private PictureBox TankBullet = new PictureBox();
        private PictureBox OwnerDesign = new PictureBox();

        public Bullet() { }

        public Bullet(Game.Move TankDirectionFromGame, Form1 FormConstruct, PictureBox BulletOwner)
        {
            OwnerDesign = BulletOwner;
            TankDirection = TankDirectionFromGame;
            FormAccessControl = FormConstruct.Controls;

            GetAllyTankDesign = new DelegateToGetAllyTanks(FormConstruct.GetAllyTankDesign);
            AlmostDestroyed = new DelegateListAlmostDestroyed(FormConstruct.GetAlmostDestroyed);
            SetIsEnemyDead = new DelegateSetIsEnemyDead(FormConstruct.SetIsEnemyDead);

            if (TankDirection == Game.Move.Down)
            {
                TankBullet.Image = Properties.Resources.BulletDown;
                TankBullet.Size = new Size(6, 11);
            }
            else if (TankDirection == Game.Move.Up)
            {
                TankBullet.Image = Properties.Resources.BulletUp;
                TankBullet.Size = new Size(6, 11);
            }
            else if (TankDirection == Game.Move.Left)
            {
                TankBullet.Image = Properties.Resources.BulletLeft;
                TankBullet.Size = new Size(11, 6);
            }
            else if (TankDirection == Game.Move.Right)
            {
                TankBullet.Image = Properties.Resources.BulletRight;
                TankBullet.Size = new Size(11, 6);
            }

            TankBullet.Location = new Point(OwnerDesign.Left + (OwnerDesign.ClientSize.Width / 2) - 3, OwnerDesign.Top + (OwnerDesign.ClientSize.Height / 2) - 3);
            TankBullet.BackColor = Color.Transparent;
            FormAccessControl.Add(TankBullet);
            BulletMove();
        }

        public void BulletMove()
        {
            var Moving = new Action(() => { });
            var Disable = new Action(() => { FormAccessControl.Remove(TankBullet); });

            switch (TankDirection)
            {
                case Game.Move.Down:
                    Moving = new Action(() => { TankBullet.Top += 2; });
                    break;

                case Game.Move.Up:
                    Moving = new Action(() => { TankBullet.Top -= 2; });
                    break;

                case Game.Move.Right:
                    Moving = new Action(() => { TankBullet.Left += 2; });
                    break;

                case Game.Move.Left:
                    Moving = new Action(() => { TankBullet.Left -= 2; });
                    break;
            }

            Task PufPuf = new Task(() =>
            {
                do
                {
                    TankBullet.Invoke(Moving);
                    Thread.Sleep(10);
                } while (WhereToGo());

                TankBullet.Invoke(Disable);
            });

            PufPuf.Start();

            if (PufPuf.IsCompleted)
                PufPuf.Dispose();
        }

        public bool WhereToGo()
        {
            Game game = new Game();

            PictureBox GoingToPufBam = new PictureBox();
            var Disable2 = new Action(() => { FormAccessControl.Remove(GoingToPufBam); if (GoingToPufBam.Name == "EnemyTanksDesign") SetIsEnemyDead(true); });
            var Destroy = new Action(() =>
            {
                GoingToPufBam.Image = Properties.Resources.explo1;
                Thread.Sleep(100);
                GoingToPufBam.Image = Properties.Resources.explo1;
                Thread.Sleep(100);
                GoingToPufBam.Image = Properties.Resources.explo2;
                Thread.Sleep(100);
                GoingToPufBam.Image = Properties.Resources.explo3;
                Thread.Sleep(100);
                GoingToPufBam.Image = Properties.Resources.explo5;
                Thread.Sleep(100);
            });

            var halfDestroy = new Action(() => { GoingToPufBam.Image = Properties.Resources.halfdestroyed; });

            if (TankDirection == Game.Move.Down && TankBullet.Top > 680)
                return false;

            if (TankDirection == Game.Move.Up && TankBullet.Top < 1)
                return false;

            if (TankDirection == Game.Move.Left && TankBullet.Left < 1)
                return false;

            if (TankDirection == Game.Move.Right && TankBullet.Left > 740)
                return false;

            if (GetAllyTankDesign().Equals(OwnerDesign))
            {
                GoingToPufBam = game.CollisionsForBullets(TankDirection, true, TankBullet);

                if (GoingToPufBam != null)
                {

                    if (AlmostDestroyed().Contains(GoingToPufBam))
                    {
                        GoingToPufBam.Invoke(halfDestroy);
                        AlmostDestroyed().Remove(GoingToPufBam);
                    }

                    else
                    {
                        Destroy.Invoke();
                        GoingToPufBam.Invoke(Disable2);
                    }
                    //TODO: Repair Object Destroy
                    return false;
                }
            }

            else
            {
                GoingToPufBam = game.CollisionsForBullets(TankDirection, false, TankBullet);

                if (GoingToPufBam != null)
                {
                    if (AlmostDestroyed().Contains(GoingToPufBam))
                    {
                        GoingToPufBam.Invoke(halfDestroy);
                        AlmostDestroyed().Remove(GoingToPufBam);
                    }

                    else
                    {
                        Destroy.Invoke();
                        GoingToPufBam.Invoke(Disable2);
                    }

                    return false;
                }
            }
            return true;
        }
    }
}
