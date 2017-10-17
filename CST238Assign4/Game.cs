using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Media;

namespace CST238Assign4
{
    public class Game
    {

        private const float TIME_LIMIT = 30.0f;
        private const float TURN_SPEED = 5.0f;
        private const float MIN_SPEED = 1.0f;
        private const float MAX_SPEED = 40.05f;
        private const float SPEED_INCREMENT = 5.0f;
        private const float ROAD_WIDTH = 7.5f;
        private const float ROAD_LENGTH = 2000.0f;
        private const float VEHICLE_WIDTH = 3f;
        private const float VEHICLE_LENGTH = 2f;
        private const float VEHICLE_HIT_LENGTH = 2f;
        private const float VEHICLE_HIT_WIDTH = 2f;

        private const int OBSTACLE_POINTS = 5;
        private const float OBSTACLE_WIDTH = 5f;
        private const float OBSTACLE_HIT_WIDTH = 1f;

        private int currentScore;
        private TimeSpan timeLimit;
        private RiderPlayer player;
        private Road road;
        private List<GameObject> objects;
        private List<Obstacle> obstacles;
        private List<Obstacle> enemies;
        private List<Obstacle> bonuses;
        private SoundPlayer music;

        public RiderPlayer Player => player;
        public IEnumerable<Obstacle> Obstacles => obstacles;
        public IEnumerable<Obstacle> Bonuses => bonuses;
        public IEnumerable<Obstacle> Enemies => enemies;
        public int CurrentScore => currentScore;
        public TimeSpan TimeRemaining => timeLimit;
        public Road Road => road;

        //============================================================================================================//
        //                                                  METHODS                                                   //
        //============================================================================================================//

        public Game()
        {
            NewGame();
        }

        public void NewGame()
        {

            currentScore = 0;
            timeLimit = TimeSpan.FromSeconds(TIME_LIMIT);

            // GAME MUSIC //
            Stream MachRider = Resource1.machrider;
            this.music = new SoundPlayer(MachRider);
            this.music.PlayLooping();


            // create some challenging obstacles
            Random r = new Random();

            player = new RiderPlayer(new PointF(0.0f, VEHICLE_LENGTH), new SizeF(VEHICLE_WIDTH, VEHICLE_LENGTH), new SizeF(VEHICLE_HIT_WIDTH, VEHICLE_HIT_LENGTH), new PointF(0.0f, MIN_SPEED));
            road = new Road(new PointF(0.0f, ROAD_LENGTH / 2.0f), new SizeF(ROAD_WIDTH, ROAD_LENGTH), new SizeF(ROAD_WIDTH, ROAD_LENGTH));
            obstacles = new List<Obstacle>();
            bonuses = new List<Obstacle>();
            enemies = new List<Obstacle>();

            ////coins
            //for (int j = 0; j < 30; j++)
            //{
            //    bonuses.Add(new Obstacle(new PointF(0, (j + 2) * VEHICLE_LENGTH), new SizeF(OBSTACLE_WIDTH, OBSTACLE_WIDTH), new SizeF(OBSTACLE_HIT_WIDTH, OBSTACLE_HIT_WIDTH), new PointF(0, 0)));
            //}

            //trees 
            for (int i = 0; i < 200; i++)
            {
                obstacles.Add(new Obstacle(new PointF(5, (i + 2) * VEHICLE_LENGTH), new SizeF(OBSTACLE_WIDTH, OBSTACLE_WIDTH), new SizeF(OBSTACLE_HIT_WIDTH, OBSTACLE_HIT_WIDTH), new PointF(0, 0)));
                obstacles.Add(new Obstacle(new PointF(-5, (i + 2) * VEHICLE_LENGTH), new SizeF(OBSTACLE_WIDTH, OBSTACLE_WIDTH), new SizeF(OBSTACLE_HIT_WIDTH, OBSTACLE_HIT_WIDTH), new PointF(0, 0)));
                obstacles.Add(new Obstacle(new PointF(((float)(r.NextDouble() - 0.5)) * ROAD_WIDTH, ((i + 2) * 4) * VEHICLE_LENGTH), new SizeF(OBSTACLE_WIDTH, OBSTACLE_WIDTH), new SizeF(OBSTACLE_HIT_WIDTH, OBSTACLE_HIT_WIDTH), new PointF(0, 0)));

            }

            //enemies
            for (int k = 0; k < 10; k++)
            {
                enemies.Add(new Obstacle(new PointF((float)(r.NextDouble() - 0.5) * ROAD_WIDTH, (k + 2) * VEHICLE_LENGTH), new SizeF(OBSTACLE_WIDTH, OBSTACLE_WIDTH), new SizeF(OBSTACLE_HIT_WIDTH, OBSTACLE_HIT_WIDTH), new PointF(0, 0)));

            }


        }

        public void turnLeft()
        {

            player.TurnSpeed -= TURN_SPEED;
            if (player.TurnSpeed < -TURN_SPEED)
                player.TurnSpeed = -TURN_SPEED;
        }

        public void turnRight()
        {
            player.TurnSpeed += TURN_SPEED;
            if (player.TurnSpeed > TURN_SPEED)
                player.TurnSpeed = TURN_SPEED;

        }

        public void speedUp()
        {

            player.Speed += SPEED_INCREMENT;
            if (player.Speed > MAX_SPEED)
            {
                player.Speed = MAX_SPEED;
            }
        }

        public void slowDown()
        {
            player.Speed -= SPEED_INCREMENT;
            if (player.Speed < MIN_SPEED)
            {
                player.Speed = MIN_SPEED;
            }
        }

        public void reverse()
        {

            player.Speed -= SPEED_INCREMENT;
            if (player.Speed > MIN_SPEED)
                player.Speed = MIN_SPEED;
        }

        //public void turnLeft()
        //{
        //    // left
        //    player.Speed += SPEED_INCREMENT;
        //    if (player.Speed > MAX_SPEED)
        //    {
        //        player.Speed = MAX_SPEED;
        //    }

        //}

        //public void turnRight()
        //{
        //    // right
        //    player.Speed -= SPEED_INCREMENT;
        //    if (player.Speed < MIN_SPEED)
        //    {
        //        player.Speed = MIN_SPEED;
        //    }
        //}

        //public void speedUp()
        //{
        //    //up
        //    player.TurnSpeed = -TURN_SPEED;
        //}

        //public void slowDown()
        //{
        //    //down
        //    player.TurnSpeed = TURN_SPEED;
        //}

        //public void reverse()
        //{
        //    // right
        //    player.Speed -= SPEED_INCREMENT;
        //    if (player.Speed > MIN_SPEED)
        //        player.Speed = MIN_SPEED;
        //}

        public void Step(TimeSpan timeIncrement)
        {
            if (player.alive)
            {
                player.Move(timeIncrement);

                //If player goes off road
                if (!road.OnRoad(player))
                {
                    slowDown();
                    if (player.Speed < MIN_SPEED)
                        player.Speed = MIN_SPEED;
                }

            }

            //if player goes through obstacles or avoids them
            foreach (Obstacle obs in obstacles)
            {
                obs.Move(timeIncrement);
                if (player.alive)
                {
                    if (player.DetectCollision(obs))
                    {
                        music.Stop();
                        Stream boom = Resource1.gameover;
                        this.music = new SoundPlayer(boom);
                        this.music.Play();
                        player.alive = false;
                        return;
                    }

                    else if (obs.HasPassed(player))
                    {
                        currentScore += (int)(OBSTACLE_POINTS * player.Speed);
                    }
                    else { /*do nothing */ };

                }
            }

            ////if player goes through bonus targets or avoids them
            //foreach (Obstacle bon in bonuses)
            //{
            //    bon.Move(timeIncrement);
            //    if (player.alive)
            //    {
            //        if (player.DetectCollision(bon))
            //        {
            //            currentScore += (int)(OBSTACLE_POINTS * player.Speed);
            //        }
            //        else
            //        {
            //            // do nothing
            //        }

            //    }
            //}

            //if time limit expires to zero
            timeLimit -= timeIncrement;
            if (timeLimit.TotalSeconds <= 0.0)
            {
                music.Stop();
                player.alive = false;
            }
        }
    }
}

