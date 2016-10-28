using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SurvivalKit
{
	//[CustomPropertyDrawer (typeof(RoundPropertyi))]
	public class RoundPropertyiEditor : PropertyDrawer
	{
/*		private SerializedObject serObj;

		private SerializedProperty _operator;
		private SerializedProperty _percentageChange;
		private SerializedProperty _initialCount;
		private SerializedProperty _threshold;

		void Awake ()
		{
			serObj = new SerializedObject (target);

			_operator = serObj.FindProperty ("operatorToPerform");
			_initialCount = serObj.FindProperty ("initialCount");
			_percentageChange = serObj.FindProperty ("percentageChange");
			_threshold = serObj.FindProperty ("threshold");
		}
	
		public override void OnInspectorGUI ()
		{
			serObj.Update ();

			EditorGUILayout.PropertyField (_initialCount, new GUIContent ("Initial Count"));

			EditorGUILayout.PropertyField (_operator, new GUIContent ("Operator To Perform"));

			if (_operator.enumValueIndex != (int)Operator.None) {

				EditorGUILayout.PropertyField (_percentageChange, new GUIContent ("Percentage Change"));

				string text = "";

				EditorGUILayout.PropertyField (_threshold, new GUIContent (text));
			}

			serObj.ApplyModifiedProperties ();
		} */

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{

			//EditorGUILayout.PropertyField (property.FindPropertyRelative ("initialCount"));
/*
			
			EditorGUILayout.PropertyField (property.FindPropertyRelative ("operatorToPerform"));*/
			
			/*	if (property.FindPropertyRelative ("operatorToPerform").enumValueIndex != (int)Operator.None) {
				
				EditorGUILayout.PropertyField (property.FindPropertyRelative ("percentageChange"), new GUIContent ("Percentage Change"));
				
				string text = "";
				
				EditorGUILayout.PropertyField (property.FindPropertyRelative ("_threshold"), new GUIContent (text));
			}
*/

		}
	}
}
