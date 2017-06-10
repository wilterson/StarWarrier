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

namespace StarWarrier
{
    public class Player : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public enum Direcoes { Cima, Baixo, Esquerda, Direita }
 
        public SpriteBatch spriteBatch { get; set; }
        public Texture2D textura { get; set; }
        public Rectangle boundingBox { get; set; }
        public SpriteFont fontePontos { get; set; }

        public Vector2 posicao;
        Rectangle celula;
        Direcoes direcao;

        int pontos;

        public SoundEffect shot { get; set; }
        private SoundEffectInstance shotInstance;

        public Player(Game game)
            : base(game)
        {
            int x = (Constantes.SCREEN_WIDTH - Constantes.PLAYER_WIDTH) / 2;
            int y = Constantes.SCREEN_HEIGHT - Constantes.PLAYER_HEIGHT - 5;

            posicao = new Vector2(x, y);
            boundingBox = new Rectangle((int)posicao.X, (int)posicao.Y, Constantes.PLAYER_WIDTH, Constantes.PLAYER_HEIGHT);

            pontos = 0;
        }

        public void SetPosicao(Vector2 p)
        {
            posicao = p;
        }

        public Vector2 GetPosicao()
        {
            return posicao;
        }
        
        public override void Initialize()
        {
             base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            celula = new Rectangle(0, 0, Constantes.PLAYER_WIDTH, Constantes.PLAYER_HEIGHT);
            boundingBox = new Rectangle((int)posicao.X, (int)posicao.Y, Constantes.PLAYER_WIDTH, Constantes.PLAYER_HEIGHT);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textura, posicao, celula, Color.White);
            spriteBatch.DrawString(fontePontos, pontos.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ShotSound()
        {
            if (shotInstance == null)
                shotInstance = shot.CreateInstance();

            shotInstance.Play();
        }

        public void Mover(Direcoes argDirecao)
        {
            direcao = argDirecao;
            switch (direcao)
            {
                case Direcoes.Cima:
                    posicao.Y -= 4;
                    if (posicao.Y < 0) posicao.Y = 0;
                    break;
                case Direcoes.Baixo:
                    posicao.Y += 4;
                    if (posicao.Y > Constantes.SCREEN_HEIGHT - Constantes.PLAYER_HEIGHT) posicao.Y = Constantes.SCREEN_HEIGHT - Constantes.PLAYER_HEIGHT;
                    break;
                case Direcoes.Esquerda:
                    posicao.X -= 4;
                    if (posicao.X < 0) posicao.X = 0;
                    break;
                case Direcoes.Direita:
                    posicao.X += 4;
                    if (posicao.X > Constantes.SCREEN_WIDTH - Constantes.PLAYER_WIDTH) posicao.X = Constantes.SCREEN_WIDTH - Constantes.PLAYER_WIDTH;
                    break;
            }
        }

        public void Pontuar(int valor)
        {
            pontos += valor;
        }
    }
}
