namespace Login.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Email { get; set; }
        public byte[] SenhaHash { get; set; }
        public byte[] SenhaSalt { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public Papel Papel { get; set; }
    }

    public enum Papel 
    {
        Gerente,
        Auxiliar,
        Cliente
    }
}
