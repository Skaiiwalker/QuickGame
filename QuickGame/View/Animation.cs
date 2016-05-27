using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace QuickGame
{
	public class Animation
	{
		Texture2D spriteStrip;
		float scale;

		int elapsedTime;
		int frameTime;
		int frameCount;
		int currentFrame;

		Color color;
		Rectangle sourceRect = new Rectangle();
		Rectangle destinationRect = new Rectangle();

		public int FrameWidth;
		public int FrameHeight;

		public bool Active;
		public bool Looping;
		public Vector2 Position;

		public void Initializate()
		{
			
		}

		public void Update()
		{
			
		}

		public void Draw()
		{
			
		}

		public Animation ()
		{
			
		}
	}
}

