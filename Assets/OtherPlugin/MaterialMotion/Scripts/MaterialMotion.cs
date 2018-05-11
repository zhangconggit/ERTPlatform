using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MaterialMotion
{
	[ExecuteInEditMode]
	[AddComponentMenu("Miscellaneous/MaterialMotion")]
	public class MaterialMotion : MonoBehaviour 
	{
		public bool mUpdate = true;

		public List<MotionLayer> mLayers = new List<MotionLayer>();

		[SerializeField]
		List<List<MotionLayer>> mMapping = new List<List<MotionLayer>>();

		int mMaterialCount = 0;

		public void AddLayer()
		{
			mLayers.Add( new MotionLayer() );
			UpdateMapping();
		}

		public void RemoveLayer( MotionLayer pLayer )
		{			
			if( mLayers.Contains( pLayer ) )
		    {
				mLayers.Remove( pLayer );
				UpdateMapping();
			}
		}

		public void RemoveLayer( int pIndex )
		{
			if( 0 <= pIndex && pIndex < mLayers.Count )
			{
				mLayers.RemoveAt( pIndex );
				UpdateMapping();
			}
		}
	
		public void UpdateMapping()
		{
			mMapping.Clear();
			foreach( var currentlayer in mLayers )
			{
				foreach( var set in mMapping )
				{
					if( set.Contains( currentlayer ))
					{
						continue;
					}
				}

				List<MotionLayer> motions = new List<MotionLayer>();
				motions.Add( currentlayer );
				foreach( var layer in mLayers )
				{
					if( layer == currentlayer ) continue;

					if( layer.mMaterialID == currentlayer.mMaterialID &&
					   	layer.mMotionProperty == currentlayer.mMotionProperty &&
					    layer.mShaderProperty == currentlayer.mShaderProperty &&					   	
					    layer.mTextureMotionType == currentlayer.mTextureMotionType )
					{
						motions.Add( layer );
					}
				}
				mMapping.Add( motions );
			}			
		}


		void UpdateMaterials()
		{
			for( int i=0; i<mMapping.Count; ++i)
			{
				Material material = GetComponent<Renderer>().sharedMaterials[ mMapping[i][0].mMaterialID ];
				if( material == null ) continue;

				switch( mMapping[i][0].mMotionProperty )
				{
					case MotionPropertyType.TexEnv:
						UpdateTexEnv( mMapping[i], ref material );
						break;

					case MotionPropertyType.Color:
						UpdateColor( mMapping[i], ref material );
						break;

					case MotionPropertyType.Float:
					case MotionPropertyType.Range:
						UpdateFloat( mMapping[i], ref material );
						break;
				}
			}
		}

		void UpdateColor( List<MotionLayer> pMotionLayers, ref Material pMaterial )
		{
			Color c = new Color(0,0,0,0);
			foreach( var layer in pMotionLayers )
			{
				float time = Time.time;
#if UNITY_EDITOR
				time = Time.realtimeSinceStartup;
#endif
				c += layer.EvaluateColor( time );
			}

			c.r = ( c.r / pMotionLayers.Count );
			c.g = ( c.g / pMotionLayers.Count );
			c.b = ( c.b / pMotionLayers.Count );
			c.a = ( c.a / pMotionLayers.Count );

			pMaterial.SetColor( pMotionLayers[0].mShaderProperty, c );
		}

		void UpdateFloat( List<MotionLayer> pMotionLayers, ref Material pMaterial )
		{			
			float value = 0;
			foreach( var layer in pMotionLayers )
			{
				float time = Time.time;
#if UNITY_EDITOR
				time = Time.realtimeSinceStartup;
#endif
				value += layer.EvaluateFloat( time );
			}

			pMaterial.SetFloat( pMotionLayers[0].mShaderProperty, value );
		}


		void UpdateTexEnv( List<MotionLayer> pMotionLayers, ref Material pMaterial )
		{
			Vector2 v = Vector2.zero;
			foreach( var layer in pMotionLayers )
			{
				float time = Time.time;
#if UNITY_EDITOR
				time = Time.realtimeSinceStartup;
#endif
				v += layer.EvaluateVector2( time );
			}

			switch( pMotionLayers[0].mTextureMotionType )
			{
				case TextureMotionType.scroll:
					v = WrapVector2( ref v );
					pMaterial.SetTextureOffset( pMotionLayers[0].mShaderProperty, v );
					break;
				case TextureMotionType.flipbook:
					v = WrapVector2( ref v );
					pMaterial.SetTextureOffset( pMotionLayers[0].mShaderProperty, v );						
					break;
				case TextureMotionType.scale:
					pMaterial.SetTextureScale( pMotionLayers[0].mShaderProperty, v );
					break;
			}
		}


		bool ValidateMaterialIndices()
		{
			for( int i=0; i<mMapping.Count; ++i)
			{
				int index = mMapping[i][0].mMaterialID;
				index = Mathf.Clamp( index, 0, GetComponent<Renderer>().sharedMaterials.Length -1 );

				if( index == -1 )
				{
					return false;
				}		
				mMapping[i][0].mMaterialID = index;
			}

			mMaterialCount = GetComponent<Renderer>().sharedMaterials.Length;
			return true;
		}

		Vector2 WrapVector2( ref Vector2 pVector )
		{
			pVector.x = pVector.x % 1;
			pVector.y = pVector.y % 1;

			return pVector;
		}

		#region unity callbacks
		public void Start()
		{
			if( GetComponents<MaterialMotion>().Length > 1 )
			{
				Debug.LogWarning( "MaterialMotion script already added to gameobject: " + this.gameObject.name  );
				DestroyImmediate( this );
			}

			UpdateMapping();
			ValidateMaterialIndices();
		}

		void Reset()
		{
			mLayers.Clear();
			AddLayer();
			UpdateMapping();
			ValidateMaterialIndices();
		}

		public void Update () 
		{
			if( mLayers == null || mLayers.Count == 0 || GetComponent<Renderer>() == null ) return;

			if( mMaterialCount != GetComponent<Renderer>().sharedMaterials.Length )
			{
				if( !ValidateMaterialIndices() )
				{
					return;
				}
			}

			if( Application.isPlaying )
			{
				UpdateMaterials();
			}
			else if( mUpdate )
			{
				UpdateMaterials();
			}
		}

		#endregion
	}

}
