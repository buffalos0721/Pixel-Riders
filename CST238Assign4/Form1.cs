using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.IO;
using System.Media;

namespace CST238Assign4
{
    public partial class Form1 : Form
    {
        const float PIXELS_PER_METER = 30.0f;
        private DateTime lastTick;
        private Game game;
        private SpriteSheet bikeSheet, treeSheet, coinSheet, grassyRoadSheet, enemySheet;
        private List<Tree> trees;
        private List<Enemies> enemy;
        private SpriteSheet explosionSheet;

        public Form1()
        {
            trees = new List<Tree>();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // SPRITE DATABASE
            ResourceManager rm = new ResourceManager("CST238Assign4.Resource1", typeof(Form1).Assembly);

            // ==== BIKER ==== //

                // get the object
            Bitmap bike = (Bitmap)rm.GetObject("biker1");
            bike.MakeTransparent(bike.GetPixel(1, 1));

                // get the spritesheet
            bikeSheet = new SpriteSheet(bike, 1, 1, 1, 1624, 1176);
            bikeSheet.SetCurrentFrame(0);

            /*
            bikeSheet = new SpriteSheet(bike, 1, 1, 1, 36, 72);
            bikeSheet = new SpriteSheet(bike, 1, 1, 1, 1000, 710);
            bikeSheet.SetCurrentFrame(0);
            */
            
            // ==== TREE ==== //

                // get the object
            Bitmap treesBmp = (Bitmap)rm.GetObject("Foliage2");
            treesBmp.MakeTransparent(treesBmp.GetPixel(1, 1));

                // get the spritesheet
            treeSheet = new SpriteSheet(treesBmp, 10, 1, 10, 132, 132);
            treeSheet.SetCurrentFrame(0);

            // ==== COINS ==== //

                // get the object
            Bitmap coins = (Bitmap)rm.GetObject("coin");
            coins.MakeTransparent(coins.GetPixel(1, 1));

                // get the spritesheet
            coinSheet = new SpriteSheet(coins, 1, 1, 1, 1300, 1300);
            coinSheet.SetCurrentFrame(0);

            // ==== GRASSY ROAD ==== //

                // get the object
            Bitmap grassyRoadBmp = (Bitmap)rm.GetObject("grass");
            grassyRoadBmp.MakeTransparent(grassyRoadBmp.GetPixel(1, 1));

                // get the spritesheet
            grassyRoadSheet = new SpriteSheet(grassyRoadBmp, 1, 1, 1, 450, 450);
            grassyRoadSheet.SetCurrentFrame(0);

            // ==== ENEMIES ==== //

                // get the object
            Bitmap enemies = (Bitmap)rm.GetObject("enemy");
            enemies.MakeTransparent(enemies.GetPixel(1, 1));
    
                // get the spritesheet
            enemySheet = new SpriteSheet(enemies, 6, 1, 6, 434, 434);
            enemySheet.SetCurrentFrame(0);

            // ==== EXPLOSION ==== //

            // get the object
            Bitmap explosion = (Bitmap)rm.GetObject("Explosion");
//            explosion.MakeTransparent(explosion.GetPixel(1, 1));

            // get the spritesheet
            explosionSheet = new SpriteSheet(explosion, 0x10, 4, 4, 200, 200);
            explosionSheet.SetCurrentFrame(0);



            game = new Game();
            Random r = new Random();
            foreach (Obstacle o in game.Obstacles)
                trees.Add(new Tree(r.Next(0, 4), o));
            //foreach (Obstacle en in game.Enemies)
            //    enemy.Add(new Enemies(r.Next(0, 4), en));

            lastTick = DateTime.Now;
            gameTimer.Start();
            game.speedUp();

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan timeSinceLastTick = (TimeSpan)(DateTime.Now - lastTick);
            lastTick = DateTime.Now;
            game.Step(timeSinceLastTick);
            Refresh();
            if (!game.Player.alive)
            {
                gameTimer.Stop();

                explosionSheet.SetCurrentFrame(0);
                explosionTimer.Start();
                GameOver();
            }

        }


        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private Point ModelToScreen(PointF modelPt, float cameraOffset = 0.0f)
        {
            Point screenPt = new Point();

            // how the model will move on the screen

            /* Moving up and down */
            //screenPt.X = (int)((ClientRectangle.Width / 2) + modelPt.X * PIXELS_PER_METER);
            //screenPt.Y = (int)(ClientRectangle.Height - (modelPt.Y - cameraOffset) * PIXELS_PER_METER);

            /* Moving left and right */
            screenPt.Y = (int)((ClientRectangle.Height / 2) + modelPt.X * PIXELS_PER_METER);
            screenPt.X = (int)(ClientRectangle.Width - (modelPt.Y - cameraOffset) * PIXELS_PER_METER);


            return screenPt;
        }

        private Rectangle ModelToScreen(PointF model, SizeF modelExtents, float cameraOffset = 0.0f)
        {
            //Rectangle screenRect = new Rectangle();

            //screenRect.Location = ModelToScreen(modelRect.Location, cameraOffset);
            //screenRect.Size = new Size((int)(modelRect.Width * PIXELS_PER_METER), (int)(modelRect.Height * PIXELS_PER_METER));

            //return screenRect;

            Point location = ModelToScreen(model, cameraOffset);
            Size size = new Size((int)(modelExtents.Width * PIXELS_PER_METER), (int)(modelExtents.Height * PIXELS_PER_METER));
            location.Offset(-size.Width / 2, -size.Height / 2);
            return new Rectangle(location, size);
        }


        private void myPanel_Paint(object sender, PaintEventArgs e)
        {

        }


        private class Tree
        {
            public int frame;
            public Obstacle obstacle;

            public Tree(int frame, Obstacle obstacle)
            {
                this.frame = frame;
                this.obstacle = obstacle;
            }

        }

        private void explosionTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
            explosionSheet.NextFrame();
            if (explosionSheet.Done)
            {
                explosionTimer.Stop();
            }
        }

        private class Enemies
        {
            public int frame;
            public Obstacle enemies;

            public Enemies(int frame, Obstacle enemies)
            {
                this.frame = frame;
                this.enemies = enemies;
            }

        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.DarkGreen);

            // where is camera
            float cameraBottom = game.Player.Location.Y - 3.0f;

            //  Draw Everything

            /* Draw the biker */
            //Rectangle riderRect = ModelToScresen(game.Player.BoundingRect, cameraBottom);
            Rectangle riderRect = ModelToScreen(game.Road.Location, this.game.Road.Extents, cameraBottom);
            e.Graphics.FillRectangle(Brushes.LightGray, riderRect);
            Rectangle riderDest = ModelToScreen(game.Player.Location, game.Player.Extents, cameraBottom);
            bikeSheet.DrawCurrentFrame(e.Graphics, riderDest);
            //bikeSheet.DrawCurrentFrame(e.Graphics, riderRect);


            /* Draw the obstacles */

                //treeSheet.SetCurrentFrame(0);
            foreach (Tree t in trees)
            {
                Rectangle obstacleRect= ModelToScreen(t.obstacle.Location, t.obstacle.Extents, cameraBottom);
                treeSheet.SetCurrentFrame(t.frame);
                treeSheet.DrawCurrentFrame(e.Graphics, obstacleRect);

            }

            //foreach (Enemies en in enemy)
            //{
            //    Rectangle obstacleRect2 = ModelToScreen(en.enemies.BoundingRect, cameraBottom);
            //    enemySheet.SetCurrentFrame(en.frame);
            //    enemySheet.DrawCurrentFrame(e.Graphics, obstacleRect2);

            //}


            /* Draw the bonus items */

            //foreach (GameObject bo in game.Bonuses)
            //{
            //    Rectangle obstacleRect2 = ModelToScreen(bo.BoundingRect, cameraBottom);
            //    coinSheet.DrawCurrentFrame(e.Graphics, obstacleRect2);

            //}

            //  SCORE AND TIME LIMIT

            e.Graphics.DrawString("Score : " + game.CurrentScore.ToString(), new Font("Comic Sans MS", 10, FontStyle.Bold), Brushes.DarkRed, new PointF(0,0));
            e.Graphics.DrawString("Time : " + game.TimeRemaining.TotalSeconds.ToString("0.0"), new Font("Comic Sans MS", 10, FontStyle.Bold), Brushes.DarkSalmon, new PointF(120, 0));
            e.Graphics.DrawString("Speed : " + game.Player.Speed.ToString("0.0"), new Font("Comic Sans MS", 10, FontStyle.Bold), Brushes.LightGoldenrodYellow, new PointF(230, 0));

        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void GameOver()
        {
            game.Player.alive = false;

            string message = "GAME OVER! Retry? Yes - Start Game all Over. No - Quit Game and Close App";
            string caption = "Game Over";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult go;

            go = MessageBox.Show(message, caption, buttons);

            if (go == System.Windows.Forms.DialogResult.Yes)
            {
                MessageBox.Show("You can do it! :D ", "Yes!", MessageBoxButtons.OK);
                game.NewGame();
                gameTimer.Start();
            }

            else
            {
                MessageBox.Show("Ugh... QUITTER!! >:( ", "No!", MessageBoxButtons.OK);
                Application.Exit();
            }
        }



private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    game.turnLeft();
                    break;

                case Keys.Down:
                    game.turnRight();
                    break;

                case Keys.Space:
                    game.reverse();
                    break;

                case Keys.Right:
                    game.slowDown();
                    break;

                case Keys.Left:
                    game.speedUp();
                    break;

                case Keys.Enter:
                    game.NewGame();
                    break;
            }


        }
    }
}
