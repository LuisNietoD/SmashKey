using System.Collections.Generic;
using Hats.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Hats.Editor.CustomEditors
{
    [CustomEditor(typeof(TeamsCatalog))]
    public class TeamsCatalogEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset _teamsCatalogTemplate;
        [SerializeField] private VisualTreeAsset _teamTemplate;
        
        private List<Team> _teamsList;
        private ListView _listView;

        private void OnEnable()
        {
            _teamsList = ((TeamsCatalog)target).teamsList;
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = _teamsCatalogTemplate.Instantiate();
            
            _listView = inspector.Q<ListView>("TeamsList");
            _listView.overridingAddButtonBehavior = OnAdd;
            _listView.onRemove = OnRemove;
            _listView.makeItem = MakeItem;
            _listView.bindItem = BindItem;
            _listView.unbindItem = UnBindItem;
            
            return inspector;
        }

        private VisualElement MakeItem()
        {
            InspectorElement element = new();
            return element;
        }

        private void BindItem(VisualElement element, int i)
        {
            if(_teamsList.Count == 0 || i > _teamsList.Count -1) return;
            
            Team team = _teamsList[i];
            element.Bind(new SerializedObject(team));
        }

        private void UnBindItem(VisualElement element, int i) => element.Unbind();

        private void OnAdd(BaseListView listView, Button button)
        {
            ((TeamsCatalog)target).AddTeam();
            listView.Rebuild();
        }
        
        private void OnRemove(BaseListView listView)
        {
            if (_teamsList.Count <= 1) return; 

            Team selectedItem;
            if (listView.selectedItem == null || listView.selectedIndex == -1) selectedItem = _teamsList[^1];
            else selectedItem = _teamsList[listView.selectedIndex];
            
            ((TeamsCatalog)target).RemoveTeam(selectedItem);

            listView.Rebuild();
        }
    }
}