using System;
using System.Collections.Generic;
using System.Text;

namespace CodeFactory.Web.Security
{
    public class Role
    {
        private string _Name;
        private List<string> _UserNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">A name.</param>
        public Role(string name)
        {
            _Name = name;
            _UserNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        public Role()
        {
            _UserNames = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="name">A name.</param>
        /// <param name="userNames">A list of users in role.</param>
        public Role(string name, List<string> userNames)
        {
            _Name = name;
            _UserNames = userNames;
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <value>The users.</value>
        public List<string> Users
        {
            get { return _UserNames; }
            //set { _UserNames = value; }
        }
    }
}
