using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RevitAddinExample
{
    /// <summary> This is where requests are listed. </summary>
    /// <remarks> There should be a request per interrupt of the Revit process (modification of the Revit database). </remarks>
    internal enum RequestId : int
    {
        None = 0,
        //TODO: Add a name per tool's operation that will require a Request being made to the Revit thread, and assign it an integer.
        CropViewToElements = 1,
    }

    /// <summary> Creates a request to the thread running Revit so the RequestHandler can slip in an operation. </summary>
    class Request
    {
        //Initialized integer representing the request.
        private int _request = (int)RequestId.None;

        /// <summary> The request is taken here. </summary>
        /// <returns> For more information regarding Interlocked.Exchange, see https://docs.microsoft.com/en-us/dotnet/api/system.threading.interlocked.exchange?view=netframework-4.8 </returns>
        internal RequestId Take()
        {
            return (RequestId)Interlocked.Exchange(ref _request, (int)RequestId.None);
        }

        /// <summary> Makes the request </summary>
        /// <param name="requestId"> The enum representing the request. </param>
        internal void Make(RequestId requestId)
        {
            Interlocked.Exchange(ref _request, (int)requestId);
        }
    }
}
