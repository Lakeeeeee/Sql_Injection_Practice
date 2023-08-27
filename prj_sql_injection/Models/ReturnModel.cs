namespace prj_sql_injection.Models
{
    public class ReturnModel
    {
        public bool Success { get; set; }

        public string Message { get; set; }
        public object Data { get; set; }

        public ReturnModel(bool success, string message, object data = null)
        {
            this.Success = success; this.Message = message; this.Data = data;
        }

        public ReturnModel() { }

    }
}