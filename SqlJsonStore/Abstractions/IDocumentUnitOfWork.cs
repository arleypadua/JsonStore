using System.Threading.Tasks;

namespace JsonStore.Abstractions
{
    public interface IDocumentUnitOfWork
    {
        Task CommitAsync();
    }
}