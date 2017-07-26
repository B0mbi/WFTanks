using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFTanks
{
    class AllyTanks : Tanks
    {
        private delegate int DelegateGetFormX();
        private DelegateGetFormX GetFormX;
        private delegate int DelegateGetFormY();
        private DelegateGetFormY GetFormY;

        public PictureBox AllyTankDesign = new PictureBox();

        public AllyTanks()
        {
            Side = Sides.Buddy;

            AllyTankDesign.BackColor = System.Drawing.Color.Transparent;
            AllyTankDesign.Image = Properties.Resources.Up1;
            AllyTankDesign.Location = new System.Drawing.Point(720, 660);
            AllyTankDesign.Name = "AllyTanksDesign";
            AllyTankDesign.Size = new System.Drawing.Size(30, 30);
            AllyTankDesign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            AllyTankDesign.TabIndex = 0;
            AllyTankDesign.TabStop = false;
        }

        public void SetFrom1(Form1 FormConstruct)
        {

            GetFormX = new DelegateGetFormX(FormConstruct.GetX);
            GetFormY = new DelegateGetFormY(FormConstruct.GetY);
            FormConstruct.Controls.Add(AllyTankDesign);
        }

        public override void Shot(Game.Move TankDirection, Form1 FormAccess)
        {
            Bullet Bullets = new Bullet(TankDirection, FormAccess, AllyTankDesign);
        }

        public void Movement(Game.Move Move)
        {
            var MoveDown = new Action(() => { if (!(AllyTankDesign.Top > 660)) { AllyTankDesign.Top += 3; } });
            var MoveUp = new Action(() => { if (!(AllyTankDesign.Top < 0)) { AllyTankDesign.Top -= 3; } });
            var MoveLeft = new Action(() => { if (!(AllyTankDesign.Left < 0)) { AllyTankDesign.Left -= 3; } });
            var MoveRight = new Action(() => { if (!(AllyTankDesign.Left > 720)) { AllyTankDesign.Left += 3; } });

            Task MovementTask = new Task(() =>
            {
                if (Game.Move.Down == Move)
                {
                    AllyTankDesign.Invoke(MoveDown);
                    Thread.Sleep(10);

                    if (Enumerable.Range(0, 10).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Down4;

                    else if (Enumerable.Range(11, 20).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Down3;

                    else if (Enumerable.Range(21, 30).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Down2;

                    else if (Enumerable.Range(31, 40).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.down1;
                }

                else if (Game.Move.Up == Move)
                {
                    AllyTankDesign.Invoke(MoveUp);
                    Thread.Sleep(10);

                    if (Enumerable.Range(0, 10).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Up4;

                    else if (Enumerable.Range(11, 20).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Up3;

                    else if (Enumerable.Range(21, 30).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Up2;

                    else if (Enumerable.Range(31, 40).Contains(Math.Abs(GetFormY() - AllyTankDesign.Top)))
                        AllyTankDesign.Image = Properties.Resources.Up1;

                }

                else if (Game.Move.Left == Move)
                {
                    AllyTankDesign.Invoke(MoveLeft);
                    Thread.Sleep(10);

                    if (Enumerable.Range(0, 10).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Left4;

                    else if (Enumerable.Range(11, 20).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Left3;

                    else if (Enumerable.Range(21, 30).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Left2;

                    else if (Enumerable.Range(31, 40).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Left1;
                }

                else if (Game.Move.Right == Move)
                {
                    AllyTankDesign.Invoke(MoveRight);
                    Thread.Sleep(10);

                    if (Enumerable.Range(0, 10).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Right4;

                    else if (Enumerable.Range(11, 20).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Right3;

                    else if (Enumerable.Range(21, 30).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Right2;

                    else if (Enumerable.Range(31, 40).Contains(Math.Abs(GetFormX() - AllyTankDesign.Left)))
                        AllyTankDesign.Image = Properties.Resources.Right1;
                }
            });

            MovementTask.Start();
            if (MovementTask.IsCompleted)
                MovementTask.Dispose();
        }
    }
}
