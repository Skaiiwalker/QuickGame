using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickGame
{
	public class Player
	{
		public Animation PlayerAnimation;
		public Texture2D playerTexture;
		public Vector2 Position;
		public bool Active;
		public int Health;

		public int Width
		{
			get 
			{
				return playerTexture.Width;
			}

		}

		public int Height
		{
			get 
			{
				return playerTexture.Height;
			}
		}

		float playerSpeed;

		public void Initialize(Texture2D texture, Vector2 position)
		{
			playerTexture = texture;
			Position = position;
			Active = true;
			Health = 100;
		}

		public void Update()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (playerTexture, Position, null, Color.White, 0f, new Vector2 (Width / 2, Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}
