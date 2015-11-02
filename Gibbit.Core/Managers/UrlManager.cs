using Gibbit.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Managers
{
    public class UrlManager
    {
        public string User = "https://api.github.com/user";

        public string Starred(User user)
        {
            return user.Url + "/starred";
        }

        public string Search(string query)
        {
            var searchSettings = "&sort=stars&order=desc&per_page=10";

            return "https://api.github.com/search/repositories?q=" + 
                    query + 
                    searchSettings;
        }

        public string Readme(Repo repo)
        {
            return repo.Url + "/readme";
        }

        public string Commits(Repo repo)
        {
            return repo.Url + "/commits";
        }

        public string Star(Repo repo)
        {
            return "https://api.github.com/user/starred/" +
                    repo.Owner.Name +
                    "/" +
                    repo.Name;
        }
    }
}
