namespace WebApi.Entities {
    public class ValidationResult {
        public bool valid { get; set; }
        public int code { get; set; }
        public string message { get; set; }

        public object value { get; set; }
    }
}