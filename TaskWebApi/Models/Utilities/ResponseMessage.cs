using System.Collections.Generic;

namespace TaskWebApi.Models.Utilities
{
    public class ResponseMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseMessage"/> class.
        /// </summary>
        public ResponseMessage()
        {
            error_messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        
        /// <summary>
        /// Gets or sets the error_messages.
        /// </summary>
        /// <value>
        /// The error_messages.
        /// </value>
        public List<string> error_messages { get; set; }

        /// <summary>
        /// Gets or sets the extra_object that would be sent along with the response.
        /// </summary>
        /// <value>
        /// The extra_object.
        /// </value>
        public object returned_object { get; set; }

    }

    /// <summary>
    /// enumeration of possible error responses
    /// </summary>
    public enum ResponseErrors
    {
        resource_not_found_at_specified_endpoint,
        internal_server_error,
        resource_already_exist_at_specified_endpoint,
        user_not_found,
        invalid_endpoint_address
    }

    /// <summary>
    /// enumeration of possible response status
    /// </summary>
    public enum ResponseStatus { failed, success, partial }
    
}