﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace QuickGame.Controller
{
	public class QuickGame : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Player player;

		KeyboardState currentKeyboardState;
		KeyboardState previousKeyboardState;

		GamePadState currentGamePadState;
		GamePadState previousGamePadState;

		float playerMoveSpeed;
		Texture2D mainBackground;

		ParallaxingBackground bgLayer1;
		ParallaxingBackground bgLayer2;

		Texture2D enemyTexture;
		List<Enemy> enemies;

		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		Random random;

		public QuickGame ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
		}
			
		protected override void Initialize ()
		{
			player = new Player ();
			playerMoveSpeed = 8.0f;
			TouchPanel.EnabledGestures = GestureType.FreeDrag;

			bgLayer1 = new ParallaxingBackground ();
			bgLayer2 = new ParallaxingBackground ();

			enemies = new List<Enemy> ();
			previousSpawnTime = TimeSpan.Zero;
			enemySpawnTime = TimeSpan.FromSeconds (1.0f);
			random = new Random ();

			base.Initialize ();
		}
			
		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Animation playerAnimation = new Animation ();
			Texture2D playerTexture = Content.Load<Texture2D> ("Animation/shipAnimation");
			playerAnimation.Initialize (playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

			Vector2 playerPosition = new Vector2 (GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			player.Initialize (playerAnimation, playerPosition);

			bgLayer1.Initialize (Content, "Texture/bgLayer1", GraphicsDevice.Viewport.Width, -1);
			bgLayer2.Initialize (Content, "Texture/bgLayer2", GraphicsDevice.Viewport.Width, -2);

			enemyTexture = Content.Load<Texture2D> ("Animation/mineAnimation");

			mainBackground = Content.Load<Texture2D> ("Texture/mainbackground");
		}
			
		protected override void Update (GameTime gameTime)
		{
			#if !__IOS__ &&  !__TVOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();
			#endif
            
			previousGamePadState = currentGamePadState;
			previousKeyboardState = currentKeyboardState;

			currentKeyboardState = Keyboard.GetState ();
			currentGamePadState = GamePad.GetState (PlayerIndex.One);

			UpdatePlayer (gameTime);

			bgLayer1.Update ();
			bgLayer2.Update ();

			UpdateEnemies (gameTime);

			base.Update (gameTime);
		}
			
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin ();

			spriteBatch.Draw (mainBackground, Vector2.Zero, Color.White);
			bgLayer1.Draw (spriteBatch);
			bgLayer2.Draw (spriteBatch);

			for(int i = 0; i < enemies.Count; i++)
			{
				enemies [i].Draw (spriteBatch);
			}

			player.Draw (spriteBatch);
			spriteBatch.End ();

			base.Draw (gameTime);
		}

		private void UpdatePlayer(GameTime gameTime)
		{
			player.Update (gameTime);

			player.Position.X += currentGamePadState.ThumbSticks.Left.X * playerMoveSpeed;
			player.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * playerMoveSpeed;

			if((currentKeyboardState.IsKeyDown(Keys.Left) || currentKeyboardState.IsKeyDown(Keys.A)) || currentGamePadState.DPad.Left == ButtonState.Pressed)
			{
				player.Position.X -= playerMoveSpeed;
			}

			if((currentKeyboardState.IsKeyDown(Keys.Right) || currentKeyboardState.IsKeyDown(Keys.D)) || currentGamePadState.DPad.Right == ButtonState.Pressed)
			{
				player.Position.X += playerMoveSpeed;
			}

			if((currentKeyboardState.IsKeyDown(Keys.Up) || currentKeyboardState.IsKeyDown(Keys.W)) || currentGamePadState.DPad.Up == ButtonState.Pressed)
			{
				player.Position.Y -= playerMoveSpeed;
			}

			if((currentKeyboardState.IsKeyDown(Keys.Down) || currentKeyboardState.IsKeyDown(Keys.S)) || currentGamePadState.DPad.Down == ButtonState.Pressed)
			{
				player.Position.Y += playerMoveSpeed;
			}

			player.Position.X = MathHelper.Clamp (player.Position.X, 0, GraphicsDevice.Viewport.Width - player.Width);
			player.Position.Y = MathHelper.Clamp (player.Position.Y, 0, GraphicsDevice.Viewport.Height - player.Height);
		}

		private void AddEnemy()
		{
			Animation enemyAnimation = new Animation ();
			enemyAnimation.Initialize (enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);

			Vector2 position = new Vector2 (GraphicsDevice.Viewport.Width + enemyTexture.Width / 2, random.Next (100, GraphicsDevice.Viewport.Height - 100));

			Enemy enemy = new Enemy ();
			enemy.Initialize (enemyAnimation, position);
			enemies.Add (enemy);
		}

		private void UpdateEnemies(GameTime gameTime)
		{
			if(gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
			{
				previousSpawnTime = gameTime.TotalGameTime;
				AddEnemy ();
			}
			for(int i = enemies.Count - 1; i >= 0; i--)
			{
				enemies [i].Update (gameTime);

				if(enemies[i].active == false)
				{
					enemies.RemoveAt (i);
				}
			}
		}

	}
}

