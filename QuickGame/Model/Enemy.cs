using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QuickGame
{
	public class Enemy
	{
		public Animation enemyAnimation;
		public Vector2 position;
		public bool active;
		public int health;
		public int damage;
		public int value;

		public int width
		{
			get{ return enemyAnimation.FrameWidth; }
		}
		public int height
		{
			get{ return enemyAnimation.FrameHeight; }
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

		public void Update(GameTime gameTime)
		{
			position.X -= enemyMoveSpeed;
			enemyAnimation.Position = position;
			enemyAnimation.Update (gameTime);

			if(position.X < -width || health <= 0)
			{
				active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			enemyAnimation.Draw (spriteBatch);
		}




	}
}

