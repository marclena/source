namespace XX.Template.Library.Contracts.Dto
{
    /// <summary>
    ///     Todos los contratos deben estar documentados
    /// </summary>
    public class FooRqDto
    {
        /// <summary>
        ///     Todas las propiedades publicas deben estar documentadas. Esto es utilizado por Swagger
        /// </summary>
        public string RecordLocator { get; set; }

        /// <summary>
        /// </summary>
        public string StationCode { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }
    }
}