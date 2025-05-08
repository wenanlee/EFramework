using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectMenu
{
	internal sealed class AssetMenu : BaseMenu
	{
		private static readonly HashSet<string> m_ExcludeAssemblies = new HashSet<string>
		{
			"Unity", "UnityEngine", "UnityEditor", "System", "Mono"
		};

		private static GenericMenu m_AssetPopupMenu;

		[MenuItem(EDITOR_ASSET_MENU_PATH + "Asset", true, EDITOR_ASSET_MENU_PRIORITY)]
		internal static bool Validate ()
		{
			// Disable during domain reload
			return !EditorApplication.isCompiling;
		}

		[MenuItem(EDITOR_ASSET_MENU_PATH + "Asset", false, EDITOR_ASSET_MENU_PRIORITY)]
		internal static void Initiate ()
		{
			// Populate Asset menu
			if (m_AssetPopupMenu == null)
			{
				m_AssetPopupMenu = new GenericMenu();

				// Traverse assemblies
				foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
				{
					// Filter by assembly
					var root = asm.GetName().Name.Split('.')[0];

					if (!m_ExcludeAssemblies.Contains(root))
					{
						// Traverse types
						foreach (var type in asm.GetTypes())
						{
							// Filter by type
							if (type.IsClass &&
							   !type.IsAbstract &&
							   IsScriptableObject(type))
							{
								// Convert namespace to path
								var path = type.FullName.Replace('.', '/');

								// Append type to menu
								m_AssetPopupMenu.AddItem(new GUIContent(path), false, () =>
								{
									CreateAsset(type);
								});
							}
						}
					}
				}
			}

			// Display when populated
			if (m_AssetPopupMenu.GetItemCount() > 0)
			{
				var window = CreateInstance<AssetMenu>();
				window.position = new Rect(0f, 0f, 2f, 2f);
				window.ShowPopup();
			}

			// Otherwise alert user
			else if (EditorUtility.DisplayDialog("Error", "No Scriptable Object Found", "OK"))
			{
				// And display the Script menu
				ScriptMenu.Initiate();
			}
		}

		private static void CreateAsset (Type type)
		{
			// Display save dialog
			var path = EditorUtility.SaveFilePanelInProject("Save Asset", type.Name, "asset", string.Empty, TryGetProjectPath());

			if (!string.IsNullOrEmpty(path))
			{
				// Create asset instance
				var asset = CreateInstance(type);

				if (asset != null)
				{
					// Save asset to file
					AssetDatabase.CreateAsset(asset, path);
					AssetDatabase.SaveAssets();

					UpdateAssetDatabase(type, path);

					// Log on complete
					Debug.Log($"Asset created at \"{path}\"");
				}
				else
				{
					// Alert on type error
					EditorUtility.DisplayDialog("Error", $"Invalid Asset\n\n\"{type.Name}\"", "OK");
				}
			}
		}

		private static bool IsScriptableObject (Type type)
		{
			// Get base
			type = type.BaseType;

			// Match type
			if (type != null &&
				type != typeof(Editor) &&
				type != typeof(EditorWindow))
			{
				if (type == typeof(ScriptableObject))
				{
					// Return match
					return true;
				}
				else
				{
					// Otherwise recurse
					return IsScriptableObject(type);
				}
			}

			return false;
		}

		private void OnGUI ()
		{
			// Display popup
			m_AssetPopupMenu.ShowAsContext();

			// Close this Window
			Close();
		}
	}
}