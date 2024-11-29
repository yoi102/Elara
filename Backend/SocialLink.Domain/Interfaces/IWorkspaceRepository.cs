﻿using SocialLink.Domain.Entities;

namespace SocialLink.Domain.Interfaces
{
    public interface IWorkspaceRepository
    {
        Task<Workspace?> FindByIdAsync(WorkspaceId workspaceId);

        Task<Workspace?> FindByNameAsync(string name);

        Task<WorkspaceConversation[]> GetWorkspaceAllConversationsAsync(WorkspaceId id);

        Task<WorkspaceMember[]> GetWorkspaceAllWorkspaceMemberAsync(WorkspaceId id);

        Task<Workspace[]> SearchWorkspacesByNameAsync(string partialName);
    }
}