﻿using Microsoft.AspNetCore.Identity;
using SocialLink.Domain.Entities;
using SocialLink.Domain.Results;

namespace SocialLink.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityResult> AccessFailedAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(UserId userId, string newPassword);

        Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

        Task<User?> FindByEmailAsync(string email);

        Task<User?> FindByIdAsync(UserId userId);

        Task<User?> FindByNameAsync(string name);

        Task<PersonalConversation[]> GetConversationsByIdAsync(UserId userId);

        Task<ContactInvitation[]> GetReceivedContactInvitationsByIdAsync(UserId userId);

        Task<WorkspaceInvitation[]> GetReceivedWorkspaceInvitationsByIdAsync(UserId userId);

        Task<UserContact[]> GetUserContactsByIdAsync(UserId userId);

        Task<Workspace[]> GetWorkspacesByIdAsync(UserId userId);

        Task<IdentityResult> RemoveUserAsync(UserId id);

        Task<IdentityResult> ResetPasswordByEmailAsync(string email, string newPassword);

        Task<IdentityResult> ResetPasswordByIdAsync(UserId id, string newPassword);

        Task<User[]> SearchUsersByNameAsync(string partialName);

        Task<SignUpResult> SignUpAsync(string name, string email, string password);
    }
}