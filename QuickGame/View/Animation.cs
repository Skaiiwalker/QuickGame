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

		public void Initialize(Texture2D myTexture, Vector2 myPosition, int myFrameWidth, int myFrameHeight, int myFrameCount, int myFrameTime, Color myColor, float myScale, bool isLooping)
		{
			this.color = myColor;
			this.FrameWidth = myFrameWidth;
			this.FrameHeight = myFrameHeight;
			this.frameCount = myFrameCount;
			this.frameTime = myFrameTime;
			this.scale = myScale;

			Looping = isLooping;
			Position = myPosition;
			spriteStrip = myTexture;

			elapsedTime = 0;
			currentFrame = 0;

			Active = true;
		}

		public void Update(GameTime gameTime)
		{
			if (Active == false)
				return;

			elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

			if(elapsedTime > frameTime)
			{
				currentFrame++;

				if(currentFrame == frameCount)
				{
					currentFrame = 0;
					if (Looping == false)
						Active = false;
				}

				elapsedTime = 0;
			}

			sourceRect = new Rectangle (currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
			destinationRect = new Rectangle ((int)Position.X - (int)(FrameWidth * scale) / 2, (int)Position.Y - (int)(FrameHeight * scale) / 2, (int)(FrameWidth * scale), (int)(FrameHeight * scale));
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Active)
			{
				spriteBatch.Draw (spriteStrip, destinationRect, sourceRect, color);
			}
		}

		public Animation ()
		{
			
		}
	}
}

