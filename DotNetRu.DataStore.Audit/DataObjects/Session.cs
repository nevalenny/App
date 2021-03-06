﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;

namespace XamarinEvolve.DataObjects
{
    using DotNetRu.DataStore.Audit.DataObjects;

    public class Session : BaseDataObject
    {
        public Session()
        {
            this.Speakers = new List<Speaker>();
            this.Categories = new List<Category>();
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short title that is displayed in the navigation bar
        /// For instance "Intro to X.Forms"
        /// </summary>
        /// <value>The short title.</value>
        public string ShortTitle { get; set; }

        /// <summary>
        /// Gets or sets the abstract.
        /// </summary>
        /// <value>The abstract.</value>
        public string Abstract { get; set; }

        /// <summary>
        /// Gets or sets the speakers.
        /// </summary>
        /// <value>The speakers.</value>
        public virtual ICollection<Speaker> Speakers { get; set; }

        /// <summary>
        /// Gets or sets the room.
        /// </summary>
        /// <value>The room.</value>
        public virtual Room Room { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>The main categories.</value>
        public virtual ICollection<Category> Categories { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the level of the session [100 - 400]
        /// </summary>
        /// <value>The session level.</value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the url to the presentation material
        /// </summary>
        public string PresentationUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to the recorded session video
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// Gets or sets the url to the code from session
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public string CodeUrl { get; set; }

        private string speakerNames;

        [Newtonsoft.Json.JsonIgnore]
        public string SpeakerNames
        {
            get
            {
                if (speakerNames != null) return speakerNames;

                speakerNames = string.Empty;

                if (Speakers == null || Speakers.Count == 0) return speakerNames;

                var allSpeakers = Speakers.ToArray();
                speakerNames = string.Empty;
                for (int i = 0; i < allSpeakers.Length; i++)
                {
                    speakerNames += allSpeakers[i].FullName;
                    if (i != Speakers.Count - 1) speakerNames += ", ";
                }


                return speakerNames;
            }
        }

        private string speakerHandles;

        [Newtonsoft.Json.JsonIgnore]
        public string SpeakerHandles
        {
            get
            {
                if (speakerHandles != null) return speakerHandles;

                speakerHandles = string.Empty;

                if (Speakers == null || Speakers.Count == 0) return speakerHandles;

                var allSpeakers = Speakers.ToArray();
                speakerHandles = string.Empty;
                for (int i = 0; i < allSpeakers.Length; i++)
                {
                    var handle = allSpeakers[i].TwitterUrl;
                    if (!string.IsNullOrEmpty(handle))
                    {
                        if (i != 0)
                        {
                            speakerHandles += ", ";
                        }
                        speakerHandles += $"@{handle}";
                    }
                }

                return speakerHandles;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public DateTime StartTimeOrderBy
        {
            get
            {
                return StartTime.HasValue ? StartTime.Value : DateTime.MinValue;
            }
        }

        const string delimiter = "|";

        string haystack;

        [Newtonsoft.Json.JsonIgnore]
        public string Haystack
        {
            get
            {
                if (haystack != null) return haystack;

                var builder = new StringBuilder();
                builder.Append(delimiter);
                builder.Append(Title);
                builder.Append(delimiter);
                if (Categories != null)
                {
                    foreach (var c in Categories) builder.Append($"{c.Name}{delimiter}{c.ShortName}{delimiter}");
                }
                if (Speakers != null)
                {
                    foreach (var p in Speakers)
                        builder.Append(
                            $"{p.FirstName} {p.LastName}{delimiter}{p.FirstName}{delimiter}{p.LastName}{delimiter}");
                }
                haystack = builder.ToString();
                return haystack;
            }
        }

        bool feedbackLeft;

        [Newtonsoft.Json.JsonIgnore]
        public bool FeedbackLeft
        {
            get
            {
                return feedbackLeft;
            }
            set
            {
                SetProperty(ref feedbackLeft, value);
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string LevelString => $"Level: {Level}";
    }
}