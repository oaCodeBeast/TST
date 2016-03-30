using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using TST.DAL;
namespace TST.MVC.Hubs
{   

    public class User
    {
        public string Email { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }



    [Authorize]
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, User> Users =
             new ConcurrentDictionary<string, User>();


        public override Task OnConnected()
        {

            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            var user = Users.GetOrAdd(userName, _ => new User {
                Email = userName,
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                if(user.ConnectionIds.Count == 1)
                {
                    user.ConnectionIds.Add(connectionId);
                    Clients.Others.userConnected(userName);
                }
          
            }

            Clients.AllExcept(user.ConnectionIds.ToArray()).userConnected(userName);

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            User user;
            Users.TryGetValue(userName, out user);

            if(user != null)
            {
                user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));

                if(!user.ConnectionIds.Any())
                {
                    User removedUser;
                    Users.TryRemove(userName, out removedUser);


                    Clients.Others.userDisconnected(userName);
                }
            }


            return base.OnDisconnected(stopCalled);
        }





    }
}