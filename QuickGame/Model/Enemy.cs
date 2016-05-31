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




	}
}

