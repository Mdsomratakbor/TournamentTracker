﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    /// <summary>
    /// Represents one person in a team.
    /// </summary>
    public class PersonModel
    {
        public int Id { get; set; }
        /// <summary>
        /// The first name of the person.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// The last name of the person.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// The primary email address  of the person.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// The primary cell phone number  of the person.
        /// </summary>
        public string CellPhoneNumber { get; set; }
    }
}
