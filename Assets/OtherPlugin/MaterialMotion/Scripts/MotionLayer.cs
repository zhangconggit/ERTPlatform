using UnityEngine;
using System.Collections;


namespace MaterialMotion
{
	[System.Serializable]
	public class MotionLayer
	{
		public int mMaterialID = 0;
		public string mShaderProperty = string.Empty;
		public MotionPropertyType mMotionProperty;

		public AnimationCurve mAnimCurveX;
		public AnimationCurve mAnimCurveY;

		public TextureMotionType mTextureMotionType;

		public float mSpeed;

		public int mRows;
		public int mCollumns;
		public int mStartFrame;
		public int mEndFrame;

		public Gradient mGradient;

		public MotionLayer()
		{
			mMaterialID = 0;
			mAnimCurveX = AnimationCurve.Linear( 0, 0, 1.0f, 1.0f );
			mAnimCurveX.preWrapMode = WrapMode.Loop;
			mAnimCurveX.postWrapMode = WrapMode.Loop;
			mAnimCurveY = AnimationCurve.Linear( 0, 0, 1.0f, 1.0f );
			mAnimCurveY.preWrapMode = WrapMode.Loop;
			mAnimCurveY.postWrapMode = WrapMode.Loop;
			mTextureMotionType = TextureMotionType.scroll;
			mGradient = new Gradient();
			mSpeed = 1.0f;
		}

		public Vector2 EvaluateVector2( float pTime )
		{
			Vector2 result = Vector2.zero;
			result.x = mAnimCurveX.Evaluate( pTime * mSpeed );
			result.y = mAnimCurveY.Evaluate( pTime * mSpeed );
			return result;
		}

		public Color EvaluateColor( float pTime )
		{
			return mGradient.Evaluate( (pTime * mSpeed) % 1f );
		}

		public float EvaluateFloat( float pTime )
		{
			return mAnimCurveX.Evaluate( pTime * mSpeed );
		}
	}
}
