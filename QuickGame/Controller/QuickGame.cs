using System;

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

		Texture2D enemyTexture1;
		Texture2D enemyTexture2;
		Texture2D enemyTexture3;
		List<Enemy> enemies;

		TimeSpan enemySpawnTime;
		TimeSpan previousSpawnTime;

		Random random;

		Texture2D projectileTexture;
		List<Projectile> projectiles;

		TimeSpan fireTime;
		TimeSpan previousFireTime;

		Texture2D explosionTexture;
		List<Animation> explosions;

		int score;
		SpriteFont font;

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

			projectiles = new List<Projectile> ();
			fireTime = TimeSpan.FromSeconds (.15f);

			explosions = new List<Animation> ();

			base.Initialize ();
		}
			
		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			Animation playerAnimation = new Animation ();
			Texture2D playerTexture = Content.Load<Texture2D> ("Animation/explosion");
			playerAnimation.Initialize (playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);

			Vector2 playerPosition = new Vector2 (GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			player.Initialize (playerAnimation, playerPosition);

			bgLayer1.Initialize (Content, "Texture/bgLayer1", GraphicsDevice.Viewport.Width, -1);
			bgLayer2.Initialize (Content, "Texture/bgLayer2", GraphicsDevice.Viewport.Width, -2);


			enemyTexture1 = Content.Load<Texture2D> ("Animation/mineAnimation");
			enemyTexture2 = Content.Load<Texture2D> ("Texture/penguin");
			enemyTexture3 = Content.Load<Texture2D> ("Texture/luigi");

			projectileTexture = Content.Load<Texture2D> ("Texture/laser");
			explosionTexture = Content.Load<Texture2D> ("Animation/explosion");

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

			UpdateCollision ();
			UpdateProjectiles ();

			UpdateExplosions(gameTime);

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

			for(int i = 0; i < projectiles.Count; i++)
			{
				projectiles [i].Draw (spriteBatch);
			}

			for(int i = 0; i < explosions.Count; i++)
			{
				explosions [i].Draw (spriteBatch);
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

			if(gameTime.TotalGameTime - previousFireTime > fireTime)
			{
				previousFireTime = gameTime.TotalGameTime;
				AddProjectile (player.Position + new Vector2 (player.Width / 2, 0));
			}
		}

		private void AddEnemy()
		{
			Enemy enemy = new Enemy ();

			Texture2D texture = enemyTexture1;
			Vector2 position = new Vector2 (GraphicsDevice.Viewport.Width + texture.Width / 2, random.Next (100, GraphicsDevice.Viewport.Height - 100));
			int r = random.Next (1, 4);
			if(r == 1)
			{
				texture = enemyTexture2;
				enemy.StaticInitialize (texture, position);
			}
			else if(r == 2)
			{
				texture = enemyTexture3;
				enemy.StaticInitialize (texture, position);
			}
			else if(r == 3)
			{
				Animation enemyAnimation = new Animation ();
				enemyAnimation.Initialize (texture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
				enemy.Initialize (enemyAnimation, position);
			}
			enemies.Add (enemy);
		}

		private void AddProjectile(Vector2 myPosition)
		{
			Projectile projectile = new Projectile ();
			projectile.Initialize (GraphicsDevice.Viewport, projectileTexture, myPosition);
			projectiles.Add (projectile);
		}

		private void AddExplosion(Vector2 position)
		{
			Animation explosion = new Animation ();
			explosion.Initialize (explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
			explosions.Add(explosion);
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
					AddExplosion(enemies[i].position);
					enemies.RemoveAt (i);
				}
			}
		}

		private void UpdateProjectiles()
		{
			for(int i = projectiles.Count - 1; i >=0; i--)
			{
				projectiles [i].Update ();

				if(projectiles[i].active == false)
				{
					projectiles.RemoveAt (i);
				}
			}
		}

		private void UpdateCollision()
		{
			Rectangle rectangle1;
			Rectangle rectangle2;

			rectangle1 = new Rectangle ((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);

			for(int i = 0; i < enemies.Count; i++)
			{
				rectangle2 = new Rectangle ((int)enemies [i].position.X, (int)enemies [i].position.Y, enemies [i].width, enemies [i].height);

				if(rectangle1.Intersects(rectangle2))
				{
					player.Health -= enemies [i].damage;

					enemies [i].health = 0;

					if (player.Health <= 0)
						player.Active = false;
				}
			}

			for(int i = 0; i < projectiles.Count; i++)
			{
				for(int j = 0; j < enemies.Count; j++)
				{
					rectangle1 = new Rectangle ((int)projectiles [i].position.X - projectiles [i].width / 2, (int)projectiles [i].position.Y - projectiles [i].height / 2, projectiles [i].width, projectiles [i].height);
					rectangle2 = new Rectangle ((int)enemies [j].position.X - enemies [j].width / 2, (int)enemies [j].position.Y - enemies [j].height / 2, enemies [j].width, enemies [j].height);

					if(rectangle1.Intersects(rectangle2))
					{
						enemies [j].health -= projectiles [i].damage;
						projectiles [i].active = false;
					}
				}
			}
		}

		private void UpdateExplosions(GameTime gameTime)
		{
			for(int i = explosions.Count - 1; i >= 0; i--)
			{
				explosions [i].Update (gameTime);
				if(explosions[i].Active == false)
				{
					explosions.RemoveAt (i);
				}
			}
		}
	}
}

