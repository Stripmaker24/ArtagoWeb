using Data.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Authentication;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appication.Users.Queries
{
    public record UserListSimpleDto(IEnumerable<string> usernames);
    public class ListUsersQuery : IRequest<UserListSimpleDto>
    {
        public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, UserListSimpleDto> 
        {
            private readonly UserManager<SystemUser> userManager;

            public ListUsersQueryHandler(UserManager<SystemUser> userManager)
            {
                this.userManager = userManager;
            }

            public async Task<UserListSimpleDto> Handle(ListUsersQuery request, CancellationToken cancellationToken) 
            {
                var query = userManager.Users.AsNoTracking();
                var users = await query.ToListAsync();

                List<string> usernamelist = new List<string>();
                foreach (var user in users) 
                {
                    usernamelist.Add(user.UserName ?? string.Empty);
                }
                return new UserListSimpleDto(usernamelist);
            }
        }
    }
}
