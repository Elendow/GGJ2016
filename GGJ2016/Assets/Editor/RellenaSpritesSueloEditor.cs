#if UNITY_EDITOR
using UnityEngine;
using System.Collections;

using UnityEditor;


[CustomEditor(typeof(RellenaSpritesSuelo))]
public class RellenaSpritesSueloEditor : Editor {
	
	public override void OnInspectorGUI(){

		DrawDefaultInspector ();

		RellenaSpritesSuelo builder = (RellenaSpritesSuelo)target;

		if (GUILayout.Button ("Build")) {

			builder.Build();
		}

		DrawDefaultInspector ();

	}
}
#endif