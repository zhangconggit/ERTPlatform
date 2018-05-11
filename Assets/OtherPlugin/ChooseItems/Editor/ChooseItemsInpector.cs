using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ChooseItems))]
public class ChooseItemsInpector : Editor
{
    ChooseItems script;//所对应的脚本对象
    GameObject rootObject;//脚本的GameObject
    SerializedObject seriObject;//所对应的序列化对象
    SerializedProperty headColor;//一个[SerializeField][HideInInspector]且private的序列化的属性

    //初始化
    public void OnEnable()
    {
        seriObject = base.serializedObject;
        headColor = seriObject.FindProperty("headColor");
        var tscript = (ChooseItems)(base.serializedObject.targetObject);
        if (tscript != null)
        {
            script = tscript;
            rootObject = script.gameObject;
        }
        else
        {
        }
    }

    //清理
    public void OnDisable()
    {
        var tscript = (ChooseItems)(base.serializedObject.targetObject);
        if (tscript == null)
        {
            // 这种情况是脚本对象被移除了;  
            //Debug.Log("tscript == null");
        }
        else
        {
            // 这种情况是编译脚本导致的重刷;  
            //Debug.Log("tscript != null");
        }
        seriObject = null;
        script = null;
        rootObject = null;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("camera"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NamePrefab"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("okIcon"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("errorIcon"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("items"), true);
        if(GUILayout.Button("Run"))
        {
            script.cameraPath = script.camera.transform.position;
            script.cameraAngle = script.camera.transform.rotation.eulerAngles;
            for (int i = 0; i < script.items.Count; i++)
            {
                if (script.items[i].obj == null)
                    return;
                GameObject ui = rootObject.transform.Find(script.items[i].obj.name) == null ? null: rootObject.transform.Find(script.items[i].obj.name).gameObject;
                if (ui == null)
                {
                    Vector3 pos = script.camera.WorldToScreenPoint(script.items[i].obj.transform.position);
                    ui = Instantiate(script.NamePrefab) as GameObject;
                    ui.name = script.items[i].obj.name;
                    ui.transform.parent = rootObject.transform;
                    ui.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    ui.GetComponent<RectTransform>().position = pos;
                }
                ui.GetComponent<soloItem>().errorIcon.GetComponent<Image>().sprite = script.errorIcon;
                ui.GetComponent<soloItem>().okIcon.GetComponent<Image>().sprite = script.okIcon;
                ui.GetComponent<soloItem>().bIsOk = script.items[i].isOk;
                ui.GetComponent<soloItem>().textName = script.items[i].Name;
                ui.GetComponent<soloItem>().target = script.items[i].obj;
                ui.GetComponent<soloItem>().LookAtCamera = script.camera;
                if (ui.GetComponent<soloItem>().target.GetComponent<Collider>() == null)
                    ui.GetComponent<soloItem>().target.AddComponent<BoxCollider>();
            }
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("objectParent"), true);
        if (GUILayout.Button("Add GameObject"))
        {
            //Debug.Log("path=" + script.cameraPath);
            for (int i = 0; i < rootObject.transform.childCount; i++)
            {
                rootObject.transform.GetChild(i).GetComponent<soloItem>().target = script.objectParent.transform.Find(rootObject.transform.GetChild(i).name).gameObject;
                ItemObject obj = script.items.Find(temp => {
                    if (temp.Name == rootObject.transform.GetChild(i).GetComponent<Text>().text)
                    {
                        return true;
                    }
                    else
                        return false;
                });
                obj.obj = rootObject.transform.GetChild(i).GetComponent<soloItem>().target;
            }
        }
        EditorGUILayout.LabelField("------ Paramter ------");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mode"), true);
        ChooseItems item = target as ChooseItems;
        if (item.mode == ChooseMode.Choose)
        {
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("chooseParam"), true);
        }
        else if (item.mode == ChooseMode.MoveItem)
        {
            Rect rect = EditorGUILayout.BeginHorizontal("MoveParam");
            rect.height = 60;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveParam") ,true);
            if (GUILayout.Button("update"))
            {
                if(script.endObject != null && script.startObject != null)
                {
                    Vector3 v3 = script.endObject.transform.position - script.startObject.transform.position;
                    script.moveParam = v3;
                }
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("startObject"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("endObject"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("moveSpeed"), true);
            
        }
        serializedObject.ApplyModifiedProperties();
    }
}
