using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MaterialMotion
{
	[InitializeOnLoad]
	public static class Updater
	{
		static Updater()
		{
			GatherMotions();
			StartMotions();

			EditorApplication.hierarchyWindowChanged += GatherMotions;
			EditorApplication.update += Update;
		}

		static MaterialMotion[] motions;

		static void StartMotions()
		{
            if(motions!=null)
            {
                foreach (var motion in motions)
                {
                    motion.Start();
                }
            }
			
		}

		static void GatherMotions()
		{
			if( EditorApplication.isPlayingOrWillChangePlaymode ) return;

			motions = GameObject.FindObjectsOfType( typeof(MaterialMotion) ) as MaterialMotion[];
			//Debug.Log( "Found: " + motions.Length.ToString() + " MaterialMotion instances" );
		}

		public static MaterialMotion[] IsUpdatedBy( Material pMaterial )
		{
            
			HashSet<MaterialMotion> updatingTheMaterial = new HashSet<MaterialMotion>();
            if (motions != null)
            {
                foreach (var motion in motions)
                {
                    foreach (var layer in motion.mLayers)
                    {
                        Material m = motion.GetComponent<Renderer>().sharedMaterials[layer.mMaterialID];
                        if (m != null && m == pMaterial)
                        {
                            updatingTheMaterial.Add(motion);
                        }
                    }
                }
            }
			
			MaterialMotion[] result = new MaterialMotion[ updatingTheMaterial.Count ];
			updatingTheMaterial.CopyTo( result );
			return result; 
		}

		static void Update()
		{
			if( EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling) return;

			if( motions != null )
			{
				for( int i=0; i<motions.Length; ++i)
				{
					if( motions[i] != null && motions[i].mUpdate && motions[i].enabled )
					{
						motions[i].Update();
					}
				}
			}
		}
	}

}

