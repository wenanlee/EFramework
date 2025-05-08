using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectMenu
{
	internal abstract class BaseMenu : EditorWindow
	{
		// The Editor's create asset menu path
		protected const string EDITOR_ASSET_MENU_PATH = "Assets/Create/Scriptable Object/";

		// The Editor's create asset menu position
		protected const int EDITOR_ASSET_MENU_PRIORITY = 82;

		protected static void UpdateAssetDatabase (Type type, string path)
		{
			// Update database
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);

			// Focus asset
			var asset = AssetDatabase.LoadAssetAtPath(path, type);
			ProjectWindowUtil.ShowCreatedAsset(asset);
			EditorGUIUtility.PingObject(asset);
		}

		protected static string TryGetProjectPath ()
		{
			var obj = Selection.activeObject;

			if (obj != null)
			{
				var path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

				if (!string.IsNullOrEmpty(path))
				{
					var attributes = File.GetAttributes(path);

					if ((attributes & FileAttributes.Directory) != FileAttributes.Directory)
					{
						return Path.GetDirectoryName(path);
					}
					else
					{
						return path;
					}
				}
			}

			return Application.dataPath;
		}
	}
}