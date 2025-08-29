using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Unity.Properties;
using System.Collections.ObjectModel;
using System.Collections.Generic;

public class ListViewPopulator : MonoBehaviour
{
    [SerializeField] private UIDocument uIDocument;

    private DummyDatas _repo;
    private ListView _listView;


    void OnEnable()
    {
        _repo = new DummyDatas(); ;
        var _root = uIDocument.rootVisualElement;
        _listView = _root.Q<ListView>("ListView");


        var testItems = new List<string>
        {
            "테스트 아이템 1",
            "Test Item 2",
            "이것이 보이면 성공"
        };

        _root.dataSource = _repo;
        _listView.itemsSource = _repo.LocalFiles;


        // _listView.makeItem = () => new Label();
        // _listView.bindItem = (item, index) =>
        // {
        //     (item as Label).text = _repo.LocalFiles[index].Name;
        // };

        // 프로브
        _root.schedule.Execute(() =>
        {
            var il = _listView.itemsSource as System.Collections.IList;
            Debug.Log($"[probe] items after binding = {(il != null ? il.Count : -1)}"); // 5
        }).ExecuteLater(0);


        // repo = new DummyDatas(); 라고 이미 만든 상태라고 가정
        ObservableCollection<DummyDatas.FileItem> list;

        // 1) 먼저 LocalFiles 컬렉션 자체를 꺼냄
        var okList = PropertyContainer.TryGetValue(
            ref _repo,
            new PropertyPath(nameof(DummyDatas.LocalFiles)),
            out list);

        Debug.Log($"okList={okList}, list.Count={list?.Count ?? -1}");

        // 2) 첫 아이템의 Name 속성을 꺼냄
        if (list != null && list.Count > 0)
        {
            var item0 = list[0];  // 인덱서 결과는 ref 못 쓰니 지역 변수로
            var okName = PropertyContainer.TryGetValue(
                ref item0,
                new PropertyPath(nameof(DummyDatas.FileItem.Name)),
                out string nm);

            Debug.Log($"okName={okName}, nm='{nm}'");
        }
        // Debug.Log("[probe] LocalFiles.Count = " + _repo.LocalFiles.Count);

        // _root.schedule.Execute(() =>
        // {
        //     var il = _listView.itemsSource as System.Collections.IList;
        //     Debug.Log("[probe] items after binding = " + il != null ? il.Count : 0);
        // }).ExecuteLater(0);

        // // 1프레임 뒤 itemsSource 카운트
        // _root.schedule.Execute(() =>
        // {
        //     var il = _listView.itemsSource as System.Collections.IList;
        //     Debug.Log($"items after binding = {(il != null ? il.Count : -1)}");  // 5
        //     Debug.Log($"[probe] first item name (direct) = {_repo.LocalFiles[0]?.Name ?? "<null>"}");
        // }).ExecuteLater(0);

        // // 첫 행 텍스트
        // _root.schedule.Execute(() =>
        // {
        //     var first = _listView.Q<Label>(); // 템플릿의 Label
        //     Debug.Log($"first row text = {first?.text}"); // "default.installation" 기대
        // }).ExecuteLater(100);
    }

    private IEnumerator AddItemAfterDelay(float delay)
    {
        _repo.LocalFiles.Add(new DummyDatas.FileItem { Name = "new_added.txt" });
        yield return new WaitForSeconds(delay);
        _repo.LocalFiles.Add(new DummyDatas.FileItem { Name = "new_added.txt" });

        _listView.RefreshItems();
    }
}
