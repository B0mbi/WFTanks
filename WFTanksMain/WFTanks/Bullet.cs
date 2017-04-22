﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFTanks
{
    class Bullet
    {
        private Game.Move TankDirection;
        private Form1 FormAccess;
        public PictureBox TankBullet = new PictureBox();
        public Bullet(Game.Move TankDirectionFromGame, Form1 FormConstruct, PictureBox BulletOwner)
        {
            FormAccess = FormConstruct;
            TankDirection = TankDirectionFromGame;

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

            TankBullet.Location = new System.Drawing.Point(BulletOwner.Left + (BulletOwner.ClientSize.Width / 2) - 3, BulletOwner.Top + (BulletOwner.ClientSize.Height / 2) - 3);
            TankBullet.BackColor = System.Drawing.Color.Transparent;
            FormAccess.Controls.Add(TankBullet);
        }

        public void BulletMove()
        {
            var Moving = new Action(() => { });
            var Disable = new Action(() => { FormAccess.Controls.Remove(TankBullet);});
            switch (TankDirection)
            {
                case Game.Move.Down:
                    Moving = new Action(() => { TankBullet.Top += 1; });
                    break;
                case Game.Move.Up:
                    Moving = new Action(() => { TankBullet.Top -= 1; });
                    break;
                case Game.Move.Right:
                    Moving = new Action(() => { TankBullet.Left += 1; });
                    break;
                case Game.Move.Left:
                    Moving = new Action(() => { TankBullet.Left -= 1; });
                    break;
                default:
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
            Game game = new Game(FormAccess);
            if (TankDirection == Game.Move.Down && TankBullet.Top > 680)
                return false;

            if (TankDirection == Game.Move.Up && TankBullet.Top < 1)
                return false;

            if (TankDirection == Game.Move.Left && TankBullet.Left < 1)
                return false;

            if (TankDirection == Game.Move.Right && TankBullet.Left > 740)
                return false;
            if (game.CollisionsForBullets(TankDirection, true, TankBullet))
                return false;

            return true;
        }
    }
}
