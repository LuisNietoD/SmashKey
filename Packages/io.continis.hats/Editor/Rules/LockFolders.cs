using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hats.Editor.Attributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hats.Editor.Rules
{
    public class LockFolders : RuleBase
    {
        [AssetsPath] public List<string> foldersToLock;
        
        public override string RuleName => "Lock Folders";
        
        public override string RuleDescription =>
            "Locks the specified folders in the Project view. \n \nTo setup, add the paths you want to lock, without /Assets/ in the beginning. For example, to lock the folder Assets/Scripts, add Scripts to the list.";

        public override bool OnBecameActive(Hats.ChangeReason reason)
        {
            SetupListeners();
            return true;
        }

        public override bool OnBecameInactive()
        {
            RemoveListeners();
            return true;
        }

        private void SetupListeners()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
            DragAndDrop.AddDropHandler(HandleDragAndDrop);
            EditorApplication.RepaintProjectWindow();
        }
        
        private void RemoveListeners()
        {
            EditorApplication.projectWindowItemOnGUI -= ProjectWindowItemOnGUI;
            EditorApplication.RepaintProjectWindow();
            DragAndDrop.RemoveDropHandler(HandleDragAndDrop);
        }

        private DragAndDropVisualMode HandleDragAndDrop(int id, string destinationPath, bool released)
        {
            if (!released) return DragAndDropVisualMode.Generic;

            return IsLockedPath(destinationPath) ?
                DragAndDropVisualMode.Rejected : DragAndDropVisualMode.None;
        }

        private void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            bool anyPathLocked = false;
            
            // Check if the folder is in the list of folders to lock
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if(IsLockedPath(path))
            {
                Rect adjustedRect = new(selectionRect.x - 50, selectionRect.y, selectionRect.width + 50,
                    selectionRect.height);
                Color color = EditorGUIUtility.isProSkin
                    ? new Color32(56, 56, 56, 180)
                    : new Color32(200, 200, 200, 180);
                EditorGUI.DrawRect(adjustedRect, color);
                GUI.Button(adjustedRect, "", "Label");
                anyPathLocked = true;
            }
            
            if(!anyPathLocked) return;

            // Check if the selection contains a locked folder
            bool nullSelection = false;
            foreach (string selectionGuid in Selection.assetGUIDs)
            {
                path = AssetDatabase.GUIDToAssetPath(selectionGuid);
                if (IsLockedPath(path))
                {
                    nullSelection = true;
                }
                if (!nullSelection) continue;
                
                // Found a match, clear the selection
                Selection.objects = Array.Empty<Object>();
                EditorApplication.RepaintProjectWindow();
                break;
            }
        }

        private bool IsLockedPath(string path)
        {
            // Don't lock files
            if(Path.HasExtension(path)) return false;
            
            // For a string like "Editor",
            // Second catches parent folders, like "/Editor/Whatever" and "/Editor/Scripts"
            // First catches exact paths ending in "/Editor"
            return foldersToLock?.Any(lockedFolderName => path.EndsWith($"/{lockedFolderName}")) == true
                || foldersToLock?.Any(lockedFolderName => path.Contains($"/{lockedFolderName}/")) == true;
        }
    }
}