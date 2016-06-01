using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickGame
{
	public class Enemy
	{
		public Animation enemyAnimation;
		public Texture2D texture;
		public Vector2 position;
		public bool active;
		public int health;
		public int damage;
		public int value;

		public int width
		{
			get
			{
				try
				{
					return enemyAnimation.FrameWidth;
				}
				catch(Exception error)
				{
					return texture.Width;
				}
			}
		}
		public int height
		{
			get
			{
				try
				{
					return enemyAnimation.FrameHeight;
				}
				catch(Exception error)
				{
					return texture.Height;
				}
			}
		}

		float enemyMoveSpeed; 

		public Enemy ()
		{
			
		}

		public void Initialize(Animation myAnimation, Vector2 myPosition)
		{
			enemyAnimation = myAnimation;
			position = myPosition;
			active = true;
			health = 10;
			damage = 10;
			enemyMoveSpeed = 6f;
			value = 100;
		}

		public void StaticInitialize(Texture2D myTexture, Vector2 myPosition)
		{
			texture = myTexture;
			position = myPosition;
			active = true;
			health = 10;
			damage = 10;
			enemyMoveSpeed = 6f;
			value = 100;
		}

		public void Update(GameTime gameTime)
		{
			position.X -= enemyMoveSpeed;
			try 
			{
				enemyAnimation.Position = position;
				enemyAnimation.Update (gameTime);
			}
			catch(Exception error) {}
				
			if(position.X < -width || health <= 0)
			{
				active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			try
			{
				enemyAnimation.Draw (spriteBatch);
			}
			catch(Exception error)
			{
				spriteBatch.Draw (texture, position, null, Color.White, 0f, new Vector2(width / 2, height / 2), 1f, SpriteEffects.None, 0f);
			}
		}




	}
}

