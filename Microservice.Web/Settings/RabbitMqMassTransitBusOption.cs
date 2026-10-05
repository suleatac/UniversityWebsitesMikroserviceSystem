namespace Microservice.Web.Settings
{
    /// <summary>
    /// RabbitMQ/MassTransit baglanti ayarlari.
    /// Appsettings'te "RabbitMqMassTransitBusOption" bolumune baglanir.
    /// </summary>
    public class RabbitMqMassTransitBusOption
    {
        public required string Address { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Port { get; set; }
    }
}
