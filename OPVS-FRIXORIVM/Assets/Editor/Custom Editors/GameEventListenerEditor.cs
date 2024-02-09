using UnityEditor;

[CustomEditor(typeof(GameEventListener), true)]
public class GameEventListenerEditor: Editor
{
    private SerializedProperty _gameEvent;
    private SerializedProperty _useChannels;
    private SerializedProperty _channels;
    private SerializedProperty _unityEvent;

    private void OnEnable()
    {
        _gameEvent = serializedObject.FindProperty("_gameEvent");
        _useChannels = serializedObject.FindProperty("_useChannels");
        _channels = serializedObject.FindProperty("_channels");
        _unityEvent = serializedObject.FindProperty("_unityEvent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_gameEvent);
        EditorGUILayout.PropertyField(_useChannels);
        if (_useChannels.boolValue)
        {
            EditorGUILayout.PropertyField(_channels);
            if (_channels.arraySize == 0)
            {
                EditorGUILayout.HelpBox("This listener is currently not subscribing to any channels. For a global listener, uncheck \"Use Channels\".", MessageType.Warning);
            }
        }
        EditorGUILayout.PropertyField(_unityEvent);

        serializedObject.ApplyModifiedProperties();
    }
}
