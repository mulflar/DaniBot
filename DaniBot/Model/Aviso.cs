namespace DaniBot.Model
{
    using Microsoft.Bot.Builder.FormFlow;
    using System;
    [Serializable]
    public class AvisoForm
    {
        [Prompt("Si es usted cliente porfavor indiqueme el nombre de la empresa")]
        [Optional]
        public string Empresa;
        [Prompt("Indiqueme un nombre de contacto")]
        public string Contacto;
        [Prompt("Indiqueme un telefono")]
        public string Telefono;
        [Prompt("Explique el motivo del aviso")]
        public string Motivo;
    }
}