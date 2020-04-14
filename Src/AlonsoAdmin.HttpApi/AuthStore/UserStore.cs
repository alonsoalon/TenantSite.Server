using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlonsoAdmin.HttpApi.AuthStore
{

    public class UserStore
    {

        private static readonly List<User> _users = new List<User>()
        {
            new User {
                Id="adminstrator",
                Name="admin",
                Password="111111",
                Roles=new List<string>(new string[]{"admin","test" })
            },
            new User {
                Id="1234",
                Name="test",
                Password="111111",
                Roles=new List<string>(new string[]{"test"})
            },

        };

        public List<User> GetAll()
        {
            return _users;
        }

        public User Find(string id)
        {
            return _users.Find(_ => _.Id == id);
        }

        public User Find(string userName, string password)
        {
            return _users.FirstOrDefault(_ => _.Name == userName && _.Password == password);
        }

        public bool Exists(string id)
        {
            return _users.Any(_ => _.Id == id);
        }

        public void Add(User doc)
        {
            doc.Id = doc.Name;
            _users.Add(doc);
        }

        public void Update(string id, User doc)
        {
            var oldDoc = _users.Find(_ => _.Id == id);
            if (oldDoc != null)
            {
                oldDoc.Name = doc.Name;
                oldDoc.Password = doc.Password;
            }
        }

        public void Remove(User doc)
        {
            if (doc != null)
            {
                _users.Remove(doc);
            }
        }


        public bool CheckPermission(string userId, string permissionName)
        {
            var user = Find(userId);
            if (user == null) return false;
            return user.Roles.Any(p => permissionName == p);
        }
    }

    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
