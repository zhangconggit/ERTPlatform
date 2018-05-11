using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace MaterialMotion
{
	[ CustomEditor(typeof(MaterialMotion)) ]
	public class MaterialMotionEditor : Editor
	{
		const string iconStringPro = "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";
		const string iconStringFree = "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";
		Texture2D icon;

		MaterialMotion mMaterialMotion;

		SerializedProperty mAlwaysUpdate;
		SerializedProperty mMotionLayers;

		void OnEnable()
		{
			if( icon == null )
			{
				icon = new Texture2D(1,1);
				icon.hideFlags = HideFlags.HideAndDontSave;

				string s = EditorGUIUtility.isProSkin? iconStringPro : iconStringFree;
				icon.LoadImage( System.Convert.FromBase64String( s ));
			}

			mMaterialMotion = target as MaterialMotion;
	
			mAlwaysUpdate = serializedObject.FindProperty( "mUpdate" );
			mMotionLayers = serializedObject.FindProperty( "mLayers" );

			if( mMotionLayers.arraySize == 0 )
			{
				mMaterialMotion.AddLayer();
				serializedObject.Update();
			}
		}

		void OnDisable()
		{
			if( icon != null )
			{
				DestroyImmediate( icon );
			}
		}

		int mLayerToRemove = -1;

		public override void OnInspectorGUI()
		{
			Rect iconRect = EditorGUILayout.GetControlRect();
			iconRect.y -= 16;
			iconRect.width = icon.width;
			iconRect.height = icon.height;
			GUI.DrawTexture( iconRect , icon );

			GUILayout.BeginHorizontal();
			EditorGUILayout.Space();

			mAlwaysUpdate.boolValue =  EditorGUILayout.Toggle ( "Show animation in Editor", mAlwaysUpdate.boolValue );

			GUILayout.EndHorizontal();
			EditorGUILayout.Space();

			int layerIndex = 0;

			if( mMaterialMotion.GetComponent<Renderer>() == null )
			{
				EditorGUILayout.HelpBox( "No Renderer found!", MessageType.Error );
				GUI.enabled = false;
			}	

			if( mMaterialMotion.GetComponent<Renderer>().sharedMaterials.Length == 0 )
			{
				EditorGUILayout.HelpBox( "No Materials found!", MessageType.Error );
				GUI.enabled = false;
			}

			Rect r = GUILayoutUtility.GetLastRect();
			r.height = 4;
			r.y += 14;

			foreach( SerializedProperty layer in mMotionLayers )
			{
				if( !GUI.enabled ) break;
				
				GUI.Box( r , ""  );

				EditorGUI.indentLevel++;
				GUILayout.BeginHorizontal();
				EditorGUILayout.Space();

				if( mMotionLayers.arraySize > 1 )
				{
					if( GUILayout.Button( "x", EditorStyles.miniButton, GUILayout.MaxWidth(16), GUILayout.MaxHeight(16) ))
					{
						mLayerToRemove = layerIndex;
					}
				}	
				else
				{
					GUILayoutUtility.GetRect( r.width, 16 );
				}
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();

				if( DrawMaterialOptions( layer ) )
				{
					break;
				}

				if( DrawShaderPropertyOptions( layer ) )
				{
					continue;
				}

				//motion type
				DrawMotionPropertyOptions( layer );


				SerializedProperty speed = layer.FindPropertyRelative( "mSpeed" );
				speed.floatValue = EditorGUILayout.FloatField( "Speed:", speed.floatValue );

				Rect lastRect = GUILayoutUtility.GetLastRect();
				GUILayoutUtility.GetRect( r.width, 15 );
				r.y = lastRect.yMax + 24;
				EditorGUI.indentLevel--;
				layerIndex++;

			}

			if( mLayerToRemove > -1 )
			{
				mMaterialMotion.RemoveLayer( mLayerToRemove );
				serializedObject.Update();
				mLayerToRemove = -1;
			}

			EditorGUILayout.Space();
			if( GUILayout.Button( "Add Layer" ))
			{
				mMaterialMotion.AddLayer();
				serializedObject.Update();			
			}

			if( serializedObject.ApplyModifiedProperties() )
			{					
				mMaterialMotion.UpdateMapping();
				serializedObject.Update();
			}
		}

		bool DrawMaterialOptions (SerializedProperty pLayer)
		{
			SerializedProperty materialID = pLayer.FindPropertyRelative( "mMaterialID" );

			materialID.intValue = Mathf.Clamp( materialID.intValue, 0, mMaterialMotion.GetComponent<Renderer>().sharedMaterials.Length -1 );

			string alreadyUpdated = GetUpdateOverlap( materialID );
			if( alreadyUpdated != string.Empty && !EditorApplication.isPlayingOrWillChangePlaymode )
			{
				EditorGUILayout.HelpBox( "Material also updated by:\r\n" + alreadyUpdated, MessageType.Info );
			}			
			
			string[] materialNames = new string[]{};
			int[] materialIndices = new int[]{};			
			GetMaterialNames( out materialNames, out materialIndices );

			if( materialNames.Length > 1 )
			{
				materialID.intValue = EditorGUILayout.IntPopup( "Material:", materialID.intValue, materialNames, materialIndices );
			}
			else
			{
				materialID.intValue = 0;
				EditorGUILayout.LabelField( "Material:", materialNames[materialID.intValue] );
			}

			if( materialNames[materialID.intValue] == "None" )
			{
				EditorGUILayout.HelpBox( "Material is None!", MessageType.Error );
				EditorGUI.indentLevel--;
				return true;
			}

			return false;
		}
		
		bool DrawShaderPropertyOptions (SerializedProperty pLayer)
		{
			SerializedProperty shaderProperty = pLayer.FindPropertyRelative( "mShaderProperty" );
			SerializedProperty materialID = pLayer.FindPropertyRelative( "mMaterialID" );

			List<string> texturePropertyNames = new List<string>();
			List<string> textureDescriptions = new List<string>();			
			Shader shader = mMaterialMotion.GetComponent<Renderer>().sharedMaterials[ materialID.intValue ].shader;
			for( int i=0; i<ShaderUtil.GetPropertyCount( shader ); ++i )
			{
				switch( ShaderUtil.GetPropertyType( shader, i ) )
				{
					case ShaderUtil.ShaderPropertyType.TexEnv:
					case ShaderUtil.ShaderPropertyType.Float:
					case ShaderUtil.ShaderPropertyType.Color:
					case ShaderUtil.ShaderPropertyType.Range:
						textureDescriptions.Add( ShaderUtil.GetPropertyDescription( shader, i ) );
						texturePropertyNames.Add( ShaderUtil.GetPropertyName( shader, i ) );
						break;

				}
			}
			if( texturePropertyNames.Count == 0 )
			{
				EditorGUILayout.HelpBox( "Shader does not have any valid Inputs (Texture, Float, Color)", MessageType.Info );
				return true;
			}
			
			int index = texturePropertyNames.FindIndex( x => x.Equals( shaderProperty.stringValue ) );
			if( index == -1 )
			{
				index = 0;
			}
			index = EditorGUILayout.Popup( "Shader Property:", index, textureDescriptions.ToArray() );
			shaderProperty.stringValue = texturePropertyNames[index];
			pLayer.FindPropertyRelative( "mMotionProperty" ).enumValueIndex = (int)ShaderUtil.GetPropertyType( shader, index );

			return false;
		}

		void DrawMotionPropertyOptions (SerializedProperty pLayer)
		{
			switch( (MotionPropertyType)pLayer.FindPropertyRelative( "mMotionProperty" ).enumValueIndex )
			{
				case MotionPropertyType.TexEnv:
					DrawTexEnvOptions( pLayer );
					break;
				case MotionPropertyType.Float:
				case MotionPropertyType.Range:
					DrawFloatOptions( pLayer );
					break;
				case MotionPropertyType.Color:
					DrawColorOptions( pLayer );
					break;
			}
		}

		void DrawTexEnvOptions( SerializedProperty pLayer )
		{
			SerializedProperty motionTyp = pLayer.FindPropertyRelative( "mTextureMotionType" );
			motionTyp.enumValueIndex = (int)((TextureMotionType)EditorGUILayout.EnumPopup( "Motion Type:", (TextureMotionType)motionTyp.enumValueIndex ));
			
			switch( (TextureMotionType)motionTyp.enumValueIndex )
			{
			case TextureMotionType.scale:
			case TextureMotionType.scroll:
				DrawCurvesOptions( pLayer );
				break;
				
			case TextureMotionType.flipbook:
				DrawFlipBookOptions( pLayer  );
				break;
			}
		}

		void DrawCurvesOptions( SerializedProperty pLayer )
		{
			SerializedProperty animCurveU = pLayer.FindPropertyRelative( "mAnimCurveX" );
			SerializedProperty animCurveV = pLayer.FindPropertyRelative( "mAnimCurveY" );
			
			animCurveU.animationCurveValue = EditorGUILayout.CurveField( "U Curve:", animCurveU.animationCurveValue );
			animCurveV.animationCurveValue = EditorGUILayout.CurveField( "V Curve:", animCurveV.animationCurveValue );
		}
		
		void DrawFlipBookOptions( SerializedProperty pLayer )
		{
			SerializedProperty materialID = pLayer.FindPropertyRelative( "mMaterialID" );
			SerializedProperty shaderProperty = pLayer.FindPropertyRelative( "mShaderProperty" );

			SerializedProperty animCurveU = pLayer.FindPropertyRelative( "mAnimCurveX" );
			SerializedProperty animCurveV = pLayer.FindPropertyRelative( "mAnimCurveY" );
			SerializedProperty rows = pLayer.FindPropertyRelative( "mRows" );
			SerializedProperty collumns = pLayer.FindPropertyRelative( "mCollumns" );
			SerializedProperty start = pLayer.FindPropertyRelative( "mStartFrame" );
			SerializedProperty end = pLayer.FindPropertyRelative( "mEndFrame" );

			rows.intValue = Mathf.Clamp( EditorGUILayout.IntField( "Rows: ", rows.intValue ), 1, 100);
			collumns.intValue = Mathf.Clamp( EditorGUILayout.IntField( "Collumns: ", collumns.intValue ), 1, 100);

			int max = rows.intValue * collumns.intValue;
			start.intValue = Mathf.Clamp( EditorGUILayout.IntField( "Start: ", start.intValue ), 1, max - 1);
			end.intValue = Mathf.Clamp( EditorGUILayout.IntField( "End: ", end.intValue ), start.intValue + 1, max);

			animCurveU.animationCurveValue = SetFlipBookCurves( collumns.intValue, 1, start.intValue, end.intValue );
			animCurveV.animationCurveValue = SetFlipBookCurves( rows.intValue, collumns.intValue, start.intValue, end.intValue );

			Vector2 scale = new Vector2( 1f/(float)rows.intValue, 1f/(float)collumns.intValue );
			mMaterialMotion.GetComponent<Renderer>().sharedMaterials[ materialID.intValue ].SetTextureScale( shaderProperty.stringValue, scale ); 
		}

		void DrawFloatOptions( SerializedProperty pLayer )
		{
			SerializedProperty animCurveU = pLayer.FindPropertyRelative( "mAnimCurveX" );
			animCurveU.animationCurveValue = EditorGUILayout.CurveField( "Float Curve:", animCurveU.animationCurveValue );
		}

		void DrawColorOptions(SerializedProperty pLayer)
		{
			GUIContent text = new GUIContent( "Colors: " );
			EditorGUILayout.PropertyField( pLayer.FindPropertyRelative( "mGradient" ), text);
		}

		AnimationCurve SetFlipBookCurves( int pSize, int pScale, int pStart, int pEnd )
		{
			var curve = new AnimationCurve();
			curve.preWrapMode = WrapMode.Loop;
			curve.postWrapMode = WrapMode.Loop;

			bool isCollumns = pScale <= 1;
			int length = pEnd - pStart + 1;

			for( int i=0; i<=length; ++i )
			{
				int index = i + pStart-1; 
				float time, value;

				if( isCollumns )
				{
					time = index * ( 1f/pSize ) * pScale;
					value = index * ( 1f/pSize );
				}
				else
				{
					time = index * ( 1f/pSize );
					value = ( 1f - 1f/pSize ) - Mathf.Floor( index / (float)pScale) * ( 1f/pSize );
				}

				Keyframe keyframe = CurveExtended.KeyframeUtil.GetNew( time, value, CurveExtended.TangentMode.Stepped );
				curve.AddKey( keyframe );
			}

			return curve;
		}

		void GetMaterialNames( out string[] pMaterialNames, out int[] pMaterialIndices )
		{
			Material[] materials = mMaterialMotion.GetComponent<Renderer>().sharedMaterials;
			pMaterialNames = new string[ materials.Length ];
			pMaterialIndices = new int[ materials.Length ];

			for( int i=0; i<materials.Length; ++i )
			{
				if( materials[i] != null )
				{
					pMaterialNames[i] = materials[i].name;
				}
				else
				{
					pMaterialNames[i] = "None";
				}

				pMaterialIndices[i] = i;
			}
		}

		string GetUpdateOverlap( SerializedProperty pMaterialID )
		{
			string overlapNames = string.Empty;

			Material m = mMaterialMotion.GetComponent<Renderer>().sharedMaterials[ pMaterialID.intValue ];
			MaterialMotion[] overlapMotions = Updater.IsUpdatedBy( m );

			foreach( var motion in overlapMotions )
			{
				if( motion == mMaterialMotion ) continue;

				overlapNames += "-" + motion.gameObject.name + System.Environment.NewLine;
			}	

			return overlapNames;
		}
	
	}
}
