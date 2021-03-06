﻿using System;
using System.Xml.Serialization;

namespace DotNetRu.DataStore.Audit.Models
{
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Meetup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string CommunityId { get; set; }
        [XmlElement(DataType = "date")]
        public DateTime Date { get; set; }
        [XmlArrayItem("FriendId", IsNullable = false)]
        public string[] FriendIds { get; set; }
        public string VenueId { get; set; }
        [XmlArrayItem("TalkId", IsNullable = false)]
        public string[] TalkIds { get; set; }
    }
}