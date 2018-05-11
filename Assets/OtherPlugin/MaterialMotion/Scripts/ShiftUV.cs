using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MaterialMontion
{
	[AddComponentMenu("Mesh/ShiftUV")]
	[RequireComponent(typeof(MeshFilter))]
	public class ShiftUV : MonoBehaviour 
	{
		[SerializeField] bool random_U = false;
		[SerializeField] bool random_V = false;
		[SerializeField] float U_Offset = 0f;
		[SerializeField] float V_Offset = 0f;
		
		void Awake () 
		{	
			Vector2 v = new Vector2();

			v.x = random_U ? Random.value : U_Offset;
			v.y = random_V ? Random.value : V_Offset;
								
			Mesh mesh = this.GetComponent<MeshFilter>().mesh;
			Vector2[] uv = mesh.uv;
			
			for ( int i=0; i<uv.Length; i++)
			{			
				uv[i] = uv[i] + v;  
			}
			
			mesh.uv = uv;
		}		
	}
}
