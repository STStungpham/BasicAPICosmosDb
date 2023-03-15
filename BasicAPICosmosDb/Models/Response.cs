using BasicAPICosmosDb.Enums;

namespace BasicAPICosmosDb.Models
{
    public class Response
    {
        public ResponseStatus Status { get; set; }
        public double RequestCharge { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public object Result { get; set; }

        public Response()
        {
            Message = string.Empty;
            Status = ResponseStatus.OK;
        }

        public Response(ResponseStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        public Response(ResponseStatus status)
        {
            this.Status = status;
        }
    }

    public class ListResponse<T>
    {
        public ResponseStatus Status { get; set; }
        public double RequestCharge { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public ArrayOutput<T> Result { get; set; }

        public ListResponse()
        {
            Message = string.Empty;
            Status = ResponseStatus.OK;
            Result = new ArrayOutput<T>();
            StackTrace = string.Empty;
        }

        public ListResponse(List<T> values)
        {
            Message = string.Empty;
            Status = ResponseStatus.OK;
            Result = new ArrayOutput<T>(values);
            StackTrace = string.Empty;
        }

    }

    public class ArrayOutput<T>
    {
        public int Count { get; set; }
        public List<T> Values { get; set; }
        public ArrayOutput() 
        {
            Count = 0;
            Values = new List<T>();
        }
        public ArrayOutput(List<T> values)
        {
            Count = values.Count;
            Values = values;
        }
    }
}
