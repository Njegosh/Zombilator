using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(Spawner))]
[CanEditMultipleObjects]
public class SpawnerEditor : Editor
{
    private BoxBoundsHandle boundsHandle;
    bool canEdit;

    Color[] cols = new Color[]{
        Color.green,
        new Color(0.85f, 0.85f, 0.1f),
        new Color(1f, 0.6f, 0.02f),
        Color.red,
    };

    string[] names = new string[] {
        "0 - Safe",
        "1 - Medium",
        "2 - Hard",
        "3 - Muahaha"
        };
    int[] dangers = {0, 1, 2, 3};

    GUIStyle st = new GUIStyle();

    int maxZombies;
    int numZombies;

    int danger;

    Spawner s;

    private void OnEnable()
    {
        boundsHandle = new BoxBoundsHandle();
        boundsHandle.axes = PrimitiveBoundsHandle.Axes.None; // Allow manipulation on all axes
        boundsHandle.SetColor(cols[0]);

        
        st.fontSize = 16;
        
        Spawner s = (Spawner)target;
        danger = s.danger;
    }

    public override void OnInspectorGUI()
    {
        s = (Spawner)target;
        
        st.normal.textColor = cols[s.danger];

        maxZombies = (int)((s.bounds.size.x / 3f) *  (int)(s.bounds.size.z / 3f));
        numZombies = (int)(Mathf.Clamp((float)(danger + 1)/ 4, 0.15f, 1) * maxZombies);

        GUILayout.Label("DEBUG X: " + s.bounds.size.x.ToString());
        GUILayout.Label("DEBUG Z: " + s.bounds.size.z.ToString());

        
        EditorGUI.BeginChangeCheck();

        GUILayout.Label("Danger: ");
        danger = EditorGUILayout.IntPopup(danger, names, dangers, st);

        EditorGUILayout.Space(5);

        GUILayout.Label("Max zombies: " + maxZombies.ToString());
        GUILayout.Label("Number of zombies: " + numZombies.ToString());


        EditorGUILayout.Space(5);
        // add space for better look

        canEdit = GUILayout.Toggle(canEdit, "Edit Bounds");

        EditorGUILayout.Space(5);

        s.danger = danger;
        
        if(canEdit){
            boundsHandle.axes = PrimitiveBoundsHandle.Axes.All;
        }
        else{
            boundsHandle.axes = PrimitiveBoundsHandle.Axes.None;
        }
        
        EditorUtility.SetDirty(target);
        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
        
        //EditorGUILayout.LabelField("Hello");
       
        // draw everything else
        base.OnInspectorGUI();
    }

    public void OnSceneGUI()
    {
        s = (Spawner)target;
        // Set the bounds handle's  center and size based on the target object's bounds
        boundsHandle.center = s.transform.position;
        boundsHandle.size = s.bounds.size;

        Handles.color = cols[s.danger];
        GUI.color = cols[s.danger];

        string str = "\n" + names[s.danger] + "\nMax: " + maxZombies + "\n\nNum: " + numZombies;

        Handles.Label(boundsHandle.center, str ,st);
        
        Handles.DrawWireCube(s.transform.position, s.bounds.size);

        if(canEdit){

            EditorGUI.BeginChangeCheck();
            using (var scope = new Handles.DrawingScope(Handles.matrix))
            {
                boundsHandle.DrawHandle();
            }
            
            if (EditorGUI.EndChangeCheck())
            {
                // Update the target object's bounds with the modified values
                Undo.RecordObject(target, "Modify Bounds");
                s.bounds = new Bounds(s.transform.position, boundsHandle.size);
            }
        }
    }
}
