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
    public class GameOver : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteBatch spriteBatch { get; set; }
        public Texture2D textura { get; set; }

        Vector2 posicao;
        Rectangle celula;

        public GameOver(Game game, Vector2 pos)
            : base(game)
        {
            posicao = pos;
        }

        public override void Initialize()
        {
             base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            celula = new Rectangle(0, 0, Constantes.GAMEOVER_WIDTH, Constantes.GAMEOVER_HEIGHT);
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
