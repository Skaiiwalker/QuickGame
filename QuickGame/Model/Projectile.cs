using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickGame
{
	public class Projectile
	{
		public Texture2D texture;
		public Vector2 position;
		public bool active;
		public int damage;

		Viewport viewport;

		float projectileMoveSpeed;

		public Projectile ()
		{
			
		}

		public int width
		{
			get
			{
				return texture.Width;
			}
		}
		public int height
		{
			get
			{
				return texture.Height;
			}
		}

		public void Initialize(Viewport viewport, Texture2D myTexture, Vector2 myPosition)
		{
			texture = myTexture;
			position = myPosition;
			this.viewport = viewport;

			active = true;
			damage = 2;
			projectileMoveSpeed = 20f;
		}

		public void Update()
		{
			position.X += projectileMoveSpeed;
			if (position.X + texture.Width / 2 > viewport.Width)
				active = false;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (texture, position, null, Color.White, 0f, new Vector2(width / 2, height / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}

