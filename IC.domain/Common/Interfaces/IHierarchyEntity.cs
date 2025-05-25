namespace IC.Domain.Common.Interfaces
{
    public interface IHierarchyEntity
    {
		string Path { get; set; }
		int? LevelId { get; set; }
		int? ParentId { get; set; }
		bool? HasChild { get; set; }
	}
}
