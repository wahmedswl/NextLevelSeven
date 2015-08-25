﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextLevelSeven.Codecs;

namespace NextLevelSeven.Core
{
    public class Message : IMessage
    {
        /// <summary>
        /// Create a message with a default MSH segment.
        /// </summary>
        public Message()
        {
            _message = new Cursors.Message(@"MSH|^~\&|");
        }

        /// <summary>
        /// Create a message using an HL7 data string.
        /// </summary>
        /// <param name="message">Message data to interpret.</param>
        public Message(string message)
        {
            _message = new Cursors.Message(message);
        }

        /// <summary>
        /// Create a message cursor wrapper.
        /// </summary>
        /// <param name="internalMessage">Internal message to wrap.</param>
        internal Message(Cursors.Message internalMessage)
        {
            _message = internalMessage;
        }

        /// <summary>
        /// Internal message cursor.
        /// </summary>
        private readonly Cursors.Message _message;

        /// <summary>
        /// Get a descendant element at the specified index. Indices match the HL7 specification, and are not necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Element that was found at the index.</returns>
        IElement IElement.this[int index]
        {
            get { return _message[index]; }
        }

        /// <summary>
        /// Get a descendant segment at the specified index. Indices match the HL7 specification, and are not necessarily zero-based.
        /// </summary>
        /// <param name="index">Index to query.</param>
        /// <returns>Segment that was found at the index.</returns>
        public ISegment this[int index]
        {
            get { return _message[index] as ISegment; }
        }

        /// <summary>
        /// Get segments of a specific segment type.
        /// </summary>
        /// <param name="segmentType">The 3-character segment type to query for.</param>
        /// <returns>Segments that match the query.</returns>
        public IEnumerable<ISegment> this[string segmentType]
        {
            get { return Segments.Where(s => s.Type == segmentType); }
        }

        /// <summary>
        /// Get segments of a type that matches one of the specified segment types. They are returned in the order they are found in the message.
        /// </summary>
        /// <param name="segmentTypes">The 3-character segment types to query for.</param>
        /// <returns>Segments that match the query.</returns>
        public IEnumerable<ISegment> this[IEnumerable<string> segmentTypes]
        {
            get { return Segments.Where(s => segmentTypes.Contains(s.Type)); }
        }

        /// <summary>
        /// Get conversion options for non-string datatypes, such as numeric and dates.
        /// </summary>
        public ICodec As
        {
            get { return _message.As; }
        }

        /// <summary>
        /// Always returns null. A message is the highest level in the heirarchy.
        /// </summary>
        public IElement AncestorElement
        {
            get { return null; }
        }

        /// <summary>
        /// Create a deep clone of the message.
        /// </summary>
        /// <returns>Clone of the message.</returns>
        public IMessage Clone()
        {
            return new Message(Value);
        }

        /// <summary>
        /// Create a deep clone of the message. Because a message is at the top of the heirarchy, this is identical to calling Clone().
        /// </summary>
        /// <returns>Clone of the message.</returns>
        public IElement CloneDetached()
        {
            return Clone();
        }

        /// <summary>
        /// Get or set the message Control ID from MSH-10.
        /// </summary>
        public string ControlId
        {
            get { return Msh[10].Value; }
            set { Msh[10].Value = value; }
        }

        /// <summary>
        /// Throws an exception. A message is at the highest level and cannot be deleted from an ancestor.
        /// </summary>
        public void Delete()
        {
            throw new ElementException(@"The root element of a message cannot be deleted.");
        }

        /// <summary>
        /// Get the number of descendant elements.
        /// </summary>
        public int DescendantCount
        {
            get { return _message.DescendantCount; }
        }

        /// <summary>
        /// Get all descendant elements.
        /// </summary>
        public IEnumerable<IElement> DescendantElements
        {
            get { return _message.DescendantElements; }
        }

        /// <summary>
        /// Throws an exception. A message is at the highest level and cannot be erased from an ancestor.
        /// </summary>
        public void Erase()
        {
            throw new ElementException(@"The root element of a message cannot be erased.");
        }

        /// <summary>
        /// Always returns true. A message has no ancestors, and will therefore never be 'non-existant'.
        /// </summary>
        public bool Exists
        {
            get { return true; }
        }

        /// <summary>
        /// Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segment">Segment index.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public IElement GetField(int segment, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            var segmentElement = _message[segment];
            if (field < 0)
            {
                return segmentElement;
            }
            if (repetition < 0)
            {
                return segmentElement[field];
            }
            if (component < 0)
            {
                return segmentElement[field][repetition];
            }
            if (subcomponent < 0)
            {
                return segmentElement[field][repetition][component];
            }
            return segmentElement[field][repetition][component][subcomponent];
        }

        /// <summary>
        /// Get data from a specific place in the message. Depth is determined by how many indices are specified.
        /// </summary>
        /// <param name="segmentName">Segment name.</param>
        /// <param name="field">Field index.</param>
        /// <param name="repetition">Repetition number.</param>
        /// <param name="component">Component index.</param>
        /// <param name="subcomponent">Subcomponent index.</param>
        /// <returns>The first occurrence of the specified element.</returns>
        public IElement GetField(string segmentName, int field = -1, int repetition = -1, int component = -1, int subcomponent = -1)
        {
            var segment = _message.FirstOrDefault(s => s.Value != null && s.Value.StartsWith(segmentName));
            if (field < 0 || segment == null)
            {
                return segment;
            }
            if (repetition < 0)
            {
                return segment[field];
            }
            if (component < 0)
            {
                return segment[field][repetition];
            }
            if (subcomponent < 0)
            {
                return segment[field][repetition][component];
            }
            return segment[field][repetition][component][subcomponent];
        }

        /// <summary>
        /// Returns zero.
        /// </summary>
        public int Index
        {
            get { return _message.Index; }
        }

        /// <summary>
        /// Returns a unique identifier for the message.
        /// </summary>
        public string Key
        {
            get { return _message.Key; }
        }

        /// <summary>
        /// Get the root message for this element.
        /// </summary>
        IMessage IElement.Message
        {
            get { return this; }
        }

        /// <summary>
        /// Get the first MSH segment.
        /// </summary>
        IElement Msh
        {
            get { return this["MSH"].First(); }
        }

        /// <summary>
        /// Set the message data to null.
        /// </summary>
        public void Nullify()
        {
            Value = null;
        }

        /// <summary>
        /// Get or set the message Processing ID from MSH-11.
        /// </summary>
        public string ProcessingId
        {
            get { return Msh[11].Value; }
            set { Msh[11].Value = value; }
        }

        /// <summary>
        /// Get or set the message Receiving Application from MSH-5.
        /// </summary>
        public string ReceivingApplication
        {
            get { return Msh[5].Value; }
            set { Msh[5].Value = value; }
        }

        /// <summary>
        /// Get or set the message Receiving Facility from MSH-6.
        /// </summary>
        public string ReceivingFacility
        {
            get { return Msh[6].Value; }
            set { Msh[6].Value = value; }
        }

        /// <summary>
        /// Get or set the message Security from MSH-8.
        /// </summary>
        public string Security
        {
            get { return Msh[8].Value; }
            set { Msh[8].Value = value; }
        }

        /// <summary>
        /// Get all segments in the message.
        /// </summary>
        public IEnumerable<ISegment> Segments
        {
            get { return _message.Segments; }
        }

        /// <summary>
        /// Get or set the message Sending Application from MSH-3.
        /// </summary>
        public string SendingApplication
        {
            get { return Msh[3].Value; }
            set { Msh[3].Value = value; }
        }

        /// <summary>
        /// Get or set the message Sending Facility from MSH-4.
        /// </summary>
        public string SendingFacility
        {
            get { return Msh[4].Value; }
            set { Msh[4].Value = value; }
        }

        /// <summary>
        /// Get or set the message Time from MSH-7.
        /// </summary>
        public DateTimeOffset? Time
        {
            get { return Codec.ConvertToDateTime(Msh[7].Value); }
            set { Msh[7].Value = Codec.ConvertFromDateTime(value); }
        }

        /// <summary>
        /// Get or set the message Trigger Event from MSH-7-2.
        /// </summary>
        public string TriggerEvent
        {
            get { return Msh[9][0][2].Value; }
            set { Msh[9][0][2].Value = value; }
        }

        /// <summary>
        /// Get or set the message Trigger Event from MSH-7-1.
        /// </summary>
        public string Type
        {
            get { return Msh[9][0][1].Value; }
            set { Msh[9][0][1].Value = value; }
        }

        /// <summary>
        /// Check for validity of the message. Returns true if the message can reasonably be parsed.
        /// </summary>
        /// <returns>True if the message can be parsed, false otherwise.</returns>
        public bool Validate()
        {
            var value = Value;

            if (value == null)
            {
                return false;
            }

            if (!Value.StartsWith("MSH"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Get or set the element data. When setting new data, descendents will automatically be repopulated.
        /// </summary>
        public string Value
        {
            get
            {
                return _message.Value;
            }
            set
            {
                _message.Value = value;
            }
        }

        /// <summary>
        /// Get or set the element data. Delimiters are automatically inserted.
        /// </summary>
        public string[] Values
        {
            get
            {
                return _message.Values;
            }
            set
            {
                _message.Values = value;
            }
        }

        /// <summary>
        /// Get or set the message Version from MSH-12.
        /// </summary>
        public string Version
        {
            get { return Msh[12].Value; }
            set { Msh[12].Value = value; }
        }

        /// <summary>
        /// Get an element enumerator.
        /// </summary>
        /// <returns>Typed IEnumerator.</returns>
        public IEnumerator<IElement> GetEnumerator()
        {
            return _message.GetEnumerator();
        }

        /// <summary>
        /// Get an element enumerator.
        /// </summary>
        /// <returns>Generic IEnumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _message.GetEnumerator();
        }
    }
}