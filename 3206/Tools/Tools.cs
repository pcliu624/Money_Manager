namespace _3206.Tools
{
    public class Tools
    {
        public class JsonResponse
        {
            public string code { get; set; }
            public string message { get; set; } = string.Empty;
        }
        public class JsonDataResponse<T> : JsonResponse
        {
            public T data { get; set; }
        }
        public class JsonResponse<T>
        {
            public string code { get; set; }
            public string message { get; set; }
            public int total { get; set; }
            public List<T> rows { get; set; }
            public JsonResponse()
            {
                this.rows = new List<T>();
            }
        }
    }
}
