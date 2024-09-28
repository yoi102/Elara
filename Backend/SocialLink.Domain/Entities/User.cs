﻿using DomainCommons;
using Microsoft.AspNetCore.Identity;
using Strongly;

namespace SocialLink.Domain.Entities
{

    [Strongly(converters: StronglyConverter.EfValueConverter | StronglyConverter.SwaggerSchemaFilter | StronglyConverter.SystemTextJson)]
    public partial struct UserId
    { }

    public class User : IdentityUser<UserId>, ISoftDelete, IHasCreationTime, IHasDeletionTime
    {
        
        private User()
        {
                
        }
        public User(string name)
        {
            Id = UserId.New();
            UserName = name;
            DisplayName = name;
            CreationTime = DateTimeOffset.Now;
        }
           
        public Uri? Avatar { get; set; }
        public DateTimeOffset CreationTime { get; private set; }
        public DateTimeOffset? DeletionTime { get; private set; }
        public string? DisplayName { get; set; }
        public bool IsDeleted { get; private set; }
        public void SoftDelete()
        {
            IsDeleted = true;
            DeletionTime = DateTimeOffset.Now;
        }
    }
}