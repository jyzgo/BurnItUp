using UnityEngine;
using System.Collections;
using System;



#if UNITY_EDITOR 
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
#endif

[ExecuteInEditMode]
public class SortingLayerHelper : MonoBehaviour {
	[HideInInspector]
	public string SortingLayerName = "Default";

	void Start()
	{
		ChangeSortingLayer(transform);
	}
	
	void ChangeSortingLayer(Transform trans)
	{
		var par = trans.GetComponent<Renderer>();
		if(par != null)
		{
			par.sortingLayerName = SortingLayerName;
		}
		
		int childCount = trans.transform.childCount;
		for (int i = 0; i < childCount; ++i) {
			var curTrans = trans.GetChild(i);
			if(curTrans.GetComponent<SortingLayerHelper>() == null)
			{
				ChangeSortingLayer(curTrans);
			}
			
		}
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(SortingLayerHelper),true)]
public class SortingLayerHelpEditor : Editor
{
	string[] sortingLayers;
	int  sortingLayerIndex  = 1;
	public override void  OnInspectorGUI()
	{
		sortingLayers = GetSortingLayerNames();
		base.DrawDefaultInspector();
		GUILayout.Label ("选择层级：");
		
		var myScript = target as SortingLayerHelper;
		
		int curIndex = 0;
		for(int i = 0 ; i  < sortingLayers.Length; i ++)
		{
			if(myScript.SortingLayerName == sortingLayers[i])
			{
				curIndex = i;
			}
		}
		
		
		GUILayout.BeginHorizontal ();
		sortingLayerIndex = EditorGUILayout.Popup (curIndex, sortingLayers, GUILayout.Width (100));
		
		myScript.SortingLayerName = sortingLayers[sortingLayerIndex];
		
		
		GUILayout.Space (10);
		
		GUILayout.EndHorizontal ();
		
		
		
	}  
	
	public string[] GetSortingLayerNames() {
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
}
#endif
