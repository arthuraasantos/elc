namespace Infra
{
    public interface IUnitofWork
    {
        string ActionUser { get; }
        Guid ActionUserId { get; }
        Task<int> CommitAsync();
    }
}
