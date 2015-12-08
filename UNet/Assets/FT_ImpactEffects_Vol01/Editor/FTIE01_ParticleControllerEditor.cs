using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FTIE01_ParticleController))]
public class FTIE01_ParticleControllerEditor : Editor {
	
	
	
	private SerializedProperty scaleProperty;
	private SerializedProperty scaleLifeProperty;
	
	void OnEnable(){
		scaleProperty = serializedObject.FindProperty("scale");
		scaleLifeProperty = serializedObject.FindProperty("scaleLife");
	}
	
	public override void OnInspectorGUI()
	{
		//DrawDefaultInspector();
		FTIE01_ParticleController myScript = (FTIE01_ParticleController)target;
		serializedObject.Update();
		
		var scaleValue = EditorGUILayout.Slider( "Scaling Particle", scaleProperty.floatValue, 0.1f, 10.0f );
		
		if (scaleValue != scaleProperty.floatValue)
		{
			scaleProperty.floatValue = scaleValue;
		}
		
		var scaleLifeValue = EditorGUILayout.Slider( "Scaling Lifetime", scaleLifeProperty.floatValue, 0.1f, 10.0f );
		
		if (scaleLifeValue != scaleLifeProperty.floatValue)
		{
			scaleLifeProperty.floatValue = scaleLifeValue;
		}
		
		EditorGUILayout.LabelField ("Color Editor");
		EditorGUILayout.LabelField ("------------------------------------------------------------------------------------------------------------------------------");
		EditorGUILayout.PropertyField(serializedObject.FindProperty("particleSystems"),true);
		
		EditorGUILayout.PropertyField(serializedObject.FindProperty("particleColor"),true);
		
		EditorGUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Get Particle Color"))
		{
			myScript.GetColor();
		}
		if(GUILayout.Button("Clear"))
		{
			myScript.ClearColor();
		}
		EditorGUILayout.EndHorizontal();
		
		
		
		
		
		serializedObject.ApplyModifiedProperties();
	}
}
