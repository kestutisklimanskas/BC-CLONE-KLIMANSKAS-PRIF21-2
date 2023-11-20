using System.Drawing.Drawing2D;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;

namespace WinFormsApp1
{
    public partial class Form1 : Form

    {
        public Tank playerTank;

        private List<Tank> enemyTanks = new List<Tank>();
        PictureBox EnemyTank = new PictureBox();
        private List<PictureBox> EnemyTanks = new List<PictureBox>();
        private System.Windows.Forms.Timer gameTimer;
        private DateTime lastShotTime = DateTime.MinValue;
        private DateTime lastShotTimePlayer = DateTime.MinValue;
        private TimeSpan shotCooldown = TimeSpan.FromMilliseconds(0);
        private TimeSpan shotCooldownEnemy = TimeSpan.FromMilliseconds(1000);
        public int speed = 5;
        private int enemyTankCount = 5;
        public bool isShooting = false;
        Random rnd = new Random();
        bool gameOver = false;
        bool victory = false;
        public Form1()
        {
            
            DialogResult result = MessageBox.Show("Welcome to BattleCity!\nYour objective is to protect the Eagle in your base, \nand eliminate all enemy tanks!\nAre you ready to play?\nArrow keys to move and spacebar to shoot", "Game Start", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                InitializeComponent();
                InitializeGame();
            }
            else if (result == DialogResult.No)
            {
                Application.Exit();
            }

        }


        private void InitializeGame()
        {
            playerTankCreation();
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 20;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            gameOver = false;
            enemyTankCreation();
            this.KeyUp += Form1_KeyUp;
            this.KeyDown += Form1_KeyDown;
            this.KeyPreview = true;
        }
        private void playerTankCreation()
        {
            playerTank = new Tank(3, 5);
            playerTank.Direction = Direction.Up;
            playerTank.Type = TankType.Player;
            this.Controls.Add(pictureBox1);

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                if (e.KeyCode == Keys.Up)
                    playerTank.Direction = Direction.Up;
                else if (e.KeyCode == Keys.Down)
                    playerTank.Direction = Direction.Down;
                else if (e.KeyCode == Keys.Left)
                    playerTank.Direction = Direction.Left;
                else if (e.KeyCode == Keys.Right)
                    playerTank.Direction = Direction.Right;

                playerTank.StartMoving();
            }
            else if (e.KeyCode == Keys.Space)
            {
                if ((DateTime.Now - lastShotTimePlayer) >= shotCooldown && isShooting == false)
                {
                    playerTank.Fire(this, pictureBox1);
                    lastShotTimePlayer = DateTime.Now;
                    isShooting = true;
                }
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            playerTank.StopMoving();
            if (e.KeyCode == Keys.Space)
            {
                isShooting = false;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {

            List<PictureBox> wallPictureBoxes = GetWalls();
            if (playerTank.Health == 0)
            {
                gameOver = true;
            }
            if (playerTank.IsMoving)
            {   
                playerTank.PlayerMove(this.ClientSize.Width, this.ClientSize.Height, pictureBox1, wallPictureBoxes);
            }

            foreach (Tank enemyTank in enemyTanks)
            {
                enemyTank.EnemyMove(this.ClientSize.Width, this.ClientSize.Height, this, wallPictureBoxes, EnemyTanks, enemyTanks);
                //enemyTank.EnemyMove(this.ClientSize.Width, this.ClientSize.Height, EnemyTank, this, wallPictureBoxes);
                if ((DateTime.Now - lastShotTime) >= shotCooldownEnemy)
                {
                    enemyTank.EnemyFire(this, EnemyTanks, enemyTanks);
                    lastShotTime = DateTime.Now;
                }
            }
            foreach (Control x in this.Controls)
            {
                foreach (Control j in this.Controls)
                {

                    if (j is PictureBox && (string)j.Tag == "playerBullet" && x is PictureBox && (string)x.Tag == "enemy")
                    {

                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {

                            if (enemyTanks.Count != 0 && EnemyTanks.Count != 0)
                            {
                                enemyTanks.Remove(enemyTanks[0]);
                                EnemyTanks.Remove(EnemyTanks[0]);
                                this.Controls.Remove(j);
                                this.Controls.Remove(x);
                            }
                            enemyTankCreation();
                            Refresh();
                        }
                    }
                    else if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "playerBullet")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            this.Controls.Remove(j);
                            this.Controls.Remove(x);
                            Refresh();
                        }
                    }
                    else if ((j is PictureBox && (string)j.Tag == "brickWall" || j is PictureBox && (string)j.Tag == "brickWallHalf" || (string)j.Tag == "eagle" || j is PictureBox && (string)j.Tag == "steelWall" || j is PictureBox && (string)j.Tag == "steelWallHalf") && x is PictureBox && (string)x.Tag == "bullet" || (string)x.Tag == "playerBullet")
                    {
                        if (j.Bounds.IntersectsWith(x.Bounds))
                        {
                            if ((string)j.Tag == "brickWall" || j is PictureBox && (string)j.Tag == "brickWallHalf")
                            {
                                this.Controls.Remove(j);

                                this.Controls.Remove(x);
                                Refresh();
                            }
                            if ((string)j.Tag == "eagle")
                            {
                                this.Controls.Remove(j);
                                this.Controls.Remove(x);
                                gameOver = true;
                                victory = false;
                                endScreen(victory);
                            }
                            if ((string)j.Tag == "steelWall" || j is PictureBox && (string)j.Tag == "steelWallHalf")
                            {
                                this.Controls.Remove(x);
                                Refresh();
                            }

                        }
                    }

                    else if (j is PictureBox && (string)j.Tag == "playerTankPicture" && x is PictureBox && (string)x.Tag == "bullet")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds) && playerTank.Health > 0)
                        {
                            this.Controls.Remove(x);
                            pictureBox1.Left = 295;
                            pictureBox1.Top = 653;
                            playerTank.Health--;
                        }
                        else if (playerTank.Health == 0)
                        {
                            this.Controls.Remove(j);
                            this.Controls.Remove(x);
                            gameOver = true;
                            victory = false;
                            endScreen(victory);
                        }
                        
                    }
                }

            }
            label2.Text = "Your health: " + playerTank.Health.ToString() + " Enemy tanks left to destroy: " + (EnemyTanks.Count+enemyTankCount);
            
            Refresh();
        }
        public List<PictureBox> GetWalls()
        {
            List<PictureBox> wallPictureBoxes = new List<PictureBox>();

            foreach (Control control in Controls)
            {
                if (control is PictureBox wall && (wall.Tag != null) && (wall.Tag.ToString() == "brickWall" || wall.Tag.ToString() == "steelWall" || wall.Tag.ToString() == "steelWallHalf" || wall.Tag.ToString() == "brickWallHalf" || wall.Tag.ToString() == "eagle"))
                {
                    wallPictureBoxes.Add(wall);

                }
            }

            return wallPictureBoxes;
        }

        /*private void enemyTankCreation()
        {
            Tank enemyTank = new Tank(1, 5);
            EnemyTank.Image = Image.FromFile(@"Resources\Enemy1TankUp.png");
            EnemyTank.Tag = "enemy";
            EnemyTank.SizeMode = PictureBoxSizeMode.StretchImage;
            EnemyTank.Size = new Size(53, 50);
            enemyTank.Direction = Direction.Left;
            enemyTank.Speed = 5;
            enemyTank.Type = TankType.Enemy1;
            EnemyTank.Left = rnd.Next(17, 893);
            EnemyTank.Top = 23;
            enemyTanks.Add(enemyTank);
            this.Controls.Add(EnemyTank);

        }*/
        private void endScreen(bool victory)
        {
            if (gameOver && victory)
            {
                gameTimer.Stop();
                gameTimer.Dispose();
                DialogResult result = MessageBox.Show("Game Over, you won!\nDo you want to restart?", "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    RestartGame();
                }
                else if (result == DialogResult.No)
                {
                    this.Close(); 
                }
            }
            else if (gameOver && !victory)
            {
                gameTimer.Stop();
                gameTimer.Dispose();
                DialogResult result = MessageBox.Show("Game Over, you lost!\nDo you want to restart?", "Game Over", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    RestartGame();
                }
                else if (result == DialogResult.No)
                {
                    this.Close();
                }
            }
        }
        private void RestartGame()
        {
            enemyTankCount = 5;
            EnemyTanks.Clear();
            enemyTanks.Clear();
            this.Controls.Clear();
            InitializeComponent();
            InitializeGame();


        }
        private void enemyTankCreation()
        {

            for (int i = 0; i < enemyTankCount; i++)
            {
                Tank enemyTank = new Tank(1, 5);
                EnemyTank = new PictureBox();
                EnemyTank.Image = Image.FromFile(@"Resources\Enemy1TankUp.png");
                EnemyTank.Tag = "enemy";
                EnemyTank.SizeMode = PictureBoxSizeMode.StretchImage;
                EnemyTank.Size = new Size(53, 50);
                enemyTank.Direction = Direction.Left;
                enemyTank.Speed = 3;
                enemyTank.Type = TankType.Enemy1;
                EnemyTank.Left = rnd.Next(17, 893);
                EnemyTank.Top = 23;
                enemyTanks.Add(enemyTank);
                EnemyTanks.Add(EnemyTank);
                this.Controls.Add(EnemyTank);
                enemyTankCount--;
            }
            if (EnemyTanks.Count == 0)
            {
                gameOver = true;
                victory = true;
                endScreen(victory);
            }


            /* if (enemyTankCount > 0)
             {
                 Tank enemyTank = new Tank(1, 5);
                 EnemyTank = new PictureBox();
                 EnemyTank.Image = Image.FromFile(@"Resources\Enemy1TankUp.png");
                 EnemyTank.Tag = "enemy";
                 EnemyTank.SizeMode = PictureBoxSizeMode.StretchImage;
                 EnemyTank.Size = new Size(53, 50);
                 enemyTank.Direction = Direction.Left;
                 enemyTank.Speed = 5;
                 enemyTank.Type = TankType.Enemy1;
                 EnemyTank.Left = rnd.Next(17, 893);
                 EnemyTank.Top = 23;
                 enemyTanks.Add(enemyTank);
                 this.Controls.Add(EnemyTank);
                 enemyTankCount--;
             }
             else
             {
                 gameOver = true;
             }*/
        }
        
    }

}
