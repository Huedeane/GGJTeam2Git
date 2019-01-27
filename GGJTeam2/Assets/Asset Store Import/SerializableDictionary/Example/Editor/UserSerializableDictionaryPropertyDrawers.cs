using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
[CustomPropertyDrawer(typeof(ColorArrayStorage))]

//Condition
[CustomPropertyDrawer(typeof(QuestE_QuestStatusDictionary))]
[CustomPropertyDrawer(typeof(ItemE_ItemStatusDictionary))]



//ConditionManager
//[CustomPropertyDrawer(typeof(ConditionBoolDictionary))]

public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
