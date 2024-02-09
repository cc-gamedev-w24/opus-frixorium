using UnityEditor;

[CustomEditor(typeof(GameEventListener), true)]
public class GameEventListenerEditor: Editor
{
    private SerializedProperty _gameEvent;
    private SerializedProperty _useChannels;
    private SerializedProperty _channels;

    private void OnEnable()
    {
        _gameEvent = serializedObject.FindProperty("_gameEvent");
        _useChannels = serializedObject.FindProperty("_useChannels");
        _channels = serializedObject.FindProperty("_channels");
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

        var prop = serializedObject.GetIterator();
        prop.NextVisible(true);
        do {
            if (
                prop.name == _channels.name ||
                prop.name == _useChannels.name ||
                prop.name == _gameEvent.name ||
                prop.name == "m_Script") continue;
            EditorGUILayout.PropertyField(prop);
        } while (prop.NextVisible(false));
        
        serializedObject.ApplyModifiedProperties();
    }
}
