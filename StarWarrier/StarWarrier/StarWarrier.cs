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

        List<RedLaser> shots = new List<RedLaser>();

        Player player;
        GreenLaser enemyLaser;
        MeteorBig meteorBig;
        MeteorSmall meteorSmall;
        EnemyShip enemyShip;

        GameOver gameOver;

        bool enemy_increment;
        
        EnemyUfo enemyUfo;
        Timer enemyUfoTimer;
        bool show_enemy_ufo;

        int y;
        int starSpeed;

        int enemy_laser_x;
        int enemy_laser_y;

        List<int> player_laser_x = new List<int>();
        List<int> player_laser_y = new List<int>();

        int meteor_big_x;
        int meteor_big_y;

        int meteor_small_x;
        int meteor_small_y;

        int enemy_ufo_x;
        int enemy_ufo_y;

        int enemy_ship_x;
        int enemy_ship_y;

        int gameover_x;
        int gameover_y;


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

            for (int i = 0; i < Constantes.SHOT_COUNT; i++)
            {
                player_laser_x.Add(Constantes.DEFAULT_POSITION);
                player_laser_y.Add(Constantes.DEFAULT_POSITION);
            }

            enemy_laser_x = Constantes.DEFAULT_POSITION;
            enemy_laser_y = Constantes.DEFAULT_POSITION;


            gameover_x = (Constantes.SCREEN_WIDTH - Constantes.GAMEOVER_WIDTH) / 2;
            gameover_y = (Constantes.SCREEN_HEIGHT - Constantes.GAMEOVER_HEIGHT) / 2;

            // Inicializa a posição dos objetos de forma randômica
            SetMeteorBigPosition();
            SetMeteorSmallPosition();
            SetEnemyUfoPosition();
            SetEnemyShipPosition();

            player = new Player(this);

            for (int i = 0; i < Constantes.SHOT_COUNT; i++)
            {
                shots.Add(new RedLaser(this, new Vector2(player_laser_x[i], player_laser_y[i])));
            }

            enemyLaser = new GreenLaser(this, new Vector2(enemy_laser_x, enemy_laser_y));
            meteorBig = new MeteorBig(this, new Vector2(meteor_big_x, meteor_big_y));
            meteorSmall = new MeteorSmall(this, new Vector2(meteor_small_x, meteor_small_y));
            enemyUfo = new EnemyUfo(this, new Vector2(enemy_ufo_x, enemy_ufo_y));
            enemyShip = new EnemyShip(this, new Vector2(enemy_ship_x, enemy_ship_y));
            gameOver = new GameOver(this, new Vector2(gameover_x, gameover_y));

            // Referência pra movimentação da tela
            y = 0;

            // Velocidade da tela
            starSpeed = 15;

            // Cria um timer para o enemyUFO
            // O evento é executado a cada 12 segundos
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

            for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                shots[i].Initialize();

            enemyLaser.Initialize();
            meteorBig.Initialize();
            meteorSmall.Initialize();
            enemyUfo.Initialize();
            enemyShip.Initialize();
            gameOver.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.spriteBatch = spriteBatch;
            player.textura = Content.Load<Texture2D>("player");

            for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                shots[i].textura = Content.Load<Texture2D>("laserRed");
            for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                shots[i].spriteBatch = spriteBatch;

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

            gameOver.spriteBatch = spriteBatch;
            gameOver.textura = Content.Load<Texture2D>("gameover");


            player.fontePontos = Content.Load<SpriteFont>("Pontos");

            player.shot = Content.Load<SoundEffect>("somhit");

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

            if (!game_over)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    player.Mover(Player.Direcoes.Cima);
                else if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    player.Mover(Player.Direcoes.Baixo);
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    player.Mover(Player.Direcoes.Esquerda);
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    player.Mover(Player.Direcoes.Direita);

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Vector2 v = player.GetPosicao();
                    int size_x = Constantes.PLAYER_WIDTH;
                    int shot_size_x = Constantes.PLAYER_SHOT_WIDTH;
                    v.X = v.X + (size_x - shot_size_x) / 2;

                    for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                    {
                        if (Constantes.DEFAULT_POSITION == player_laser_y[i])
                        {
                            // posiciona o laser junto a nave
                            player_laser_x[i] = (int)v.X;
                            player_laser_y[i] = (int)v.Y;
                            shots[i].SetPosicao(v);

                            break;
                        }
                    }

                    player.ShotSound();
                }

                enemy_ship_y += 2;
                if (enemy_ship_y > Constantes.SCREEN_HEIGHT)
                {
                    SetEnemyShipPosition();
                }

                if (enemy_increment)
                    enemy_ship_x += 4;
                else
                    enemy_ship_x -= 4;

                if (enemy_ship_x >= (Constantes.SCREEN_WIDTH - Constantes.ENEMY_WIDTH))
                {
                    enemy_increment = false;
                    enemy_ship_x -= 4;
                }

                if (enemy_ship_x <= 4)
                {
                    enemy_increment = true;
                    enemy_ship_x += 4;
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


                // Laser do inimigo
                if (enemy_laser_x != Constantes.DEFAULT_POSITION)
                {
                    if (enemy_laser_y > Constantes.ENEMY_HEIGHT && enemy_laser_y < Constantes.SCREEN_HEIGHT)
                    {
                        enemy_laser_y += 5;
                    }
                    else
                    {
                        enemy_laser_x = Constantes.DEFAULT_POSITION;
                        enemy_laser_y = Constantes.DEFAULT_POSITION;
                    }
                }
                else
                {
                    Vector2 v = enemyShip.GetPosicao();
                    int size_x = Constantes.ENEMY_WIDTH;
                    int shot_size_x = Constantes.ENEMY_SHOT_WIDTH;
                    v.X = v.X + (size_x - shot_size_x) / 2;
                    v.Y += Constantes.ENEMY_HEIGHT;
                    enemy_laser_x = (int)v.X;
                    enemy_laser_y = (int)v.Y;
                }

                player.Update(gameTime);

                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    shots[i].Update(gameTime);
                }

                meteorBig.Update(gameTime);
                meteorSmall.Update(gameTime);

                enemyShip.Update(gameTime);
                enemyLaser.Update(gameTime);

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
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    if (shots[i].boundingBox.Intersects(meteorBig.boundingBox))
                    {
                        SetMeteorBigPosition();
                        player_laser_x[i] = Constantes.DEFAULT_POSITION;
                        player_laser_y[i] = Constantes.DEFAULT_POSITION;

                        player.Pontuar(5);

                        break;
                    }
                }

                // Testa colisão do player com o meteoro grande
                if (player.boundingBox.Intersects(meteorBig.boundingBox))
                {
                    SetMeteorBigPosition();
                    game_over = true;
                    HidePlayer();
                }

                // Testa colisão do laser com o meteoro pequeno
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    if (shots[i].boundingBox.Intersects(meteorSmall.boundingBox))
                    {
                        SetMeteorSmallPosition();
                        player_laser_x[i] = Constantes.DEFAULT_POSITION;
                        player_laser_y[i] = Constantes.DEFAULT_POSITION;

                        player.Pontuar(10);

                        break;
                    }
                }

                // Testa colisão do player com o meteoro pequeno
                if (player.boundingBox.Intersects(meteorSmall.boundingBox))
                {
                    SetMeteorSmallPosition();

                    game_over = true;
                    HidePlayer();
                }

                // Testa colisão do laser com o meteoro pequeno
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    if (shots[i].boundingBox.Intersects(enemyUfo.boundingBox))
                    {
                        SetEnemyUfoPosition();
                        player_laser_x[i] = Constantes.DEFAULT_POSITION;
                        player_laser_y[i] = Constantes.DEFAULT_POSITION;
                        show_enemy_ufo = false;

                        player.Pontuar(50);

                        break;
                    }
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
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    if (shots[i].boundingBox.Intersects(enemyShip.boundingBox))
                    {
                        SetEnemyShipPosition();
                        player_laser_x[i] = Constantes.DEFAULT_POSITION;
                        player_laser_y[i] = Constantes.DEFAULT_POSITION;

                        player.Pontuar(25);

                        break;
                    }
                }

                // Testa colisão do player com a nave inimiga
                if (player.boundingBox.Intersects(enemyShip.boundingBox))
                {
                    SetEnemyShipPosition();
                    game_over = true;
                    HidePlayer();
                }

                // O tiro do inimigo acertou a nave do jogador
                if (player.boundingBox.Intersects(enemyLaser.boundingBox))
                {
                    SetEnemyShipPosition();
                    game_over = true;
                    HidePlayer();
                }

                enemyLaser.SetPosicao(new Vector2(enemy_laser_x, enemy_laser_y));
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    shots[i].SetPosicao(new Vector2(player_laser_x[i], player_laser_y[i]));
                }
            }
            else
            {
                gameOver.Update(gameTime);
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

            if (!game_over)
            {
                for (int i = 0; i < Constantes.SHOT_COUNT; i++)
                {
                    if (player_laser_x[i] != Constantes.DEFAULT_POSITION)
                    {
                        if (player_laser_y[i] > -Constantes.PLAYER_SHOT_HEIGHT)
                        {
                            player_laser_y[i] -= 20;
                        }
                        else
                        {
                            player_laser_x[i] = Constantes.DEFAULT_POSITION;
                            player_laser_y[i] = Constantes.DEFAULT_POSITION;
                        }

                        shots[i].Draw(gameTime);
                    }
                }

                enemyShip.Draw(gameTime);
                enemyLaser.Draw(gameTime);
                meteorBig.Draw(gameTime);
                meteorSmall.Draw(gameTime);
                if (show_enemy_ufo)
                {
                    enemyUfo.Draw(gameTime);
                }

                player.Draw(gameTime);
            }
            else
            {
                gameOver.Draw(gameTime);
            }
            
            base.Draw(gameTime);
        }
    }
}
