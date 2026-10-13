using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sexy
{
	public class GraphicsState
	{
		public int mScreenWidth
		{
			get
			{
				return GlobalStaticVars.gSexyAppBase == null
					? GraphicsState.mGraphicsDeviceManager.PreferredBackBufferWidth
					: (int)Math.Round(GlobalStaticVars.gSexyAppBase.mVirtualWidth - GlobalStaticVars.gSexyAppBase.mVirtualNegativeWidth);
			}
		}

		public int mScreenHeight
		{
			get
			{
				return GlobalStaticVars.gSexyAppBase == null
					? GraphicsState.mGraphicsDeviceManager.PreferredBackBufferHeight
					: (int)Math.Round(GlobalStaticVars.gSexyAppBase.mVirtualHeight - GlobalStaticVars.gSexyAppBase.mVirtualNegativeHeight);
			}
		}

		protected TRect ScreenBounds
		{
			get
			{
				float left = GlobalStaticVars.gSexyAppBase?.mVirtualNegativeWidth ?? 0f;
				float top = GlobalStaticVars.gSexyAppBase?.mVirtualNegativeHeight ?? 0f;
				return new TRect((int)Math.Floor(left), (int)Math.Floor(top),
					(int)Math.Ceiling(left + this.mScreenWidth) - (int)Math.Floor(left),
					(int)Math.Ceiling(top + this.mScreenHeight) - (int)Math.Floor(top));
			}
		}

		internal static Vector2 RenderOffset;

		public GraphicsState(Game game)
		{
			GraphicsState.mGraphicsDeviceManager = new GraphicsDeviceManager(game);
			this.Reset();
		}

		public GraphicsState()
		{
			this.Reset();
		}

		private void Reset()
		{
			this.mTransX = 0;
			this.mTransY = 0;
			this.mFastStretch = false;
			this.mWriteColoredString = false;
			this.mLinearBlend = false;
			this.mScaleX = 1f;
			this.mScaleY = 1f;
			this.mScaleOrigX = 0f;
			this.mScaleOrigY = 0f;
			this.mFont = null;
			this.mColor = default(SexyColor);
			this.mColorizeImages = false;
			this.WorldRotation = 0f;
			this.mDrawMode = 0;
			this.mClipRect = this.ScreenBounds;
		}

		public static void Init()
		{
			Texture2D texture2D = new Texture2D(GraphicsState.mGraphicsDeviceManager.GraphicsDevice, 1, 1);
			texture2D.SetData<Color>(new Color[] { Color.White });
			GraphicsState.dummy = new Image(texture2D);
		}

		public void CopyStateFrom(GraphicsState theState)
		{
			this.mTransX = theState.mTransX;
			this.mTransY = theState.mTransY;
			this.mFastStretch = theState.mFastStretch;
			this.mWriteColoredString = theState.mWriteColoredString;
			this.mLinearBlend = theState.mLinearBlend;
			this.mScaleX = theState.mScaleX;
			this.mScaleY = theState.mScaleY;
			this.mScaleOrigX = theState.mScaleOrigX;
			this.mScaleOrigY = theState.mScaleOrigY;
			this.mClipRect = theState.mClipRect;
			this.mFont = theState.mFont;
			this.mColor = theState.mColor;
			this.mDrawMode = theState.mDrawMode;
			this.mColorizeImages = theState.mColorizeImages;
			this.WorldRotation = theState.WorldRotation;
		}

		public void SetWorldRotation(float theRotation)
		{
			this.WorldRotation = theRotation;
			this.NeedToSetWorldRotation = true;
		}

		public void ApplyWorldRotation()
		{
		}

		public static int GetClosestPowerOf2Above(int theNum)
		{
			int i;
			for (i = 1; i < theNum; i <<= 1)
			{
			}
			return i;
		}

		public static int GraphicsDeviceWidth;

		public static int GraphicsDeviceHeight;

		public int mTransX;

		public int mTransY;

		public float mScaleX;

		public float mScaleY;

		public float mScaleOrigX;

		public float mScaleOrigY;

		public bool mFastStretch;

		public bool mWriteColoredString;

		public bool mLinearBlend;

		public TRect mClipRect;

		public Color mColor;

		protected Font mFont;

		public int mDrawMode;

		public bool mColorizeImages;

		protected static Image dummy;

		public float WorldRotation;

		public bool NeedToSetWorldRotation;

		public static GraphicsDeviceManager mGraphicsDeviceManager;
	}
}
