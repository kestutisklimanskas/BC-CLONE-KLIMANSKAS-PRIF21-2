using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1.Properties;

namespace WinFormsApp1
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public enum TankType
    {
        Player,
        Enemy1,
        Enemy2,
        Enemy3
    }
   
    public class Tank
    {
        public int Health { get; set; }
        public int Speed { get; set; }
        public Direction Direction { get; set; }
        public TankType Type { get; set; }
        public bool IsMoving { get; set; }
        public bool IsShooting { get; set; }
        public Image playerTankUP= Image.FromFile(@"Resources\PlayerTankUP.png");
        public Image playerTankDOWN = Image.FromFile(@"Resources\PlayerTankDown.png");
        public Image playerTankLEFT = Image.FromFile(@"Resources\PlayerTankLeft.png");
        public Image playerTankRIGHT = Image.FromFile(@"Resources\PlayerTankRight.png");
        public Image enemyTankUP = Image.FromFile(@"Resources\Enemy1TankUp.png");
        public Image enemyTankDOWN = Image.FromFile(@"Resources\enemyTank1Down.jpg");
        public Image enemyTankLEFT = Image.FromFile(@"Resources\enemyTank1Left.jpg");
        public Image enemyTankRIGHT = Image.FromFile(@"Resources\enemyTank1Right.jpg");



        public List<Bullet> Bullets { get; } = new List<Bullet>();

        public Tank(int health, int speed)
        {

            Health = health;
            Speed = speed;
            Direction = Direction.Up;
            Type = TankType.Player;
            IsMoving = false;

        }

        public void PlayerMove(int width, int height, PictureBox box, List<PictureBox> walls)
        {
            if (IsMoving)
            {
                int newLeft = box.Left;
                int newTop = box.Top; 
                switch (Direction)
                {
                    case Direction.Up:
                        newTop -= Speed;
                        box.Image = playerTankUP;
                        break;
                    case Direction.Down:
                        newTop += Speed;
                        box.Image = playerTankDOWN;

                        break;
                    case Direction.Left:
                        newLeft -= Speed;
                        box.Image = playerTankLEFT;
                        break;
                    case Direction.Right:
                        newLeft += Speed;
                        box.Image = playerTankRIGHT;
                        break;
                }


                foreach (PictureBox wall in walls) 
                {
                    Rectangle wallRectangle = new Rectangle(wall.Left, wall.Top, wall.Width, wall.Height); 
                    Rectangle tankRectangle = new Rectangle(newLeft, newTop, box.Width, box.Height); 
                    if (tankRectangle.IntersectsWith(wallRectangle)) 
                    {
                        return; // returnina, jeigu susiduria wall objektu
                    }
                    
                }
                if (newTop >= 0 && newTop + box.Height <= height && newLeft >= 0 && newLeft + box.Width <= width) 
                {
                    box.Top = newTop;
                    box.Left = newLeft;
                }
            }
        }
        /*       public void EnemyMove(int width, int height, PictureBox enemyBox, Form form, List<PictureBox> pictureBoxes)
               {
                   int newTop = enemyBox.Top;
                   int newLeft = enemyBox.Left;
                   int nextTop = newTop;
                   int nextLeft = newLeft;

                   switch (Direction)
                   {
                       case Direction.Right:
                           nextLeft += Speed;
                           enemyBox.Image = enemyTankRIGHT;
                           break;
                       case Direction.Left:
                           nextLeft -= Speed;
                           enemyBox.Image = enemyTankLEFT;
                           break;
                       case Direction.Up:
                           nextTop -= Speed;
                           enemyBox.Image = enemyTankUP;
                           break;
                       case Direction.Down:
                           nextTop += Speed;
                           enemyBox.Image = enemyTankDOWN;
                           break;
                   }

                   

                   foreach (Control j in form.Controls)
                   {
                       foreach (PictureBox walls in pictureBoxes)
                       {
                           Rectangle wallRectangle = new Rectangle(walls.Left, walls.Top, walls.Width, walls.Height);
                            Rectangle tankRectangle = new Rectangle(nextLeft, nextTop, enemyBox.Width, enemyBox.Height);
                           if ((j is PictureBox && ((string)j.Tag == "brickWall" || (string)j.Tag == "brickWallHalf" || (string)j.Tag == "steelWall" || (string)j.Tag == "steelWallHalf")) && ((string)enemyBox.Tag == "enemy"))
                           {
                               if (tankRectangle.IntersectsWith(wallRectangle))
                               {
                                   Random random = new Random();
                                   Direction = (Direction)random.Next(0, 4); // Change direction to a random one out of direction enum
                                   return;
                               }
                           }

                       }
                   }

                   if (nextTop >= 0 && nextTop + enemyBox.Height <= height && nextLeft >= 0 && nextLeft + enemyBox.Width <= width)
                   {
                       enemyBox.Top = nextTop;
                       enemyBox.Left = nextLeft;

                   }
                   if (enemyBox.Top <= 4 || enemyBox.Left <= 4 || enemyBox.Top >= height - 65 || enemyBox.Left >= width - 60)
                   {
                       Random random = new Random();
                       Direction = (Direction)random.Next(0, 4);
                   }
               }*/

        public void EnemyMove(int width, int height, Form form, List<PictureBox> pictureBoxes, List<PictureBox> EnemyTanks, List<Tank> tanks)
        {
            foreach(PictureBox enemyBox in EnemyTanks)
            {
                Tank currentTank = tanks[EnemyTanks.IndexOf(enemyBox)]; 

                int newTop = enemyBox.Top;
                int newLeft = enemyBox.Left;
                int nextTop = newTop;
                int nextLeft = newLeft;

                switch (currentTank.Direction)
                {
                    case Direction.Right:
                        nextLeft += currentTank.Speed;
                        enemyBox.Image = enemyTankRIGHT;
                        break;
                    case Direction.Left:
                        nextLeft -= currentTank.Speed;
                        enemyBox.Image = enemyTankLEFT;
                        break;
                    case Direction.Up:
                        nextTop -= currentTank.Speed;
                        enemyBox.Image = enemyTankUP;
                        break;
                    case Direction.Down:
                        nextTop += currentTank.Speed;
                        enemyBox.Image = enemyTankDOWN;
                        break;
                }

                foreach (Control j in form.Controls)
                {
                    foreach (PictureBox walls in pictureBoxes)
                    {
                        Rectangle wallRectangle = new Rectangle(walls.Left, walls.Top, walls.Width, walls.Height);
                        Rectangle tankRectangle = new Rectangle(nextLeft, nextTop, enemyBox.Width, enemyBox.Height);

                            if (tankRectangle.IntersectsWith(wallRectangle))
                            {
                                Random random = new Random();
                                Direction = (Direction)random.Next(0, 4);
                                return;
                            }
                    }
                }

                if (nextTop >= 0 && nextTop + enemyBox.Height <= height && nextLeft >= 0 && nextLeft + enemyBox.Width <= width)
                {
                    enemyBox.Top = nextTop;
                    enemyBox.Left = nextLeft;

                }
                if (enemyBox.Top <= 4 || enemyBox.Left <= 4 || enemyBox.Top >= height - 65 || enemyBox.Left >= width - 60)
                {
                    Random random = new Random();
                    Direction = (Direction)random.Next(0, 4);
                }
            }
        }



        public void StartMoving()
        {
            IsMoving = true;
        }

        public void StopMoving()
        {
            IsMoving = false;
        }

        public void Fire(Form form, PictureBox box)
        {
            Bullet bullet = new Bullet();
            bullet.Position = new Point(box.Left + 21, box.Top + 10);
            bullet.Direction = Direction;
            bullet.bulletCreation(form, box);
        }
        public void EnemyFire(Form form, List <PictureBox> box, List <Tank>tanks)
        {
            foreach(PictureBox enemyBox in box)
            {
                Direction CurrentDir = tanks[box.IndexOf(enemyBox)].Direction;
                Bullet bullet = new Bullet();
                bullet.Position = new Point(enemyBox.Left + 21, enemyBox.Top + 10);
                bullet.Direction = CurrentDir;
                bullet.bulletCreation(form, enemyBox);
            }
           
        }


    }

}
