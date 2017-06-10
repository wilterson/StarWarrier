using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Timers;

namespace StarWarrier
{
    public class StarWarrier : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;

        Player player;
        RedLaser playerLaser;
        GreenLaser enemyLaser;
        MeteorBig meteorBig;
        MeteorSmall meteorSmall;
        EnemyShip enemyShip;
        bool enemy_increment;

        EnemyUfo enemyUfo;
        Timer enemyUfoTimer;
        bool show_enemy_ufo;

        int y;
        int starSpeed;

        int player_laser_x;
        int player_laser_y;

        int enemy_laser_x;
        int enemy_laser_y;

        int meteor_big_x;
        int meteor_big_y;

        int meteor_small_x;
        int meteor_small_y;

        int enemy_ufo_x;
        int enemy_ufo_y;

        int enemy_ship_x;
        int enemy_ship_y;

        bool game_over;


        bool gamePaused = false;
        KeyboardState currentKB, previousKB;

        public StarWarrier()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = Constantes.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = Constantes.SCREEN_HEIGHT;
            graphics.PreferMultiSampling = false;
            graphics.IsFullScreen = true;

            player_laser_x = -50;
            player_laser_y = -50;

            enemy_laser_x = -50;
            enemy_laser_y = -50;


            SetMeteorBigPosition();
            SetMeteorSmallPosition();
            SetEnemyUfoPosition();
            SetEnemyShipPosition();

            player = new Player(this);
            playerLaser = new RedLaser(this, new Vector2(player_laser_x, player_laser_y));
            enemyLaser = new GreenLaser(this, new Vector2(enemy_laser_x, enemy_laser_y));
            meteorBig = new MeteorBig(this, new Vector2(meteor_big_x, meteor_big_y));
            meteorSmall = new MeteorSmall(this, new Vector2(meteor_small_x, meteor_small_y));
            enemyUfo = new EnemyUfo(this, new Vector2(enemy_ufo_x, enemy_ufo_y));
            enemyShip = new EnemyShip(this, new Vector2(enemy_ship_x, enemy_ship_y));

            y = 0;

            starSpeed = 15;

            enemyUfoTimer = new Timer(12 * 1000);
            enemyUfoTimer.Start();
            enemyUfoTimer.Elapsed += new ElapsedEventHandler(OnEnemyUfoTimer);

            enemy_increment = true;

            game_over = false;
        }

        void OnEnemyUfoTimer(object sender, ElapsedEventArgs e)
        {
            show_enemy_ufo = true;
        }

        void SetMeteorBigPosition()
        {
            Random rnd = new Random();
            meteor_big_x = rnd.Next(0, Constantes.SCREEN_WIDTH - Constantes.METEOR_BIG_WIDTH);
            meteor_big_y = -Constantes.METEOR_BIG_HEIGHT;
        }

        void SetMeteorSmallPosition()
        {
            Random rnd = new Random();
            meteor_small_x = rnd.Next(0, Constantes.SCREEN_WIDTH - Constantes.METEOR_SMALL_WIDTH);
            meteor_small_y = -Constantes.METEOR_SMALL_HEIGHT;
        }

        void SetEnemyUfoPosition()
        {
            Random rnd = new Random();
            enemy_ufo_x = -Constantes.ENEMY_UFO_WIDTH;
            enemy_ufo_y = Constantes.ENEMY_UFO_HEIGHT / 2;
        }

        void SetEnemyShipPosition()
        {
            Random rnd = new Random();
            enemy_ship_x = rnd.Next(0, Constantes.SCREEN_WIDTH - Constantes.ENEMY_WIDTH);
            enemy_ship_y = -Constantes.ENEMY_HEIGHT;
        }

        void HidePlayer()
        {
            player.SetPosicao(new Vector2(-Constantes.PLAYER_WIDTH, -Constantes.PLAYER_HEIGHT));
        }

        protected override void Initialize()
        {
            player.Initialize();
            playerLaser.Initialize();
            enemyLaser.Initialize();
            meteorBig.Initialize();
            meteorSmall.Initialize();
            enemyUfo.Initialize();
            enemyShip.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.spriteBatch = spriteBatch;
            player.textura = Content.Load<Texture2D>("player");

            playerLaser.spriteBatch = spriteBatch;
            playerLaser.textura = Content.Load<Texture2D>("laserRed");

            enemyLaser.spriteBatch = spriteBatch;
            enemyLaser.textura = Content.Load<Texture2D>("laserGreen");


            meteorBig.spriteBatch = spriteBatch;
            meteorBig.textura = Content.Load<Texture2D>("meteorBig");

            meteorSmall.spriteBatch = spriteBatch;
            meteorSmall.textura = Content.Load<Texture2D>("meteorSmall");

            enemyUfo.spriteBatch = spriteBatch;
            enemyUfo.textura = Content.Load<Texture2D>("enemyUFO");

            enemyShip.spriteBatch = spriteBatch;
            enemyShip.textura = Content.Load<Texture2D>("enemyShip");

            player.fontePontos = Content.Load<SpriteFont>("Pontos");

 //           player.shot = Content.Load<SoundEffect>("somhit");

            background = Content.Load<Texture2D>("back");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            previousKB = currentKB;
            currentKB = Keyboard.GetState();
            if (currentKB.IsKeyUp(Keys.P) && previousKB.IsKeyDown(Keys.P)) gamePaused = !gamePaused;
            if (gamePaused) return;

            if (game_over) return;
            

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                player.Mover(Player.Direcoes.Cima);
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                player.Mover(Player.Direcoes.Baixo);
            else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                player.Mover(Player.Direcoes.Esquerda);
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                player.Mover(Player.Direcoes.Direita);

            if ( Keyboard.GetState().IsKeyDown(Keys.Space) )
            {
                Vector2 v = player.GetPosicao();
                int size_x = Constantes.PLAYER_WIDTH;
                int shot_size_x = Constantes.PLAYER_SHOT_WIDTH;
                v.X = v.X + (size_x - shot_size_x) / 2;
                player_laser_x = (int)v.X;
                player_laser_y = (int)v.Y;
                playerLaser.SetPosicao(v);
                //player.ShotSound();
            }

            enemy_ship_y += 3;
            if (enemy_ship_y > Constantes.SCREEN_HEIGHT)
            {
                SetEnemyShipPosition();
            }

            if (enemy_increment)
                enemy_ship_x += 6;
            else
                enemy_ship_x -= 6;

            if (enemy_ship_x >= (Constantes.SCREEN_WIDTH - Constantes.ENEMY_WIDTH))
            {
                enemy_increment = false;
                enemy_ship_x -= 6;
            }

            if (enemy_ship_x <= 0)
            {
                enemy_increment = true;
                enemy_ship_x += 6;
            }


            meteor_big_y += 3;
            if (meteor_big_y > Constantes.SCREEN_HEIGHT)
            {
                SetMeteorBigPosition();
            }

            meteor_small_y += 3;
            if (meteor_small_y > Constantes.SCREEN_HEIGHT)
            {
                SetMeteorSmallPosition();
            }

            meteorBig.SetPosicao(new Vector2(meteor_big_x, meteor_big_y));
            meteorSmall.SetPosicao(new Vector2(meteor_small_x, meteor_small_y));
            enemyShip.SetPosicao(new Vector2(enemy_ship_x, enemy_ship_y));











            player.Update(gameTime);
            playerLaser.Update(gameTime);

            meteorBig.Update(gameTime);
            meteorSmall.Update(gameTime);

            enemyShip.Update(gameTime);

            if (show_enemy_ufo)
            {
                enemy_ufo_x += 10;
                if (enemy_ufo_x > Constantes.SCREEN_WIDTH)
                {
                    SetEnemyUfoPosition();
                    show_enemy_ufo = false;
                }
                enemyUfo.SetPosicao(new Vector2(enemy_ufo_x, enemy_ufo_y));
                enemyUfo.Update(gameTime);
            }

            // Verifica se os meteoros não estão sobrepostos
            if (meteorBig.boundingBox.Intersects(meteorSmall.boundingBox))
            {
                SetMeteorBigPosition();
            }

            // Testa colisão do laser com o meteoro grande
            if (playerLaser.boundingBox.Intersects(meteorBig.boundingBox))
            {
                SetMeteorBigPosition();
                player_laser_x = -50;
                player_laser_y = -50;

                player.Pontuar(5);
            }

            // Testa colisão do player com o meteoro grande
            if (player.boundingBox.Intersects(meteorBig.boundingBox))
            {
                SetMeteorBigPosition();
                game_over = true;
                HidePlayer();
            }

            // Testa colisão do laser com o meteoro pequeno
            if (playerLaser.boundingBox.Intersects(meteorSmall.boundingBox))
            {
                SetMeteorSmallPosition();
                player_laser_x = -50;
                player_laser_y = -50;

                player.Pontuar(10);
            }

            // Testa colisão do player com o meteoro pequeno
            if (player.boundingBox.Intersects(meteorSmall.boundingBox))
            {
                SetMeteorSmallPosition();

                game_over = true;
                HidePlayer();
            }

            // Testa colisão do laser com o meteoro pequeno
            if (playerLaser.boundingBox.Intersects(enemyUfo.boundingBox))
            {
                SetEnemyUfoPosition();
                player_laser_x = -50;
                player_laser_y = -50;
                show_enemy_ufo = false;

                player.Pontuar(50);
            }

            // Testa colisão do player com o meteoro pequeno
            if (player.boundingBox.Intersects(enemyUfo.boundingBox))
            {
                SetEnemyUfoPosition();
                show_enemy_ufo = false;

                game_over = true;
                HidePlayer();
            }


            // Testa colisão do laser com a nave inimiga
            if (playerLaser.boundingBox.Intersects(enemyShip.boundingBox))
            {
                SetEnemyShipPosition();
                player_laser_x = -50;
                player_laser_y = -50;

                player.Pontuar(25);

            }

            // Testa colisão do player com a nave inimiga
            if (player.boundingBox.Intersects(enemyShip.boundingBox))
            {
                SetEnemyShipPosition();
                game_over = true;
                HidePlayer();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, y - Constantes.SCREEN_HEIGHT, background.Width, background.Height), Color.White);
            spriteBatch.Draw(background, new Rectangle(0, y, background.Width, background.Height), Color.White);

            if (!game_over)
            {
                y += starSpeed;
                if (y == Constantes.SCREEN_HEIGHT) y = 0;
            }

            spriteBatch.End();

            if (game_over) return;
            
            if (player_laser_y > -Constantes.PLAYER_SHOT_HEIGHT)
            {
                player_laser_y -= 20;
            }
            else
            {
                player_laser_x = -50;
                player_laser_y = -50;
            }

            //if (enemy_laser_x != -50)
            //{
            //    if (enemy_laser_y > -Constantes.ENEMY_SHOT_HEIGHT)
            //    {
            //        enemy_laser_y -= 20;
            //    }
            //    else
            //    {
            //        enemy_laser_x = -50;
            //        enemy_laser_y = -50;
            //    }
            //}

            playerLaser.SetPosicao(new Vector2(player_laser_x, player_laser_y));
            playerLaser.Draw(gameTime);

            meteorBig.Draw(gameTime);
            meteorSmall.Draw(gameTime);

            if (show_enemy_ufo)
            {
                enemyUfo.Draw(gameTime);
            }

            enemyShip.Draw(gameTime);
            enemyLaser.Draw(gameTime);

            player.Draw(gameTime);
            
            base.Draw(gameTime);
        }
    }
}
