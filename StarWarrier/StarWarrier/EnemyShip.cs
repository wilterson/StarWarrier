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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EnemyShip : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteBatch spriteBatch { get; set; }
        public Texture2D textura { get; set; }
        public Rectangle boundingBox { get; set; }

        Vector2 posicao;
        Rectangle celula;

        public EnemyShip(Game game, Vector2 pos)
            : base(game)
        {
            posicao = pos;
            boundingBox = new Rectangle((int)posicao.X, (int)posicao.Y, Constantes.ENEMY_WIDTH, Constantes.ENEMY_HEIGHT);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void SetPosicao(Vector2 p)
        {
            posicao = p;
        }

        public Vector2 GetPosicao()
        {
            return posicao;
        }

        public override void Update(GameTime gameTime)
        {
            celula = new Rectangle(0, 0, Constantes.ENEMY_WIDTH, Constantes.ENEMY_HEIGHT);
            boundingBox = new Rectangle((int)posicao.X, (int)posicao.Y, Constantes.ENEMY_WIDTH, Constantes.ENEMY_HEIGHT);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textura, posicao, celula, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
