using System.Collections;

namespace MaterialMotion
{
	public enum TextureMotionType
	{
		scroll,
		scale,
		flipbook,
	}

	//Mirror of ShaderUtil.ShaderPropertyType to have the enum available at runtime
	public enum MotionPropertyType
	{
		Color,
		Vector,
		Float,
		Range,
		TexEnv,
	}
}

