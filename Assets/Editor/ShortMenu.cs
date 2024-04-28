using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ShortMenu: Editor
{
[MenuItem("Open Scenes/Game")]//0
    public static void Option1(){
        string scenePath = 
        "Assets/_Game/Scenes/Game.unity";//s0
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
[MenuItem("Open Scenes/MainMenu")]//1
    public static void Option2(){
        string scenePath = 
        "Assets/_Game/Scenes/MainMenu.unity";//s1
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
[MenuItem("Open Scenes/Master")]//2
    public static void Option3(){
        string scenePath = 
        "Assets/_Game/Scenes/Master.unity";//s2
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
//menu item placeholder3
    public static void Option4(){
        string scenePath = 
        "scene name placeholder3";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    //menu item placeholder4
    public static void Option5(){
        string scenePath = 
        "scene name placeholder4";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
    //menu item placeholder5
    public static void Option6(){
        string scenePath = 
        "scene name placeholder5";
        EditorSceneManager.SaveOpenScenes();
        EditorSceneManager.OpenScene(scenePath);
    }
}
