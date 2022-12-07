namespace Login.Service
{
    public interface IAutenticacao
    {
        public void CriarSenhaComHashESalt(string senha, out byte[] senhaHash, out byte[] senhaSalt);
        public Task<bool> ChecarSeSenhaECorretaAsync(string senha, string email);
        public bool VerificarSenhaHash(string senha, byte[] senhaHash, byte[] senhaSalt);
        public Task<bool> ChecarQueUsuarioExisteAsync(string email);
        public Task<string> CriarTokenAsync(string email);
    }
}
