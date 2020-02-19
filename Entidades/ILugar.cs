namespace CoreEscuela.Entidades
{
    public interface ILugar
    {
        public string Direccion { get; set; }

        void LimpiarLugar();
    }
}