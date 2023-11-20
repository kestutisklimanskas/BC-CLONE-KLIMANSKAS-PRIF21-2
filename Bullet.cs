using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public class Bullet
    {

        public Point Position { get; set; }
        public Direction Direction { get; set; }
        private int speed = 20;
        private PictureBox bullet = new PictureBox();
        private System.Windows.Forms.Timer bulletTimer = new System.Windows.Forms.Timer();


        public void bulletCreation(Form form, PictureBox box)
        {
            bullet.Tag = "bullet";
            if ((string)box.Tag == "playerTankPicture")
            {
                bullet.Tag = "playerBullet";
            }
            bullet.BackColor = Color.White;
            bullet.Size = new Size(5, 5);
            bullet.BringToFront();
            bullet.Left = Position.X;
            bullet.Top = Position.Y;
            bullet.BringToFront();
            form.Controls.Add(bullet);
            bulletTimer.Interval = speed;
            bulletTimer.Tick += new EventHandler(BulletTimerEvent);
            bulletTimer.Start();
        }
        
       

        private void BulletTimerEvent(object sender, EventArgs e)
        {
           
            if(Direction == Direction.Up)
            {
                bullet.Top -= speed;
            }
            if(Direction == Direction.Down)
            {
                bullet.Top += speed;
            }
            if(Direction == Direction.Left)
            {
                bullet.Left -= speed;
            }
            if(Direction == Direction.Right)
            {
                bullet.Left += speed;
            }
            if(bullet.Left < 0 || bullet.Left > 961 || bullet.Top < 0 || bullet.Top > 916)
            {
              
                    bulletTimer.Stop();
                    bulletTimer.Dispose();
                    bullet.Dispose();
               
            }
        }

     
    }
}
