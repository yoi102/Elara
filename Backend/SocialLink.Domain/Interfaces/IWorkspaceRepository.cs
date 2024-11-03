using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Interfaces
{
    public interface IWorkspaceRepository
    {
        Task<Workspace?> FindByIdAsync(WorkspaceId workspaceId);
        Task<Workspace?> FindByNameAsync(string name);
        Task<Workspace[]> SearchWorkspacesContainingNameAsync(string partialName);





    }
}
