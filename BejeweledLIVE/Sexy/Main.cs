using System;
// using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using MonoGame.Framework.Utilities;

namespace Sexy
{
	public class Main : Game
	{
		public Main()
		{
			Main.SetupTileSchedule();
			this.graphics = Graphics.GetNew(this);
			try
			{
				XNAMusicInterface.UserMusicVolume = MediaPlayer.Volume;
			}
			catch
			{
			}
			this.graphics.IsFullScreen = false;
			base.IsMouseVisible = true;
			this.graphics.PreferredBackBufferWidth = Constants.StartWidth;
			this.graphics.PreferredBackBufferHeight = Constants.StartHeight;
			if (PlatformInfo.MonoGamePlatform != MonoGamePlatform.Android && PlatformInfo.MonoGamePlatform != MonoGamePlatform.iOS)
			{
				base.Window.AllowUserResizing = true;
				base.Window.ClientSizeChanged += this.Window_ClientSizeChanged;
			}
			GraphicsState.mGraphicsDeviceManager.SupportedOrientations = Constants.SupportedOrientations;
			GraphicsState.mGraphicsDeviceManager.DeviceCreated += new EventHandler<EventArgs>(this.graphics_DeviceCreated);
			GraphicsState.mGraphicsDeviceManager.DeviceReset += new EventHandler<EventArgs>(this.graphics_DeviceReset);
			GraphicsState.mGraphicsDeviceManager.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(this.mGraphicsDeviceManager_PreparingDeviceSettings);
			base.IsFixedTimeStep = true;
			base.TargetElapsedTime = TimeSpan.FromSeconds(0.01);
			base.Exiting += new EventHandler<ExitingEventArgs>(this.Main_Exiting);
			// PhoneApplicationService.Current.UserIdleDetectionMode = 1;
			// PhoneApplicationService.Current.Launching += new EventHandler<LaunchingEventArgs>(this.Game_Launching);
			// PhoneApplicationService.Current.Activated += new EventHandler<ActivatedEventArgs>(this.Game_Activated);
			// PhoneApplicationService.Current.Closing += new EventHandler<ClosingEventArgs>(this.Current_Closing);
			// PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Current_Deactivated);
			// Main.GamerServicesComp = new GamerServicesComponent(this);
			// base.Components.Add(Main.GamerServicesComp);
		}

		// private void Current_Deactivated(object sender, DeactivatedEventArgs e)
		// {
			// GlobalStaticVars.gSexyAppBase.Tombstoned();
		// }

		// private void Current_Closing(object sender, ClosingEventArgs e)
		// {
			// // PhoneApplicationService.Current.State.Clear();
		// }

		// private void Game_Activated(object sender, ActivatedEventArgs e)
		// {
		// }

		// private void Game_Launching(object sender, LaunchingEventArgs e)
		// {
			// // PhoneApplicationService.Current.State.Clear();
		// }

		// public static bool RunWhenLocked
		// {
			// get
			// {
				// // return PhoneApplicationService.Current.ApplicationIdleDetectionMode == 1;
			// }
			// set
			// {
				// try
				// {
					// // PhoneApplicationService.Current.ApplicationIdleDetectionMode = (value ? 1 : 0);
				// }
				// catch
				// {
				// }
			// }
		// }

		private static void SetupTileSchedule()
		{
		}

		private void mGraphicsDeviceManager_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
		}

		private void graphics_DeviceReset(object sender, EventArgs e)
		{
		}

		private void graphics_DeviceCreated(object sender, EventArgs e)
		{
		}

		private void Main_Exiting(object sender, EventArgs e)
		{
			GlobalStaticVars.gSexyAppBase.AppExit();
			try
			{
				MediaPlayer.Volume = XNAMusicInterface.UserMusicVolume;
			}
			catch
			{
			}
		}

		protected override void Initialize()
		{
			GlobalStaticVars.initialize(this);
			base.Window.OrientationChanged += new EventHandler<EventArgs>(this.Window_OrientationChanged);
			base.Initialize();
			this.SetupInterfaceOrientation();
		}

		protected override void LoadContent()
		{
			GraphicsState.Init();
			GlobalStaticVars.mGlobalContent.LoadSplashScreen();
			GlobalStaticVars.gSexyAppBase.StartLoadingThread();
		}

		protected override void UnloadContent()
		{
			GlobalStaticVars.mGlobalContent.cleanUp();
			this.windowSpriteBatch?.Dispose();
			this.windowRenderTarget?.Dispose();
		}

		protected override void BeginRun()
		{
			base.BeginRun();
			try
			{
				XNAMusicInterface.PlayingUserMusic = !MediaPlayer.GameHasControl;
			}
			catch (Exception)
			{
				XNAMusicInterface.PlayingUserMusic = true;
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (this.windowResizePending)
			{
				int width = base.Window.ClientBounds.Width;
				int height = base.Window.ClientBounds.Height;
				if (width > 0 && height > 0)
				{
					GraphicsDeviceManager manager = GraphicsState.mGraphicsDeviceManager;
					if (manager.PreferredBackBufferWidth != width || manager.PreferredBackBufferHeight != height)
					{
						manager.PreferredBackBufferWidth = width;
						manager.PreferredBackBufferHeight = height;
						manager.ApplyChanges();
					}
					this.SetupInterfaceOrientation();
				}
				this.windowResizePending = false;
			}
			if (base.Window.AllowUserResizing)
			{
				this.SetupInterfaceOrientation();
			}
			this.UpdateVirtualDimensions();
			if (!base.IsActive)
			{
				return;
			}
			if (GlobalStaticVars.gSexyAppBase.WantsToExit)
			{
				base.Exit();
			}
			this.HandleInput(gameTime);
			GlobalStaticVars.gSexyAppBase.UpdateApp();
			if (this.mFrameCnt % 3 != 0 || Main.wantToSuppressDraw)
			{
				Main.wantToSuppressDraw = false;
				base.SuppressDraw();
			}
			this.mFrameCnt++;
			if (!Main.trialModeChecked)
			{
				bool flag = Main.trialModeCachedValue;
				// Main.trialModeCachedValue = Guide.IsTrialMode;
				if (flag != Main.trialModeCachedValue && flag)
				{
					this.LeftTrialMode();
				}
				Main.trialModeChecked = true;
			}
			try
			{
				base.Update(gameTime);
			}
			// catch (GameUpdateRequiredException ex)
			// {
				// string message = ex.Message;
				// GlobalStaticVars.gSexyAppBase.ShowUpdateRequiredMessage();
			// }
			catch (Exception ex2)
			{
				string message2 = ex2.Message;
			}
		}

		private void LeftTrialMode()
		{
			if (GlobalStaticVars.gSexyAppBase != null)
			{
				GlobalStaticVars.gSexyAppBase.LeftTrialMode();
			}
			this.Window_OrientationChanged(null, null);
		}

		public static void SuppressNextDraw()
		{
			Main.wantToSuppressDraw = true;
		}

		// public static SignedInGamer GetGamer()
		// {
			// if (Gamer.SignedInGamers.Count == 0)
			// {
				// return null;
			// }
			// return Gamer.SignedInGamers[0];
		// }

		public static void NeedToSetUpOrientationMatrix(UI_ORIENTATION orientation)
		{
			Main.orientationUsed = orientation;
			Main.newOrientation = true;
		}

		private static void SetupOrientationMatrix(UI_ORIENTATION orientation)
		{
			Main.newOrientation = false;
		}

		private void Window_OrientationChanged(object sender, EventArgs e)
		{
			this.SetupInterfaceOrientation();
		}

		private void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			this.windowResizePending = true;
		}

		private void SetupInterfaceOrientation()
		{
			if (GlobalStaticVars.gSexyAppBase != null)
			{
				if (base.Window.AllowUserResizing)
				{
					UI_ORIENTATION orientation = base.Window.ClientBounds.Width > base.Window.ClientBounds.Height
						? UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT : UI_ORIENTATION.UI_ORIENTATION_PORTRAIT;
					if (GlobalStaticVars.gSexyAppBase.GetOrientation() != orientation)
					{
						GlobalStaticVars.gSexyAppBase.InterfaceOrientationChanged(orientation);
					}
					return;
				}
				UI_ORIENTATION mobileOrientation = base.Window.CurrentOrientation == DisplayOrientation.LandscapeLeft
					|| base.Window.CurrentOrientation == DisplayOrientation.LandscapeRight
					? UI_ORIENTATION.UI_ORIENTATION_LANDSCAPE_LEFT : UI_ORIENTATION.UI_ORIENTATION_PORTRAIT;
				if (GlobalStaticVars.gSexyAppBase.GetOrientation() != mobileOrientation)
				{
					GlobalStaticVars.gSexyAppBase.InterfaceOrientationChanged(mobileOrientation);
				}
			}
		}

		private void UpdateVirtualDimensions()
		{
			GlobalStaticVars.gSexyAppBase.UpdateVirtualDimensions(
				base.GraphicsDevice.PresentationParameters.BackBufferWidth,
				base.GraphicsDevice.PresentationParameters.BackBufferHeight);
		}

		protected override void Draw(GameTime gameTime)
		{
			if (Main.newOrientation)
			{
				Main.SetupOrientationMatrix(Main.orientationUsed);
			}
			this.UpdateVirtualDimensions();
			int width = this.graphics.mScreenWidth;
			int height = this.graphics.mScreenHeight;
			if (this.windowRenderTarget == null || this.windowRenderTarget.Width != width || this.windowRenderTarget.Height != height)
			{
				this.windowRenderTarget?.Dispose();
				this.windowRenderTarget = new RenderTarget2D(base.GraphicsDevice, width, height, false,
					SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			}
			this.frameRenderTarget = this.windowRenderTarget;
			GraphicsState.RenderOffset = new Vector2(-GlobalStaticVars.gSexyAppBase.mVirtualNegativeWidth,
				-GlobalStaticVars.gSexyAppBase.mVirtualNegativeHeight);
			base.GraphicsDevice.SetRenderTarget(this.frameRenderTarget);
			base.GraphicsDevice.Clear(Color.Black);
			GlobalStaticVars.gSexyAppBase.DrawGame(gameTime);
			this.frameRenderTarget = null;
			GraphicsState.RenderOffset = Vector2.Zero;
			base.GraphicsDevice.SetRenderTarget(null);
			base.GraphicsDevice.Clear(Color.Black);
			this.windowSpriteBatch ??= new SpriteBatch(base.GraphicsDevice);
			this.windowSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp);
			this.windowSpriteBatch.Draw(this.windowRenderTarget, this.GetGameBounds(), Color.White);
			this.windowSpriteBatch.End();
			base.Draw(gameTime);
		}

		private Rectangle GetGameBounds()
		{
			int width = base.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int height = base.GraphicsDevice.PresentationParameters.BackBufferHeight;
			float scale = Math.Min((float)width / this.graphics.mScreenWidth, (float)height / this.graphics.mScreenHeight);
			int gameWidth = Math.Max(1, (int)Math.Round(this.graphics.mScreenWidth * scale));
			int gameHeight = Math.Max(1, (int)Math.Round(this.graphics.mScreenHeight * scale));
			return new Rectangle((width - gameWidth) / 2, (height - gameHeight) / 2, gameWidth, gameHeight);
		}

		private CGPoint GetGamePosition(float x, float y, bool mouse = false)
		{
			Rectangle bounds = this.GetGameBounds();
			if (mouse)
			{
				x *= (float)base.GraphicsDevice.PresentationParameters.BackBufferWidth / Math.Max(1, base.Window.ClientBounds.Width);
				y *= (float)base.GraphicsDevice.PresentationParameters.BackBufferHeight / Math.Max(1, base.Window.ClientBounds.Height);
			}
			return new CGPoint((x - bounds.X) * this.graphics.mScreenWidth / bounds.Width + GlobalStaticVars.gSexyAppBase.mVirtualNegativeWidth,
				(y - bounds.Y) * this.graphics.mScreenHeight / bounds.Height + GlobalStaticVars.gSexyAppBase.mVirtualNegativeHeight);
		}

		public void HandleInput(GameTime gameTime)
		{
			GamePadState state = GamePad.GetState(0);
			KeyboardState keyboard = Keyboard.GetState();
			if ((state.Buttons.Back == ButtonState.Pressed && this.previousGamepadState.Buttons.Back == ButtonState.Released) ||
				(keyboard.IsKeyDown(Keys.Escape) && this.previousKeyboardState.IsKeyUp(Keys.Escape)))
			{
				GlobalStaticVars.gSexyAppBase.BackButtonPress();
			}
			TouchCollection state2 = TouchPanel.GetState();
			bool flag = false;
			foreach (TouchLocation touchLocation in state2)
			{
				_Touch touch = default(_Touch);
				touch.location = this.GetGamePosition(touchLocation.Position.X, touchLocation.Position.Y);
				if (touchLocation.TryGetPreviousLocation(out TouchLocation previousTouch))
				{
					touch.previousLocation = this.GetGamePosition(previousTouch.Position.X, previousTouch.Position.Y);
				}
				touch.timestamp = gameTime.TotalGameTime.TotalSeconds;
				if (touchLocation.State == TouchLocationState.Pressed && !flag)
				{
					GlobalStaticVars.gSexyAppBase.TouchBegan(touch);
					flag = true;
				}
				else if (touchLocation.State == TouchLocationState.Moved)
				{
					GlobalStaticVars.gSexyAppBase.TouchMoved(touch);
				}
				else if (touchLocation.State == TouchLocationState.Released)
				{
					GlobalStaticVars.gSexyAppBase.TouchEnded(touch);
				}
				else if (touchLocation.State == TouchLocationState.Invalid)
				{
					GlobalStaticVars.gSexyAppBase.TouchesCanceled();
				}
			}
			MouseState mouse = Mouse.GetState();
			if (state2.Count == 0)
			{
				_Touch touch = default(_Touch);
				touch.location = this.GetGamePosition(mouse.X, mouse.Y, true);
				touch.previousLocation = this.GetGamePosition(this.previousMouseState.X, this.previousMouseState.Y, true);
				touch.timestamp = gameTime.TotalGameTime.TotalSeconds;
				if (mouse.LeftButton == ButtonState.Pressed && this.previousMouseState.LeftButton == ButtonState.Released)
				{
					GlobalStaticVars.gSexyAppBase.TouchBegan(touch);
				}
				else if (mouse.LeftButton == ButtonState.Pressed)
				{
					GlobalStaticVars.gSexyAppBase.TouchMoved(touch);
				}
				else if (this.previousMouseState.LeftButton == ButtonState.Pressed)
				{
					GlobalStaticVars.gSexyAppBase.TouchEnded(touch);
				}
			}
			this.previousMouseState = mouse;
			this.previousKeyboardState = keyboard;
			this.previousGamepadState = state;
		}

		protected override void OnActivated(object sender, EventArgs args)
		{
			if (GlobalStaticVars.gSexyAppBase == null)
			{
				base.OnActivated(sender, args);
				return;
			}
			Main.trialModeChecked = false;
			GlobalStaticVars.gSexyAppBase.GotFocus();
			if (!XNAMusicInterface.PlayingUserMusic)
			{
				GlobalStaticVars.gSexyAppBase.mMusicInterface.ResumeMusic();
			}
			base.OnActivated(sender, args);
			double mMusicVolume = GlobalStaticVars.gSexyAppBase.mMusicVolume;
			GlobalStaticVars.gSexyAppBase.mMusicVolume = mMusicVolume;
		}

		protected override void OnDeactivated(object sender, EventArgs args)
		{
			if (GlobalStaticVars.gSexyAppBase == null)
			{
				base.OnDeactivated(sender, args);
				return;
			}
			GlobalStaticVars.gSexyAppBase.LostFocus();
			if (!XNAMusicInterface.PlayingUserMusic)
			{
				GlobalStaticVars.gSexyAppBase.mMusicInterface.PauseMusic();
			}
			base.OnDeactivated(sender, args);
		}

		public static bool IsInTrialMode
		{
			get
			{
				return Main.trialModeCachedValue;
			}
		}

		private static SexyTransform2D orientationTransform;

		private static UI_ORIENTATION orientationUsed;

		private static bool newOrientation;

		private static bool trialModeChecked = false;

		private static bool trialModeCachedValue;

		public Graphics graphics;

		// public static GamerServicesComponent GamerServicesComp;

		private int mFrameCnt;

		private static bool wantToSuppressDraw;

		private GamePadState previousGamepadState = default(GamePadState);

		private MouseState previousMouseState;

		private KeyboardState previousKeyboardState;

		private bool windowResizePending;

		private RenderTarget2D windowRenderTarget;

		private SpriteBatch windowSpriteBatch;

		internal RenderTarget2D frameRenderTarget;
	}
}
