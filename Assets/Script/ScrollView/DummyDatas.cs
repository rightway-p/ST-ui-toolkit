using System.Collections.ObjectModel;
using Unity.Properties;

[GeneratePropertyBag]                 // ★ 루트에도 Bag을 생성해야 경로 탐색 가능
public class DummyDatas
{
    [GeneratePropertyBag]
    public class FileItem
    {
        [CreateProperty] public string Name { get; set; }
    }

    [CreateProperty]
    public ObservableCollection<FileItem> LocalFiles { get; } = new()
    {
        new FileItem { Name="default.installation" },
        new FileItem { Name="default.variables" },
        new FileItem { Name="fast_move.installation" },
        new FileItem { Name="slow_move.variables" },
        new FileItem { Name="myScript.script" },
    };
}