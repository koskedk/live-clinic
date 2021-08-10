namespace LiveClinic.SharedKernel.Config
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMq";
        public string Host { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
