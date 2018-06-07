namespace CommandApplication
{
    public class ConnectionAddress
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public bool isActive { get; set; }
    }
}